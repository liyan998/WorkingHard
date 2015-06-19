using UnityEngine;
using System.Collections;

/// <summary>
/// 逻辑场景
/// </summary>
public class GameScene
{
    /// <summary>
    /// 游戏场景枚举
    /// </summary>
    public enum SCENE
    {
        SCENE_LOGO,
        /// <summary>
        /// Loading
        /// </summary>
        SCENE_LOADING,        
        /// <summary>
        /// 单机大厅
        /// </summary>
        SCENE_STANDLONE_HALL,
        /// <summary>
        /// 网络版大厅
        /// </summary>
        SCENE_NETSCENE_HALL,
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

    /// <summary>
    /// 场景切换
    /// </summary>
    /// <param name="scene"></param>
    public void SetGameScene(SCENE scene)
    {
        switch(scene)
        {
            case SCENE.SCENE_LOADING:
                Application.LoadLevelAsync(SC_Loading);
                break;
            case SCENE.SCENE_STANDLONE_HALL:
                Application.LoadLevelAsync(SC_Hall);
                break;
        }        
        mCurrentScene = scene;
    }
	
}
