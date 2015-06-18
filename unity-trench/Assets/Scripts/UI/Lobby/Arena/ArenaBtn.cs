using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ArenaBtn : MonoBehaviour
{
    [SerializeField]
    Text
        modeName;
    [SerializeField]
    Image
        modeIcon;
    [SerializeField]
    Image
        roomName;
    [SerializeField]
    Text
        points;
    [SerializeField]
    Text
        range;
    [SerializeField]
    Button
        btn;
    INFO_ROOM info;
    Action<INFO_ROOM,ArenaBtn> onBtnClick;

    public void Init(string mode, Sprite room, Sprite icon, Color color, INFO_ROOM roomInfo, Action<INFO_ROOM,ArenaBtn> onBtn)
    {

        info = roomInfo;

        modeName.text = mode;
        modeIcon.sprite = icon;
        roomName.sprite = room;
        points.text = TextManager.Get("BasePoints:") + roomInfo.lCellScore.ToString();
        if(roomInfo.dwMaxBonus>10000000){
            range.text=TextManager.Get("MoreThan")+roomInfo.dwMinBonus.ToString().Replace("0000",TextManager.Get("TenThousand"))+TextManager.Get("Coin");
        }else{
            range.text=roomInfo.dwMinBonus.ToString().Replace("0000",TextManager.Get("TenThousand"))+"~"+roomInfo.dwMaxBonus.ToString().Replace("0000",TextManager.Get("TenThousand"))+ TextManager.Get("Coin");
        }
        points.color = color;
        points.GetComponent<Outline>().effectColor = color * 0.5f;
        range.color = color;
        range.GetComponent<Outline>().effectColor = color * 0.5f;
        onBtnClick = onBtn;
    }

    public void OnBtnClick()
    {
        if (onBtnClick != null)
        {
            onBtnClick(info, this);
        }
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void ToggleButton(bool isActive)
    {
        btn.interactable = isActive;
    }

    public INFO_ROOM GetRoomData()
    {
        return info;
    }
}
