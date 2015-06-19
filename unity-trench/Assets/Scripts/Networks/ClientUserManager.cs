using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngineEx.Common;
using UnityEngineEx.LogInterface;
using UnityEngineEx.CMD.i3778;

//////////////////////////////////////////////////////////////////////////
using WORD = System.UInt16;
using DWORD = System.UInt32;
using LONG = System.Int32;
using LONGLONG = System.Int64;
using ULONGLONG = System.UInt64;
using BYTE = System.Byte;
//////////////////////////////////////////////////////////////////////////
using tagUserData = UnityEngineEx.CMD.i3778.GlobalDef.tagUserData;
using tagUserScore = UnityEngineEx.CMD.i3778.GlobalDef.tagUserScore;
using tagUserStatus = UnityEngineEx.CMD.i3778.GlobalDef.tagUserStatus;

/////////////////////////////////////////////////////////////////////////
using CUserItemPtrArray = System.Collections.Generic.List<CUserItem>;

//用户信息类
public class CUserItem : IUserItem
{
    //变量定义
    public tagUserData m_UserData;						//用户信息
    public tagUserScore m_UserScore;					//用户积分

    //内部变量
    public bool m_bActive;						//激活有效

    //函数定义积分
    //构造函数
    public CUserItem()
    {
        CleanData();
    }

    //对象接口
    //访问判断
    public bool IsActive() { return m_bActive; }

    //设置Face
    public void SetFaceId(WORD wFaceId)
    {
        m_UserData.wFaceID = wFaceId;
    }

    //设置Z币
    public void SetZScore(DWORD dwZScore)
    {
        m_UserData.dwZScore = dwZScore;
    }

    //属性接口
    //游戏局数
    public LONG GetUserPlayCount()
    {
        return m_UserData.lWinCount + m_UserData.lLostCount + m_UserData.lDrawCount + m_UserData.lFleeCount;
    }
    //用户 I D
    public DWORD GetUserID() { return m_UserData.dwUserID; }
    //用户 I D
    public DWORD GetGameID() { return m_UserData.dwGameID; }
    //用户名字
    public string GetUserName() { return m_UserData.szName.ToAnsiString(); }
    //获取用户
    public tagUserData GetUserData() { return m_UserData; }
    //获取积分
    public tagUserScore GetUserScore() { return m_UserScore; }

    //设置接口
    //设置积分
    public void SetUserScore(tagUserScore pUserScore)
    {
        //效验参数
        if (m_bActive == true)
        {
            Debuger.Instance.LogError("用户未激活");
            return;
        }

        //设置变量
        m_UserData.lScore = pUserScore.lScore;
        m_UserData.llInsureScore = pUserScore.llInsureScore;
        m_UserData.lWinCount = pUserScore.lWinCount;
        m_UserData.lLostCount = pUserScore.lLostCount;
        m_UserData.lDrawCount = pUserScore.lDrawCount;
        m_UserData.lFleeCount = pUserScore.lFleeCount;
        m_UserData.lExperience = pUserScore.lExperience;
        m_UserData.dwLotteries = pUserScore.dwLotteries;

        m_UserScore = pUserScore;
    }
    //设置状态
    public void SetUserStatus(tagUserStatus pUserStatus)
    {
        //效验参数
        if (m_bActive == true)
        {
            Debuger.Instance.LogError("用户未激活");
            return;
        }

        //设置变量
        m_UserData.wTableID = pUserStatus.wTableID;
        m_UserData.wChairID = pUserStatus.wChairID;
        m_UserData.cbUserStatus = pUserStatus.cbUserStatus;
    }
    //设置金币
    public bool SetUserGold(DWORD dwGoldCount)
    {
        //效验参数
        if (m_bActive == true)
        {
            Debuger.Instance.LogError("用户未激活");
            return false;
        }

        //设置变量
        m_UserData.dwGoldCount = dwGoldCount;

        return true;
    }
    //设置至尊豆
    public bool SetUserZZBean(DWORD dwZZBean)
    {
        //效验参数
        if (m_bActive == true)
        {
            Debuger.Instance.LogError("用户未激活");
            return false;
        }

        //设置变量
        m_UserData.dwZZBean = dwZZBean;

        return true;
    }
    //设置银行存款
    public bool SetUserInsureScore(LONGLONG llInsureScore)
    {
        //效验参数
        if (m_bActive == true)
        {
            Debuger.Instance.LogError("用户未激活");
            return false;
        }

        //设置变量
        m_UserData.llInsureScore = llInsureScore;

        return true;
    }

    //添加权限
    public void AddUserRight(DWORD dwRight)
    {
        m_UserData.dwUserRight |= dwRight;
    }

    //清理数据
    public void CleanData()
    {
        m_bActive = false;
        m_UserData.wTableID = (WORD)GlobalDef.Deinfe.INVALID_TABLE;
        m_UserData.wChairID = (WORD)GlobalDef.Deinfe.INVALID_CHAIR;
    }
}

//////////////////////////////////////////////////////////////////////////

//用户管理类
class CClientUserManager : IClientUserManager
{
    //变量定义
    protected CUserItemPtrArray m_UserItemActive;					//活动数组
    protected CUserItemPtrArray m_UserItemStorage;					//存储数组

    //外部接口
    protected IUserManagerSink m_pIUserManagerSink;				//回调接口

    //函数定义
    //构造函数
    public CClientUserManager()
    {
        m_pIUserManagerSink = null;
    }

    //重置数据
    public void ReSetUserData()
    {
        m_UserItemActive.Clear();
        m_UserItemStorage.Clear();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////
    //管理接口
    //设置回调接口
    public bool SetUserManagerSink(IUserManagerSink pIUnknownEx)
    {
        m_pIUserManagerSink = pIUnknownEx;
        return (m_pIUserManagerSink != null);
    }
    //增加用户
    public IUserItem ActiveUserItem(tagUserData UserData)
    {
        //寻找用户
        CUserItem pUserItem = null;
        int nStorageCount = m_UserItemStorage.Count;
        if (nStorageCount > 0)
        {
            pUserItem = m_UserItemStorage[nStorageCount - 1];
            m_UserItemStorage.RemoveAt(nStorageCount - 1);
        }
        else
        {
            pUserItem = new CUserItem();
            if (pUserItem == null) return null;
        }

        //拷贝数据
        pUserItem.m_bActive = true;
        pUserItem.m_UserData = UserData;

        //设置变量
        tagUserScore UserScore = new tagUserScore();
        UserScore.llInsureScore = UserData.llInsureScore;
        UserScore.dwGoldCount = UserData.dwGoldCount;
        UserScore.dwZZBean = UserData.dwZZBean;
        UserScore.lScore = UserData.lScore;
        UserScore.lWinCount = UserData.lWinCount;
        UserScore.lLostCount = UserData.lLostCount;
        UserScore.lDrawCount = UserData.lDrawCount;
        UserScore.lFleeCount = UserData.lFleeCount;
        UserScore.lExperience = UserData.lExperience;
        UserScore.dwLotteries = UserData.dwLotteries;
        UserScore.dwZScore = UserData.dwZScore;

        pUserItem.SetUserScore(UserScore);

        m_UserItemActive.Add(pUserItem);

        //更新信息
        if (m_pIUserManagerSink == null)
        {
            Debuger.Instance.LogError("设置用户管理回调接口");
            return pUserItem;
        }
        m_pIUserManagerSink.OnUserItemAcitve(pUserItem);

        return pUserItem;
    }
    //删除用户
    public bool DeleteUserItem(IUserItem pIUserItem)
    {
        //查找用户
        CUserItem pUserItemActive = null;
        for (int i = 0; i < m_UserItemActive.Count; i++)
        {
            pUserItemActive = m_UserItemActive[i];
            if (pIUserItem == pUserItemActive)
            {
                //删除用户
                m_UserItemActive.RemoveAt(i);
                m_UserItemStorage.Add(pUserItemActive);
                m_pIUserManagerSink.OnUserItemDelete(pUserItemActive);
                pUserItemActive.CleanData();
                return true;
            }
        }
        Debuger.Instance.LogError("错误了");

        return false;
    }
    //更新积分
    public bool UpdateUserItemScore(IUserItem pIUserItem, tagUserScore pUserScore)
    {
        //效验参数

        //设置数据
        pIUserItem.SetUserScore(pUserScore);

        //通知更新
        m_pIUserManagerSink.OnUserItemUpdate(pIUserItem);

        return true;
    }
    //更新状态
    public bool UpdateUserItemStatus(IUserItem pIUserItem, tagUserStatus pUserStatus)
    {
        //设置数据
        pIUserItem.SetUserStatus(pUserStatus);

        //通知更新
        m_pIUserManagerSink.OnUserItemUpdate(pIUserItem);

        return true;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////
    //信息接口
    //获取人数
    public DWORD GetOnLineCount() { return (DWORD)m_UserItemActive.Count; }

    /////////////////////////////////////////////////////////////////////////////////////////////////
    //查找接口
    //枚举用户
    public IUserItem EnumUserItem(WORD wEnumIndex)
    {
        if (wEnumIndex < m_UserItemActive.Count)
        {
            CUserItem pUserItem = m_UserItemActive[wEnumIndex];
            if (pUserItem.m_bActive == false)
            {
                Debuger.Instance.LogError("用户未激活!");
            }
            return pUserItem;
        }
        return null;
    }
    //查找用户
    public IUserItem SearchUserByUserID(DWORD dwUserID)
    {
        CUserItem pUserItem = null;
        for (int i = 0; i < m_UserItemActive.Count; i++)
        {
            pUserItem = m_UserItemActive[i];
            if (pUserItem.m_bActive == false)
            {
                Debuger.Instance.LogError("用户未激活!");                
            }
            if (pUserItem.m_UserData.dwUserID == dwUserID) return pUserItem;
        }
        return null;
    }
    //查找用户
    public IUserItem SearchUserByGameID(DWORD dwGameID)
    {
        CUserItem pUserItem = null;
        for (int i = 0; i < m_UserItemActive.Count; i++)
        {
            pUserItem = m_UserItemActive[i];
            if (pUserItem.m_bActive == false)
            {
                Debuger.Instance.LogError("用户未激活!");
            }
            if (pUserItem.m_UserData.dwGameID == dwGameID) return pUserItem;
        }
        return null;
    }
}

//////////////////////////////////////////////////////////////////////////
