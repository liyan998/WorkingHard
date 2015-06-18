using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using ZZSDK;
using UnityEngineEx.Net;
using UnityEngineEx.Common;


public enum LobbyDialog
{
    Confirm=0,
    UserInfo,
    Mail,
    Setting,
    Activity,
    Friend,
    Ranking,
    Mission,
    LevelInfo,
    DailySign,
    BindPhone=10,
    HelpInfo=11,
    Upgrade=12,
    StageProDlg=13
}

public class LobbyDialogManager : DialogManager
{
    //[Tooltip("0 Confirm,1 UserInfo,2 Mail,3 Setting,4 Activity,5 Friend,6 Ranking,7 Mission,8 LevelInfo,9 LevelResult,10 DailySign")]
    public static LobbyDialogManager inst;
    ConfirmDialog confirmDialog;
    [SerializeField]
    UnityEvent
        onLowCoin;
    [SerializeField]
    UnityEvent
        onLowDiamond;
    [SerializeField]
    UnityEvent
        onLowItem;
    public bool isGoToShop = false;

    void Awake()
    {
        inst = this;
       
    }

    void Start()
    {
        //GetDialog (LobbyDialog.Confirm).GetComponent<ConfirmDialog> ().Init ("您确定要退出游戏吗？",Quit);
        confirmDialog = GetDialog(LobbyDialog.Confirm).GetComponent<ConfirmDialog>();

        //========================================================================
       
        //StartCoroutine(StartCheckVersion());        

        CheckVersion();
      
    }

    public  Dialog GetDialog(LobbyDialog dialog)
    {
        return dialogs [(int)dialog];
    }

    public  void ShowDialog(LobbyDialog dialog)
    {
        ShowDialog((int)dialog);
    }

    public override  void  OnShow()
    {
        if (currentDialog != (int)LobbyDialog.LevelInfo)
        {
            LobbyController.inst.SwitchState(LobbyState.Dialog);
        }
    }
    
    public void HideDialog(LobbyDialog dialog)
    {
        HideDialog((int)dialog);
    }

    public override void OnHide()
    {
        if (isGoToShop)
        {
            isGoToShop = false;
            LobbyController.inst.SwitchState(LobbyState.Shop);
        } else
        {
            if (openedDialogs.Count == 0)
            {
                Debug.Log(lastDialog);
                if (lastDialog != (int)LobbyDialog.LevelInfo || LobbyController.inst.currentState==LobbyState.Dialog)
                {
                    LobbyController.inst.BackToLastState();
                }
            }
        }
    }

    public void ShowConfirmDialog(string descString, Action onConfirmed, string confirmDescString=null, bool hasCancelBtn=true, Action onCancelled=null, string cancelDescString=null, string title=null)
    {
        confirmDialog.Init(descString, onConfirmed, confirmDescString, hasCancelBtn, onCancelled, cancelDescString, title);
        ShowDialog(LobbyDialog.Confirm);
    }

//    private void CloseAll()
//    {
//        foreach (Dialog dia in dialogs){
//            if (dia != null && dia.enabled){
//
//            }
//        }
//    }

    public void ShowLowCoinDialog(GoodType goodType, string descString=null)
    {
        string desc = "";
        string title = "";
        Action gotoShopPage = onLowCoin.Invoke;
        if (goodType == GoodType.Coin)
        {
            gotoShopPage = onLowCoin.Invoke;
            desc = TextManager.Get("lowCoinTip");
            title = TextManager.Get("lowCoinTitle");
        } else if (goodType == GoodType.Diamond)
        {
            gotoShopPage = onLowDiamond.Invoke;
            desc = TextManager.Get("lowDiamondTip");
            title = TextManager.Get("lowDiamondTitle");
        } else
        {
            gotoShopPage = onLowItem.Invoke;
            desc = TextManager.Get("lowItemTip");
            title = TextManager.Get("lowItemTitle");
        }
        if (string.IsNullOrEmpty(descString))
        {
            descString = desc;
        } 
        ShowConfirmDialog(descString,
            delegate
        {
            if (gotoShopPage != null)
            {
                gotoShopPage();
            }
            //CloseAll();
            openedDialogs.Clear();
//            HideDialog(currentDialog, true);
        }, 
            TextManager.Get("Buy"), true, delegate()
        {
            isGoToShop = false;
        }, null, title);
        isGoToShop = true;
    }


    IEnumerator StartCheckVersion()
    {
        Debug.Log("Application.internetReachability:" + Application.internetReachability);

        

        yield return 0;


        if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            CheckVersion();
        }
        
    }
//     public static void checkVersion(string reqDataStr, Action<CheckVersionResponse> readCallback)
//     {
//         ServiceRequestClient client = new ServiceRequestClient();
//         client.DispatchIO += (a) =>
//         {
//             readCallback(a.FromJSON<CheckVersionResponse>());
//         };
// 
//         client.ProcessIO("http://120.131.70.98:10080/ZZStatsService.svc/checkVersion?RequestData=" + reqDataStr, "");
//     }


    void CheckVersion()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Network is not onWork!");
            return;
        }

        if (DeviceManager.Instance.hasUpdate)
        {
            return;
        }

        DeviceManager.Instance.hasUpdate = true;

        Debug.Log("CheckVersion ....");

        CheckVersionRequest reqData = new CheckVersionRequest()
        {
            deviceToken = DeviceManager.Instance.deviceId,
            versionCode = "" + COMMON_CONST.versionCode
        };
        string reqDataStr = reqData.ToJSON();
        Action<CheckVersionResponse> readCallback = onCheckVersionResponse;

        //ZZProxy.checkVersion(reqDataStr, readCallback);

        ServiceRequestClient client = new ServiceRequestClient();
        client.DispatchIO += (a) =>
        {
            readCallback(a.FromJSON<CheckVersionResponse>());
        };

        client.ProcessIO("http://120.131.70.98:10080/ZZStatsService.svc/checkVersion?RequestData=" + reqDataStr, "");
    }

    void onCheckVersionResponse(CheckVersionResponse response)
    {
        Debug.Log("Response_isForceUpdate:" + response.isForceUpdate);
        Debug.Log("Response_isNeedUpdate:" + response.isNeedUpdate);
        Debug.Log("Response_newVersionURL:" + response.newVersionURL);

        if (!response.isNeedUpdate)
        {
            return;
        }

        UpgradeDlg ud = GetDialog(LobbyDialog.Upgrade).GetComponent<UpgradeDlg>();

        UpgradeDlg.MODE mode = response.isForceUpdate ? UpgradeDlg.MODE.MODE_MUSTUPDATE : UpgradeDlg.MODE.MODE_UPDATE;
        ud.ShowUpradeDialog(mode, response.newVersionURL);

        ShowDialog(LobbyDialog.Upgrade);
    }
    

    public void CloseAll()
    {
        isGoToShop = true;
        openedDialogs.Clear();
        HideCurrentDialog();
    }
}
