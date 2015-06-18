using UnityEngine;
using System.Collections;
using cn.sharesdk.unity3d;
public class PluginManager : MonoBehaviour
{
	ShareSDK shareSDK;
    static Hashtable shareContent = new Hashtable();
	#if UNITY_ANDROID
	public static AndroidJavaObject _plugin;
	#endif
	
	void Awake ()
	{
//		if (PlayerPrefs.GetInt ("DebugMode", 0) != 1) {
//			#if UNITY_ANDROID
//			using (var pluginClass = new AndroidJavaClass( "com.Superme.Trench.MainActivity" )) {
//				_plugin = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
//				//_plugin.Call("Login");
//				Debug.Log ("PLUGIN INITIAL");
//			}
//			#endif
//		}

		shareSDK = this.GetComponent<cn.sharesdk.unity3d.ShareSDK>();
	}

	void Start ()
	{
		Debug.Log ("InitShareSDK");
		cn.sharesdk.unity3d.ShareSDK.setCallbackObjectName ("Main Camera");
        cn.sharesdk.unity3d.ShareSDK.open ("73beefd5a7c4");//api20
		
		//Sina Weibo
		Hashtable sinaWeiboConf = new Hashtable ();
		sinaWeiboConf.Add ("app_key", "568898243");
		sinaWeiboConf.Add ("app_secret", "38a4f8204cc784f81f9f0daaf31e02e3");
		sinaWeiboConf.Add ("redirect_uri", "http://www.sharesdk.cn");
		cn.sharesdk.unity3d.ShareSDK.setPlatformConfig (cn.sharesdk.unity3d.PlatformType.SinaWeibo, sinaWeiboConf);
		
		//WeChat
		Hashtable wcConf = new Hashtable ();
        wcConf.Add ("app_id", "wx57e2a6302edcc841");
        //wcConf.Add ("app_secret", "3af2bd71ef008706408f17c81535a5eb");
		cn.sharesdk.unity3d.ShareSDK.setPlatformConfig (cn.sharesdk.unity3d.PlatformType.WeChatSession, wcConf);
		cn.sharesdk.unity3d.ShareSDK.setPlatformConfig (cn.sharesdk.unity3d.PlatformType.WeChatTimeline, wcConf);
		cn.sharesdk.unity3d.ShareSDK.setPlatformConfig (cn.sharesdk.unity3d.PlatformType.WeChatFav, wcConf);

        //Qzone
        Hashtable qzoneConf = new Hashtable ();
        qzoneConf.Add ("app_id", "wx57e2a6302edcc841");
        qzoneConf.Add ("app_key", "3af2bd71ef008706408f17c81535a5eb");
        cn.sharesdk.unity3d.ShareSDK.setPlatformConfig (cn.sharesdk.unity3d.PlatformType.QZone, qzoneConf);

        shareContent["content"] = "this is a test string.";
        shareContent["image"] = "http://img.baidu.com/img/image/zhenrenmeinv0207.jpg";
        shareContent["title"] = "test title";shareContent["description"] = "test description";
        shareContent["url"] = "http://sharesdk.cn";
        shareContent["type"] = ((int)ContentType.News).ToString();
        shareContent["siteUrl"] = "http://sharesdk.cn";shareContent["site"] = "ShareSDK";
        shareContent["musicUrl"] = "http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3";
	}



    public static void Share ()
    {
        ShareResultEvent evt = new ShareResultEvent(ShareResultHandler);
        ShareSDK.showShareMenu (null, shareContent, 100, 100, MenuArrowDirection.Up, evt);
    }

    static void ShareResultHandler (ResponseState state, PlatformType type, Hashtable shareInfo, Hashtable error, bool end)
    {
        if (state == ResponseState.Success)
        {
            print ("share result :");
            print (MiniJSON.jsonEncode(shareInfo));
        }
        else if (state == ResponseState.Fail)
        {
            print ("fail! error code = " + error["error_code"] + "; error msg = " + error["error_msg"]);
        }
    }
    
    public static void ShowAds ()
	{
		if (Application.platform != RuntimePlatform.WindowsEditor) {
			if (PlayerPrefs.GetInt ("DebugMode", 0) != 1 && PlayerPrefs.GetInt ("TVMode", 0) == 0) {
				if (PlayerPrefs.GetString ("NormalAD") == "1") {
					#if UNITY_ANDROID
				//_plugin.Call ("ShowAds");
					#endif
				}
			}
		}
		Debug.Log ("ShowAds");
	}

	public static void Purchase (string pid)
	{
		Debug.Log ("purchase item=" + pid);
		#if UNITY_ANDROID
				//_plugin.Call ("Buy", pid);
		#endif
	}

	public static void Quit ()
	{
		#if UNITY_ANDROID
		_plugin.Call ("Quit");
		#endif
	}
}
