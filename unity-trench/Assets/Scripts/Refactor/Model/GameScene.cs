using UnityEngine;
using System.Collections;

/// <summary>
/// 逻辑场景
/// </summary>
public class GameScene
{

    public enum SCENE
    {
        SCENE_LOGO,     //logo
        SCENE_LOADING,  //loading
        SCENE_
        
    }

    public const string SC_Logo         = "Logo";
    public const string SC_Loading      = "Loading";
    public const string SC_Hall         = "Lobby";
    public const string SC_Game         = "Game";
    public const string SC_Login        = "Login";

    /// <summary>
    /// 当前游戏场景
    /// </summary>
    private SCENE mCurrentScene;
    public SCENE CurrentScene
    {
        get
        {
            return mCurrentScene;
        }
    }

    public void SetGameScene(SCENE scene)
    {
        switch(scene)
        {
            case SCENE.SCENE_LOADING:
                Application.LoadLevelAsync(SC_Loading);
                break;
        }
        
        
        
        mCurrentScene = scene;
    }
	
}
