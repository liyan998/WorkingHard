using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LobbyState
{
    Dialog,//left
    Arena,//right
    Map,//up
    Shop,//down
    MainMenu,//Center
    Game,//HideAll
}

public class LobbyController : MonoBehaviour
{
    public static LobbyController inst;
    [SerializeField]
    LobbyBar
        topBar;
    [SerializeField]
    LobbyBar
        bottomBar;
    [SerializeField]
    ArenaUI
        arena;
//    [SerializeField]
//    MoveTweener
//        backBtn;
//  [SerializeField]
//  Mover
//      bkg;
    [Tooltip("0:final phase,1:arena,2:map,3:shop,4:main,5:Game")]
    [SerializeField]
    MoveTweener[]
        stateBtns;
    LobbyState lastState = LobbyState.MainMenu;
    public List<LobbyState> backStates = new List<LobbyState>();
    public LobbyState currentState = LobbyState.MainMenu;
    [SerializeField]
    MoveTweener
        curtain;
    [SerializeField]
    LobbyData
        data;
    UserHud hud;
    [SerializeField]
    Shop
        shop;
    [SerializeField]
    MainMenu
        mainMenu;
    LobbyDialogManager dialogs;

    void Awake()
    {
        inst = this;
    }

    void Start()
    {
        dialogs = LobbyDialogManager.inst;
        hud = UserHud.inst;
        Init();
        curtain.FromTarget();
        SOUND.Instance.PlayBGM(Bgm.inst.lobby);
    }

    public void Init()
    {
        data.Init();

        hud.Init();

        shop.Init();
        Debug.Log(data.navigation.LobbyWin);
        if (data.navigation.LobbyWin != LobbyState.MainMenu)
        {
            mainMenu.gameObject.SetActive(false);
            SwitchState(data.navigation.LobbyWin);
        } else
        {
            if (!data.sign.isTodaySigned)
            {
                dialogs.ShowDialog(LobbyDialog.DailySign);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //dialogs.HideCurrentDialog();
            dialogs.ShowConfirmDialog(TextManager.Get("quitTip"), Quit);
//            dialogs.ShowConfirmDialog(TextManager.Get("quitTip"), Quit, null, true, delegate
//            {
//                dialogs.ShowLastDialog();
//            });
        }
    }

    void Quit()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void BackToLastState()
    {
        //Debug.Log(backStates.Count);
        if (backStates.Count > 0)
        {
            SwitchState(backStates [backStates.Count - 1]);
//            if (backStates.Count > 0)
//            {
//                backStates.Remove(backStates [backStates.Count - 1]);
//            }
        } else
        {
            SwitchState(LobbyState.MainMenu);
        }
    }

    public void SwitchState(LobbyState lobbyState)
    {
        //Debug.Log(lobbyState);
        SwitchState((int)lobbyState);
    }

    public void SwitchState(int lobbyState)
    {
        Debug.Log("SwitchState " + (LobbyState)lobbyState);
        currentState = (LobbyState)lobbyState;
        //SOUND.Instance.OneShotSound(Sfx.inst.btn);
        //Debug.Log(lastState+""+lobbyState);
        if (lastState != (LobbyState)lobbyState)
        {
            mainMenu.enabled = false;
            if (backStates.Contains(currentState))
            {
                backStates.Remove(currentState);
            }
            if ((LobbyState)lobbyState != LobbyState.Game)
            {
                //bkg.MoveTo ((Direction)lobbyState);
                stateBtns [lobbyState].gameObject.SetActive(true);
                if ((LobbyState)lobbyState == LobbyState.MainMenu)
                {
                    hud.ToggleBackBtn(false);
                    stateBtns [lobbyState].FromTarget(delegate
                    {
                        mainMenu.ToPoint();
                        mainMenu.enabled = true;
                    });
                } else
                {
                    stateBtns [lobbyState].FromTarget(delegate()
                    {
                        mainMenu.gameObject.SetActive(false);
                        hud.ToggleBackBtn(true);
                    });
                }
                //Debug.Log(lastState);
                GameObject lastBtns = stateBtns [(int)lastState].gameObject;
                stateBtns [(int)lastState].ToTarget(delegate()
                {
                    lastBtns.SetActive(false);
                });
            } else
            {
                hud.ToggleBackBtn(false);
            }
            switch ((LobbyState)lobbyState)
            {
                case LobbyState.Dialog:
                    //backBtn.gameObject.SetActive(true);
                    //backBtn.FromTarget();
                    //backBtn.ToTarget();

                    bottomBar.HideBtns();
                    //data.navigation.LobbyWin = LobbyState.Dialog;
                    if (lastState != LobbyState.Dialog)
                    {
                        backStates.Add(lastState);
                    }
                    break;
                case LobbyState.Arena:
//                    backBtn.gameObject.SetActive(true);
//                    backBtn.FromTarget();
                    //hud.ToggleBackBtn(true);
                    bottomBar.HideBtns();
                    data.navigation.LobbyWin = LobbyState.Arena;
                    if (lastState != LobbyState.Dialog)
                    {
                        backStates.Add(lastState);
                    }
                    dialogs.isGoToShop = false;
                    break;
                case LobbyState.Map:
//                    backBtn.gameObject.SetActive(true);
//                    backBtn.FromTarget();
                    //hud.ToggleBackBtn(true);
                    bottomBar.HideBtns();
                    data.navigation.LobbyWin = LobbyState.Map;
                    if (lastState != LobbyState.Dialog)
                    {
                        backStates.Add(lastState);
                    }
                    dialogs.isGoToShop = false;
                    break;
                case LobbyState.Shop:
//                    backBtn.gameObject.SetActive(true);
//                    backBtn.FromTarget();
                    //hud.ToggleBackBtn(true);
                    bottomBar.HideBtns();
                    data.navigation.LobbyWin = LobbyState.Shop;
                    if (lastState != LobbyState.Shop && lastState != LobbyState.Dialog)
                    {
                        backStates.Add(lastState);
                    }
                    break;
                case LobbyState.MainMenu:
//                    backBtn.ToTarget();
                    //hud.ToggleBackBtn(false);
                    bottomBar.ShowBtns();
                    data.navigation.LobbyWin = LobbyState.MainMenu;
//                    mainMenu.enabled = true;
//                    mainMenu.ToPoint();
                    dialogs.isGoToShop = false;
                    break;
                case LobbyState.Game:
//                    backBtn.ToTarget();
                    //hud.ToggleBackBtn(false);
                    topBar.HideBtns();
                    bottomBar.HideBtns();
                    curtain.ToTarget(arena.LoadPlayScene);
                    break;
            }
            lastState = (LobbyState)lobbyState;
            SOUND.Instance.OneShotSound(Sfx.inst.hide);
        }
    }

}
