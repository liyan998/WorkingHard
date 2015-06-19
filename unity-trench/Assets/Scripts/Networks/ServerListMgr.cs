using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngineEx.Common;
using UnityEngineEx.CMD.i3778;

using WORD = System.UInt16;
using DWORD = System.UInt32;
using LONG = System.Int32;
using LONGLONG = System.Int64;
using ULONGLONG = System.UInt64;
using BYTE = System.Byte;

using tagGameType = UnityEngineEx.CMD.i3778.GlobalDef.tagGameType;
using tagGameKind = UnityEngineEx.CMD.i3778.GlobalDef.tagGameKind;
using tagGameProcess = UnityEngineEx.CMD.i3778.GlobalDef.tagGameProcess;
using tagGameStation = UnityEngineEx.CMD.i3778.GlobalDef.tagGameStation;
using tagGameServer = UnityEngineEx.CMD.i3778.GlobalDef.tagGameServer;

public class ServerListMgr 
{
    //游戏分类枚举
    public enum emGameType
    {
        GameType_Hot = 0x0001,              //热门游戏
        GameType_Card = 0x0002,             //牌类
        GameType_MJ = 0x0004,               //麻将
        GameType_Chess = 0x0008,            //棋类
        GameType_Casual = 0x0010,           //休闲
        GameType_HiLad = 0x0020,            //天梯
        GameType_Match = 0x0040,            //竞技
        GameType_Count = 7,                 //个数
    };
    //游戏种类
    List<tagGameType> mGameType;
    //游戏类型
    List<tagGameKind> mGameKind;
    //游戏房间(LO-KEY==游戏类型 HI-KEY==游戏种类)
    List<tagGameServer> mGameServer;
    //游戏进程
    List<tagGameProcess> mGameProcess;

    //目前只有一个挖坑所以需要定义出来KindID
    WORD mCurKindID = 291;
    //serverid>=1000炸弹场 其他事普通场
    WORD mServerIdFlag = 1000;
    public ServerListMgr()
    {
        mGameType = new List<tagGameType>();
        mGameKind = new List<tagGameKind>();
        mGameServer = new List<tagGameServer>();
        mGameProcess = new List<tagGameProcess>();
    }


    public void AddGameType(tagGameType v)
    {        
        mGameType.Add(v);
    }

    public void AddGameKind(tagGameKind v)
    {
        mGameKind.Add (v);
    }

    public void AddGameServer(tagGameServer v)
    {        
        mGameServer.Add(v);
        SortGameServer();
    }

    public void AddGameProcess(tagGameProcess v)
    {        
        mGameProcess.Add(v);
    }

    void SortGameServer()
    {
        mGameServer.Sort((a, b) => { return a.wSortID.CompareTo(b.wSortID); });
    }

    /// <summary>
    /// 获取单个房间服务器
    /// </summary>
    /// <param name="wServerId"></param>
    /// <returns></returns>
    public tagGameServer GetGameServer(WORD wServerId)
    {
        return mGameServer.Find(a => a.wServerID == wServerId);
    }

    /// <summary>
    /// 获取所有房间服务器列表
    /// </summary>
    /// <returns></returns>
    public List<tagGameServer> GetGameServer()
    {
        return mGameServer.ToList();
    }

    /// <summary>
    /// 获取普通场房间   (挖坑专用)
    /// </summary>
    /// <returns></returns>
    public List<INFO_ROOM> GetTrenchNormalGameServer()
    {
        //List<tagGameServer> res = new List<tagGameServer>();
        List<INFO_ROOM> res = new List<INFO_ROOM>();
        foreach (var e in mGameServer)
        {
            if (mCurKindID == e.wKindID && e.wServerID < mServerIdFlag)
            {
                INFO_ROOM temp = new INFO_ROOM();
                temp.FromServer(e);
                res.Add(temp);
            } 

        }
        return res;
    }

    /// <summary>
    /// 获取炸弹场房间  (挖坑专用)
    /// </summary>
    /// <returns></returns>
    public List<INFO_ROOM> GetTrenchBombGameServer()
    {
        //List<tagGameServer> res = new List<tagGameServer>();
        List<INFO_ROOM> res = new List<INFO_ROOM>();
        foreach (var e in mGameServer)
        {
            if (mCurKindID == e.wKindID && e.wServerID >= mServerIdFlag)
            {
                INFO_ROOM temp = new INFO_ROOM();
                temp.FromServer(e);
                res.Add(temp);
            } 

        }
        return res;
    }

    public void UpdateGameServerOnLine(GlobalDef.tagGameServer srv,DWORD dwOnLineCnt)
    {
        for (int i = 0; i < mGameServer.Count;++i )
        {
            if(mGameServer[i].wServerID == srv.wServerID)
            {
                var v = mGameServer[i];
                v.dwOnLineCount = dwOnLineCnt;
                mGameServer[i] = v;
            }
        }
    }

    public void UpdateGameServerOnLine(WORD wKindId, DWORD dwOnLineCnt)
    {
        for (int i = 0; i < mGameServer.Count; ++i)
        {
            if (mGameServer[i].wServerID == wKindId)
            {
                var v = mGameServer[i];
                v.dwOnLineCount = dwOnLineCnt;
                mGameServer[i] = v;
            }
        }
    }

}
