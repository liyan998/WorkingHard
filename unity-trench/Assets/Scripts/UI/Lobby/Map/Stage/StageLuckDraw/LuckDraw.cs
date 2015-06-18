using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LuckDraw : MonoBehaviour
{

    [SerializeField]
    Animator
        anim;
    [SerializeField]
    RectTransform
        RewardsBox;
    [SerializeField]
    RewardItem
        Reward;
    [SerializeField]
    float
        ItemHeight;
    [SerializeField]
    Text
        DrawScore;
    [SerializeField]
    Button
        BeginLuckBut;
    [SerializeField]
    ItemData
        data;
    [SerializeField]
    ParticleSystem getItemVfx;
    [SerializeField]
    ItemIcon itemIcon;
    //Dictionary<INFO_PROPERTY,RewardItem> itemDict = new Dictionary<INFO_PROPERTY, RewardItem>();
    int lastSelectedItem = 0;
    RewardItem[] rewardeArr;
    int rewardsNum = 0;
    float drawHeight = 0;
    float initHeight = 0;
    private List<INFO_PROPERTY> rewards;
    private INFO_STAGE stageDraw; //关卡抽奖
    public void SetDrawStage(INFO_STAGE stage)
    {
        itemIcon.gameObject.SetActive(false);
        this.stageDraw = stage;
        DrawScore.text = stage.LotteryCost.ToString();
        
        if (rewards != null)
        {
            UserStageRecord record = DataBase.Instance.PLAYER.GetUserStageRecord(stageDraw.Id);
            if (record!=null && record.IsUsableProperty == 1)
            { 
                for (int i = 0; i < rewards.Count; i++)
                {
                    if (record.Property == rewards[i].Id)
                    {
                        itemIcon.item = rewardeArr[i].item;
                        itemIcon.gameObject.SetActive(true);
                        itemIcon.Refresh();
                    }
                }
            }
            
        }

        
    }
    //初始化奖励项
    public void InitRewards()
    {
        
        rewards = DataBase.Instance.LOTTERY_MGR.GetLotteryProp();
        rewardsNum = rewards.Count;
        rewardeArr = new RewardItem[rewardsNum * 2];
        //滚动区高度
        drawHeight = rewardsNum * ItemHeight * 2;
        //initHeight = (rewardsNum - 1) * ItemHeight - ItemHeight * 0.34f;
        initHeight = ItemHeight * 0.66f;
        Rect rc = RewardsBox.rect;
        RewardsBox.sizeDelta = new Vector2(rc.width, drawHeight);
        Vector2 v2 = RewardsBox.anchoredPosition;
        v2.y -= initHeight;
        RewardsBox.anchoredPosition = v2;
        int temp = 0;


        UserStageRecord record = DataBase.Instance.PLAYER.GetUserStageRecord(stageDraw.Id);
        for (int i = 0; i < rewardsNum * 2; i++)
        {
            temp = i;
            
            if (i >= rewardsNum)
                temp = i - rewardsNum;
            RewardItem item = Instantiate(Reward) as RewardItem;
            INFO_PROPERTY prop = rewards [temp];
            item.SetReward(rewards [temp].Name, data.GetIcon((LevelItemType)(rewards [temp].Id)),(LevelItemType)(rewards [temp].Id));
            item.transform.SetParent(RewardsBox);
            item.transform.localScale = Vector3.one;
            item.gameObject.SetActive(true);
//            RectTransform rect = item.GetComponent<RectTransform>();
//            rect.
            item.transform.localPosition = new Vector3(0, i * ItemHeight);
            //if (!itemDict.ContainsKey(prop))
            //{
                //itemDict.Add(prop, item);
            //}
            rewardeArr[i] = item;
            if (record!=null && record.IsUsableProperty == 1)
            { 
                if (record.Property == prop.Id)
                {
                    itemIcon.item = rewardeArr[i].item;
                    itemIcon.gameObject.SetActive(true);
                    itemIcon.Refresh();
                }
            }
            
        }
    }

    //摇奖动画
    INFO_PROPERTY luckResult;
    IEnumerator Scroll()
    {
        if (stageDraw != null)
        {
            //获取抽奖结果
            
            int renum = 0;
            if (luckResult != null)
            {
                renum = rewards.IndexOf(luckResult);
                renum -= 2;
                if (renum < 0)
                    renum = rewardsNum + renum;
                int scrollNum = Random.Range(6, 10);
                Vector2 v2 = RewardsBox.anchoredPosition;
                //计算总共要滚动的距离 
                float tutor = (scrollNum * rewardsNum + renum) * ItemHeight;
                //tutor += (v2.y - ItemHeight * 0.34f);
                //tutor += v2.y;
                v2.y = -initHeight;
                RewardsBox.anchoredPosition = v2;
                float sleep = 0f;
                float drawHeight2 = (drawHeight * 0.5f + initHeight);
                while (tutor > 0)
                {
                    sleep = tutor / (scrollNum * rewardsNum + renum) + 0.1f;
                    v2 = RewardsBox.anchoredPosition;
                    v2.y -= sleep;
                    tutor -= sleep;
                    if (v2.y <= -drawHeight2){
                        v2.y += drawHeight * 0.5f;
                        SOUND.Instance.OneShotSound(Sfx.inst.slotRolling);
                    }
                    RewardsBox.anchoredPosition = v2;
                    yield return new WaitForSeconds(0.001f);
                }
                
            }
            int itemNum = renum + 2;
            rewardeArr[itemNum].SetSelected(true);
            itemIcon.item=rewardeArr[itemNum].item;
            lastSelectedItem = itemNum;
        }
        LobbyDialogManager.inst.ShowCover(false);
        BeginLuckBut.interactable = true;
        itemIcon.gameObject.SetActive(true);
        itemIcon.Refresh();
        itemIcon.GetComponent<ScaleTweener>().Show();
        SOUND.Instance.OneShotSound(Sfx.inst.show);
        getItemVfx.transform.position=itemIcon.transform.position;
        getItemVfx.Play();
        yield return 0;

    }
    // 摇奖
    public void BeginLuckDraw()
    {
        luckResult = DataBase.Instance.LOTTERY_MGR.GetLotteryResult(stageDraw);
        if (luckResult != null)
        { 
            if (lastSelectedItem != 0)
            {
                rewardeArr[lastSelectedItem].SetSelected(false);
            }
            LobbyDialogManager.inst.ShowCover(true);
            BeginLuckBut.interactable = false;
            anim.Play(0);
            StartCoroutine(Scroll());
            SOUND.Instance.OneShotSound(Sfx.inst.slotSpin);
        }
        
    }

    void Start()
    {
        InitRewards();
    }
}
