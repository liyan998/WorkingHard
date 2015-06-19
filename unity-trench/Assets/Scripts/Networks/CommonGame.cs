using System;
using UnityEngineEx.CMD.i3778;

using WORD = System.UInt16;
using DWORD = System.UInt32;
using LONG = System.Int32;
using LONGLONG = System.Int64;
using ULONGLONG = System.UInt64;
using BYTE = System.Byte;

using ConnectError = UnityEngineEx.Net.SocketClient.ConnectError;

using CMD_Command = UnityEngineEx.CMD.i3778.GlobalDef.CMD_Command;
using tagUserData = UnityEngineEx.CMD.i3778.GlobalDef.tagUserData;
using tagUserScore = UnityEngineEx.CMD.i3778.GlobalDef.tagUserScore;
using tagUserStatus = UnityEngineEx.CMD.i3778.GlobalDef.tagUserStatus;
using tagUserInfoHead = UnityEngineEx.CMD.i3778.GlobalDef.tagUserInfoHead;
using tagGameKind = UnityEngineEx.CMD.i3778.GlobalDef.tagGameKind;
using tagGameType = UnityEngineEx.CMD.i3778.GlobalDef.tagGameType;
using tagGameProcess = UnityEngineEx.CMD.i3778.GlobalDef.tagGameProcess;
using tagGameServer = UnityEngineEx.CMD.i3778.GlobalDef.tagGameServer;

//cmd_plaza
using CMD_GP_Version = UnityEngineEx.CMD.i3778.CMD_Plaza.CMD_GP_Version;
using CMD_GP_LogonSuccess = UnityEngineEx.CMD.i3778.CMD_Plaza.CMD_GP_LogonSuccess;
using CMD_GP_LogonError = UnityEngineEx.CMD.i3778.CMD_Plaza.CMD_GP_LogonError;
using CMD_GP_ShowStatus = UnityEngineEx.CMD.i3778.CMD_Plaza.CMD_GP_ShowStatus;


//cmd_game
using CMD_GR_LogonSuccess = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_LogonSuccess;
using CMD_GR_LogonError = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_LogonError;
using CMD_GR_UserStatus = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_UserStatus;
using tagHiLadderApplyCondition = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_HLApplyCondition;
using CMD_GR_HLApplyCondition = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_HLApplyCondition;
using tagHiLadderUserInfo = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_HLUserInfo;
using CMD_GR_HLUserInfo = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_HLUserInfo;
using CMD_GR_HLAwardInfo = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_HLAwardInfo;
using CMD_GR_HLEveryDayInfo = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_HLEveryDayInfo;
using CMD_GR_HLLevelScoreInfo = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_HLLevelScoreInfo;
using CMD_GR_UserScore = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_UserScore;
using CMD_GR_SitFailed = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_SitFailed;
using CMD_GR_UserChat = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_UserChat;
using CMD_GR_Wisper = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_Wisper;
using CMD_GR_UserInvite = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_UserInvite;
using CMD_GR_SendWarning = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_SendWarning;
using CMD_GR_SetUserRight = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_SetUserRight;
using CMD_GR_ServerInfo = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_ServerInfo;
using CMD_GR_ColumnInfo = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_ColumnInfo;
using CMD_GR_TableInfo = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_TableInfo;
using CMD_GR_TableStatus = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_TableStatus;
using CMD_GR_Message = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_Message;
using tagOnLineCountInfo = UnityEngineEx.CMD.i3778.CMD_Game.tagOnLineCountInfo;

//GlobalFrame
using CMD_GF_BankStorageGold = UnityEngineEx.CMD.i3778.GlobalFrame.CMD_GF_BankStorageGold;
using CMD_GF_SysAllotChair = UnityEngineEx.CMD.i3778.GlobalFrame.CMD_GF_SysAllotChair;
using CMD_GF_Option = UnityEngineEx.CMD.i3778.GlobalFrame.CMD_GF_Option;
using CMD_GF_LookonControl = UnityEngineEx.CMD.i3778.GlobalFrame.CMD_GF_LookonControl;
using CMD_GF_UserChat = UnityEngineEx.CMD.i3778.GlobalFrame.CMD_GF_UserChat;
using CMD_GF_Message = UnityEngineEx.CMD.i3778.GlobalFrame.CMD_GF_Message;
using CMD_GF_FreeAwardResult = UnityEngineEx.CMD.i3778.GlobalFrame.CMD_GF_FreeAwardResult;
using tagPropertyInfo = UnityEngineEx.CMD.i3778.GlobalProperty.tagPropertyInfo;
using tagBuyPropertyInfo = UnityEngineEx.CMD.i3778.GlobalProperty.tagBuyPropertyInfo;
using CMD_GF_BugleProperty = UnityEngineEx.CMD.i3778.GlobalProperty.CMD_GF_BugleProperty;

public interface IGameSink1
{
    void OnGameMessage(GlobalDef.CMD_Head cmd, object o);//游戏处理函数
    void ResetData();//新开一局 重置游戏数据
    //void OnBlendTrench(object o);       //黑挖
    //void OnGameStart(object o);         //游戏开始回调
    //void OnSendCard(object o);          //发牌
    //void OnGameEnd(object o);           //游戏结束
    //void OnCallScore(object o);    //叫分
    //void OnOutCard(object o);  //出牌
    //void OnNOOutCard(object o);         //不出
    //void OnPrompt(object o);            //提示
    //void OnNextTurn(object o);          //再来一局
    //void OnTimeOut(object o); //定时器超时

}

public interface INetLoginSink
{
    //登陆服务器
    #region 登陆服务器
    //连接信息
    void OnConnected(ConnectError state);
    //登陆成功
    void OnLoginSuccess(CMD_GP_LogonSuccess msg);
    //登陆失败
    void OnLoginError(CMD_GP_LogonError msg);
    //登陆完成
    void OnLoginFinish();
    //显示状态
    void OnShowState(CMD_GP_ShowStatus msg);
    //系统信息
    void OnSystemInfo(CMD_GP_Version msg);
    //服务器列表完成
    void OnServerListFinish();
    #endregion        
}

//游戏房间接口
public interface INetGameRoomSink
{
    //连接信息
    void OnConnected(ConnectError state);
    //登陆成功
    void OnLoginSuccess(CMD_GR_LogonSuccess msg);
    //登陆失败
    void OnLoginError(CMD_GR_LogonError msg);
    //登陆完成
    void OnLoginFinish();
    //显示消息
    void OnShowMessage(string msg, float time = 0);
    //====================================================================
    // Main User Command
    //====================================================================
    //用户进入
    void OnUserCome(tagUserData user);
    //用户状态
    void OnUserStatus(CMD_GR_UserStatus msg);
    //用户分数
    void OnUserScore(CMD_GR_UserScore msg);
    //坐下失败
    void OnSitFaild(CMD_GR_SitFailed msg);
    //用户聊天
    void OnUserChat(CMD_GR_UserChat msg);
    //用户私聊
    void OnUserWisper(CMD_GR_Wisper msg);
    //用户邀请
    void OnUserInvite(CMD_GR_UserInvite msg);
    //系统警告
    void OnWarnningMessage(CMD_GR_SendWarning msg);
    //设置用户权限
    void OnSetUserRight(CMD_GR_SetUserRight msg);
    //用户设置金币
    void OnUserGold(CMD_GF_BankStorageGold msg);
    //用户等待
    void OnUserWaiting(CMD_GF_SysAllotChair msg);

    //====================================================================
    // Main Server Info
    //====================================================================
    //房间配置信息
    void OnServerConfigInfo(CMD_GR_ServerInfo msg);
    //列表信息
    void OnColumnConfigInfo(CMD_GR_ColumnInfo msg);
    //房间配置完成
    void OnServerConfigFinish();

    //====================================================================
    // Main Table Status
    //====================================================================
    //桌子信息
    void OnTableInfo(CMD_GR_TableInfo msg);
    //桌子状态（人的状态也会跟着改变）
    void OnTableStatus(CMD_GR_TableStatus msg);

    //====================================================================
    // Main System Message
    //====================================================================
    void OnSysMessage(CMD_GR_Message msg);

    //====================================================================
    // Main Server Info
    //====================================================================
    void OnServerInfo(tagOnLineCountInfo[] msg);

}

//游戏框架接口
public interface INetGameFrameSink
{
    //====================================================================
    // Main Global Frame
    //====================================================================
    //游戏配置
    void OnGameOption(CMD_GF_Option msg);
    //空闲场景
    void OnGameSceneFree(CMD_S_StatusFree msg);
    //叫分场景
    void OnGameSceneScore(CMD_S_StatusScore msg);
    //游戏场景
    void OnGameScenePlaying(CMD_S_StatusPlay msg);
    //黑挖场景
    void OnGameSceneTrench(CMD_S_StatusBlankTrench msg);
    //旁观
    void OnLookOnControl(CMD_GF_LookonControl msg);
    //用户聊天
    void OnUserChat(CMD_GF_UserChat msg);
    //系统消息 (请区分弹出 全局 关闭房间等)
    void OnSysMessage(CMD_GF_Message msg);
    //天梯冲分奖励
    void OnHLLevelScoreAwardInfo(CMD_GF_FreeAwardResult msg);
    //天梯每日奖励
    void OnHLEveryDayAwardInfo(CMD_GF_FreeAwardResult msg);

    //====================================================================
    // Main Global Game
    //====================================================================
    //黑挖
    void OnSubBlackTrench(CMD_S_BlankTrench msg);
    //发送扑克
	void OnSubSendCard(CMD_S_SendCard msg);
    //用户叫分
	void OnSubLandScore(CMD_S_LandScore msg);
    //游戏开始
    void OnSubGameStart(CMD_S_GameStart msg);	
	//用户出牌	
	void OnSubOutCard(CMD_S_OutCard msg);
    //放弃出牌		
	void OnSubPassCard(CMD_S_PassCard msg);
    //游戏结束
    void OnSubGameEnd(CMD_S_GameEnd msg);
    //玩家托管
    void OnSubGameTrustee(CMD_C_UserTrustee msg);
    //比赛状态
    void OnSubMatchStatus(CMD_S_MatchStatus msg);
		        
}

//游戏内道具接口
public interface INetGameProperty
{
    //====================================================================
    // Main Game Property
    //====================================================================
    //游戏道具信息
    void OnGamePropertyInfo(tagPropertyInfo msg);
    //游戏道具完成
    void OnGamePropertyFinish();
    //用户道具
    void OnUserPropertyInfo(tagBuyPropertyInfo msg);
    //用户道具完成
    void OnUserPropertyFinish();
    //使用喇叭道具
    void OnUsedBugle(CMD_GF_BugleProperty msg);
}



