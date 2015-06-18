using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ArenaUI : MonoBehaviour
{
//  [SerializeField]
//  Image
//      title;
//  [SerializeField]
//  Sprite
//      normal;
//  [SerializeField]
//  Sprite
//      bombable;
    //[Header("freshman,advanced,master,expert")]
    [SerializeField]
    ArenaBtn
        betLevelBtn;
//  [SerializeField]
//  List<ArenaBtn>
//      betLevelBtns;
//  [SerializeField]
//  Text[]
//      betLevels;
    [SerializeField]
    ArenaData
        arena;
    [SerializeField]
    GameData
        data;
    [SerializeField]
    UserData
        user;
    [SerializeField]
    WindowData
        navi;
    [SerializeField]
    RectTransform
        scrollViewContent;
    bool isBombEnabled = false;
    //int betLevelBtnIndex = -1;
    List<ArenaBtn> btns = new List<ArenaBtn>();
    LobbyDialogManager dialogs;
    [SerializeField]
    UnityEvent
        onPlayGame;
    int maxGiftNum = 3;

    void Start()
    {
        dialogs = LobbyDialogManager.inst;
        if (navi.RoomType == GameHelper.STAGE_NORMAL)
        {
            SetBombable(false);
        } else if (navi.RoomType == GameHelper.STAGE_BOM)
        {
            SetBombable(true);
        }
    }

    public void SetBombable(bool isBombable)
    {
        //Debug.Log (isBombEnabled.ToString () + " " + btns.Count);
        if (isBombEnabled != isBombable || btns.Count == 0)
        {
            foreach (ArenaBtn btn in btns)
            {
                Destroy(btn.gameObject);
            }
            btns.Clear();
            isBombEnabled = isBombable;
            List<INFO_ROOM> info = new List<INFO_ROOM>();
            if (isBombEnabled)
            {
                //title.sprite = bombable;
                info = data.bombRoomInfo;
            } else
            {
                //title.sprite = normal;
                info = data.normalRoomInfo;
            }
            //Debug.Log(data.normalRoomInfo);
            foreach (INFO_ROOM room in info)
            {
                ArenaBtn btn = Instantiate(betLevelBtn) as ArenaBtn;
                btn.transform.SetParent(scrollViewContent.transform);
                btn.gameObject.SetActive(true);
                Debug.Log("isBombable " + isBombable + " " + room.wServerID);

                ArenaRoom roomName = isBombable ? (ArenaRoom)(room.wServerID - 999) : (ArenaRoom)room.wServerID;
                if (roomName == ArenaRoom.Master)
                {
                    btn.gameObject.SetActive(false);
                } else
                {
                    btn.Init(isBombable ? TextManager.Get("BombMode") : TextManager.Get("NormalMode"),
                         arena.GetRoom(roomName),
                         isBombable ? arena.GetIcon(CardType.Spade) : arena.GetIcon(CardType.Club),
                         arena.GetColor(roomName),
                          room, SelectRoom);
                }
                btns.Add(btn);
                //Debug.Log(btn.GetRoomData ());
//              if (user.coin <= btn.GetRoomData ().dwMinBonus || user.coin >= btn.GetRoomData ().dwMaxBonus) {
//                  btn.ToggleButton (false);
//              }
            }
        }
    }

    INFO_ROOM room;

    void SelectRoom(INFO_ROOM roomData, ArenaBtn btn)
    {
        if (user.coin < roomData.dwMinBonus)
        {
            //judge is low coin gift gone
            var database = DataBase.Instance;
            int getLowCoinGiftRest = database.PLAYER.GetLowCoinGiftRest;
            //Debug.Log(roomData.bRelief);
            if (roomData.bRelief == true && getLowCoinGiftRest > 0)
            {
                dialogs.ShowConfirmDialog(
                    string.Format(TextManager.Get("lowCoinGiftTip"),
                    (COMMON_CONST.MaxGiftNum - getLowCoinGiftRest + 1).ToString()),
                    OnLowCoinGiftGet, 
                    null, false, null, null, TextManager.Get("lowCoinGiftTitle")
                 );
            } else
            {
                dialogs.ShowLowCoinDialog(GoodType.Coin, string.Format(TextManager.Get("lowCoinRoomTip"), roomData.dwMinBonus));
            }
        } else if (user.coin >= roomData.dwMaxBonus)
        {
            dialogs.ShowConfirmDialog(TextManager.Get("highCoinRoomTip"), null, null, false,null,null,TextManager.Get("highCoinTitle"));
        } else
        {
            room = roomData;
            navi.LobbyWin = LobbyState.MainMenu;
            onPlayGame.Invoke();
            SetBetLevel(btn);
        }
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    void SetBetLevel(ArenaBtn selectedBtn)
    {
        foreach (ArenaBtn btn in btns)
        {
            btn.ToggleButton(true);
        }
        selectedBtn.ToggleButton(false);
    }

    public void LoadPlayScene()
    {
        //DataBase.Instance.SCENE_MGR.SetScene ("Game");
        data.SelectRoom(room);
    }

    void OnLowCoinGiftGet()
    {
        user.coin += 2000;
        user.SaveMoney();
        UserHud.inst.RefreshMoney();
        int getLowCoinGiftRest = PlayerPrefs.GetInt("LowCoinGift", maxGiftNum);
        getLowCoinGiftRest--;
        PlayerPrefs.SetInt("LowCoinGift", getLowCoinGiftRest);
        SOUND.Instance.OneShotSound(Sfx.inst.reward);
    }

    public void QuickStart()
    {
        ArenaBtn targetRoom = null;
        foreach (ArenaBtn btn in btns)
        {
            Debug.Log(btn.GetRoomData());
            if (btn.GetRoomData() != null)
            {
                if (user.coin >= btn.GetRoomData().dwMinBonus || (btn.GetRoomData().bRelief && PlayerPrefs.GetInt("LowCoinGift", maxGiftNum)>0))
                {
                    targetRoom = btn;
                }
            }
        }
        DataBase.Instance.IsJustStartPlay = true;
        if (targetRoom != null)
        {
            SelectRoom(targetRoom.GetRoomData(), targetRoom);
        } else
        {
            SelectRoom(btns [0].GetRoomData(), btns [0]);
            Debug.Log("No target room!");
        }
    }

}
