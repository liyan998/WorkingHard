using UnityEngine;
using System.Collections;
using JPush;

public class JPushManager : MonoBehaviour {

#if UNITY_EDITOR


#elif UNITY_ANDROID
	// Use this for initialization
	void Start () {
        gameObject.name = "Main Camera";
        JPushBinding.setDebug(true);
        JPushBinding.initJPush(gameObject.name, "");	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    // remove event listeners
    void OnDestroy()
    {
        print("unity3d---onDestroy");
        if (gameObject)
        {
            // remove all events
            JPushEventManager.instance.removeAllEventListeners(gameObject);
        }        
    }

    //开发者自己处理由JPush推送下来的消息
    void recvMessage(string str)
    {
        Debug.Log("recv----message-----" + str);
        //B_MESSAGE = true;
        //str_message = str;
        //str_unity = "有新消息";
    }

    //开发者自己处理点击通知栏中的通知
    void openNotification(string str)
    {
        Debug.Log("recv --- openNotification---" + str);
        //str_unity = str;
    }
#endif
}
