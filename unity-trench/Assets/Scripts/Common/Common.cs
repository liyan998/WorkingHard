﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineEx.Net;
using UnityEngineEx.CMD.i3778;
using UnityEngineEx.LogInterface;
using System.Collections;


/// <summary>
/// 全局常量定义集合
/// </summary>
public class COMMON_CONST
{
    //版本号
    public const string Version = "v2.3.15";
    public const int versionCode = 1;
    //渠道编号
    /// <summary>
    /// 0-官方渠道
    /// </summary>
    public const byte MarketId = 0;

    //本地数据Key
    public const string NullString = "Null";
    public const string UserBaseInfo = "UserBaseInfo";
    public const string UserInfo = "UserInfo";
    public const string UserStageRecord = "UserStageRecord";
	public const string UserSignInfo = "UserSignInfo";

	public const string TitleInfo = "TitleInfo";

     //组件名称
    public const string database_name = "DataBase";                 //数据库组件名称

    //加密数据key
    public const string DataStorageKey = "Gamezz_!@i3778*^%$";       //数据存储加密

    //错误消息
    public const string ErrCommon = "常规错误!";
    public const string ErrUserStaus = "用户状态错误!";
    public const string ErrChair="座位号错误!";
    public const string ErrGameStatus = "游戏状态错误!";
    public const string ErrCurChair="当前玩家操作非法!";                 
    public const string ErrCallScore = "叫分状态错误!";
    public const string ErrOutCard = "出牌错误!";
    public const string ErrPassCard = "不出错误!";  

    //路径相关
    public const string PathConfigData = "Data/Config/";
    public const string PathStageInfo = PathConfigData + "StageInfo";
    public const string PathStageCondition = PathConfigData + "StageCondition";
    public const string PathStageConditionInfo = PathConfigData + "StageConditionInfo";
    public const string PathRoomConfig = PathConfigData + "RoomConfig";
    public const string PathProperty = PathConfigData + "Property";
	public const string PathSign = PathConfigData + "ContinuousSignIn";
    public const string PathStageLotteryData = PathConfigData + "StageLottery";
	public const string PathTitleInfo = PathConfigData + "TitleInfo";
    public const string PathRewardInfo = PathConfigData + "RewardInfo";
    public const string PathTaskInfo = PathConfigData + "TaskConfig";
    public const string PathShare = PathConfigData + "Participation";

    public const string PathStagePoint = PathConfigData + "StagePoint";


    //角色定义
    public const int RoleLand = 1;      //地主
    public const int RoleNoLand = 0;    //平民

    //关卡相关
    public const int StageFirst = 1;    //第一关
    public const int StageLast = 50;    //最后一关

    //房间相关
    public const int RoomNormal = 0;     //传统房间
    public const int RoomStage = 1;     //关卡房间
 
    //货币相关
    public const int PriceGold = 0; //银币、金币
    public const int PriceDiamonds = 1;//钻石
    public const int PriceCash = 2;//现金

    //低保
    public const int MaxGiftNum = 3; //低保领取次数
    public const int MaxGiftScore = 2000; //低保

    
 
    //屏幕相关
    public static float CameraWidthRatio = 1.0f;
    public static float CameraHeightRatio = 1.0f;
    public static float CameraHeight
    #if UNITY_ANDROID
                                            = 480.0f;
    #elif UNITY_IPHONE
											= 640.0f;
    #else
                                            = 480.0f;
    #endif
    public static float CameraWidth
    #if UNITY_ANDROID
                                            = 800.0f;
    #elif UNITY_IPHONE
											= 960.0f;
    #else
                                            = 800.0f;
    #endif


}

public class COMMON_DEF
{
    //程序版本（单机，网络）
    public enum enVerDef{ Offline,Online, }
}

/// <summary>
/// 全局功能函数集合
/// </summary>
public class COMMON_FUNC
{
    #region 基础函数
    public static UInt16 MAKEWORD(byte l, byte h)
    {
        return (UInt16)(l | ((UInt16)h) << 8);
    }
    public static UInt32 MAKELONG(UInt16 a, UInt16 b)
    {
        return (UInt32)(((a) & 0xffff) | ((b) << 16));
    }

    public static UInt64 MAKELONG8(UInt32 a, UInt32 b)
    {
        return (UInt64)(((a) & 0xffffffff) | ((b) << 32));
    }

    public static byte LOBYTE(UInt16 w)
    {
        return (byte)(w & 0xff);
    }

    public static byte HIBYTE(UInt16 w)
    {
        return (byte)((w >> 8) & 0xff);
    }

    public static UInt16 LOWORD(UInt32 w)
    {
        return (UInt16)(w & 0xffff);
    }

    public static UInt16 HIWORD(UInt32 w)
    {
        return (UInt16)((w >> 16) & 0xffff);
    }

    public static UInt32 LODWORD(UInt64 w)
    {
        return (UInt32)(w & 0xffffffff);
    }

    public static UInt32 HIDWORD(UInt64 w)
    {
        return (UInt32)((w >> 32) & 0xffffffff);
    }
    #endregion
    #region 设备信息
    public static void SetCameraWidthHeight(float fWidthRatio, float fHeightRatio)
    {
        COMMON_CONST.CameraWidthRatio = fWidthRatio;
        COMMON_CONST.CameraHeightRatio = fHeightRatio;
    }

    public static float GetCamearaRealWidth() { return COMMON_CONST.CameraWidthRatio * Screen.width; }
    public static float GetCamearaRealHeight() { return COMMON_CONST.CameraHeightRatio * Screen.height; }
    
    #endregion

    #region 加密
    static string AESEncrypt(string data)
    {
        string encrypt = data.ToAESEncrypt(COMMON_CONST.DataStorageKey);
        return encrypt;
    }

     static string AESDecrypt(string data)
    {
        string decrypt = data.ToAESDecrypt(COMMON_CONST.DataStorageKey);
        return decrypt;
    }
    #endregion

    #region  设置本地存储
     ///  设置本地存储
    public static void SetUserBaseInfo( string value)
    {
        value = AESEncrypt(value);
        PlayerPrefs.SetString(COMMON_CONST.UserBaseInfo, value);
    }

    public static string GetUserBaseInfo()
    {
        string value = PlayerPrefs.GetString(COMMON_CONST.UserBaseInfo, COMMON_CONST.NullString);
        if(value != COMMON_CONST.NullString) value = AESDecrypt(value);
        return value;
    }
    public static void SetUserInfo(string value)
    {
        value = AESEncrypt(value);
        PlayerPrefs.SetString(COMMON_CONST.UserInfo, value);
        PlayerPrefs.Save();
    }
    public static string GetUserInfo()
    {
        string value = PlayerPrefs.GetString(COMMON_CONST.UserInfo, COMMON_CONST.NullString);
        if (value != COMMON_CONST.NullString) value = AESDecrypt(value);
        return value;
    }
    public static void SetUserStageRecord(string value)
    {
        value = AESEncrypt(value);
        PlayerPrefs.SetString(COMMON_CONST.UserStageRecord, value);
        PlayerPrefs.Save();
    }
    public static string GetUserStageRecord()
    {
        string value = PlayerPrefs.GetString(COMMON_CONST.UserStageRecord, COMMON_CONST.NullString);
        if (value != COMMON_CONST.NullString) value = AESDecrypt(value);
        return value;
    }

	public static void SetUserSignInfo( string continued,string lastDate)
	{
		//Debug.Log ("SetUserSignInfo  "+continued+"#"+lastDate);
		continued = AESEncrypt(continued+"#"+lastDate);
		PlayerPrefs.SetString(COMMON_CONST.UserSignInfo, continued);
	}
	
	public static string GetUserSignInfo()
	{
		string value = PlayerPrefs.GetString(COMMON_CONST.UserSignInfo, "");
		if(value != COMMON_CONST.NullString && !string.IsNullOrEmpty(value)) value = AESDecrypt(value);
		//Debug.Log (value);
		return value;
	}

	public static void SetTitleInfo( string titleInfo)
	{
		titleInfo = AESEncrypt(titleInfo);
		PlayerPrefs.SetString(COMMON_CONST.TitleInfo, titleInfo);
	}

	public static string GetTitleInfo()
	{
		string value = PlayerPrefs.GetString(COMMON_CONST.TitleInfo, "");
		if(value != COMMON_CONST.NullString && !string.IsNullOrEmpty(value)) value = AESDecrypt(value);
		//Debug.Log (value);
		return value;
	}
     #endregion

    #region 游戏业务
    public static bool IsInvalidChair(ushort wChair)
    {
        if ((int)wChair == (int)GlobalDef.Deinfe.INVALID_CHAIR)
        {
            return true;
        }
            
        return false;
    }

    //关卡相关
    /// <summary>
    /// 通过分数和关卡获取星星数
    /// </summary>
    /// <param name="stage"></param>
    /// <param name="score"></param>
    /// <returns></returns>
    public static int GetStarByScore(uint stage,int score)
    {
        INFO_STAGE InfoStage = DataBase.Instance.STAGE_MGR.GetStage(stage);
        if (InfoStage == null) return 1;
        return score >= InfoStage.Star3 ? 3 : (score >= InfoStage.Star2 ? 2 : 1);
    }

    //显示牌
    public static string ShowCardStr(byte cdata)
    {
        string restr = "";
        switch (cdata >> 4)
        {
            case 0:
                restr = "♦";
                break;
            case 1:
                restr = "♣";
                break;
            case 2:
                restr = "♥";
                break;
            case 3:
                restr = "♠";
                break;
        }
        int num = cdata & 0x0F;
        switch (cdata & 0x0F)
        {
            case 1:
                restr += "A";
                break;
            case 11:
                restr += "J";
                break;
            case 12:
                restr += "Q";
                break;
            case 13:
                restr += "K";
                break;
            default:
                restr += num;
                break;
        }
        return restr;
    }

    //输出友好的牌数据
    public static string ShowCardListStr(IEnumerable<byte> list)
    {
        string restr = "";
        foreach(byte cdata in list)
        {
            restr += ShowCardStr(cdata) + "　";
        }
        return restr;
    }
    #endregion

}

//扩展方法类 已经转移到unityengineex.common中
// public static class COMMON_FUNC_EXT
// {
//     public static void ArraySetAll<T>(this T[] t, T v) 
//     {
//         for (int i = 0; i < t.Length; i++) t[i] = v;
//     }
// 
//     public static string ToString<T>(this List<T> t)
//     {
//         string res = "";
//         foreach (var e in t) res += e.ToString() + " ";
//         return res;
//     }
// 
//     public static string ToString<T>(this T[] t)
//     {
//         string res = "";
//         foreach (var e in t) res += e.ToString() + " ";
//         return res;
//     } 
// 
//     public static void CopyArr<T>(this T[] src,T[] des,int length,int arrindex=0,int desindex=0)
//     {
//         for (int i = arrindex; i < arrindex + length;++i )
//         {
//             des[desindex++] = src[i];
//         }
//     }
// }