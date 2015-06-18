using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum StageLocality
{
    Xian,
    AnKang,
    HanZhong,
    Shangluo,
    Baoji,
    XianYang,
    XingPing,
    WeiNan,
    HanCheng,
    HuaYin,
    TongChuan,
    YanAn,
    Yulin=12
}

public class StageBtn : MonoBehaviour
{
    [ContextMenuItem("Refresh","Refresh")]
    public StageLocality
        locality;
    public bool isMarkOnRightSide = true;
    [SerializeField]
    Image[]
        topStars;
    [SerializeField]
    Image[]
        bottomStars;
    [SerializeField]
    Image[]
        localMarkL;
    [SerializeField]
    Image[]
        localMarkR;
    [SerializeField]
    Image
        ball;
    [SerializeField]
    Sprite[]
        localBalls;
    [SerializeField]
    Sprite
        star;
    [SerializeField]
    Sprite
        grayStar;
    [SerializeField]
    Text
        centerText;
    [SerializeField]
    Text
        bottomText;
    private uint stageId;
    
    public event System.Action<uint> clickEvent;

    public void InitStage(uint stageId, string stageName, Vector3 local)
    {
        this.gameObject.name = "stage_" + stageId;
        this.stageId = stageId;
        centerText.text = stageName;
        bottomText.text = stageName;
        this.transform.localPosition = local;
    }

    void Start()
    {
        EventTriggerListener.Get(this.gameObject).onClick += OnClick;
    }

    //点击时触发
    public void OnClick(GameObject go)
    {
        if (clickEvent != null && stageId > 0)
            this.clickEvent(stageId);
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void SetStar(int starNum)
    {
        for (int i=0; i<starNum; i++)
        {
            topStars [i].sprite = star;
            topStars [i].gameObject.SendMessage("Show");
            bottomStars [i].sprite = star;
            bottomStars [i].gameObject.SendMessage("Show");
        }
    }

    public void Refresh()
    {
        foreach(Image mark in localMarkL){
            mark.gameObject.SetActive(false);
        }
        foreach(Image mark in localMarkR){
            mark.gameObject.SetActive(false);
        }
        if (isMarkOnRightSide)
        {
//            localMarkL [(int)locality].gameObject.SetActive(false);
            localMarkR [(int)locality].gameObject.SetActive(true);
        } else
        {
            localMarkL [(int)locality].gameObject.SetActive(true);
//            localMarkR [(int)locality].gameObject.SetActive(false);
        }
        ball.sprite = localBalls [(int)locality];
        switch (locality)
        {
            case StageLocality.Xian:
            case StageLocality.AnKang:
            case StageLocality.HanZhong:
            case StageLocality.XingPing:
            case StageLocality.HanCheng:
            case StageLocality.HuaYin:
            case StageLocality.TongChuan:
            case StageLocality.YanAn:
                for (int i=0; i<3; i++)
                {
                    topStars [i].gameObject.SetActive(false);
                    bottomStars [i].gameObject.SetActive(true);
                }
                centerText.gameObject.SetActive(true);
                bottomText.gameObject.SetActive(false);
                break;
            case StageLocality.Shangluo:
            case StageLocality.XianYang:
                for (int i=0; i<3; i++)
                {
                    topStars [i].gameObject.SetActive(true);
                    bottomStars [i].gameObject.SetActive(false);

                }
                centerText.gameObject.SetActive(true);
                bottomText.gameObject.SetActive(false);
                break;
            case StageLocality.Baoji:
            case StageLocality.WeiNan:
            case StageLocality.Yulin:
                for (int i=0; i<3; i++)
                {
                    topStars [i].gameObject.SetActive(false);
                    bottomStars [i].gameObject.SetActive(true);
                    
                }
                centerText.gameObject.SetActive(false);
                bottomText.gameObject.SetActive(true);
                break;
                
        }
    }
}
