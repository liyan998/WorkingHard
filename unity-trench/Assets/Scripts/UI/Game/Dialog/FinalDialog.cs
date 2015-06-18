using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class FinalDialog : Dialog
{

    bool isVictory = false;
    bool isNormal = true;
    [SerializeField]
    GameObject
        normalReward;
    [SerializeField]
    GameObject
        levelReward;
    [SerializeField]
    GameObject
        normalButs;
    [SerializeField]
    Button
        HeightRoomBut;
    [SerializeField]
    GameObject
        stageButs;
    [SerializeField]
    Text
        normalRewardLabel;
    [SerializeField]
    Text
        levelRewardLabel;//本局得分

    [SerializeField]
    Text
        levelLabelCount;//总分
    [SerializeField]
    Text
        shareBtnLabel;
    [SerializeField]
    ResultTip
        tip;
    [SerializeField]
    VfxManager
        vfx;
    [SerializeField]
    ScaleTweener
        winTitle;
    [SerializeField]
    ScaleTweener
        loseTitle;

    [SerializeField]
    Button[] NormalButs;
    [SerializeField]
    Button[] StageButs;
    //Action onShareBtn;

    //游戏结算按钮处理函数
    Action onNormalBut1;
    Action onNormalBut2;
    Action onNormalBut3;

    //关卡结算按钮处理
    Action onStageBut1;
    Action onStageBut2;
    Action onStageBut3;

//    [SerializeField]
//    Button mConintue;
//    
//
//    List<ScoreItem> mAllItem;
//
//    Action mAction;

//    void Start()
//    {
//        mAllItem = new List<ScoreItem>();        
//    }

    public void Init(bool isSuccess, bool isNormalMode, int[] allData, Action but1=null, Action but2=null, Action but3=null)
    {
        isVictory = isSuccess;
        isNormal = isNormalMode;
        winTitle.gameObject.SetActive(false);
        loseTitle.gameObject.SetActive(false);
        //激活按钮
        foreach (Button but in NormalButs)
            but.interactable = true;
        foreach (Button but in StageButs)
            but.interactable = true;
        if (isNormal)
        {
            normalReward.SetActive(true);
            levelReward.SetActive(false);
            normalButs.SetActive(true);
            stageButs.SetActive(false);
            HeightRoomBut.gameObject.SetActive(false);
            if (allData.Length > 0)
            {
                normalRewardLabel.text = allData [0] > 0 ? "+" + allData [0] : allData [0].ToString();
            }
            var database = DataBase.Instance;
            enRoomType roomType = 
                GameHelper.Instance.GetStage() == GameHelper.STAGE_BOM ? enRoomType.enBomb : enRoomType.enNormal;

            INFO_ROOM room = database.ROOM_MGR.GetCanPlayRoom(roomType, database.PLAYER.Gold);

            if (room.dwMinBonus > database.CurRoom.dwMinBonus)
            {
                HeightRoomBut.gameObject.SetActive(true);
                onNormalBut2 = delegate
                {
                    MainGame.inst.OnExitGame();
                    database.EnterNomalRoom(room.wServerID);
                };

            }
            onNormalBut1 = but1;
            
            onNormalBut3 = but3;
        } else
        {
            normalReward.SetActive(false);
            levelReward.SetActive(true);
            normalButs.SetActive(false);
            stageButs.SetActive(true);
            if (allData.Length > 1)
            { 
                levelRewardLabel.text = allData [0] > 0 ? "+" + allData [0] : allData [0].ToString();
                levelLabelCount.text = allData [1].ToString();
            }

            onStageBut1 = but1;
            onStageBut2 = but2;
            onStageBut3 = but3;
                
        }
        SetWin(isVictory);
    }

    public void SetWin(bool isWin)
    {
        tip.SetResultTip(isWin);
        tip.ShowResult(isWin);
        if (isWin)
        {
            shareBtnLabel.text = TextManager.Get("Flaunt");
            StartCoroutine(ShowWinLabel());
            vfx.Play(VFX.FireWork);
        } else
        {
            shareBtnLabel.text = TextManager.Get("SeekHelp");
            StartCoroutine(ShowLoseLabel());
        }
    }

    IEnumerator ShowWinLabel()
    {
        yield return new WaitForSeconds(0.5f);
        vfx.Play(VFX.Win);
    }

    IEnumerator ShowLoseLabel()
    {
        yield return new WaitForSeconds(0.5f);
        vfx.Play(VFX.Lose);
    }

    public void OnCloseBtn()
    {
//        base.Hide();
//        vfx.StopFireWork();
//        vfx.HideWinLabel();
//        vfx.HideLoseLabel();
        Hide();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void OnShareBtn()
    {
        // onShareBtn();
        vfx.StopFireWork();
        vfx.HideWinLabel();
        vfx.HideLoseLabel();
        OnDragBegin();
        GameDialog.inst.ShowDialog(GameDialogIndex.Share);
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }


    //普通游戏结算处理
    public void OnNormalBut1()
    {
//        base.Hide();
        NormalButs[0].interactable = false;
        Hide();
        if (onNormalBut1 != null)
        {
            onNormalBut1();
        }
//        vfx.StopFireWork();
//        vfx.HideWinLabel();
//        vfx.HideLoseLabel();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void OnNormalBut2()
    {
//        base.Hide();
        NormalButs[1].interactable = false;
        Hide();
        if (onNormalBut2 != null)
        {
            onNormalBut2();
        }
//        vfx.StopFireWork();
//        vfx.HideWinLabel();
//        vfx.HideLoseLabel();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void OnNormalBut3()
    {
//        base.Hide();
        NormalButs[2].interactable = false;
        Hide();
        if (onNormalBut3 != null)
        {
            onNormalBut3();
        }
//        vfx.StopFireWork();
//        vfx.HideWinLabel();
//        vfx.HideLoseLabel();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }


    //关卡结算按钮处理
    public void OnStageBut1()
    {
//        base.Hide();
        StageButs[0].interactable = false;
        Hide();
        if (onStageBut1 != null)
        {
            onStageBut1();
        }
//        vfx.StopFireWork();
//        vfx.HideWinLabel();
//        vfx.HideLoseLabel();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void OnStageBut2()
    {
//        base.Hide();
        StageButs[1].interactable = false;
        Hide();
        if (onStageBut2 != null)
        {
            onStageBut2();
        }
//        vfx.StopFireWork();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void OnStageBut3()
    {
//        base.Hide();
        StageButs[2].interactable = false;
        Hide();
        if (onStageBut3 != null)
        {
            onStageBut3();
        }
//        vfx.StopFireWork();
//        vfx.HideWinLabel();
//        vfx.HideLoseLabel();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void OnDragBegin()
    {
        if (isVictory)
        {
            vfx.HideWinLabel();
            winTitle.gameObject.SetActive(true);
            winTitle.Show();
        } else
        {
            vfx.HideLoseLabel();
            loseTitle.gameObject.SetActive(true);
            loseTitle.Show();
        }
    }

    public override void Hide()
    {
        base.Hide();
        OnDragBegin();
        vfx.StopFireWork();
        vfx.HideWinLabel();
        vfx.HideLoseLabel();
    }
//    public override void Show()
//    {
//        base.Show();
//        mConintue.enabled = true;       
//    }
//
//    public override void Hide()
//    {
//        base.Hide();
//      if (mAction != null) {
//          mAction ();
//      }
//        ClearAllItem();
//    }


//    IEnumerator AddItem(int[] allData)
//    {
//        yield return new WaitForSeconds(0.5f);
//
//        for (int i = 0; i < allData.Length; i++)
//        {            
//            Vector3 pos =  mMove.position + new Vector3(50, i * 20 - 100, 0);
//
//            ScoreItem tLab = Instantiate(mLab, pos, Quaternion.identity) as ScoreItem;
//            tLab.transform.SetParent(mPanelContext.transform);
//            tLab.SetData(allData[i]);
//
//            //yield return new WaitForSeconds(0.05f);
//
//            mAllItem.Add(tLab);          
//        }
//
//   
////         for(int i = 0;i < mAllItem.Count;i++)
////         {
////             iTween.MoveFrom(
////                mAllItem[i].gameObject,
////                iTween.Hash(
////                    "position",  new Vector3(100, 0, 0),
////                    "islocal", false,
////                    "easetype", iTween.EaseType.easeInOutBack,
////                    "time", 0.5f
////                ));
//// 
//           yield return new WaitForSeconds(0.5f);
////         }
//    }

    /// <summary>
    /// 设置结算统计数据
    /// </summary>
    /// <param name="allData"></param>
//    public void SetStatisticsData(int[] allData)
//    {
//        StartCoroutine(AddItem(allData));
//    }
//
//    /// <summary>
//    /// 关卡结算
//    /// </summary>
//    /// <param name="allData"></param>
//    public void SetStageData(int[] allData)
//    {
//        StartCoroutine(AddItem(allData));
//    }
//
//
//    public void SetConuite(Action action)
//    {
//        mAction = action;
//    }
//
//
//    void ClearAllItem()
//    {
//        foreach (ScoreItem go in mAllItem)
//        {
//            go.transform.SetParent(null);
//
//            Destroy(go.gameObject);            
//        }
//
//        mAllItem.Clear();
//    }
}
