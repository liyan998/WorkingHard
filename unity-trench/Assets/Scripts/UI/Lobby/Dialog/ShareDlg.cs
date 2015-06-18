using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using cn.sharesdk.unity3d;

public class ShareDlg : Dialog
{
    [SerializeField]
    Button
        rewardBtn;
    [SerializeField]
    Button[]
        shareBtns;
    [SerializeField]
    Text[]
        rewardNums;
    [SerializeField]
    Image[]
        availableMarks;
    [SerializeField]
    Image[]
        rewardableMarks;
//    [SerializeField]
//    ParticleSystem
//        gotVfx;
    [SerializeField]
    Text
        shareContent;
    [SerializeField]
    ShareData
        data;
    [SerializeField]
    UserData
        user;
    LobbyState state;

    public override void Init()
    {
        data.Init();
        rewardBtn.interactable = false;
        for (int i=0; i<3; i++)
        {
            if (data.todaySharedStates [i] == 1)
            {
                availableMarks [i].gameObject.SetActive(true);
//                availableMarks [i].text = TextManager.Get("rewardAvailableTip");
                rewardableMarks [i].CrossFadeAlpha(1f, 0.5f, true);
                rewardBtn.interactable = true;
            } else if (data.todaySharedStates [0] == 2)
            {
                availableMarks [i].gameObject.SetActive(true);
//                availableMarks [i].text = TextManager.Get("rewardGotTip");
                rewardableMarks [i].CrossFadeAlpha(0.5f, 0.5f, true);
            } else
            {
                availableMarks [i].gameObject.SetActive(false);
                rewardableMarks [i].CrossFadeAlpha(1f, 0.5f, true);
            }
            if (GameHelper.Instance.GetStage() == GameHelper.STAGE_BOM || GameHelper.Instance.GetStage() == GameHelper.STAGE_NORMAL)
            {
                rewardNums [i].text = data.GetInfo(i, LobbyState.Arena).GivenValue.ToString();
                state = LobbyState.Arena;
            } else
            {
                rewardNums [i].text = data.GetInfo(i, LobbyState.Map).GivenValue.ToString();
                state = LobbyState.Map;
            }
        }
        if (state == LobbyState.Arena)
        {
            shareContent.text = string.Format(data.GetInfo(PlatformType.SinaWeibo, state).ShareText, 10);
        } else
        {
            shareContent.text = data.GetInfo(PlatformType.SinaWeibo, state).ShareText;
        }
    }

    public void Share()
    {
        Hashtable content = new Hashtable();
        content ["content"] = "this is a test string.";
        content ["image"] = "http://img.baidu.com/img/image/zhenrenmeinv0207.jpg";
        content ["title"] = "test title";
        content ["description"] = "test description";
        content ["url"] = "http://sharesdk.cn";
        content ["type"] = Convert.ToString((int)ContentType.News);
        content ["siteUrl"] = "http://sharesdk.cn";
        content ["site"] = "ShareSDK";
        content ["musicUrl"] = "http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3";
        ShareResultEvent evt = new ShareResultEvent(ShareResultHandler);
        ShareSDK.showShareMenu(null, content, 100, 100, MenuArrowDirection.Up, evt);
    }

    public void ShareToWeibo()
    {
        Hashtable content = new Hashtable();
        if (state == LobbyState.Arena)
        {
            content ["content"] = string.Format(data.GetInfo(PlatformType.SinaWeibo, state).ShareText, 10);
        } else
        {
            content ["content"] = data.GetInfo(PlatformType.SinaWeibo, state).ShareText;
        }
//        Application.CaptureScreenshot("shared.png");
//        string path = Application.dataPath + "/shared.png";
//        content ["image"] = "file://" + path;
        content ["title"] = TextManager.Get("Trench");
        content ["description"] = content ["content"];
        content ["url"] = "http://weibo.com/rein1";
        content ["type"] = Convert.ToString((int)ContentType.News);
        content ["siteUrl"] = "http://weibo.com/rein1";
        content ["site"] = "Rein";
        content ["musicUrl"] = "http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3";
        ShareResultEvent evt = new ShareResultEvent(ShareResultHandler);
        ShareSDK.shareContent(PlatformType.SinaWeibo, content, evt);
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void ShareToWeChat()
    {
        Hashtable content = new Hashtable();
        if (state == LobbyState.Arena)
        {
            content ["content"] = string.Format(data.GetInfo(PlatformType.WeChatTimeline, state).ShareText, 10);
        } else
        {
            content ["content"] = data.GetInfo(PlatformType.WeChatTimeline, state).ShareText;
        }
//        Application.CaptureScreenshot("shared.png");
//        string path = Application.dataPath + "/shared.png";
//        content ["image"] = "file://" + path;
        content ["title"] = TextManager.Get("Trench");
        content ["description"] = content ["content"];
        content ["url"] = "http://weibo.com/rein1";
        content ["type"] = Convert.ToString((int)ContentType.News);
        content ["siteUrl"] = "http://weibo.com/rein1";
        content ["site"] = "Rein";
        content ["musicUrl"] = "http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3";
        ShareResultEvent evt = new ShareResultEvent(ShareResultHandler);
        ShareSDK.shareContent(PlatformType.WeChatTimeline, content, evt);
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void ShareToQzone()
    {
        Hashtable content = new Hashtable();
        if (state == LobbyState.Arena)
        {
            content ["content"] = string.Format(data.GetInfo(PlatformType.QZone, state).ShareText, 10);
        } else
        {
            content ["content"] = data.GetInfo(PlatformType.QZone, state).ShareText;
        }
        Application.CaptureScreenshot("shared.png");
        string path = Application.dataPath + "/shared.png";
        content ["image"] = "file://" + path;
        content ["title"] = TextManager.Get("Trench");
        content ["description"] = content ["content"];
        content ["url"] = "http://weibo.com/rein1";
        content ["type"] = Convert.ToString((int)ContentType.News);
        content ["siteUrl"] = "http://weibo.com/rein1";
        content ["site"] = "Rein";
        content ["musicUrl"] = "http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3";
        ShareResultEvent evt = new ShareResultEvent(ShareResultHandler);
        ShareSDK.shareContent(PlatformType.QZone, content, evt);
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }
    
    void ShareResultHandler(ResponseState state, PlatformType type, Hashtable shareInfo, Hashtable error, bool end)
    {
        if (state == ResponseState.Success)
        {
            print("share result :");
            print(MiniJSON.jsonEncode(shareInfo));

            if (data.todaySharedStates [0] == 0 && type == PlatformType.WeChatTimeline)
            {
                //availableMarks [0].gameObject.SetActive(true);
//                availableMarks [0].text = TextManager.Get("rewardAvailableTip");
                rewardableMarks [0].CrossFadeAlpha(1f, 0.5f, true);
                rewardBtn.interactable = true;
                data.todaySharedStates [0] = 1;
            }

            if (data.todaySharedStates [1] == 0 && type == PlatformType.QZone)
            {
                //availableMarks [1].gameObject.SetActive(true);
//                availableMarks [1].text = TextManager.Get("rewardAvailableTip");
                rewardableMarks [1].CrossFadeAlpha(1f, 0.5f, true);
                rewardBtn.interactable = true;
                data.todaySharedStates [1] = 1;
            }

            if (data.todaySharedStates [2] == 0 && type == PlatformType.SinaWeibo)
            {
                //availableMarks [2].gameObject.SetActive(true);
//                availableMarks [2].text = TextManager.Get("rewardAvailableTip");
                rewardableMarks [2].CrossFadeAlpha(1f, 0.5f, true);
                rewardBtn.interactable = true;
                data.todaySharedStates [2] = 1;
            }
            PlayerPrefs.SetString("ShareStates", data.todaySharedStates [0] + "," + data.todaySharedStates [1] + "," + data.todaySharedStates [2]);

        } else if (state == ResponseState.Fail)
        {
            print("fail! error code = " + error ["error_code"] + "; error msg = " + error ["error_msg"]);
        }
    }

    public void OnGetButton()
    {
//        gotVfx.Play();
        rewardBtn.interactable = false;
        if (data.todaySharedStates [0] == 1)
        {
            GetReward(PlatformType.WeChatTimeline);
            data.todaySharedStates [0] = 2;
            availableMarks [0].gameObject.SetActive(true);
//            availableMarks [0].text = TextManager.Get("rewardGotTip");
            rewardableMarks [0].CrossFadeAlpha(0.5f, 0.5f, true);
        }
        if (data.todaySharedStates [1] == 1)
        {
            GetReward(PlatformType.QZone);
            data.todaySharedStates [1] = 2;
            availableMarks [0].gameObject.SetActive(true);
//            availableMarks [0].text = TextManager.Get("rewardGotTip");
            rewardableMarks [0].CrossFadeAlpha(0.5f, 0.5f, true);

           
        }
        if (data.todaySharedStates [2] == 1)
        {
            GetReward(PlatformType.SinaWeibo);
            data.todaySharedStates [2] = 2;
            availableMarks [0].gameObject.SetActive(true);
//            availableMarks [0].text = TextManager.Get("rewardGotTip");
            rewardableMarks [0].CrossFadeAlpha(0.5f, 0.5f, true);
        }
        PlayerPrefs.SetString("ShareStates", data.todaySharedStates [0] + "," + data.todaySharedStates [1] + "," + data.todaySharedStates [2]);
        SOUND.Instance.OneShotSound(Sfx.inst.reward);


    }

    void GetReward(PlatformType platform)
    {
        GameDialog.inst.ShowConfirmDialog(string.Format(data.GetInfo(platform, state).RewardedText,
                                                        TextManager.Get(platform.ToString()),
                                                        TextManager.Get(((GoodType)(data.GetInfo(platform, state).Type)).ToString()),
                                                        data.GetInfo(platform, state).GivenValue), delegate()
        {
            switch ((GoodType)data.GetInfo(platform, state).Type)
            {
                case GoodType.Diamond:
                    user.diamond += data.GetInfo(platform, state).GivenValue;
                    break;
                case GoodType.Coin:
                    user.coin += data.GetInfo(platform, state).GivenValue;
                    break;
                case GoodType.Character:
                    break;
                case GoodType.Item1:
                    break;
                default:
                    break;
            }
            user.SaveMoney();
            UserHud.inst.RefreshMoney();
        }, null, false);
    }

    public void OnCloseBtn()
    {
//        GameDialog.inst.ShowLastDialog();
        GameDialog.inst.HideDialog(GameDialogIndex.Share);
        //GameDialog.inst.ShowLastDialog();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }
}
