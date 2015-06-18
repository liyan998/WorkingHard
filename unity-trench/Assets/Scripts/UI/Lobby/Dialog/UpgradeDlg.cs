using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UpgradeDlg : Dialog
{
    [SerializeField]
    GameObject[] mAllLayout;

    [SerializeField]
    Text mText;

    string mUrl;

    public enum MODE
    {
        MODE_MUSTUPDATE,//必须更新
        MODE_UPDATE     //更新
    }

    void Awake()
    {        
        //ShowUpradeDialog(MODE.MODE_UPDATE);
    }


	public void ShowUpradeDialog(MODE mode, string url)
    {
        mText.text = "天天挖坑已发布全新版本，赶紧更新一下吧！";
        mUrl = url;
        switch(mode)
        {
            case MODE.MODE_MUSTUPDATE:
                mAllLayout[(int)MODE.MODE_MUSTUPDATE].SetActive(true);
                mAllLayout[(int)MODE.MODE_UPDATE].SetActive(false);                
                break;
            case MODE.MODE_UPDATE:
                mAllLayout[(int)MODE.MODE_MUSTUPDATE].SetActive(false);
                mAllLayout[(int)MODE.MODE_UPDATE].SetActive(true);
                break;
        }
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
        }
        //Show();
       
       
    }

    public void CannelCallBack()
    {        
        LobbyDialogManager.inst.HideDialog(LobbyDialog.Upgrade);
    }

    public void UpdateCallBack()
    {
        LobbyDialogManager.inst.HideDialog(LobbyDialog.Upgrade);
        UpdateVersion(mUrl);
    }

    void UpdateVersion(string updateurl)
    {
#if UNITY_ANDROID
        
            try
            {
                Debug.Log("Unity Layer:" + updateurl);
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject MainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
                MainActivity.Call("updateApk", updateurl);
            }
            catch (System.Exception ex)
            {
                //sDeviceManager.Instance.ShowToast(ex.Message, true);
                //bPressedCancel = false;
            }
        
#endif
    }
}
