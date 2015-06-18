using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StageController : MonoBehaviour {

    [SerializeField]
    WindowData winData;

    [SerializeField]
    RectTransform DrawParent;
    [SerializeField]
    RectTransform StageDraw; //拖动对象

    [SerializeField]
    RectTransform[] MapBack; //地图背景
    [SerializeField]
    Transform Decoration; //地图装饰

    [SerializeField]
    PrintGenerator Route; //路径

    [SerializeField]
    Transform Stages; //关卡组

    [SerializeField]
    StageBtn stage;   //关卡对象

    [SerializeField]
    Text StarCount;

    [SerializeField]
    LobbyDialogManager DialogMar;
    //当前用户
    [SerializeField]
    ParticleSystem openStageParticle;

    [SerializeField]
    GameObject CoverObj;
    //地图宽、高度
    private float mapWidth = 0;
    private float mapHeight = 0;

    private StageMgr sMgr;
    private Player player;

    void Awake()
    {
        sMgr = DataBase.Instance.STAGE_MGR;
        player = DataBase.Instance.PLAYER;
    }
    //初始化关卡地图
    public void InitStageMap()
    {
        float beginX = 0;
        float endX = 0;
        //读取所有关卡
        List<INFO_STAGE> stageList = sMgr.GetAllStage();
        //读取用户完成关卡状态
        StarCount.text = player.GetTotalStar().ToString();
        //InitStageMapBack();
        int stageNum = stageList.Count;
        float parentWidth = DrawParent.rect.width;
        mapHeight = DrawParent.rect.height;
        if (stageNum > 0)
        {
            beginX = stageList[0].X;
            endX = stageList[stageNum-1].X;
            //计算地图宽度
            mapWidth = beginX + endX;
            float posx = -mapWidth / 2;
            float posy = -mapHeight / 2;
            Rect rc = StageDraw.rect;
            //StageDraw.rect.Set(rc.xMin, rc.yMin, mapWidth, rc.height);
            //StageDraw.sizeDelta = new Vector2(mapWidth,480);
            Vector3 drawLocal = StageDraw.localPosition;
            int backCount = MapBack.Length;
            if (backCount > 0)
            {
                //float rcWidth = rc.width;
                float rcWidth = 6554;
                float backwidth = rcWidth / backCount;
                for(int i=0;i<MapBack.Length;i++)
                {
                    Vector2 v2 = MapBack[i].sizeDelta;
                    v2.x = backwidth;
                    MapBack[i].sizeDelta = v2;
                    MapBack[i].localPosition = new Vector3(i * backwidth,0);
                }
                
            }
            uint curStageId = DataBase.Instance.PLAYER.GetLastSuccessStage();
            if (curStageId > 0)
            {
                INFO_STAGE curStage = DataBase.Instance.STAGE_MGR.GetStage(curStageId);
                if (parentWidth * 0.5f < curStage.X)
                                drawLocal.x =  - curStage.X;
            }
            

            StageDraw.localPosition = drawLocal;
            //StageDraw.anchoredPosition = new Vector2(-curStage.X + (parentWidth*0.5f), 0);
            uint gotoStage = DataBase.Instance.PLAYER.NextStage(curStageId);
            if (curStageId > 0)
                Route.InitStageFoot((int)curStageId);
            else
                Route.InitStageFoot(1);
            if (winData.StageId == 0)
            { 
                Route.GoToStage((int)gotoStage);
            }
            
            foreach (INFO_STAGE stage in stageList)
            {
               
                StageBtn stageBut = Instantiate(this.stage) as StageBtn;
                StageStatus status = player.GetStageStatus(stage.Id);
                UserStageRecord ustage = player.GetUserStageRecord(stage.Id);
                if (status == StageStatus.Compelet)
                {
                    //stageBut.clickEvent += PopStageInfo;
                    int starNum = COMMON_FUNC.GetStarByScore(stage.Id, ustage.BestScore);
                    stageBut.SetStar(starNum);
                }
                stageBut.clickEvent += StageButClick;
                stageBut.transform.SetParent(Stages);
                Vector3 v3 = new Vector3(stage.X,posy+stage.Y);
                stageBut.transform.localScale = Vector3.one;
                stageBut.InitStage(stage.Id, stage.Id.ToString(), v3);
                stageBut.locality = (StageLocality)stage.StageMark;
                stageBut.isMarkOnRightSide = (stage.MarkDirection == 1);
                stageBut.Refresh();

            }
        }
        
        


    
    }

    void InitStageMapBack()
    {

        SingleDownMgr.Instance.GetBundle("prefab/mapback.android.unity3d", typeof(UnityEngine.Texture),
        (UnityEngine.Object obj)=>{
            
            
        });
    }


    //关卡按钮处理
    void StageButClick(uint stageId)
    {
        INFO_STAGE stage = DataBase.Instance.STAGE_MGR.GetStage(stageId);
        StageStatus status = player.GetStageStatus(stageId);
        if (status == StageStatus.CanPlay || status == StageStatus.Compelet)
        {
            PopStageInfo(stageId);
        }
        else if (status == StageStatus.NotUnlock)
        {
            //提示
            LobbyDialogManager.inst.ShowConfirmDialog(
                    string.Format(TextManager.Get("StageUnlockTip"), stage.NeedStar),
                    null,null,false
            );
        }
        else if (status == StageStatus.Unlocked)
        {
            //开启 前置关卡未完成
            LobbyDialogManager.inst.ShowConfirmDialog(
                    string.Format(TextManager.Get("StageNotCanPlayTip"), stage.OpenCondition),
                    null,null,false
            );
        }
        winData.StageId = 0;
        
    }
    //弹出关卡信息
    INFO_STAGE openStage;
    public void PopStageInfo(uint stageId)
    {
        openStage = DataBase.Instance.STAGE_MGR.GetStage(stageId);
        UserStageRecord record = DataBase.Instance.PLAYER.GetUserStageRecord(stageId);
        if (record != null && record.LastStatus == (int)StageRecordStatus.Progress)
        {
            //正在进行的关卡，显示关卡进度
            List<INFO_STAGE_COND_LINK> stageConds = 
                DataBase.Instance.STAGE_MGR.GetStageCondInfoByGroup(stageId, StageCond.COND_GROUPTHREE);
            INFO_STAGE_COND_LINK stageCondThree = null;
            if (stageConds.Count > 0) stageCondThree = stageConds[0];

            //作弊效果
            string propStr = "无";
            int propId = record.Property;
            if (propId != 0)
            {
                INFO_PROPERTY prop = DataBase.Instance.PROP_MGR.GetProperty(propId);
                propStr = prop.Name;
            }
            //弹出关卡进度
            Dialog proDialog = DialogMar.GetDialog(LobbyDialog.StageProDlg);
            StageProgressDlg stagepro = proDialog.GetComponent<StageProgressDlg>();
            stagepro.Init(
                string.Format(TextManager.Get("StageProgressTitle"), stageId),
                openStage.Describe + "(" + record.CondVal3 + "/" + stageCondThree.Condition + ")",
                propStr,
                delegate{
                    record.LastStatus = (int)StageRecordStatus.Failure;
                    StageButClick(stageId);
                }
                ,
                TextManager.Get("StageReplayBut"),
                true,
                rePlayStage,
                TextManager.Get("StageContinueBut")
                );
            DialogMar.ShowDialog(LobbyDialog.StageProDlg);
            return;
            
        }
        Dialog stageDialog = DialogMar.GetDialog(LobbyDialog.LevelInfo);
        StageInfo stageInfo = stageDialog.GetComponent<StageInfo>();

        if (stageInfo != null)
        {
            //获取奖励信息
            var rewardList = DataBase.Instance.REWARD_MGR.GetStageReward(stageId);
            //string rewardInfo = "";
            //foreach(var v in rewardList)
                //rewardInfo += v.describe + " ";
            stageInfo.InitStage(openStage, record, rewardList);
            stageInfo.BeginStageClick = rePlayStage;
            DialogMar.ShowDialog(LobbyDialog.LevelInfo);
        }
    }
    //重玩关卡
    void rePlayStage()
    {
        DataBase.Instance.STAGE_MGR.ReplayStage(openStage.Id);
    }

	void Start () {
        //StartCoroutine(init());
        InitStageMap();
        
        if (winData.StageId > 0)
        {

            if (winData.StageId > DataBase.Instance.PLAYER.GetLastSuccessStage())
            {
                CoverObj.SetActive(true);
                Route.GoToStage((int)winData.StageId, delegate
                {
                    var stageBut = Stages.FindChild("stage_" + winData.StageId);
                    if (stageBut != null)
                    {
                        openStageParticle.transform.SetParent(Stages);
                        openStageParticle.transform.localPosition = stageBut.localPosition;
                        openStageParticle.Play();
                    }

                    Invoke("stageOpen",1.8f);
                });
            }
            else
            {
                StageButClick(winData.StageId);
            }
        }
            
	}

    void stageOpen()
    {
        StageButClick(winData.StageId);
        CoverObj.SetActive(false);
    }
    
	
}
