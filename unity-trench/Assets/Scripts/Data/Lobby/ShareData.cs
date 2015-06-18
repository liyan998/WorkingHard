using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using cn.sharesdk.unity3d;

public class ShareData : MonoBehaviour
{
    public List<INFO_SHARE> shareInfo = new List<INFO_SHARE>();
    //public bool[] isTodayShared = new bool[true, true, true];
    public int[] todaySharedStates;//value: 0:no share no reward yet,1:shared,no reward yet,2:shared rewarded
    public void Init()
    {
        shareInfo = DataBase.Instance.SHARE_MGR.GetAllShareData();
        todaySharedStates = new int[3]; 
        string[] sharedStates = PlayerPrefs.GetString("ShareStates", "0,0,0").Split(',');
        todaySharedStates [0] = int.Parse(sharedStates [0]);
        todaySharedStates [1] = int.Parse(sharedStates [1]);
        todaySharedStates [2] = int.Parse(sharedStates [2]);
    }

    public INFO_SHARE GetInfo(PlatformType platform, LobbyState state)
    {
    
        if (platform == PlatformType.SinaWeibo)
        {
            if (state == LobbyState.Map)
            {
                return shareInfo [4];
            } else
            {
                return shareInfo [5];
            }
        } else if (platform == PlatformType.WeChatTimeline)
        {
            if (state == LobbyState.Map)
            {
                return shareInfo [0];
            } else
            {
                return shareInfo [1];
            }
        } else if (platform == PlatformType.QZone)
        {
            if (state == LobbyState.Map)
            {
                return shareInfo [2];
            } else
            {
                return shareInfo [3];
            }
        } else
        {
            return null;
        }
    }

    public INFO_SHARE GetInfo(int platform,LobbyState state)
    {
        if (platform == 2)
        {
            if (state == LobbyState.Map)
            {
                return shareInfo [4];
            } else
            {
                return shareInfo [5];
            }
        } else if (platform == 0)
        {
            if (state == LobbyState.Map)
            {
                return shareInfo [0];
            } else
            {
                return shareInfo [1];
            }
        } else if (platform == 1)
        {
            if (state == LobbyState.Map)
            {
                return shareInfo [2];
            } else
            {
                return shareInfo [3];
            }
        } else
        {
            return null;
        }
    }

}
