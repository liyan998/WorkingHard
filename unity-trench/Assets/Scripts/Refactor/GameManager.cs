using UnityEngine;
using System.Collections;
using GameEvent;

public class GameManager : MonoBehaviour, IEventHandler
{

    #region
    private static GameManager mInstance;

    public static GameManager Instance
    {
        get
        {
            mInstance = GameObject.FindObjectOfType<GameManager>() as GameManager;

            if(mInstance == null)
            {
                GameObject temp = new GameObject();
                temp.name = typeof(GameManager).ToString();
                mInstance = temp.AddComponent<GameManager>();
                DontDestroyOnLoad(temp);
            }

            return mInstance;
        }
    }

    #endregion

    //场景管理
    GameScene mGameScene;
    public GameScene GameScene
    {
        get
        {
            return mGameScene;
        }
    }

    //网络服务
    NetService mNs;
    public NetService NetService
    {
        get
        {
            return mNs;
        }
    }
    
    //---------------------------------------------------


    void Awake()
    {
        InitServcie();
    }
  
	void Start ()
    {        
        EventSystem.Instance.RegisterEvent(1, this);        
	}


    void InitServcie()
    {
        //网络服务
        mNs         = new NetService();
        //场景
        mGameScene  = new GameScene();


    }

    public void ActionEvent(CEvent gameevent)
    {
        Debug.Log(gameevent.mEventId);
    }
}
