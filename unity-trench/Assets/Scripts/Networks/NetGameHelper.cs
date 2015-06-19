using UnityEngine;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngineEx.Net;
using UnityEngineEx.CMD.i3778;
using UnityEngineEx.LogInterface;
using UnityEngineEx;
using UnityEngineEx.Common;
using System.Runtime.InteropServices;
using UnityEngineEx.MThread;

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
using tagGlobalUserData = UnityEngineEx.CMD.i3778.GlobalDef.tagGlobalUserData;

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
using CMD_GR_UserRule = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_UserRule;
using CMD_GR_HiLadderApplyReq = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_HiLadderApplyReq;
using CMD_GR_UserSitReq = UnityEngineEx.CMD.i3778.CMD_Game.CMD_GR_UserSitReq;

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
using CMD_GF_BuyPropertySuccess = UnityEngineEx.CMD.i3778.GlobalProperty.CMD_GF_BuyPropertySuccess;
using CMD_GF_Property_Used = UnityEngineEx.CMD.i3778.GlobalProperty.CMD_GF_Property_Used;



//using SerializationUnit = UnityEngineEx.Net.SerializationUnit;
//using SocketClient = Client.SocketClient;


public class NetGameHelper : SingleClass<NetGameHelper>
{
    //连接服务器类型
    enum enConnType
    {
        ConnType_Null,						//无效服务器
        ConnType_LoginService,				//登录服务器
        ConnType_GameService,				//游戏服务器
    };

    //房间服务状态
    enum enServiceStatus
    {
        ServiceStatus_Null,					//没有状态
        ServiceStatus_Connecting,			//连接状态
        ServiceStatus_EfficacyUser,			//效验用户
        ServiceStatus_RecvConfigInfo,		//接收配置
        ServiceStatus_RecvRoomInfo,			//接收信息
        ServiceStatus_Serviceing,			//服务状态
        ServiceStatus_NetShutDown,			//网络中断
    };

    //桌子类型
    enum DeskStyle
    {
        emDeskStyleNormal,	//普通桌子
        emDeskStyleMatch,	//比赛桌子
        emDeskStyleRand,	//随机分配
        emDeskStyleHundred,	//百人模式
        emDeskStyleHiLadder,//天梯模式
    };

    class PropSvrReq
    {
        public WORD wMainCmd;
        public WORD wSubCmd;
        public byte[] cbBuffer;
        public WORD wDataSize;
        public System.Action<SocketClient.ConnectError> OnConnected;
        public System.Action<WORD, WORD, byte[], WORD> OnRecieved;
    }

    const string sLoginIp = "192.168.1.195";
    const WORD wLoginPort = 9011;

    const string sPropIp = "192.168.1.195";
    const WORD wPropPort = 9012;

    //通用Socket
    SocketClient mSocket = null;
    //道具Socket
    SocketClient mSocketProp = null;



    //用户名
    string mLoginUser = "";
    string mPwd = "";

    //道具请求
    List<PropSvrReq> mPropSvrReqLst = new List<PropSvrReq>();

    //网络游戏消息传递接口
    INetLoginSink mLoginSink = null;
    INetGameRoomSink mGameRoomSink = null;
    INetGameFrameSink mGameFrameSink = null;
    INetGameProperty mGamePropertySink = null;

    /////////////////////////////////////////////////////////////////////////////////////////////////////
    //管理变量    
    public GlobalDef.tagGameServer mCurGameServer;                  //当前进入的游戏房间
    ServerListMgr mServerListMgr;                                   //服务器列表管理
    IUserItem m_pMeUserItem;
    bool m_cbHideUserInfo;
    CClientUserManager m_ClientUserManager;
    public WORD m_wGameGenre;                                       //房间类型
    //连接类型
    enConnType mConnectType;
    enServiceStatus m_ServiceStatus;		                        //服务状态
    //游戏
    bool m_bLookonMode;					                            //旁观模式
    BYTE m_bGameStatus;					                            //游戏状态
    bool m_bAllowLookon;					                        //允许旁观

    public ServerListMgr GServerListMgr {
        get { return mServerListMgr; }
    }

    //比赛
    public BYTE m_cbMatchMode;						                //比赛模式
    public BYTE m_cbDistributeMode;                                 //房间作为分配模式    
    public DWORD m_dwAreaType;                                      //地区类型
    //领取金币
    public BYTE m_cbGrant;							                //是否领取
    public LONG m_lGrantScore;						                //领取金币
    public DWORD m_dwGrantInterval;					                //定时间隔（100s）
    public WORD m_wDeskStyle;						                //桌子模式

    //天梯信息
    #region 天梯信息
    BYTE m_cbSysAllotChair;					                                //系统分配座位
    WORD m_wReqTableID;						                                //请求桌子
    WORD m_wReqChairID;						                                //请求位置
    public tagHiLadderApplyCondition m_HiLadderApplyCondition;			    //报名条件
    public tagHiLadderUserInfo m_meHLUserInfo;						        //自己天梯信息
    public List<CMD_GR_HLAwardInfo> m_HiLadderAwardInfoArr;				    //天梯奖励
    public List<CMD_GR_HLEveryDayInfo> m_HiLadderEveryDayInfoArr;			//每日奖励
    public List<CMD_GR_HLLevelScoreInfo> m_HiLadderLevelScoreInfoArr;		//冲分奖励
    #endregion
    /////////////////////////////////////////////////////////////////////////////////////////////////////

    NetGameHelper()
    {
        mConnectType = enConnType.ConnType_Null;
        m_ServiceStatus = enServiceStatus.ServiceStatus_Null;

        m_HiLadderApplyCondition = new tagHiLadderApplyCondition();
        m_meHLUserInfo = new tagHiLadderUserInfo();
        m_HiLadderEveryDayInfoArr = new List<CMD_GR_HLEveryDayInfo>();
        m_HiLadderLevelScoreInfoArr = new List<CMD_GR_HLLevelScoreInfo>();


        m_bGameStatus = CMD_Trench.GS_WK_FREE;
        m_bAllowLookon = false;
        m_cbSysAllotChair = 0;
        m_wReqTableID = (WORD)GlobalDef.Deinfe.INVALID_TABLE;
        m_wReqChairID = (WORD)GlobalDef.Deinfe.INVALID_CHAIR;
        m_ClientUserManager = new CClientUserManager();
        m_cbHideUserInfo = true;
        mServerListMgr = new ServerListMgr();
        m_pMeUserItem = null;

        mLoginUser = "";
        mPwd = "";

        mSocket = new SocketClient((int state, SocketClient sockClient) =>
        {
            MThreadPool.Instance.NewThreadToMain((p) => { OnConnected(state, sockClient); }, null);
        },
        (CMD_Command cmd, byte[] buffer, WORD wDataSize) =>
        {
            bool bres = true;
            MThreadPool.Instance.NewThreadToMain(p => { bres = OnRecieved(cmd, buffer, wDataSize); }, null);
            return bres;
        },
        (SocketClient sockClient, bool byServer) =>
        {
            MThreadPool.Instance.NewThreadToMain(p => { OnDisconnected(sockClient, byServer); }, null);
        });

        //mSocketProp = new SocketClient(OnPropConnected, OnPropRecieved, OnPropDisconnected);
        mSocketProp = new SocketClient((int state, SocketClient sockClient) =>
        {
            MThreadPool.Instance.NewThreadToMain((p) => { OnPropConnected(state, sockClient); }, null);
        },
        (CMD_Command cmd, byte[] buffer, WORD wDataSize) =>
        {
            bool bres = true;
            MThreadPool.Instance.NewThreadToMain(p => { bres = OnPropRecieved(cmd, buffer, wDataSize); }, null);
            return bres;
        },
        (SocketClient sockClient, bool byServer) =>
        {
            MThreadPool.Instance.NewThreadToMain(p => { OnPropDisconnected(sockClient, byServer); }, null);
        });
    }

    public void SetLoginSink(INetLoginSink sink)
    {
        mLoginSink = sink;
    }
    public void SetGameRoomSink(INetGameRoomSink sink)
    {
        mGameRoomSink = sink;
    }
    public void SetGameFrameSink(INetGameFrameSink sink)
    {
        mGameFrameSink = sink;
    }
    public void SetGamePropertySink(INetGameProperty sink)
    {
        mGamePropertySink = sink;
    }

    //void SendSinkMessage(WORD wMainCmd, WORD wSubCmd, object o)
    //{
    //    GlobalDef.CMD_Command cmd = new GlobalDef.CMD_Command();
    //    cmd.wMainCmdID = wMainCmd;
    //    cmd.wSubCmdID = wSubCmd;
    //    if (mGameSink != null) mGameSink.OnGameMessage(cmd, o);
    //}

    // Use this for initialization
    void Start()
    {
        //test
        //LoginPlaza("", "");
    }

    // Update is called once per frame
    void Update()
    {

    }


    void ASSERT(bool b, string msg = "")
    {
        if (b == false) Debuger.Instance.LogError("ASSERT:" + msg);
    }

    void Connect()
    {
        string ip;
        WORD wPort;
        if (mConnectType == enConnType.ConnType_LoginService)
        {
            ip = sLoginIp;
            wPort = wLoginPort;
        }
        else
        {
            ip = new IPAddress(mCurGameServer.dwServerAddr).ToString();
            wPort = mCurGameServer.wServerPort;
        }
        Disconnect();
        mSocket.Connect(ip, wPort);
    }

    void Disconnect()
    {
        if (mSocket.IsConnected) mSocket.Disconnect();
    }

    void ConnectProp()
    {
        DisconnectProp();
        mSocketProp.Connect(sPropIp, wPropPort);
    }

    void DisconnectProp()
    {
        if (mSocket.IsConnected) mSocketProp.Disconnect();
    }

    #region 回调函数
    void OnConnected(int state, SocketClient sockClient)
    {
        if (mLoginSink != null) mLoginSink.OnConnected((SocketClient.ConnectError)state);

        if ((SocketClient.ConnectError)state != SocketClient.ConnectError.Conntected)
        {
            Debuger.Instance.LogWarning("网络链接失败!");
            //添加界面提示
            return;
        }
        if (mConnectType == enConnType.ConnType_LoginService)
        {
            //发送登陆大厅包
            SendLoginPacket();
        }
        else if (mConnectType == enConnType.ConnType_GameService)
        {
            m_ServiceStatus = enServiceStatus.ServiceStatus_EfficacyUser;
            //发送登陆房间包
            SendGameLogonPacket();
        }
    }

    bool OnRecieved(GlobalDef.CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        bool breturn = false;
        if (mConnectType == enConnType.ConnType_LoginService)
        {
            if (Command.wMainCmdID == CMD_Plaza.MDM_GP_LOGON)
                return OnSocketMainLogon(Command, pBuffer, wDataSize);
            else if (Command.wMainCmdID == CMD_Plaza.MDM_GP_SYSTEM)
                return OnSocketMainSystem(Command, pBuffer, wDataSize);
            else if (Command.wMainCmdID == CMD_Plaza.MDM_GP_SERVER_LIST)
                return OnSocketMainServerList(Command, pBuffer, wDataSize);
        }
        else if (mConnectType == enConnType.ConnType_GameService)
        {
            switch (Command.wMainCmdID)
            {
                case CMD_Game.MDM_GR_LOGON:			        //登录消息
                    {
                        breturn = OnSocketGameMainLogon(Command, pBuffer, wDataSize);
                        ASSERT(breturn == true);
                        return breturn;
                    }
                case CMD_Game.MDM_GR_USER:			        //用户消息
                    {
                        breturn = OnSocketMainUser(Command, pBuffer, wDataSize);
                        ASSERT(breturn == true);
                        return breturn;
                    }
                case CMD_Game.MDM_GR_INFO:			        //配置信息
                    {
                        breturn = OnSocketMainInfo(Command, pBuffer, wDataSize);
                        ASSERT(breturn == true);
                        return breturn;
                    }
                case CMD_Game.MDM_GR_STATUS:			    //状态信息
                    {
                        breturn = OnSocketMainStatus(Command, pBuffer, wDataSize);
                        ASSERT(breturn == true);
                        return breturn;
                    }
                case CMD_Game.MDM_GR_SYSTEM:			    //系统消息
                    {
                        breturn = OnSocketGameMainSystem(Command, pBuffer, wDataSize);
                        ASSERT(breturn == true);
                        return breturn;
                    }
                case CMD_Game.MDM_GR_SERVER_INFO:	        //房间信息
                    {
                        breturn = OnSocketMainServerInfo(Command, pBuffer, wDataSize);
                        ASSERT(breturn == true);
                        return breturn;
                    }
                case GlobalFrame.MDM_GF_GAME:			    //游戏消息
                    {
                        breturn = OnSocketMainGame(Command, pBuffer, wDataSize);
                        ASSERT(breturn == true);
                        return breturn;
                    }
                case GlobalFrame.MDM_GF_FRAME:			    //框架消息
                    ////case CMD_Game.MDM_GF_VIDEO:			//视频消息                    
                    {
                        breturn = OnSocketMainGlobalFrame(Command, pBuffer, wDataSize);
                        ASSERT(breturn == true);
                        return breturn;
                    }
                case GlobalProperty.MDM_GF_PROPERTY:		//道具消息
                    {
                        breturn = OnSocketMainGameProperty(Command, pBuffer, wDataSize);
                        ASSERT(breturn == true);
                        return breturn;
                    }
                //case CMD_Game.MDM_GR_MATCH_INFO:		    //比赛信息
                //    {
                //        breturn = OnSocketMainMatchInfo(Command, pBuffer, wDataSize, pIClientSocke);
                //        ASSERT(breturn == true);
                //        return breturn;
                //    }
                //case CMD_Game.MDM_GR_HILADDER:		    //天梯信息
                //    {
                //        breturn = OnSocketMainHiLadderInfo(Command, pBuffer, wDataSize, pIClientSocke);
                //        ASSERT(breturn == true);
                //        return breturn;
                //    }
            }
        }
        return true;
    }

    void OnDisconnected(SocketClient sockClient, bool byServer)
    {
        m_ServiceStatus = enServiceStatus.ServiceStatus_NetShutDown;
    }

    void OnPropConnected(int state, SocketClient sockClient)
    {
        PropSvrReq req = mPropSvrReqLst[0];
        req.OnConnected((SocketClient.ConnectError)state);
        if (state == (int)SocketClient.ConnectError.Conntected)
        {
            mSocketProp.Send(req.wMainCmd, req.wSubCmd, req.cbBuffer, req.wDataSize);
        }
        else mSocketProp.Disconnect();
    }

    bool OnPropRecieved(GlobalDef.CMD_Command cmd, byte[] buffer, WORD wDataSize)
    {
        mSocketProp.Disconnect();

        PropSvrReq req = mPropSvrReqLst[0];
        req.OnRecieved(cmd.wMainCmdID, cmd.wSubCmdID, buffer, wDataSize);
        mPropSvrReqLst.Clear();

        return true;
    }

    void OnPropDisconnected(SocketClient sockClient, bool byServer)
    {

    }

    #endregion

    #region 控制动作
    //启动游戏
    int StartGameClient()
    {
        return 0;
    }
    //登陆大厅
    public void LoginPlaza(string user, string pwd)
    {
        mLoginUser = user;
        mPwd = pwd;
        mConnectType = enConnType.ConnType_LoginService;
        Connect();
    }
    //登陆房间
    public void LoginGame()
    {
        mConnectType = enConnType.ConnType_GameService;
        Connect();
    }
    void SendLoginPacket()
    {
        BYTE[] cbBuffer = new BYTE[GlobalConfig.SOCKET_PACKAGE];
        string pwd = mPwd;
        if (mPwd.Length < 42) pwd = UnityEngineEx.Net.EncryptHelper.ToMySqlEncrypt(mPwd);// mPwd.ToMySqlEncrypt();

        WORD wSpecialUser = COMMON_FUNC.MAKEWORD((BYTE)CMD_Plaza.enUserType.en_UserType_General, 0);
        #region 新倚天用户验证
        //COptionUI* pOpSelYTSvr = (COptionUI*)m_PaintManager.FindControl(TEXT("op_sel_yt_user"));
        //CComboUI* pCbSelYTSvr = (CComboUI*)m_PaintManager.FindControl(TEXT("cb_sel_yt_svr"));
        //if (pOpSelYTSvr->IsSelected()) wSpecialUser = MAKEWORD(en_UserType_XYT, pCbSelYTSvr->GetCurSel());

        //if (en_UserType_XYT == LOBYTE(wSpecialUser))
        //{
        //    CString sRes = YTUserVerify(HIBYTE(wSpecialUser), m_szAccounts, m_szPassword);

        //    if (sRes == TEXT("E00001"))
        //    {
        //        g_spMainFrame->ShowMessageBox(m_hWnd, TEXT("非法请求，错误码:E00001"), TEXT("错误"), MODAL_WND, true, 10, 0, IDOK, false);
        //        return true;
        //    }
        //    else if (sRes == TEXT("E00002"))
        //    {
        //        g_spMainFrame->ShowMessageBox(m_hWnd, TEXT("用户名密码错误，错误码:E00002"), TEXT("错误"), MODAL_WND, true, 10, 0, IDOK, false);
        //        return true;
        //    }
        //    else if (sRes == TEXT("E00000"))
        //    {
        //        //g_spMainFrame->ShowMessageBox(sRes,TEXT("错误"),MODAL_WND,true,10,IDOK);
        //        //g_spMainFrame->ShowMessageBox(sRes,TEXT("错误"),MODAL_WND,false,0,IDOK);
        //    }
        //    else
        //    {
        //        g_spMainFrame->ShowMessageBox(m_hWnd, TEXT("未知错误！"), TEXT("错误"), MODAL_WND, true, 10, 0, IDOK, false);
        //        return true;
        //    }
        //    //return true;
        //}

        #endregion
        WORD wPlatform = COMMON_FUNC.MAKEWORD(COMMON_CONST.MarketId, (byte)Application.platform);

        //登陆包
        {
            //构造数据
            CMD_Plaza.CMD_GP_LogonByAccounts pLogonByAccounts;
            //memset(pLogonByAccounts,0,sizeof(CMD_GP_LogonByAccounts));
            //pLogonByAccounts->dwPlazaVersion=g_GlobalUnits.GetPlazaVersion();

            pLogonByAccounts.szPassWord = pwd;
            pLogonByAccounts.szAccounts = mLoginUser;

            //CopyMemory(pLogonByAccounts->szPassWord,szPassword,sizeof(pLogonByAccounts->szPassWord));
            //lstrcpyn(pLogonByAccounts->szAccounts,m_szAccounts,CountArray(pLogonByAccounts->szAccounts));

            //if (en_UserType_XYT == LOBYTE(wSpecialUser)) _stprintf( pLogonByAccounts->szAccounts, _T("%s%s"),g_YTSvrInfo[HIBYTE(wSpecialUser)].szShortName, m_szAccounts);

            //特殊处理E商盟用户
            //BOOL bEShop=(((CButton *)GetDlgItem(IDC_E_SHOP))->GetCheck()==BST_CHECKED);
            //if (bEShop)_stprintf( pLogonByAccounts->szAccounts, _T("tze%s"), m_szAccounts);
            //if (bEShop==TRUE)pLogonByAccounts->wSpecialUser=en_SpecialUser_EShop;
            //else 

            pLogonByAccounts.wSpecialUser = wSpecialUser;
            pLogonByAccounts.wPlatform = wPlatform;
            pLogonByAccounts.dwPlazaVersion = System.BitConverter.ToUInt32(IPAddress.Parse(COMMON_CONST.Version).GetAddressBytes(), 0);
            pLogonByAccounts.szUserDevice = DeviceManager.Instance.deviceId;
            //pLogonByAccounts.dwPlazaVersion=(DWORD)IPAddress.NetworkToHostOrder((int)pLogonByAccounts.dwPlazaVersion);

            //机器序列号
            GlobalDef.tagClientSerial ClientSerial = new GlobalDef.tagClientSerial();
            //ClientSerial.dwSystemVer = DeviceManager.Instance.OSVersion;
            //ClientSerial.dwComputerID DeviceManager.Instance.deviceId
            //g_GlobalUnits.GetClientSerial(ClientSerial);

            byte[] buf = SerializationUnit.StructToBytes(pLogonByAccounts);
            buf.CopyArr(cbBuffer, buf.Length);

            //发送数据包
            CSendPacketHelper Packet = new CSendPacketHelper(cbBuffer, (WORD)cbBuffer.Length, buf.Length);
            byte[] serial = SerializationUnit.StructToBytes(ClientSerial);
            Packet.AddPacket(serial, (WORD)serial.Length, GlobalField.DTP_NULL);

            //byte[] sendBuf = new byte[buf.Length + Packet.GetDataSize()];
            //buf.CopyArr(sendBuf, buf.Length);
            //Packet.GetDataBuffer().CopyArr(sendBuf, Packet.GetDataSize(), 0,buf.Length);

            mSocket.Send(CMD_Plaza.MDM_GP_LOGON, CMD_Plaza.SUB_GP_LOGON_ACCOUNTS, cbBuffer, (WORD)(buf.Length + Packet.GetDataSize()));
            //CSendPacketHelper Packet(cbBuffer+sizeof(CMD_GP_LogonByAccounts),sizeof(cbBuffer)-sizeof(CMD_GP_LogonByAccounts));
            //Packet.AddPacket(&ClientSerial,sizeof(ClientSerial),DTP_COMPUTER_ID);
            //pIClientSocke->SendData(MDM_GP_LOGON,SUB_GP_LOGON_ACCOUNTS,cbBuffer,sizeof(CMD_GP_LogonByAccounts)+Packet.GetDataSize());
        }
    }

    bool SendGameLogonPacket()
    {
        //获取信息
        BYTE[] cbBuffer = new BYTE[GlobalConfig.SOCKET_PACKAGE];
        GlobalDef.tagGlobalUserData GlobalUserData = RefactorData.Instance.NETPLAYER.GlobalUserData;

        //登录数据包
        CMD_Game.CMD_GR_LogonByUserID pLogonByUserID = new CMD_Game.CMD_GR_LogonByUserID();
        pLogonByUserID.dwUserID = GlobalUserData.dwUserID;
        pLogonByUserID.szPassWord = GlobalUserData.szPassWord;
        pLogonByUserID.dwPlazaVersion = System.BitConverter.ToUInt32(IPAddress.Parse(COMMON_CONST.Version).GetAddressBytes(), 0); ;
        pLogonByUserID.dwProcessVersion = 0;

        ////获取目录
        //TCHAR szFilePath[MAX_PATH + 1]; 
        //CString strProcessName ;
        //GetModuleFileName(NULL, szFilePath, MAX_PATH); 
        //(_tcsrchr(szFilePath, _T('\\')))[1] = 0;
        //strProcessName.Format("%s\\GamePlaza.exe",szFilePath);

        ////获取版本
        //CWinFileInfo WinFileInfo;
        //if (WinFileInfo.OpenWinFile(strProcessName))
        //{
        //    //获取版本
        //    DWORD dwFileVerMS=0L,dwFileVerLS=0L;
        //    WinFileInfo.GetFileVersion(dwFileVerMS,dwFileVerLS);

        //    //版本分析
        //    BYTE cbFileVer1=(BYTE)(HIWORD(dwFileVerMS));
        //    BYTE cbFileVer2=(BYTE)(LOWORD(dwFileVerMS));
        //    BYTE cbFileVer3=(BYTE)(HIWORD(dwFileVerLS));
        //    BYTE cbFileVer4=(BYTE)(LOWORD(dwFileVerLS));
        //    pLogonByUserID->dwPlazaVersion=MAKELONG(MAKEWORD(cbFileVer1,cbFileVer2),MAKEWORD(cbFileVer3,cbFileVer4));
        //}
        byte[] cbLogonByUserID = SerializationUnit.StructToBytes(pLogonByUserID);
        cbLogonByUserID.CopyArr(cbBuffer, cbLogonByUserID.Length);
        //机器序列号
        GlobalDef.tagClientSerial ClientSerial = new GlobalDef.tagClientSerial();
        byte[] cbClientSerial = SerializationUnit.StructToBytes(ClientSerial);
        //g_GlobalUnits.GetClientSerial(ClientSerial);

        //发送数据包
        CSendPacketHelper Packet = new CSendPacketHelper(cbBuffer, (WORD)cbBuffer.Length, cbLogonByUserID.Length);
        Packet.AddPacket(cbClientSerial, (WORD)cbClientSerial.Length, GlobalField.DTP_NULL);
        mSocket.Send(CMD_Game.MDM_GR_LOGON, CMD_Game.SUB_GR_LOGON_USERID, cbBuffer, (WORD)(cbLogonByUserID.Length + Packet.GetDataSize()));

        return true;
    }

    //发送设置命令
    public bool SendOptionsPacket()
    {
        return true;
    }
    //发送起来命令
    public bool SendStandUpPacket()
    {
        mSocket.Send(CMD_Game.MDM_GR_USER, CMD_Game.SUB_GR_USER_STANDUP_REQ, null, 0);

        return true;
    }
    //发送强退命令
    public bool SendLeftGamePacket()
    {
        mSocket.Send(CMD_Game.MDM_GR_USER, CMD_Game.SUB_GR_USER_LEFT_GAME_REQ, null, 0);

        return true;
    }
    //发送户用排队命令
    public bool SendQueuePacket()
    {
        mSocket.Send(CMD_Game.MDM_GR_USER, CMD_Game.SUB_GR_USER_NO_QUEUE_REQ, null, 0);

        return true;
    }
    //发送房间规则
    public bool SendUserRulePacket()
    {
        //构造数据包
        //CGameOption* pGameOpt = g_GlobalOption.GetGameOption(m_pListServer->GetListKind());
        //CServerOption* pSrvOpt = g_GlobalOption.GetServerOption(m_pListServer);
        //CMD_GR_UserRule UserRule = new CMD_GR_UserRule(); ;
        //UserRule.bLimitWin = pGameOpt->m_bLimitWin;
        //UserRule.bLimitFlee = pGameOpt->m_bLimitFlee;
        //UserRule.wWinRate = pGameOpt->m_wWinRate;
        //UserRule.wFleeRate = pGameOpt->m_wFleeRate;
        //UserRule.lMaxScore = pGameOpt->m_lMaxScore;
        //UserRule.lLessScore = pGameOpt->m_lLessScore;
        //UserRule.bLimitScore = pGameOpt->m_bLimitScore;
        //UserRule.bPassword = pSrvOpt->m_bPassword;
        //UserRule.bCheckSameIP = g_GlobalOption.m_bCheckSameIP;
        //lstrcpyn(UserRule.szPassword, pSrvOpt->m_szPassword, CountArray(UserRule.szPassword));

        ////发送数据包
        //m_ClientSocket->SendData(MDM_GR_USER, SUB_GR_USER_RULE, &UserRule, sizeof(UserRule));

        return true;
    }
    //发送天梯用户命令
    public bool SendHiLadderMeUserInfoPacket()
    {
        if ((m_wGameGenre & GlobalDef.GAME_GENRE_HILADDER) != 0)
        {
            mSocket.Send(CMD_Game.MDM_GR_HILADDER, CMD_Game.SUB_GR_HL_USER_INFO_REQ, null, 0);
        }

        return true;
    }
    //发送天梯用户命令
    public bool SendHiLadderRankUserListPacket()
    {
        if ((m_wGameGenre & GlobalDef.GAME_GENRE_HILADDER) != 0)
        {
            mSocket.Send(CMD_Game.MDM_GR_HILADDER, CMD_Game.SUB_GR_HL_USER_LIST_REQ, null, 0);
        }
        return true;
    }
    //发送天梯匹配命令
    public bool SendHiLadderQuickJoinPacket()
    {
        if ((m_wGameGenre & GlobalDef.GAME_GENRE_HILADDER) != 0)
        {
            mSocket.Send(CMD_Game.MDM_GR_HILADDER, CMD_Game.SUB_GR_HL_QUICK_JOIN_REQ, null, 0);
        }
        return true;
    }
    //发送天梯报名
    public bool SendHiLadderApply()
    {
        if ((m_wGameGenre & GlobalDef.GAME_GENRE_HILADDER) != 0)
        {
            if (m_meHLUserInfo.dwUserID == 0)
            {
                //if (IDCANCEL==ShowMessageBox(TEXT("您还未报名天梯，是否报名？"), \
                //    TEXT("天梯报名"),MODAL_WND,false,0,IDOK|IDCANCEL) )
                //    return false;
            }
            CMD_GR_HiLadderApplyReq Cmd = new CMD_GR_HiLadderApplyReq();
            Cmd.dwUserID = m_meHLUserInfo.dwUserID;
            byte[] cbBuffer = SerializationUnit.StructToBytes(Cmd);
            mSocket.Send(CMD_Game.MDM_GR_HILADDER, CMD_Game.SUB_GR_HL_APPLY_REQ, cbBuffer, (WORD)cbBuffer.Length);
        }
        return true;
    }
    //发送旁观命令
    public bool SendLookonPacket(WORD wTableID, WORD wChairID, string pszTablePass)
    {
        //构造数据包
        CMD_GR_UserSitReq UserUserSitReq = new CMD_GR_UserSitReq();
        UserUserSitReq.wTableID = wTableID;
        UserUserSitReq.wChairID = wChairID;
        UserUserSitReq.szTablePass = pszTablePass.ToBytes();
        UserUserSitReq.cbPassLen = (byte)UserUserSitReq.szTablePass.Length;

        byte[] sendbuf = SerializationUnit.StructToBytes(UserUserSitReq);
        WORD wSendSize = (WORD)((int)Marshal.OffsetOf(typeof(CMD_GR_UserSitReq), "szTablePass") + UserUserSitReq.cbPassLen);
        mSocket.Send(CMD_Game.MDM_GR_USER, CMD_Game.SUB_GR_USER_LOOKON_REQ, sendbuf, wSendSize);
        //发送数据包
        //WORD wSendSize = sizeof(UserUserSitReq) - sizeof(UserUserSitReq.szTablePass) + UserUserSitReq.cbPassLen;
        //m_ClientSocket->SendData(MDM_GR_USER, SUB_GR_USER_LOOKON_REQ, &UserUserSitReq, wSendSize);

        return true;
    }
    //发送坐下命令
    public bool SendSitDownPacket(WORD wTableID, WORD wChairID, string pszTablePass)
    {
        //构造数据包
        CMD_GR_UserSitReq UserUserSitReq = new CMD_GR_UserSitReq();
        UserUserSitReq.wTableID = wTableID;
        UserUserSitReq.wChairID = wChairID;
        UserUserSitReq.szTablePass = pszTablePass.ToBytes();
        UserUserSitReq.cbPassLen = (byte)UserUserSitReq.szTablePass.Length;

        byte[] sendbuf = SerializationUnit.StructToBytes(UserUserSitReq);
        WORD wSendSize = (WORD)((int)Marshal.OffsetOf(typeof(CMD_GR_UserSitReq), "szTablePass") + UserUserSitReq.cbPassLen);

        mSocket.Send(CMD_Game.MDM_GR_USER, CMD_Game.SUB_GR_USER_SIT_REQ, sendbuf, wSendSize);
        return true;
    }
    //发送聊天命令
    public bool SendChatPacket(DWORD dwTargetUserID, string pszChatMessage, DWORD crFontColor)
    {
        //获取用户
        ASSERT(m_pMeUserItem != null);
        tagUserData pUserData = m_pMeUserItem.GetUserData();

        //构造数据
        CMD_GR_UserChat UserChat;
        UserChat.crFontColor = crFontColor;
        UserChat.dwTargetUserID = dwTargetUserID;
        UserChat.dwSendUserID = pUserData.dwUserID;
        UserChat.szChatMessage = pszChatMessage.ToBytes();
        UserChat.wChatLength = (WORD)UserChat.szChatMessage.Length;

        byte[] sendbuf = SerializationUnit.StructToBytes(UserChat);
        WORD wSendSize = (WORD)((int)Marshal.OffsetOf(typeof(CMD_GR_UserSitReq), "szChatMessage") + UserChat.wChatLength);

        mSocket.Send(CMD_Game.MDM_GR_USER, CMD_Game.SUB_GR_USER_CHAT, sendbuf, wSendSize);
        //发送命令
        //WORD wSendSize = sizeof(UserChat) - sizeof(UserChat.szChatMessage) + UserChat.wChatLength;
        //m_ClientSocket->SendData(MDM_GR_USER, SUB_GR_USER_CHAT, &UserChat, wSendSize);

        return true;
    }
    //发送购买道具命令
    public bool SendBuyPropertyPaket(DWORD dwGameID, DWORD dwPropertyID, string pszPropName, LONGLONG llGoldCount, DWORD dwZZBean, DWORD dwZScore)
    {
        //获取用户
        ASSERT(m_pMeUserItem != null);
        tagUserData pUserData = m_pMeUserItem.GetUserData();

        //构造数据
        CMD_GF_BuyPropertySuccess BuyPropertySuccess = new CMD_GF_BuyPropertySuccess();
        BuyPropertySuccess.dwGameID = pUserData.dwGameID;
        BuyPropertySuccess.dwPropertyID = dwPropertyID;
        BuyPropertySuccess.llInsureScore = llGoldCount;
        BuyPropertySuccess.dwZZBean = dwZZBean;
        BuyPropertySuccess.dwZScore = dwZScore;
        BuyPropertySuccess.szPropName = pszPropName.ToBytes();

        byte[] sendbuff = SerializationUnit.StructToBytes(BuyPropertySuccess);

        //发送命令
        mSocket.Send(GlobalProperty.MDM_GF_PROPERTY, GlobalProperty.SUB_GF_BUY_PROPERTY_SUCCESS, sendbuff, (WORD)sendbuff.Length);

        return true;
    }
    //发送使用道具命令
    public bool SendUsedPropertyPaket(DWORD dwGameID, DWORD dwPropertyID, DWORD dwPropCount, DWORD dwKindID, string pszKindName)
    {
        //获取用户
        ASSERT(m_pMeUserItem != null);
        tagUserData pUserData = m_pMeUserItem.GetUserData();

        //构造数据
        CMD_GF_Property_Used Property_Used = new CMD_GF_Property_Used();
        Property_Used.dwGameID = pUserData.dwGameID;
        Property_Used.dwPropertyID = dwPropertyID;
        Property_Used.dwKindID = dwKindID;
        Property_Used.szKindName = pszKindName.ToBytes();

        byte[] sendbuff = SerializationUnit.StructToBytes(Property_Used);

        //发送命令
        mSocket.Send(GlobalProperty.MDM_GF_PROPERTY, GlobalProperty.SUB_GF_USED_PROPERTY_SUCCESS, sendbuff, (WORD)sendbuff.Length);
        //m_ClientSocket->SendData(MDM_GF_PROPERTY, SUB_GF_USED_PROPERTY_SUCCESS, &Property_Used, sizeof(CMD_GF_Property_Used));

        return true;
    }
    //请求用户闯关任务
    public bool SendRequestDailyTask()
    {
        if (m_ServiceStatus != enServiceStatus.ServiceStatus_Serviceing) return false;
        //获取用户
        ASSERT(m_pMeUserItem != null);
        tagUserData pUserData = m_pMeUserItem.GetUserData();
        //发送命令
        mSocket.Send(GlobalProperty.MDM_GF_DAILY_TASK_REQUEST, GlobalProperty.SUB_GF_REQ_USER_TASK, null, 0);

        return true;
    }

    //////////////////////////////////////////////////////////////////////////////////    
    #endregion

    #region 登陆网络消息
    bool OnSocketMainLogon(CMD_Command cmd, byte[] buffer, WORD wDataSize)
    {
        switch (cmd.wSubCmdID)
        {
            case CMD_Plaza.SUB_GP_LOGON_SUCCESS:
                {
                    //效验参数
                    if (wDataSize < Marshal.SizeOf(typeof(CMD_Plaza.CMD_GP_LogonSuccess)))
                    {
                        Debuger.Instance.LogError("大小不对");
                        return false;
                    }

                    GlobalDef.tagGlobalUserData globalUser = new GlobalDef.tagGlobalUserData();
                    CMD_Plaza.CMD_GP_LogonSuccess pLogonSuccess = SerializationUnit.BytesToStruct<CMD_Plaza.CMD_GP_LogonSuccess>(buffer);
                    globalUser.wFaceID = pLogonSuccess.wFaceID;
                    globalUser.cbGender = (byte)pLogonSuccess.cbGender;
                    globalUser.cbMember = (byte)pLogonSuccess.cbMember;
                    globalUser.dwUserID = pLogonSuccess.dwUserID;
                    globalUser.dwGameID = pLogonSuccess.dwGameID;
                    globalUser.dwExperience = pLogonSuccess.dwExperience;
                    globalUser.dwGoldCount = pLogonSuccess.dwGoldCount;
                    globalUser.dwZZBean = pLogonSuccess.dwZZBean;
                    globalUser.llInsureScore = pLogonSuccess.llInsureScore;
                    globalUser.dwAreaType = pLogonSuccess.dwAreaType;
                    globalUser.dwZScore = pLogonSuccess.dwZScore;
                    globalUser.dwLotteries = pLogonSuccess.dwLotteries;
                    globalUser.llInsureLotteries = pLogonSuccess.llInsureLotteries;
                    RefactorData.Instance.NETPLAYER.SignState = pLogonSuccess.dwSignInStatus == 0 ? false : true;
                    RefactorData.Instance.NETPLAYER.VipLv = pLogonSuccess.cbMember;
                    //SetSignStatus(pLogonSuccess.dwSignInStatus);
                    //SetVipLevel(pLogonSuccess.cbMember);

                    byte[] DataDes = new byte[wDataSize - Marshal.SizeOf(typeof(CMD_Plaza.CMD_GP_LogonSuccess))];
                    buffer.CopyArr(DataDes, DataDes.Length, Marshal.SizeOf(typeof(CMD_Plaza.CMD_GP_LogonSuccess)), 0);

                    tagDataDescribe DataDescribe = new tagDataDescribe();
                    CRecvPacketHelper RecvPacket = new CRecvPacketHelper(DataDes, (WORD)DataDes.Length);
                    while (true)
                    {
                        byte[] pDataBuffer = RecvPacket.GetData(ref DataDescribe);
                        if (DataDescribe.wDataDescribe == GlobalField.DTP_NULL) break;
                        switch (DataDescribe.wDataDescribe)
                        {
                            case GlobalField.DTP_USER_ACCOUNTS:		//用户帐户
                                {
                                    globalUser.szAccounts = pDataBuffer.ToAnsiString();
                                    break;
                                }
                            case GlobalField.DTP_USER_REG_ACCOUNTS:		//用户昵称
                                {
                                    globalUser.szRegAccounts = pDataBuffer.ToAnsiString();
                                    break;
                                }
                            case GlobalField.DTP_USER_PASS:			//用户密码
                                {
                                    globalUser.szPassWord = pDataBuffer.ToAnsiString();
                                    break;
                                }
                            case GlobalField.DTP_UNDER_WRITE:		//个性签名
                                {
                                    globalUser.szUnderWrite = pDataBuffer.ToAnsiString();
                                    break;
                                }
                            case GlobalField.DTP_USER_GROUP_NAME:	//社团名字
                                {
                                    globalUser.szGroupName = pDataBuffer.ToAnsiString();
                                    break;
                                }
                            case GlobalField.DTP_STATION_PAGE:		//游戏主站
                                {
                                    //ASSERT(pDataBuffer != NULL);
                                    //if (pDataBuffer != NULL)
                                    //{
                                    //    g_GlobalUnits.SetStationPage((LPCTSTR)pDataBuffer);
                                    //}
                                    //globalUser.szGroupName = System.Text.Encoding.GetEncoding("GBK").GetString(pDataBuffer);
                                    break;
                                }
                            //default: { Debuger.Instance.LogError(COMMON_CONST.ErrCommon); }
                        }
                    }

                    //设置全局用户
                    RefactorData.Instance.NETPLAYER.GlobalUserData = globalUser;

                    //通知控制层
                    if (mLoginSink != null) mLoginSink.OnLoginSuccess(pLogonSuccess);

                    break;
                }
            case CMD_Plaza.SUB_GP_LOGON_ERROR:
                {
                    mSocket.Disconnect(false);

                    //byte[] recv = new byte[Marshal.SizeOf(typeof(CMD_Plaza.CMD_GP_LogonError))];
                    //System.Buffer.BlockCopy(buffer, 0, recv, 0, recv.Length);
                    //效验参数
                    CMD_Plaza.CMD_GP_LogonError pLogonError = SerializationUnit.BytesToStruct<CMD_Plaza.CMD_GP_LogonError>(buffer);
                    //Debuger.Instance.LogError(pLogonError.szErrorDescribe.ToAnsiString());

                    //通知控制层
                    if (mLoginSink != null) mLoginSink.OnLoginError(pLogonError);
                    break;
                }
            case CMD_Plaza.SUB_GP_SHOW_STATUS:
                {
                    break;
                }
            case CMD_Plaza.SUB_GP_LOGON_FINISH:
                {
                    mSocket.Disconnect(false);
                    if (mLoginSink != null) mLoginSink.OnLoginFinish();
                    break;
                }
            case CMD_Plaza.SUB_GP_SERVER_KIND:
                {
                    //效验参数
                    //ASSERT(wDataSize >= sizeof(tagServerKindInfo));
                    //if (wDataSize < sizeof(tagServerKindInfo)) return false;
                    //tagServerKindInfo* pInfo = (tagServerKindInfo*)pBuffer;

                    break;
                }
        }

        return true;
    }

    //系统消息
    bool OnSocketMainSystem(CMD_Command Command, byte[] buffer, WORD wDataSize)
    {
        switch (Command.wSubCmdID)
        {
            case CMD_Plaza.SUB_GP_VERSION:			//版本信息
                {
                    //效验参数
                    //ASSERT(wDataSize>=sizeof(CMD_GP_Version));
                    if (wDataSize < Marshal.SizeOf(typeof(CMD_GP_Version)))
                    {
                        Debuger.Instance.LogError(COMMON_CONST.ErrCommon);
                        return false;
                    }


                    CMD_GP_Version pVersion = SerializationUnit.BytesToStruct<CMD_GP_Version>(buffer);
                    if (mLoginSink != null) mLoginSink.OnSystemInfo(pVersion);
                    //if (pVersion.bAllowConnect == 0)//版本不对，关闭客户端
                    //{

                    //}

                    return true;
                }
        }

        return true;
    }

    //列表信息
    bool OnSocketMainServerList(CMD_Command Command, byte[] buffer, WORD wDataSize)
    {
        switch (Command.wSubCmdID)
        {
            case CMD_Plaza.SUB_GP_LIST_TYPE:			//类型信息
                {
                    int wItemSize = Marshal.SizeOf(typeof(GlobalDef.tagGameType));
                    //效验参数
                    //ASSERT(wDataSize % sizeof(tagGameType) == 0);
                    if (wDataSize % wItemSize != 0)
                    {
                        Debuger.Instance.LogError(COMMON_CONST.ErrCommon);
                        return false;
                    }

                    //处理消息

                    int wItemCount = wDataSize / wItemSize;
                    for (int i = 0; i < wItemCount; i++)
                    {
                        tagGameType pGameType = SerializationUnit.BytesToStruct<tagGameType>(buffer, i * wItemSize);
                        mServerListMgr.AddGameType(pGameType);
                    }

                    return true;
                }
            case CMD_Plaza.SUB_GP_LIST_KIND:			//种类消息
                {
                    //效验参数
                    int wItemSize = Marshal.SizeOf(typeof(GlobalDef.tagGameKind));
                    if (wDataSize % wItemSize != 0) return false;

                    //处理消息

                    int wItemCount = wDataSize / wItemSize;
                    for (int i = 0; i < wItemCount; i++)
                    {
                        tagGameKind pGameKind = SerializationUnit.BytesToStruct<tagGameKind>(buffer, i * wItemSize);
                        mServerListMgr.AddGameKind(pGameKind);
                    }

                    return true;
                }
            case CMD_Plaza.SUB_GP_LIST_PROCESS:		//进程信息
                {
                    //效验参数
                    int wItemSize = Marshal.SizeOf(typeof(GlobalDef.tagGameProcess));
                    if (wDataSize % wItemSize != 0) return false;

                    //处理消息

                    int wItemCount = wDataSize / wItemSize;
                    for (int i = 0; i < wItemCount; i++)
                    {
                        tagGameProcess pGameProcess = SerializationUnit.BytesToStruct<tagGameProcess>(buffer, i * wItemSize);
                        mServerListMgr.AddGameProcess(pGameProcess);
                    }

                    return true;
                }
            //        case CMD_Plaza.SUB_GP_LIST_STATION:		//站点消息
            //            {
            //                //效验参数
            //                ASSERT(wDataSize % sizeof(tagGameStation) == 0);
            //                if (wDataSize % sizeof(tagGameStation) != 0) return false;

            //                //处理消息
            //                tagGameStation* pGameStation = (tagGameStation*)pBuffer;
            //                WORD wItemCount = wDataSize / sizeof(tagGameStation);
            //                g_GlobalUnits.m_ServerListManager.InsertStationItem(pGameStation, wItemCount);

            //                return true;
            //            }
            case CMD_Plaza.SUB_GP_LIST_SERVER:		//服务器房间
                {
                    //效验参数
                    int wItemSize = Marshal.SizeOf(typeof(GlobalDef.tagGameServer));
                    if (wDataSize % wItemSize != 0) return false;

                    //处理消息

                    int wItemCount = wDataSize / wItemSize;

                    //添加游戏服务房间
                    for (int i = 0; i < wItemCount; i++)
                    {
                        tagGameServer pGameServer = SerializationUnit.BytesToStruct<tagGameServer>(buffer, i * wItemSize);
                        mServerListMgr.AddGameServer(pGameServer);
                    }
                    return true;
                }
            case CMD_Plaza.SUB_GP_LIST_FINISH:		//列表发送完成
                {
                    //                //更新人数
                    //                INT_PTR nIndex = 0;
                    //                DWORD dwAllOnLineCount = 0L;
                    //                CListKind* pListKind = NULL;
                    //                do
                    //                {
                    //                    pListKind = g_GlobalUnits.m_ServerListManager.EnumKindItem(nIndex++);
                    //                    if (pListKind == NULL) break;
                    //                    dwAllOnLineCount += pListKind->GetItemInfo()->dwOnLineCount;
                    //                } while (true);
                    //                g_GlobalUnits.m_ServerListManager.UpdateGameOnLineCount(dwAllOnLineCount);
                    //                g_GlobalUnits.SetAllOnLineCount(dwAllOnLineCount);
                    //                //////////////////////////////////////////////////////////////////////////
                    //#if PLAZA_VERSION == SKIN_V4
                    //                //初始化
                    //                m_RoomManager.InitRoomManager(&m_PaintManager);
                    //#elif PLAZA_VERSION == SKIN_V5
                    //            //初始化
                    //            m_GameManager.Init(&m_PaintManager);
                    //#endif
                    //////////////////////////////////////////////////////////////////////////
                    return true;
                }
            //        case CMD_Plaza.SUB_GP_LIST_CONFIG:		//列表配置
            //            {
            //                //效验参数
            //                ASSERT(wDataSize % sizeof(CMD_GP_ListConfig) == 0);
            //                if (wDataSize % sizeof(CMD_GP_ListConfig) != 0) return false;

            //                //处理消息
            //                CMD_GP_ListConfig* pListConfig = (CMD_GP_ListConfig*)pBuffer;

            //                return true;
            //            }
            //        case CMD_Plaza.SUB_GP_LIST_JOINGAME:		//加入游戏列表
            //            {
            //                //效验参数
            //                ASSERT(wDataSize % sizeof(tagJoinGameInfo) == 0);
            //                if (wDataSize % sizeof(tagJoinGameInfo) != 0) return false;

            //                //处理消息
            //                tagJoinGameInfo* pListJoinGame = (tagJoinGameInfo*)pBuffer;
            //                WORD wItemCount = wDataSize / sizeof(tagJoinGameInfo);
            //                g_GlobalUnits.m_ServerListManager.ResetJoinGameItem();
            //                g_GlobalUnits.m_ServerListManager.InsertJoinGameItem(pListJoinGame, wItemCount);

            //                return true;
            //            }
        }

        return true;
    }
    #endregion

    #region 游戏房间网络消息
    //发送房间规则
    //bool SendUserRulePacket()
    //{
    //    //构造数据包
    //    CGameOption* pGameOpt = g_GlobalOption.GetGameOption(m_pListServer->GetListKind());
    //    CServerOption* pSrvOpt = g_GlobalOption.GetServerOption(m_pListServer);
    //    CMD_GR_UserRule UserRule;
    //    UserRule.bLimitWin=pGameOpt->m_bLimitWin;
    //    UserRule.bLimitFlee=pGameOpt->m_bLimitFlee;
    //    UserRule.wWinRate=pGameOpt->m_wWinRate;
    //    UserRule.wFleeRate=pGameOpt->m_wFleeRate;
    //    UserRule.lMaxScore=pGameOpt->m_lMaxScore;
    //    UserRule.lLessScore	=pGameOpt->m_lLessScore;
    //    UserRule.bLimitScore=pGameOpt->m_bLimitScore;
    //    UserRule.bPassword=pSrvOpt->m_bPassword;
    //    UserRule.bCheckSameIP=g_GlobalOption.m_bCheckSameIP;
    //    lstrcpyn(UserRule.szPassword,pSrvOpt->m_szPassword,CountArray(UserRule.szPassword));

    //    //发送数据包
    //    m_ClientSocket->SendData(MDM_GR_USER,SUB_GR_USER_RULE,&UserRule,sizeof(UserRule));

    //    return true;
    //}
    //发送游戏登录命令    
    bool OnSocketGameMainLogon(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        switch (Command.wSubCmdID)
        {
            case CMD_Game.SUB_GR_LOGON_SUCCESS:		//登录成功
                {
                    //设置变量
                    m_ServiceStatus = enServiceStatus.ServiceStatus_RecvConfigInfo;
                    CMD_GR_LogonSuccess cmd = SerializationUnit.BytesToStruct<CMD_GR_LogonSuccess>(pBuffer);
                    if (mGameRoomSink != null) mGameRoomSink.OnLoginSuccess(cmd);
                    return true;
                }
            case CMD_Game.SUB_GR_LOGON_ERROR:		//登录失败
                {
                    //效验参数
                    CMD_GR_LogonError pLogonError = SerializationUnit.BytesToStruct<CMD_GR_LogonError>(pBuffer);

                    if (wDataSize < (Marshal.SizeOf(typeof(CMD_GR_LogonError)) - pLogonError.szErrorDescribe.Length)) return false;

                    //关闭连接
                    mSocket.Disconnect();
                    //CloseStatusWnd();//DUI_CLOSE_AVALID_WND(m_StatusWnd);
                    //pIClientSocke->CloseSocket(false);
                    m_ServiceStatus = enServiceStatus.ServiceStatus_NetShutDown;

                    //显示消息
                    //WORD wDescribeSize=wDataSize-(sizeof(CMD_GR_LogonError)-sizeof(pLogonError->szErrorDescribe));
                    //if (wDescribeSize>0)
                    //{
                    //    pLogonError->szErrorDescribe[wDescribeSize-1]=0;
                    //    ShowMessageBox(pLogonError->szErrorDescribe,NULL,NOT_MODAL_WND,true,10,IDOK);
                    //}

                    ////关闭房间
                    //CloseRoomItem(false);
                    //m_bIsServerLogoing =false;

                    if (mGameRoomSink != null) mGameRoomSink.OnLoginError(pLogonError);
                    return true;
                }
            case CMD_Game.SUB_GR_LOGON_FINISH:		//登录完成
                {
                    //发送规则
                    //SendUserRulePacket();

                    //关闭提示

                    //设置变量
                    m_ServiceStatus = enServiceStatus.ServiceStatus_Serviceing;

                    //重入判断
                    if (m_pMeUserItem != null)
                    {
                        tagUserData pUserData = m_pMeUserItem.GetUserData();
                        if (pUserData.wTableID != (WORD)GlobalDef.Deinfe.INVALID_TABLE)
                        {
                            int iResult = StartGameClient();
                        }
                        else
                        {

                        }
                    }
                    //m_bIsServerLogoing =false;
                    return true;
                }
        }

        return true;
    }
    //用户消息
    bool OnSocketMainUser(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        ASSERT(Command.wMainCmdID == CMD_Game.MDM_GR_USER);
        bool temp;
        switch (Command.wSubCmdID)
        {
            case CMD_Game.SUB_GR_USER_COME:			//用户进入
                {
                    temp = OnSocketSubUserCome(Command, pBuffer, wDataSize);
                    if (!temp)
                    {
                        Debuger.Instance.LogError(string.Format("{0}", Command.wSubCmdID));
                    }
                    return temp;
                }
            case CMD_Game.SUB_GR_USER_STATUS:		//用户状态
                {
                    temp = OnSocketSubStatus(Command, pBuffer, wDataSize);
                    if (!temp)
                    {
                        Debuger.Instance.LogError(string.Format("{0}", Command.wSubCmdID));
                    }
                    return temp;
                }
            case CMD_Game.SUB_GR_USER_SCORE:			//用户分数
                {
                    temp = OnSocketSubScore(Command, pBuffer, wDataSize);
                    if (!temp)
                    {
                        Debuger.Instance.LogError(string.Format("{0}", Command.wSubCmdID));
                    }
                    return temp;
                }
            case CMD_Game.SUB_GR_SIT_FAILED:			//坐下失败
                {
                    temp = OnSocketSubSitFailed(Command, pBuffer, wDataSize);
                    if (!temp)
                    {
                        Debuger.Instance.LogError(string.Format("{0}", Command.wSubCmdID));
                    }
                    return temp;
                }
            case CMD_Game.SUB_GR_USER_CHAT:			//用户聊天
                {
                    temp = OnSocketSubChat(Command, pBuffer, wDataSize);
                    if (!temp)
                    {
                        Debuger.Instance.LogError(string.Format("{0}", Command.wSubCmdID));
                    }
                    return temp;
                }
            case CMD_Game.SUB_GR_USER_WISPER:		//用户私语
                {
                    temp = OnSocketSubWisper(Command, pBuffer, wDataSize);
                    if (!temp)
                    {
                        Debuger.Instance.LogError(string.Format("{0}", Command.wSubCmdID));
                    }
                    return temp;
                }
            case CMD_Game.SUB_GR_USER_INVITE:		//邀请玩家
                {
                    temp = OnSocketSubUserInvite(Command, pBuffer, wDataSize);
                    if (!temp)
                    {
                        Debuger.Instance.LogError(string.Format("{0}", Command.wSubCmdID));
                    }
                    return temp;
                }
            case CMD_Game.SUB_GR_SEND_WARNING:       //发送警告
                {
                    temp = OnSocketSubUserWarning(Command, pBuffer, wDataSize);
                    if (!temp)
                    {
                        Debuger.Instance.LogError(string.Format("{0}", Command.wSubCmdID));
                    }
                    return temp;
                }
            case CMD_Game.SUB_GR_SET_USER_RIGHT:     //用户权限
                {
                    temp = OnSocketSubUserRight(Command, pBuffer, wDataSize);
                    if (!temp)
                    {
                        Debuger.Instance.LogError(string.Format("{0}", Command.wSubCmdID));
                    }
                    return temp;
                }
            case CMD_Game.SUB_GR_USER_GOLD:			//用户金币
                {
                    temp = OnSocketSubUserGold(Command, pBuffer, wDataSize);
                    if (!temp)
                    {
                        Debuger.Instance.LogError(string.Format("{0}", Command.wSubCmdID));
                    }
                    return temp;
                }
            case CMD_Game.SUB_GR_USER_WAITING:		//用户等待
                {
                    temp = OnSocketSubUserWaiting(Command, pBuffer, wDataSize);
                    if (!temp)
                    {
                        Debuger.Instance.LogError(string.Format("{0}", Command.wSubCmdID));
                    }
                    return temp;
                }
        }

        return true;
    }
    //配置消息
    bool OnSocketMainInfo(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        ASSERT(Command.wMainCmdID == CMD_Game.MDM_GR_INFO);
        switch (Command.wSubCmdID)
        {
            case CMD_Game.SUB_GR_SERVER_INFO:	//房间信息
                {
                    //效验数据
                    ASSERT(wDataSize >= Marshal.SizeOf(typeof(CMD_GR_ServerInfo)));
                    if (wDataSize < Marshal.SizeOf(typeof(CMD_GR_ServerInfo))) return false;

                    //消息处理
                    CMD_GR_ServerInfo pServerInfo = SerializationUnit.BytesToStruct<CMD_GR_ServerInfo>(pBuffer);

                    //设置变量
                    m_wGameGenre = pServerInfo.wGameGenre;
                    m_cbMatchMode = pServerInfo.cbMatchMode;
                    //m_dwVideoAddr=pServerInfo.dwVideoAddr;
                    m_cbHideUserInfo = pServerInfo.cbHideUserInfo == 1 ? true : false;
                    m_cbSysAllotChair = pServerInfo.cbSysAllotChair;
                    m_cbDistributeMode = pServerInfo.cbDistributeMode;
                    m_cbGrant = pServerInfo.cbGrant;
                    m_dwAreaType = pServerInfo.dwAreaType;
                    m_lGrantScore = pServerInfo.lGrantScore;
                    m_dwGrantInterval = pServerInfo.dwGrantInterval;

                    //lstrcpyn(m_szMatchRule,TEXT("www.baidu.com"),CountArray(m_szMatchRule));
                    //lstrcpyn(m_szMoreAwardUrl,TEXT("www.baidu.com"),CountArray(m_szMoreAwardUrl));
                    //lstrcpyn(m_szMatchDateDescribe,pServerInfo.szMatchDateDescribe,CountArray(pServerInfo.szMatchDateDescribe));

                    //获取信息
                    //CListKind * pListKind=m_pListServer->GetListKind();
                    //tagGameKind * pGameKind=pListKind->GetItemInfo();

                    //获取游戏进程名，即资源文件夹名
                    //TCHAR szDirectory[MAX_PATH]=TEXT("");
                    //TCHAR szIni[MAX_PATH]=TEXT("");
                    //TCHAR szRoomTitle[128]=TEXT("");
                    //GetGameResDirectory(pGameKind,szDirectory,CountArray(szDirectory));
                    //_snprintf(szIni,sizeof(szIni),TEXT("%s\\%s\\Config.INI"),m_szDirectoryPath,szDirectory);
                    //CIni logini(szIni);
                    //WORD wRoomMode=atoi(logini.GetValue("TABLE","roommode"));
                    if (pServerInfo.cbSysAllotChair == 1) m_wDeskStyle = (WORD)DeskStyle.emDeskStyleRand;
                    else if ((m_wGameGenre & GlobalDef.GAME_GENRE_MATCH) != 0) m_wDeskStyle = (WORD)DeskStyle.emDeskStyleMatch;
                    //else if (wRoomMode==ROOM_MODE_100)m_wDeskStyle=(WORD)DeskStyle.emDeskStyleHundred;
                    else if ((m_wGameGenre & GlobalDef.GAME_GENRE_HILADDER) != 0) m_wDeskStyle = (WORD)DeskStyle.emDeskStyleHiLadder;
                    else m_wDeskStyle = (WORD)DeskStyle.emDeskStyleNormal;

                    if ((m_wGameGenre & GlobalDef.GAME_GENRE_HILADDER) != 0)
                    {
                        //请求天梯用户信息
                        //SendHiLadderMeUserInfoPacket();
                        //请求天梯用户列表信息
                        //SetTimer(IDI_UPDATE_HLRANKLIST,TIME_UPDATE_HLRANKLIST);
                    }

                    //创建桌子
                    try
                    {
                        //if (m_DlgDesk.GetHWND())
                        //{
                        //    CDeskView* pDeskView = m_DlgDesk.CreateDeskView(this,m_wDeskStyle,
                        //        szDirectory,pServerInfo.wTableCount,pServerInfo.wChairCount);
                        //    if(!pDeskView) throw 0;

                        //    _snprintf(szRoomTitle,sizeof(szRoomTitle),TEXT(" %s > %s"),
                        //        pListKind->GetItemInfo()->szKindName,
                        //        m_pListServer->GetItemInfo()->szServerName);
                        //    m_DlgDesk.SetRoomTitle(szRoomTitle);

                        //}
                        if (mGameRoomSink != null) mGameRoomSink.OnServerConfigInfo(pServerInfo);
                    }
                    catch (System.Exception e)
                    {
                        //关闭网络
                        mSocket.Disconnect(false);
                        //m_ClientSocket->CloseSocket(false);

                        //关闭提示
                        //CloseStatusWnd();//DUI_CLOSE_AVALID_WND(m_StatusWnd);

                        //提示消息

                        //关闭房间
                        //CloseRoomItem(true);

                        return false;
                    }

                    return true;
                }
            case CMD_Game.SUB_GR_COLUMN_INFO:	//列表解释
                {
                    //变量定义
                    CMD_GR_ColumnInfo pColumnInfo = SerializationUnit.BytesToStruct<CMD_GR_ColumnInfo>(pBuffer);
                    //WORD wHeadSize=sizeof(CMD_GR_ColumnInfo)-sizeof(pColumnInfo->ColumnItem);

                    ////效验参数
                    //ASSERT(wDataSize>=wHeadSize);
                    //ASSERT((wHeadSize+pColumnInfo->wColumnCount*sizeof(pColumnInfo->ColumnItem[0]))==wDataSize);
                    //if (wDataSize<wHeadSize) return false;
                    //if ((wHeadSize+pColumnInfo->wColumnCount*sizeof(pColumnInfo->ColumnItem[0]))!=wDataSize) return false;

                    ////设置列表
                    //CopyMemory(&m_ListColumnInfo,pColumnInfo,__min(wDataSize,sizeof(m_ListColumnInfo)));
                    //if (m_DlgPlazaR.GetHWND()!=NULL)
                    //    m_DlgPlazaR.SetColumnDescribe(pColumnInfo->ColumnItem,pColumnInfo->wColumnCount);
                    if (mGameRoomSink != null) mGameRoomSink.OnColumnConfigInfo(pColumnInfo);

                    return true;
                }
            case CMD_Game.SUB_GR_CONFIG_FINISH:	//配置完成
                {
                    //设置变量
                    m_ServiceStatus = enServiceStatus.ServiceStatus_RecvRoomInfo;
                    if (mGameRoomSink != null) mGameRoomSink.OnServerConfigFinish();

                    return true;
                }
        }

        return true;
    }
    //状态消息
    bool OnSocketMainStatus(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        ASSERT(Command.wMainCmdID == CMD_Game.MDM_GR_STATUS);

        switch (Command.wSubCmdID)
        {
            case CMD_Game.SUB_GR_TABLE_INFO:		//桌子信息
                {
                    //变量定义
                    CMD_GR_TableInfo pTableInfo = SerializationUnit.BytesToStruct<CMD_GR_TableInfo>(pBuffer);
                    //const WORD wHeadSize=sizeof(CMD_GR_TableInfo)-sizeof(pTableInfo->TableStatus);

                    ////效验数据
                    //ASSERT(wDataSize>=wHeadSize);
                    //ASSERT((sizeof(CMD_GR_TableInfo)-sizeof(pTableInfo->TableStatus)+pTableInfo->wTableCount*sizeof(pTableInfo->TableStatus[0]))==wDataSize);

                    //if (wDataSize<wHeadSize) return false;
                    //if ((wHeadSize+pTableInfo->wTableCount*sizeof(pTableInfo->TableStatus[0]))!=wDataSize) return false;

                    ////消息处理
                    //for (WORD i=0;i<pTableInfo->wTableCount;i++)
                    //{
                    //    pDeskListUI->SetPassFlag(i,pTableInfo->TableStatus[i].bTableLock?true:false);
                    //    pDeskListUI->SetPlayFlag(i,pTableInfo->TableStatus[i].bPlayStatus?true:false);
                    //    pDeskListUI->SetPass(i,pTableInfo->TableStatus[i].szPassword);
                    //}

                    if (mGameRoomSink != null) mGameRoomSink.OnTableInfo(pTableInfo);

                    return true;
                }
            case CMD_Game.SUB_GR_TABLE_STATUS:	//桌子状态
                {
                    //效验数据
                    ASSERT(wDataSize >= Marshal.SizeOf(typeof(CMD_GR_TableStatus)));
                    if (wDataSize < Marshal.SizeOf(typeof(CMD_GR_TableStatus))) return false;

                    //消息处理
                    CMD_GR_TableStatus pTableStatus = SerializationUnit.BytesToStruct<CMD_GR_TableStatus>(pBuffer);
                    //ASSERT(pTableStatus->wTableID<pDeskListUI->GetTableCount());
                    //if (pTableStatus->wTableID<pDeskListUI->GetTableCount())
                    //{
                    //    //设置用户
                    //    IUserItem pIUserItem=null;
                    //    tagUserData pUserData=new tagUserData();
                    //    BYTE cbUserStatus = pTableStatus.bPlayStatus == 1 ? 
                    //        (byte)GlobalDef.enUserStatus.US_PLAY : (byte)GlobalDef.enUserStatus.US_SIT;
                    //    for(int i=0;i<m_ClientUserManager.GetOnLineCount();i++)
                    //    {
                    //        pIUserItem = m_ClientUserManager.EnumUserItem((WORD)i);
                    //        if(pIUserItem !=null)
                    //        {
                    //            if(pIUserItem.GetUserData().wTableID == pTableStatus.wTableID)
                    //            {
                    //                if(pIUserItem.GetUserData().cbUserStatus != (byte)GlobalDef.enUserStatus.US_OFFLINE)
                    //                {
                    //                    tagUserStatus status ;
                    //                    status.wTableID = pIUserItem.GetUserData().wTableID;
                    //                    status.wChairID = pIUserItem.GetUserData().wChairID;
                    //                    status.cbUserStatus = cbUserStatus;

                    //                    pIUserItem.SetUserStatus(status);
                    //                }
                    //            }
                    //        }
                    //    }

                    //for (WORD i=0;i<pDeskListUI->GetChairCount();i++)
                    //{
                    //    pIUserItem=pDeskListUI->GetUserInfo(pTableStatus->wTableID,i);
                    //    if (pIUserItem!=NULL)
                    //    {
                    //        pUserData=pIUserItem->GetUserData();
                    //        if (pUserData->cbUserStatus!=US_OFFLINE) 
                    //        {
                    //            pUserData->cbUserStatus=pUserData->cbUserStatus;
                    //            OnUserItemUpdate(pIUserItem);
                    //        }
                    //    }
                    //}

                    ////设置桌子
                    //pDeskListUI->SetPlayFlag(pTableStatus->wTableID,pTableStatus->bPlayStatus?true:false);
                    //pDeskListUI->SetPassFlag(pTableStatus->wTableID,pTableStatus->bTableLock?true:false);
                    //pDeskListUI->SetPass(pTableStatus->wTableID,pTableStatus->szPassword);
                    //}

                    if (mGameRoomSink != null) mGameRoomSink.OnTableStatus(pTableStatus);

                    return true;
                }
        }

        return true;
    }
    //系统消息
    bool OnSocketGameMainSystem(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        ASSERT(Command.wMainCmdID == CMD_Game.MDM_GR_SYSTEM);
        switch (Command.wSubCmdID)
        {
            case CMD_Game.SUB_GR_MESSAGE:		//系统消息
                {
                    //效验参数
                    CMD_GR_Message pMessage = SerializationUnit.BytesToStruct<CMD_GR_Message>(pBuffer);
                    //ASSERT(wDataSize>(sizeof(CMD_GR_Message)-sizeof(pMessage->szContent)));
                    //if (wDataSize<=(sizeof(CMD_GR_Message)-sizeof(pMessage->szContent))) return false;

                    ////消息处理
                    //WORD wHeadSize=sizeof(CMD_GR_Message)-sizeof(pMessage->szContent);
                    //ASSERT(wDataSize==(wHeadSize+pMessage->wMessageLength*sizeof(TCHAR)));
                    //if (wDataSize!=(wHeadSize+pMessage->wMessageLength*sizeof(TCHAR))) return false;
                    //pMessage->szContent[pMessage->wMessageLength-1]=0;

                    ////关闭房间
                    //if(pMessage->wMessageType&SMT_TIME_CLOSE_ROOM)	
                    //{
                    //    SendProcessData(IPC_MAIN_USER,IPC_SUB_CLOSE_ROOM,pBuffer,wDataSize);
                    //    return true;
                    //}
                    ////关闭连接
                    //bool bIntermet=false;
                    //if (pMessage->wMessageType&SMT_INTERMIT_LINE) bIntermet=true;
                    //else if (pMessage->wMessageType&SMT_CLOSE_ROOM) bIntermet=true;
                    //if (bIntermet==true) 
                    //{
                    //    m_bIsServerLogoing = false;
                    //    //关闭提示
                    //    CloseStatusWnd();//DUI_CLOSE_AVALID_WND(m_StatusWnd);

                    //    m_ClientSocket->CloseSocket(false);
                    //    CloseGameClient();
                    //}

                    ////显示消息
                    //USES_CONVERSION;
                    //if (pMessage->wMessageType&SMT_INFO) 
                    //{
                    //    SYSMESSAGE_HANDLER.InsertSysMessage(string("系统消息:")+string(T2A(pMessage->szContent)));
                    //    if (m_DlgPlazaR.GetHWND()!=NULL && m_DlgPlazaR.GetMessageProxy().GetInterface() )
                    //    {
                    //        m_DlgPlazaR.GetMessageProxy()->InsertSystemString(pMessage->szContent,MS_NORMAL);
                    //    }
                    //}
                    //if (pMessage->wMessageType&SMT_EJECT) ShowMessageBox(pMessage->szContent,NULL,NOT_MODAL_WND,true,10,IDOK);

                    //关闭房间
                    if ((pMessage.wMessageType & CMD_Game.SMT_CLOSE_ROOM) != 0)
                    {
                        mSocket.Disconnect();
                    }
                    ////充值对话框
                    //if (pMessage->wMessageType&SMT_PAY_EJECT) ShowGoldLackWnd(GetHWND(),pMessage->szContent);
                    ////金币过多弹出提示
                    //if (pMessage->wMessageType&SMT_SELECT_ROOM) ShowSelectRoomWnd(GetHWND(),pMessage->szContent);

                    //if (pMessage->wMessageType&SMT_DAILY_TASK_FINISH)
                    //{
                    //    SwitchWindow(SwitchType_DailyTaskWnd);
                    //}

                    if (mGameRoomSink != null) mGameRoomSink.OnSysMessage(pMessage);
                    return true;
                }
        }

        return true;
    }
    //房间消息
    bool OnSocketMainServerInfo(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        ASSERT(Command.wMainCmdID == CMD_Game.MDM_GR_SERVER_INFO);
        switch (Command.wSubCmdID)
        {
            case CMD_Game.SUB_GR_ONLINE_COUNT_INFO:		//在线信息
                {
                    //效验数据
                    ASSERT(wDataSize % Marshal.SizeOf(typeof(tagOnLineCountInfo)) == 0);
                    if (wDataSize % Marshal.SizeOf(typeof(tagOnLineCountInfo)) != 0) return false;

                    //消息处理
                    int wInfoCount = wDataSize % Marshal.SizeOf(typeof(tagOnLineCountInfo));
                    tagOnLineCountInfo[] OnLineCountArr = new tagOnLineCountInfo[wInfoCount];
                    WORD wKindID = 0;
                    DWORD dwKindOnLineCount = 0, dwAllOnLineCount = 0;


                    for (int i = 0; i < wInfoCount; i++)
                    {
                        tagOnLineCountInfo pOnLineCountInfo = SerializationUnit.BytesToStruct<tagOnLineCountInfo>(pBuffer, i * Marshal.SizeOf(typeof(tagOnLineCountInfo)));
                        OnLineCountArr[i] = pOnLineCountInfo;

                        wKindID = OnLineCountArr[i].wKindID;
                        dwKindOnLineCount = OnLineCountArr[i].dwOnLineCount;
                        dwAllOnLineCount += dwKindOnLineCount;
                        mServerListMgr.UpdateGameServerOnLine(wKindID, dwKindOnLineCount);
                        //g_GlobalUnits.m_ServerListManager.UpdateGameKindOnLine(wKindID,dwKindOnLineCount);
                    }

                    //更新总数
                    //g_GlobalUnits.m_ServerListManager.UpdateGameOnLineCount(dwAllOnLineCount);
                    //g_GlobalUnits.SetAllOnLineCount(dwAllOnLineCount);

                    if (mGameRoomSink != null) mGameRoomSink.OnServerInfo(OnLineCountArr);
                    return true;
                }
        }

        return true;
    }

    #region MainUser子命令
    //用户进入
    bool OnSocketSubUserCome(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {

        //效验参数
        ASSERT(Command.wMainCmdID == CMD_Game.MDM_GR_USER);
        ASSERT(Command.wSubCmdID == CMD_Game.SUB_GR_USER_COME);
        ASSERT(wDataSize >= Marshal.SizeOf(typeof(tagUserInfoHead)));
        if (wDataSize < Marshal.SizeOf(typeof(tagUserInfoHead))) return false;

        //读取基本信息
        tagUserData UserData = new tagUserData();

        tagUserInfoHead pUserInfoHead = SerializationUnit.BytesToStruct<tagUserInfoHead>(pBuffer);

        //读取信息
        UserData.dwUserID = pUserInfoHead.dwUserID;
        UserData.wTableID = pUserInfoHead.wTableID;
        UserData.wChairID = pUserInfoHead.wChairID;
        UserData.cbUserStatus = pUserInfoHead.cbUserStatus;
        UserData.dwUserRight = pUserInfoHead.dwUserRight;
        UserData.dwAreaType = pUserInfoHead.dwAreaType;
        UserData.dwMasterRight = pUserInfoHead.dwMasterRight;
        UserData.dwGoldCount = pUserInfoHead.dwGoldCount;
        UserData.dwZZBean = pUserInfoHead.dwZZBean;
        UserData.wUserType = pUserInfoHead.wUserType;
        UserData.dwLotteries = pUserInfoHead.UserScoreInfo.dwLotteries;
        UserData.dwZScore = pUserInfoHead.UserScoreInfo.dwZScore;

        //管理判断
        if ((pUserInfoHead.dwUserID == RefactorData.Instance.NETPLAYER.GlobalUserData.dwUserID) && (pUserInfoHead.cbMasterOrder >= 2))
        {
            m_cbHideUserInfo = false;
        }

        //读取信息
        if ((m_cbHideUserInfo == false) || (pUserInfoHead.dwUserID == RefactorData.Instance.NETPLAYER.GlobalUserData.dwUserID))
        {
            UserData.wFaceID = pUserInfoHead.wFaceID;
            UserData.cbGender = pUserInfoHead.cbGender;
            UserData.cbMemberOrder = pUserInfoHead.cbMemberOrder;
            UserData.cbMasterOrder = pUserInfoHead.cbMasterOrder;
            UserData.dwGameID = pUserInfoHead.dwGameID;
            UserData.dwGroupID = pUserInfoHead.dwGroupID;
            UserData.lScore = pUserInfoHead.UserScoreInfo.lScore;
            UserData.llInsureScore = pUserInfoHead.UserScoreInfo.llInsureScore;
            UserData.lWinCount = pUserInfoHead.UserScoreInfo.lWinCount;
            UserData.lLostCount = pUserInfoHead.UserScoreInfo.lLostCount;
            UserData.lDrawCount = pUserInfoHead.UserScoreInfo.lDrawCount;
            UserData.lFleeCount = pUserInfoHead.UserScoreInfo.lFleeCount;
            UserData.lExperience = pUserInfoHead.UserScoreInfo.lExperience;

        }

        //读取扩展信息
        tagDataDescribe DataDescribe = new tagDataDescribe();
        CRecvPacketHelper RecvPacket = new CRecvPacketHelper(pBuffer,
            (WORD)(wDataSize - Marshal.SizeOf(typeof(tagUserInfoHead))),
            (WORD)Marshal.SizeOf(typeof(tagUserInfoHead)));
        while (true)
        {
            byte[] pDataBuffer = RecvPacket.GetData(ref DataDescribe);
            if (DataDescribe.wDataDescribe == GlobalField.DTP_NULL) break;
            switch (DataDescribe.wDataDescribe)
            {
                case GlobalField.DTP_USER_ACCOUNTS:		//用户帐户
                    {
                        if ((m_cbHideUserInfo == false) || (pUserInfoHead.dwUserID == RefactorData.Instance.NETPLAYER.GlobalUserData.dwUserID))
                        {
                            ASSERT(pDataBuffer != null);
                            UserData.szName = pDataBuffer.ToAnsiString().ToBytes();
                            //ASSERT(DataDescribe.wDataSize<=UserData.szName.Length);
                            //if (DataDescribe.wDataSize<=UserData.szName.Length)
                            //{
                            //    UserData.szName = new byte[GlobalDef.NAME_LEN];
                            //    pDataBuffer.CopyArr(UserData.szName,DataDescribe.wDataSize);
                            //}
                        }
                        else
                        {
                            UserData.szName = "游戏用户".ToBytes();
                        }
                        break;
                    }
                case GlobalField.DTP_UNDER_WRITE:		//个性签名
                    {
                        if ((m_cbHideUserInfo == false) || (pUserInfoHead.dwUserID == RefactorData.Instance.NETPLAYER.GlobalUserData.dwUserID))
                        {
                            ASSERT(pDataBuffer != null);
                            UserData.szUnderWrite = pDataBuffer.ToAnsiString().ToBytes();
                            //ASSERT(DataDescribe.wDataSize<=UserData.szUnderWrite.Length);
                            //if (DataDescribe.wDataSize<=UserData.szUnderWrite.Length)
                            //{
                            //    UserData.szUnderWrite = new byte[GlobalDef.UNDER_WRITE_LEN] ;
                            //    pDataBuffer.CopyArr(UserData.szUnderWrite,DataDescribe.wDataSize);
                            //}
                        }
                        break;
                    }
                case GlobalField.DTP_USER_GROUP_NAME:	//用户社团
                    {
                        if ((m_cbHideUserInfo == false) || (pUserInfoHead.dwUserID == RefactorData.Instance.NETPLAYER.GlobalUserData.dwUserID))
                        {
                            ASSERT(pDataBuffer != null);
                            UserData.szGroupName = pDataBuffer.ToAnsiString().ToBytes();
                            //ASSERT(DataDescribe.wDataSize<=sizeof(UserData.szGroupName));
                            //if (DataDescribe.wDataSize<=sizeof(UserData.szGroupName))
                            //{
                            //    CopyMemory(&UserData.szGroupName,pDataBuffer,DataDescribe.wDataSize);
                            //    UserData.szGroupName[CountArray(UserData.szGroupName)-1]=0;
                            //}
                        }
                        break;
                    }
            }
        }

        //查找用户
        IUserItem pIUserItem = m_ClientUserManager.SearchUserByUserID(UserData.dwUserID);
        if (pIUserItem == null) pIUserItem = m_ClientUserManager.ActiveUserItem(UserData);
        //else OnUserItemUpdate(pIUserItem);
        //加入用户信息
        ASSERT(pIUserItem != null);
        if (pIUserItem != null)
        {
            //m_DlgPlazaR.SortByAccounts();
            //判断自己
            if (m_pMeUserItem == null)
            {
                m_pMeUserItem = pIUserItem;

                //请求定时领取金币
                //if (m_wGameGenre&GAME_GENRE_GOLD&&m_cbGrant)
                //{
                //    m_dwGrantTimeRemain=m_dwGrantInterval/TIME_USER_RECEIVE_REQ;
                //    SetTimer(IDI_USER_RECEIVE_REQ,m_dwGrantInterval);
                //    SetTimer(IDI_USER_RECEIVE_CNT,TIME_USER_RECEIVE_REQ);
                //}
            }

            //设置界面
            //CDeskListUI* pDeskListUI=GetDeskList();
            //if (pDeskListUI==NULL)return false;
            BYTE cbUserStatus = UserData.cbUserStatus;
            //if ((cbUserStatus>=US_SIT)&&(cbUserStatus!=US_LOOKON))
            //	pDeskListUI->SetUserInfo(UserData.wTableID,UserData.wChairID,pIUserItem);

            //提示信息
            if ((m_cbHideUserInfo == false) /*&& (m_ServiceStatus==ServiceStatus_Serviceing) && g_GlobalOption.m_bShowInOutMessage*/)
            {
                string msg = string.Format("{0}进来了", UserData.szName.ToAnsiString());
                if (mGameRoomSink != null) mGameRoomSink.OnShowMessage(msg);
                //if (m_DlgPlazaR.GetHWND()!=NULL)
                //{
                //    if ((UserData.cbCompanion==enCompanion_Friend)/*||(UserData.dwMasterRight!=0L)*/)
                //    {
                //        TCHAR szMessage[256]=TEXT("");
                //        _sntprintf(szMessage,sizeof(szMessage),TEXT("[ %s ] 进来了"),UserData.szName);
                //        m_DlgPlazaR.GetMessageProxy()->InsertSystemString(szMessage,0);
                //    }
                //    else if (g_GlobalOption.m_bShowInOutMessage==true)
                //    {
                //        TCHAR szMessage[256]=TEXT("");
                //        _sntprintf(szMessage,sizeof(szMessage),TEXT("[ %s ] 进来了"),UserData.szName);
                //        m_DlgPlazaR.GetMessageProxy()->InsertGeneralString(szMessage,0,0,true);
                //    }
                //}			
            }
        }

        //更新人数
        DWORD dwOnLineCount = m_ClientUserManager.GetOnLineCount();
        mServerListMgr.UpdateGameServerOnLine(mCurGameServer, dwOnLineCount);
        //    TCHAR szRoomTitle[128]=TEXT("");

        //    g_GlobalUnits.m_ServerListManager.UpdateGameServerOnLine(m_pListServer,dwOnLineCount);
        //// 	_sntprintf(szRoomTitle,sizeof(szRoomTitle),TEXT(" %s > %s (%ld人)"),m_pListServer->GetListKind()->GetItemInfo()->szKindName,
        //// 		m_pListServer->GetItemInfo()->szServerName,dwOnLineCount);
        //    _sntprintf(szRoomTitle,sizeof(szRoomTitle),TEXT(" %s > %s"),m_pListServer->GetListKind()->GetItemInfo()->szKindName,
        //        m_pListServer->GetItemInfo()->szServerName);
        //    m_DlgDesk.SetRoomTitle(szRoomTitle);

        if (mGameRoomSink != null) mGameRoomSink.OnUserCome(UserData);

        return true;
    }
    //用户状态
    bool OnSocketSubStatus(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        //效验参数
        ASSERT(Command.wMainCmdID == CMD_Game.MDM_GR_USER);
        ASSERT(Command.wSubCmdID == CMD_Game.SUB_GR_USER_STATUS);
        ASSERT(wDataSize >= Marshal.SizeOf(typeof(CMD_GR_UserStatus)));
        if (wDataSize < Marshal.SizeOf(typeof(CMD_GR_UserStatus)))
            return false;

        //处理数据
        CMD_GR_UserStatus pUserStatus = SerializationUnit.BytesToStruct<CMD_GR_UserStatus>(pBuffer);
        IUserItem pIUserItem = m_ClientUserManager.SearchUserByUserID(pUserStatus.dwUserID);
        //ASSERT(pIUserItem!=NULL);
        if (pIUserItem == null) return true;

        //定义变量
        tagUserData pUserData = pIUserItem.GetUserData();
        tagUserData pMeUserData = m_pMeUserItem.GetUserData();
        WORD wNowTableID = pUserStatus.wTableID, wLastTableID = pUserData.wTableID;
        WORD wNowChairID = pUserStatus.wChairID, wLastChairID = pUserData.wChairID;
        BYTE cbNowStatus = pUserStatus.cbUserStatus, cbLastStatus = pUserData.cbUserStatus;

        //CDeskListUI* pDeskListUI=GetDeskList();
        //ASSERT(pDeskListUI!=NULL);
        //if (pDeskListUI==NULL)
        //    return false;

        //清理旧状态
        if ((wLastTableID != (WORD)GlobalDef.Deinfe.INVALID_TABLE) && ((wNowTableID != wLastTableID) || (wNowChairID != wLastChairID)))
        {
            ASSERT(wLastChairID != (WORD)GlobalDef.Deinfe.INVALID_TABLE);
            //IUserItem * pITableUserItem=pDeskListUI->GetUserInfo(wLastTableID,wLastChairID);
            //if (pITableUserItem==pIUserItem) pDeskListUI->SetUserInfo(wLastTableID,wLastChairID,NULL);
        }

        //用户离开
        if (cbNowStatus == (byte)GlobalDef.enUserStatus.US_NULL)
        {
            //通知游戏
            //if ((pMeUserData.wTableID!=(WORD)GlobalDef.Deinfe.INVALID_TABLE)&&(pMeUserData.wTableID==wLastTableID))
            //{
            //    IPC_UserStatus UserStatus;
            //    memset(&UserStatus,0,sizeof(UserStatus));
            //    UserStatus.dwUserID=pUserData->dwUserID;
            //    UserStatus.cbUserStatus=pUserData->cbUserStatus;
            //    SendProcessData(IPC_MAIN_USER,IPC_SUB_USER_STATUS,&UserStatus,sizeof(UserStatus));
            //}

            //提示信息
            if ((m_cbHideUserInfo == false) /*&& (m_ServiceStatus==ServiceStatus_Serviceing)*/ )
            {
                string msg = string.Format("{0}离开了", pUserData.szName.ToAnsiString());
                if (mGameRoomSink != null) mGameRoomSink.OnShowMessage(msg);
                //if (m_DlgPlazaR.GetHWND()!=NULL)
                //{
                //    if ((pUserData->cbCompanion==enCompanion_Friend)/*||(UserData.dwMasterRight!=0L)*/)
                //    {
                //        TCHAR szMessage[256]=TEXT("");
                //        _sntprintf(szMessage,sizeof(szMessage),TEXT("[ %s ] 离开了"),pUserData->szName);
                //        m_DlgPlazaR.GetMessageProxy()->InsertSystemString(szMessage,0);
                //    }
                //    else if (g_GlobalOption.m_bShowInOutMessage==true)
                //    {
                //        TCHAR szMessage[256]=TEXT("");
                //        _sntprintf(szMessage,sizeof(szMessage),TEXT("[ %s ] 离开了"),pUserData->szName);
                //        m_DlgPlazaR.GetMessageProxy()->InsertGeneralString(szMessage,0,0,true);
                //    }
                //}

            }

            m_ClientUserManager.DeleteUserItem(pIUserItem);

            //更新人数
            DWORD dwOnLineCount = m_ClientUserManager.GetOnLineCount();
            mServerListMgr.UpdateGameServerOnLine(mCurGameServer, dwOnLineCount);

            return true;
        }

        //更新状态
        tagUserStatus UserStatus;
        UserStatus.wTableID = wNowTableID;
        UserStatus.wChairID = wNowChairID;
        UserStatus.cbUserStatus = cbNowStatus;
        m_ClientUserManager.UpdateUserItemStatus(pIUserItem, UserStatus);

        //设置新状态
        if ((wNowTableID != (WORD)GlobalDef.Deinfe.INVALID_TABLE) && ((wNowTableID != wLastTableID) || (wNowChairID != wLastChairID)))
        {
            //设置桌子
            if (cbNowStatus != (byte)GlobalDef.enUserStatus.US_LOOKON)
            {
                ASSERT(wNowChairID != (WORD)GlobalDef.Deinfe.INVALID_CHAIR);
                //pDeskListUI->SetUserInfo(wNowTableID,wNowChairID,pIUserItem);
            }

            //发送用户
            if ((m_pMeUserItem != pIUserItem) && (pMeUserData.wTableID == wNowTableID))
            {
                //CIPCSendCopyData SendCopyData(m_hWndChannel,GetHWND());
                //SendTableUser(pIUserItem,&SendCopyData);
            }
        }

        //判断发送
        bool bNotifyGame = false;
        if (pIUserItem == m_pMeUserItem) bNotifyGame = true;
        else if ((pMeUserData.wTableID != (WORD)GlobalDef.Deinfe.INVALID_TABLE) && (pMeUserData.wTableID == wNowTableID)) bNotifyGame = true;
        else if ((pMeUserData.wTableID != (WORD)GlobalDef.Deinfe.INVALID_TABLE) && (pMeUserData.wTableID == wLastTableID)) bNotifyGame = true;

        //发送状态
        if (bNotifyGame == true)
        {
            //IPC_UserStatus UserStatus;
            //memset(&UserStatus,0,sizeof(UserStatus));
            //UserStatus.dwUserID=pUserData->dwUserID;
            //UserStatus.cbUserStatus=pUserData->cbUserStatus;
            //SendProcessData(IPC_MAIN_USER,IPC_SUB_USER_STATUS,&UserStatus,sizeof(UserStatus));

        }

        if (mGameRoomSink != null) mGameRoomSink.OnUserStatus(pUserStatus);

        //判断自己
        if (pIUserItem == m_pMeUserItem)
        {
            //设置变量
            if ((wNowTableID == m_wReqTableID) && (wNowChairID == m_wReqChairID))
            {
                m_wReqTableID = (WORD)GlobalDef.Deinfe.INVALID_TABLE;
                m_wReqChairID = (WORD)GlobalDef.Deinfe.INVALID_CHAIR;
            }

            ////缓冲清理
            //if ((m_wPacketTableID != INVALID_TABLE) && ((m_wPacketTableID != wNowTableID) || (m_wPacketChairID != wNowChairID)))
            //{
            //    m_wPacketTableID = INVALID_TABLE;
            //    m_wPacketChairID = INVALID_CHAIR;
            //    m_PacketDataStorage.RemoveData(false);
            //}

            //启动游戏
            if ((wNowTableID != (WORD)GlobalDef.Deinfe.INVALID_TABLE)
                && ((wNowTableID != wLastTableID) || (wNowChairID != wLastChairID))
                || (wNowTableID == (WORD)GlobalDef.Deinfe.INVALID_TABLE && cbNowStatus == (byte)GlobalDef.enUserStatus.US_QUEUING))
            {
                //有效判断
                int iResult = StartGameClient();
                if (iResult == 0)
                    return false;


                //更新最近玩游戏注册
                //UpdatePlayedReg();
            }

            //天梯比赛更新
            if ((m_wGameGenre & GlobalDef.GAME_GENRE_HILADDER) != 0 || (m_wGameGenre & GlobalDef.GAME_GENRE_MATCH) != 0)
            {
                bool bEnable = false;
                bool bContinue = false;
                bool bReChallenge = false;
                bool bExitMatch = false;
                bEnable = (cbNowStatus == (byte)GlobalDef.enUserStatus.US_FREE);
                if ((m_wGameGenre & GlobalDef.GAME_GENRE_HILADDER) != 0)
                {
                    bContinue = (m_meHLUserInfo.dwUserID != 0);
                }
                else if ((m_wGameGenre & GlobalDef.GAME_GENRE_MATCH) != 0)
                {
                    //tagMatchUserInfo * pMUserInfo=m_matchUserManager.SearchUserByUserID(pMeUserData->dwUserID);

                    //if (m_cbMatchMode==en_MTY_Challenge)
                    //{
                    //    if (bEnable)
                    //    {
                    //        bEnable=pMUserInfo->ScoreInfo.dwCurChallengeCount < m_tagChallengeMatchInfo.dwChallengeCount;
                    //    }
                    //    bReChallenge=(pMUserInfo->ScoreInfo.lInitScore<=0);
                    //    bContinue=(bReChallenge?false:true);
                    //}
                    //else if (m_cbMatchMode==en_MTY_Time)
                    //{
                    //    bContinue = (pMUserInfo!=NULL);
                    //    if (bEnable && pMUserInfo)//guopeng add
                    //    {
                    //        bEnable=pMUserInfo->ScoreInfo.dwMatchCurrCnt < m_tagTimeMatchInfo.dwMatchCount;
                    //    }
                    //    //bEnable=pMUserInfo->ScoreInfo.dwMatchCurrCnt < m_tagTimeMatchInfo.dwMatchCount;
                    //}
                    //else if(m_cbMatchMode == en_MTY_Eliminate)
                    //{
                    //    if(GetGameAlive() == true) bEnable = false;
                    //    if(pMeUserData->cbUserStatus>US_FREE)
                    //    {
                    //        bEnable=false; 
                    //        bExitMatch = true;
                    //    }
                    //}
                }
                //pDeskListUI->UpdateTableFrame(m_wDeskStyle,bEnable,bContinue,bReChallenge,bExitMatch);
            }
            else if (m_cbSysAllotChair == 1 && (pMeUserData.cbUserStatus >= (byte)GlobalDef.enUserStatus.US_SIT))
            {
                //pDeskListUI->UpdateTableFrame(m_wDeskStyle,false);
            }

        }

        return true;
    }

    //用户分数
    bool OnSocketSubScore(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        //效验参数
        ASSERT(Command.wMainCmdID == CMD_Game.MDM_GR_USER);
        ASSERT(Command.wSubCmdID == CMD_Game.SUB_GR_USER_SCORE);
        ASSERT(wDataSize >= Marshal.SizeOf(typeof(CMD_GR_UserScore)));
        if (wDataSize < Marshal.SizeOf(typeof(CMD_GR_UserScore))) return false;

        //处理数据
        CMD_GR_UserScore pUserScore = SerializationUnit.BytesToStruct<CMD_GR_UserScore>(pBuffer);

        IUserItem pIUserItem = m_ClientUserManager.SearchUserByUserID(pUserScore.dwUserID);
        //ASSERT(pIUserItem!=NULL);
        if (pIUserItem == null) return true;

        //更新本地数据
        //tagMatchUserInfo* pMatchUser = m_matchUserManager.SearchUserByUserID(pIUserItem->GetUserID());
        //if(m_wGameGenre&GAME_GENRE_MATCH 
        //	&& m_cbMatchMode == en_MTY_Time
        //	&& pMatchUser
        //	&& pMatchUser->dwMatchID==m_tagTimeMatchInfo.dwMatchID 
        //	&& m_tagTimeMatchInfo.cbMatchStatus==MS_PLAYING
        //	)
        //{
        //	pMatchUser->ScoreInfo.lScore = pUserScore->UserScore.lScore;
        //}

        //更新判断
        if ((m_cbHideUserInfo == false) || (pIUserItem == m_pMeUserItem))
        {
            //更新分数
            m_ClientUserManager.UpdateUserItemScore(pIUserItem, pUserScore.UserScore);

            //更新游戏
            tagUserData pUserData = pIUserItem.GetUserData();
            tagUserData pMeUserData = m_pMeUserItem.GetUserData();
            tagGlobalUserData GlobalUserData = RefactorData.Instance.NETPLAYER.GlobalUserData;

            //数据同步
            if (pMeUserData.dwUserID == pUserScore.dwUserID)
            {
                if ((m_wGameGenre & GlobalDef.GAME_GENRE_GOLD) != 0)
                {
                    GlobalUserData.dwGoldCount = (DWORD)pUserScore.UserScore.lScore;
                    GlobalUserData.llInsureScore = pUserScore.UserScore.llInsureScore;
                }
                else
                {
                    GlobalUserData.dwGoldCount = (DWORD)pUserScore.UserScore.dwGoldCount;
                    GlobalUserData.llInsureScore = pUserScore.UserScore.llInsureScore;
                }

                GlobalUserData.dwLotteries = pUserScore.UserScore.dwLotteries;
                GlobalUserData.dwExperience = (DWORD)pUserScore.UserScore.lExperience;
                GlobalUserData.dwZScore = pUserScore.UserScore.dwZScore;
                GlobalUserData.wFaceID = pUserScore.wFaceID;

                m_pMeUserItem.SetFaceId(pUserScore.wFaceID);
                pIUserItem.SetFaceId(pUserScore.wFaceID);
                pIUserItem.SetUserGold(pUserScore.UserScore.dwGoldCount);
                pIUserItem.SetZScore(pUserScore.UserScore.dwZScore);

                //UpdateUserInfo();
            }
            //if(GetDeskList()&&pIUserItem&&pUserData&&pUserData->wTableID != INVALID_TABLE&&pUserData->wChairID != INVALID_CHAIR)
            //{
            //    //刷新头像
            //    GetDeskList()->SetUserInfo(pUserData->wTableID,pUserData->wChairID,pIUserItem);
            //}

            //bool bNormalSend=pMeUserData->wTableID!=INVALID_TABLE && pMeUserData->wTableID==pUserData->wTableID;

            //if(bNormalSend)
            //{
            //    IPC_UserScore UserScore;
            //    memset(&UserScore,0,sizeof(UserScore));
            //    UserScore.dwUserID=pUserScore->dwUserID;
            //    UserScore.wFaceID =pUserScore->wFaceID;
            //    UserScore.UserScore=pUserScore->UserScore;
            //    GlobalUserData.dwExperience=pMeUserData->lExperience;
            //    SendProcessData(IPC_MAIN_USER,IPC_SUB_USER_SCORE,&UserScore,sizeof(UserScore));
            //}

            if (mGameRoomSink != null) mGameRoomSink.OnUserScore(pUserScore);
        }

        return true;
    }

    //坐下失败
    bool OnSocketSubSitFailed(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        //效验参数
        ASSERT(Command.wMainCmdID == CMD_Game.MDM_GR_USER);
        ASSERT(Command.wSubCmdID == CMD_Game.SUB_GR_SIT_FAILED);

        //CDeskListUI* pDeskListUI=GetDeskList();
        //if (pDeskListUI==NULL)return false;

        //消息处理
        CMD_GR_SitFailed pSitFailed = SerializationUnit.BytesToStruct<CMD_GR_SitFailed>(pBuffer);
        if (m_wReqTableID != (WORD)GlobalDef.Deinfe.INVALID_TABLE && m_wReqChairID != (WORD)GlobalDef.Deinfe.INVALID_CHAIR)
        {
            //IUserItem * pISendUserItem=pDeskListUI->GetUserInfo(m_wReqTableID,m_wReqChairID);
            //if (pISendUserItem==m_pMeUserItem) pDeskListUI->SetUserInfo(m_wReqTableID,m_wReqChairID,NULL);
        }

        //设置变量
        m_wReqTableID = (WORD)GlobalDef.Deinfe.INVALID_TABLE;
        m_wReqChairID = (WORD)GlobalDef.Deinfe.INVALID_CHAIR;

        //显示消息
        //    if (pSitFailed->bHyperLink)
        //    {
        //#if    PLAZA_VERSION == SKIN_V5
        //        if(ReturnPlaza())
        //        {
        //            m_GameManager.SwitchLayer(em_GameLayer);
        //            m_GameManager.SetGameLayer(Layer_Server);
        //        }
        //#elif  PLAZA_VERSION == SKIN_V4		
        //        ReturnPlaza();
        //#endif
        //     m_DlgLowScore.InitLowScoreInfo(pSitFailed->szFailedDescribe); 
        //    }
        //    else
        //    {
        //        ShowMessageBox(pSitFailed->szFailedDescribe,NULL,MODAL_WND,true,10,IDOK);
        //    }

        //pDeskListUI->UpdateTableFrame(m_wDeskStyle,true);

        if (mGameRoomSink != null) mGameRoomSink.OnSitFaild(pSitFailed);

        return true;
    }

    //用户聊天
    bool OnSocketSubChat(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        //效验参数
        ASSERT(Command.wMainCmdID == CMD_Game.MDM_GR_USER);
        ASSERT(Command.wSubCmdID == CMD_Game.SUB_GR_USER_CHAT);

        //效验参数
        CMD_GR_UserChat pUserChat = SerializationUnit.BytesToStruct<CMD_GR_UserChat>(pBuffer);
        //ASSERT(wDataSize>=(Marshal.SizeOf(typeof(CMD_GR_UserChat) )-sizeof(pUserChat->szChatMessage)));
        //ASSERT(wDataSize==(sizeof(CMD_GR_UserChat)-sizeof(pUserChat->szChatMessage)+pUserChat->wChatLength));
        //if (wDataSize<(sizeof(CMD_GR_UserChat)-sizeof(pUserChat->szChatMessage))) return false;
        //if (wDataSize!=(sizeof(CMD_GR_UserChat)-sizeof(pUserChat->szChatMessage)+pUserChat->wChatLength)) return false;

        //寻找用户
        IUserItem pISendUserItem = m_ClientUserManager.SearchUserByUserID(pUserChat.dwSendUserID);
        if (pISendUserItem == null) return true;
        //tagUserData pSendUserData=pISendUserItem.GetUserData();

        //消息过滤
        //if ((pISendUserItem!=m_pMeUserItem)&&(pSendUserData->cbCompanion==enCompanion_Detest)) return true;
        //if (m_DlgPlazaR.GetHWND()==NULL) return true;

        //显示消息
        //if (pUserChat.dwTargetUserID!=0L)
        //{
        //    IUserItem * pIRecvUserItem=m_ClientUserManager.SearchUserByUserID(pUserChat->dwTargetUserID);
        //    if (pIRecvUserItem==NULL) return true;
        //    tagUserData * pRecvUserData=pIRecvUserItem->GetUserData();

        //    if(pISendUserItem->GetGameID()==m_pMeUserItem->GetGameID())
        //    {
        //        m_DlgPlazaR.GetMessageProxy()->InsertUserChat(_TEXT("你"),\
        //            pRecvUserData->szName,pUserChat->szChatMessage,pUserChat->crFontColor,MS_NORMAL);
        //    }
        //    else
        //    {
        //        if(pIRecvUserItem->GetGameID()==m_pMeUserItem->GetGameID())
        //            m_DlgPlazaR.GetMessageProxy()->InsertUserChat(pSendUserData->szName,_TEXT("你"),\
        //            pUserChat->szChatMessage,pUserChat->crFontColor,MS_NORMAL);
        //        else m_DlgPlazaR.GetMessageProxy()->InsertUserChat(pSendUserData->szName,\
        //            pRecvUserData->szName,pUserChat->szChatMessage,pUserChat->crFontColor,MS_NORMAL);
        //    }
        //}
        //else 
        //{
        //    m_DlgPlazaR.GetMessageProxy()->InsertUserChat(pSendUserData->szName,\
        //        pUserChat->szChatMessage,pUserChat->crFontColor,MS_NORMAL);
        //}

        if (mGameRoomSink != null) mGameRoomSink.OnUserChat(pUserChat);

        return true;
    }

    //用户私语
    bool OnSocketSubWisper(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        //效验参数
        //ASSERT(Command.wMainCmdID==MDM_GR_USER);
        //ASSERT(Command.wSubCmdID==SUB_GR_USER_WISPER);

        //效验参数
        CMD_GR_Wisper pWisper = SerializationUnit.BytesToStruct<CMD_GR_Wisper>(pBuffer);
        //ASSERT(wDataSize>=(sizeof(CMD_GR_Wisper)-sizeof(pWisper->szChatMessage)));
        //ASSERT(wDataSize==(sizeof(CMD_GR_Wisper)-sizeof(pWisper->szChatMessage)+pWisper->wChatLength));
        //if (wDataSize<(sizeof(CMD_GR_Wisper)-sizeof(pWisper->szChatMessage))) return false;
        //if (wDataSize!=(sizeof(CMD_GR_Wisper)-sizeof(pWisper->szChatMessage)+pWisper->wChatLength)) return false;

        //寻找用户
        IUserItem pISendUserItem = m_ClientUserManager.SearchUserByUserID(pWisper.dwSendUserID);
        IUserItem pIRecvUserItem = m_ClientUserManager.SearchUserByUserID(pWisper.dwTargetUserID);
        if ((pISendUserItem == null) || (pIRecvUserItem == null)) return true;
        tagUserData pUserDataSend = pISendUserItem.GetUserData();
        tagUserData pUserDataRecv = pIRecvUserItem.GetUserData();

        ////显示信息
        //CShortMessage * pShortMessageWnd=NULL;
        //if (pWisper->dwSendUserID==m_pMeUserItem->GetUserID())
        //{
        //	//自己发送的消息
        //	pShortMessageWnd=ActiveShortWnd(pWisper->dwTargetUserID,pISendUserItem,pIRecvUserItem,true);
        //	if (pShortMessageWnd!=NULL) pShortMessageWnd->OnRecvMessage(pUserDataSend->szName,pWisper->szChatMessage,pWisper->crFontColor,pWisper->cfSendFont,true);
        //}
        //else
        //{
        //	pShortMessageWnd=ActiveShortWnd(pWisper->dwSendUserID,pIRecvUserItem,pISendUserItem,true);
        //	if (pShortMessageWnd!=NULL)	pShortMessageWnd->OnRecvMessage(pUserDataSend->szName,pWisper->szChatMessage,pWisper->crFontColor,pWisper->cfSendFont,false);
        //}

        if (mGameRoomSink != null) mGameRoomSink.OnUserWisper(pWisper);

        return true;
    }

    //用户邀请
    bool OnSocketSubUserInvite(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        //效验参数
        //ASSERT(Command.wMainCmdID==MDM_GR_USER);
        //ASSERT(Command.wSubCmdID==SUB_GR_USER_INVITE);

        //效验参数
        ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_GR_UserInvite)));
        if (wDataSize != Marshal.SizeOf(typeof(CMD_GR_UserInvite))) return false;

        //CDeskListUI* pDeskListUI=GetDeskList();
        //if (pDeskListUI==NULL)return false;

        //消息处理
        CMD_GR_UserInvite pUserInvite = SerializationUnit.BytesToStruct<CMD_GR_UserInvite>(pBuffer);
        //ASSERT(pUserInvite->wTableID<pDeskListUI->GetTableCount());
        //if (pDeskListUI->QueryPlayFlag(pUserInvite->wTableID)==true) return true;

        //寻找用户
        IUserItem pIUserItem = m_ClientUserManager.SearchUserByUserID(pUserInvite.dwUserID);
        if (pIUserItem == null) return true;
        tagUserData pUserData = pIUserItem.GetUserData();
        if (pUserData.wTableID == (WORD)GlobalDef.Deinfe.INVALID_TABLE) return true;

        //用户过虑
        //if (pUserData->cbCompanion==enCompanion_Detest) return true;
        //if (g_GlobalOption.m_InviteMode==enInviteMode_None) return true;
        //if ((g_GlobalOption.m_InviteMode==enInviteMode_Friend)&&(pUserData->cbCompanion!=enCompanion_Friend)) return true;

        //提示消息
        //TCHAR szMessage[256]=TEXT("");
        //tagGameServer * pGameServer=m_pListServer->GetItemInfo();
        //_snprintf(szMessage,sizeof(szMessage),TEXT("[ %s ] 邀请你加入第 [ %ld ] 游戏桌进行游戏，是否同意？"),pUserData->szName,pUserInvite->wTableID+1);
        //if (ShowMessageBox(szMessage,NULL,MODAL_WND,false,0)==IDOK)
        //{
        //    WORD wChairID=INVALID_CHAIR;
        //    WORD wNullCount=pDeskListUI->GetNullChairCount(pUserInvite->wTableID,wChairID);
        //    if (wNullCount==0) 
        //    {
        //        ShowMessageBox(TEXT("此游戏桌已经没有空位置了！"),NULL,NOT_MODAL_WND,true,10,IDOK);
        //        return true;
        //    }
        //    PerformSitDownAction(pUserInvite->wTableID,wChairID);
        //}

        if (mGameRoomSink != null) mGameRoomSink.OnUserInvite(pUserInvite);

        return true;
    }

    //发送警告
    bool OnSocketSubUserWarning(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        //效验参数
        //ASSERT(Command.wMainCmdID==MDM_GR_USER);
        //ASSERT(Command.wSubCmdID==SUB_GR_SEND_WARNING);

        //效验参数
        CMD_GR_SendWarning pSendWarning = SerializationUnit.BytesToStruct<CMD_GR_SendWarning>(pBuffer);
        //ASSERT(wDataSize>(Marshal.SizeOf(typeof(CMD_GR_SendWarning))-sizeof(pSendWarning->szWarningMessage)));
        //if (wDataSize<=(sizeof(CMD_GR_SendWarning)-sizeof(pSendWarning->szWarningMessage))) return false;

        //消息处理
        //WORD wSendSize=sizeof(CMD_GR_SendWarning)-sizeof(pSendWarning->szWarningMessage)+pSendWarning->wChatLength;
        //ASSERT(wDataSize==(sizeof(CMD_GR_SendWarning)-sizeof(pSendWarning->szWarningMessage)+pSendWarning->wChatLength));
        //if (wDataSize!=(sizeof(CMD_GR_SendWarning)-sizeof(pSendWarning->szWarningMessage)+pSendWarning->wChatLength)) return false;

        //寻找用户
        IUserItem pIRecvUserItem = m_ClientUserManager.SearchUserByUserID(pSendWarning.dwTargetUserID);
        if (pIRecvUserItem == null) return true;
        tagUserData pUserDataRecv = pIRecvUserItem.GetUserData();

        ////提示消息
        //TCHAR szMessage[256]=TEXT("");
        //_snprintf(szMessage,sizeof(szMessage),TEXT("[ 管理员警告]：[ %s ] %s"),pUserDataRecv->szName,pSendWarning->szWarningMessage);
        //ShowMessageBox(szMessage,NULL,NOT_MODAL_WND,true,10,IDOK);

        if (mGameRoomSink != null) mGameRoomSink.OnWarnningMessage(pSendWarning);
        return true;
    }

    //用户权限
    bool OnSocketSubUserRight(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        //效验参数
        //ASSERT(Command.wMainCmdID==MDM_GR_USER);
        //ASSERT(Command.wSubCmdID==SUB_GR_SET_USER_RIGHT);

        //效验参数
        ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_GR_SetUserRight)));
        if (wDataSize != Marshal.SizeOf(typeof(CMD_GR_SetUserRight))) return false;

        //消息处理
        CMD_GR_SetUserRight pSetUserRight = SerializationUnit.BytesToStruct<CMD_GR_SetUserRight>(pBuffer);

        //寻找用户
        IUserItem pIRecvUserItem = m_ClientUserManager.SearchUserByUserID(pSetUserRight.dwTargetUserID);
        IUserItem pISendUserItem = m_ClientUserManager.SearchUserByUserID(pSetUserRight.dwSendUserID);
        if ((pISendUserItem == null) || (pIRecvUserItem == null)) return true;
        tagUserData pUserDataSend = pISendUserItem.GetUserData();
        tagUserData pUserDataRecv = pIRecvUserItem.GetUserData();

        //设置用户权限
        if (pSetUserRight.cbLimitRoomChat == 1) //大厅聊天
            pIRecvUserItem.AddUserRight(GlobalRight.UR_CANNOT_ROOM_CHAT);
        else
            pIRecvUserItem.AddUserRight(~(DWORD)GlobalRight.UR_CANNOT_ROOM_CHAT);

        if (pSetUserRight.cbLimitSendWisper == 1) //发送私聊
            pIRecvUserItem.AddUserRight(GlobalRight.UR_CANNOT_WISPER);
        else
            pIRecvUserItem.AddUserRight(~(DWORD)GlobalRight.UR_CANNOT_WISPER);

        if (pSetUserRight.cbLimitGameChat == 1) //游戏聊天
            pIRecvUserItem.AddUserRight(GlobalRight.UR_CANNOT_GAME_CHAT);
        else
            pIRecvUserItem.AddUserRight(~(DWORD)GlobalRight.UR_CANNOT_GAME_CHAT);

        if (pSetUserRight.cbLimitPlayGame == 1) //进行游戏
            pIRecvUserItem.AddUserRight(GlobalRight.UR_CANNOT_PLAY);
        else
            pIRecvUserItem.AddUserRight(~(DWORD)GlobalRight.UR_CANNOT_PLAY);

        if (pSetUserRight.cbLimitLookonGame == 1) //旁观游戏
            pIRecvUserItem.AddUserRight(GlobalRight.UR_CANNOT_LOOKON);
        else
            pIRecvUserItem.AddUserRight(~(DWORD)GlobalRight.UR_CANNOT_LOOKON);

        //显示消息
        //TCHAR szMessage[256]=TEXT(""); 
        //if ((pSetUserRight->dwTargetUserID==m_pMeUserItem->GetUserID())||
        //	(pSetUserRight->dwSendUserID==m_pMeUserItem->GetUserID()))
        //{
        //	//游戏者信息,管理员消息
        //	if(pSetUserRight->cbAccountsRight)
        //	{
        //		if(pSetUserRight->cbLimitRoomChat == TRUE)
        //		{		
        //			_snprintf(szMessage,sizeof(szMessage),TEXT("[ %s ] 设置 [ %s ] 禁止所有游戏大厅聊天"),pUserDataSend->szName,pUserDataRecv->szName);
        //			m_MessageProxyHelper->InsertManagerString(szMessage,MS_NORMAL);
        //		}
        //		if(pSetUserRight->cbLimitSendWisper == TRUE)
        //		{		
        //			_snprintf(szMessage,sizeof(szMessage),TEXT("[ %s ] 设置 [ %s ] 禁止所有游戏发送私聊"),pUserDataSend->szName,pUserDataRecv->szName);
        //			m_MessageProxyHelper->InsertManagerString(szMessage,MS_NORMAL);
        //		}
        //		if(pSetUserRight->cbLimitGameChat == TRUE)
        //		{		
        //			_snprintf(szMessage,sizeof(szMessage),TEXT("[ %s ] 设置 [ %s ] 禁止所有游戏聊天"),pUserDataSend->szName,pUserDataRecv->szName);
        //			m_MessageProxyHelper->InsertManagerString(szMessage,MS_NORMAL);
        //		}
        //		if(pSetUserRight->cbLimitPlayGame == TRUE)
        //		{		
        //			_snprintf(szMessage,sizeof(szMessage),TEXT("[ %s ] 设置 [ %s ] 禁止玩所有游戏"),pUserDataSend->szName,pUserDataRecv->szName);
        //			m_MessageProxyHelper->InsertManagerString(szMessage,MS_NORMAL);
        //		}
        //		if(pSetUserRight->cbLimitLookonGame == TRUE)
        //		{		
        //			_snprintf(szMessage,sizeof(szMessage),TEXT("[ %s ] 设置 [ %s ] 禁止所有游戏旁观"),pUserDataSend->szName,pUserDataRecv->szName);
        //			m_MessageProxyHelper->InsertManagerString(szMessage,MS_NORMAL);
        //		}
        //	}
        //	else
        //	{
        //		if(pSetUserRight->cbLimitRoomChat == TRUE)
        //		{		
        //			_snprintf(szMessage,sizeof(szMessage),TEXT("[ %s ] 设置 [ %s ] 禁止当前大厅聊天"),pUserDataSend->szName,pUserDataRecv->szName);
        //			m_MessageProxyHelper->InsertManagerString(szMessage,MS_NORMAL);
        //		}
        //		if(pSetUserRight->cbLimitSendWisper == TRUE)
        //		{		
        //			_snprintf(szMessage,sizeof(szMessage),TEXT("[ %s ] 设置 [ %s ] 禁止当前发送私聊"),pUserDataSend->szName,pUserDataRecv->szName);
        //			m_MessageProxyHelper->InsertManagerString(szMessage,MS_NORMAL);
        //		}
        //		if(pSetUserRight->cbLimitGameChat == TRUE)
        //		{		
        //			_snprintf(szMessage,sizeof(szMessage),TEXT("[ %s ] 设置 [ %s ] 禁止当前游戏聊天"),pUserDataSend->szName,pUserDataRecv->szName);
        //			m_MessageProxyHelper->InsertManagerString(szMessage,MS_NORMAL);
        //		}	
        //		if(pSetUserRight->cbLimitPlayGame == TRUE)
        //		{		
        //			_snprintf(szMessage,sizeof(szMessage),TEXT("[ %s ] 设置 [ %s ] 禁止玩当前游戏"),pUserDataSend->szName,pUserDataRecv->szName);
        //			m_MessageProxyHelper->InsertManagerString(szMessage,MS_NORMAL);
        //		}
        //		if(pSetUserRight->cbLimitLookonGame == TRUE)
        //		{		
        //			_snprintf(szMessage,sizeof(szMessage),TEXT("[ %s ] 设置 [ %s ] 禁止当前游戏旁观"),pUserDataSend->szName,pUserDataRecv->szName);
        //			m_MessageProxyHelper->InsertManagerString(szMessage,MS_NORMAL);
        //		}
        //	}
        //}

        if (mGameRoomSink != null) mGameRoomSink.OnSetUserRight(pSetUserRight);

        return true;
    }

    //用户金币
    bool OnSocketSubUserGold(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        //效验参数
        ASSERT(Command.wMainCmdID == CMD_Game.MDM_GR_USER);
        ASSERT(Command.wSubCmdID == CMD_Game.SUB_GR_USER_GOLD);

        //效验参数
        ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_GF_BankStorageGold)));
        if (wDataSize != Marshal.SizeOf(typeof(CMD_GF_BankStorageGold))) return false;

        //消息处理
        CMD_GF_BankStorageGold pUserGold = SerializationUnit.BytesToStruct<CMD_GF_BankStorageGold>(pBuffer);

        //寻找用户
        IUserItem pISendUserItem = m_ClientUserManager.SearchUserByGameID(pUserGold.dwGameID);
        if (pISendUserItem == null) return true;
        tagUserData pUserDataSend = pISendUserItem.GetUserData();
        pISendUserItem.SetUserGold(pUserGold.dwStorageCount);

        if (mGameRoomSink != null) mGameRoomSink.OnUserGold(pUserGold);
        return true;
    }

    //用户等待
    bool OnSocketSubUserWaiting(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        //效验参数
        ASSERT(Command.wMainCmdID == CMD_Game.MDM_GR_USER);
        ASSERT(Command.wSubCmdID == CMD_Game.SUB_GR_USER_WAITING);

        //效验参数
        ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_GF_SysAllotChair)));
        if (wDataSize != Marshal.SizeOf(typeof(CMD_GF_SysAllotChair))) return false;

        //消息处理
        CMD_GF_SysAllotChair pSysAllotChair = SerializationUnit.BytesToStruct<CMD_GF_SysAllotChair>(pBuffer);

        ////寻找用户
        //IUserItem * pISendUserItem=m_ClientUserManager.SearchUserByUserID(pSysAllotChair->dwUserID);
        //if (pISendUserItem==NULL) return true;
        //tagUserData * pUserDataSend=pISendUserItem->GetUserData();

        if (mGameRoomSink != null) mGameRoomSink.OnUserWaiting(pSysAllotChair);

        return true;
    }


    #endregion

    #endregion

    #region 游戏网络消息
    //游戏框架消息
    bool OnSocketMainGlobalFrame(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        switch (Command.wSubCmdID)
        {
            case GlobalFrame.SUB_GF_OPTION:			//游戏配置
                {
                    //效验参数
                    ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_GF_Option)));
                    if (wDataSize != Marshal.SizeOf(typeof(CMD_GF_Option))) return false;

                    //消息处理
                    CMD_GF_Option pOption = SerializationUnit.BytesToStruct<CMD_GF_Option>(pBuffer);
                    m_bGameStatus = pOption.bGameStatus;
                    m_bAllowLookon = pOption.bAllowLookon == 1 ? true : false;
                    if (mGameFrameSink != null) mGameFrameSink.OnGameOption(pOption);
                    return true;
                }
            case GlobalFrame.SUB_GF_SCENE:			//游戏场景
                {
                    //消息处理
                    if (m_bGameStatus == CMD_Trench.GS_WK_FREE)
                    {
                        //效验数据
                        if (wDataSize != Marshal.SizeOf(typeof(CMD_S_StatusFree))) return false;
                        CMD_S_StatusFree pStatusFree = SerializationUnit.BytesToStruct<CMD_S_StatusFree>(pBuffer);
                        if (mGameFrameSink != null) mGameFrameSink.OnGameSceneFree(pStatusFree);
                    }
                    else if (m_bGameStatus == CMD_Trench.GS_WK_SCORE)
                    {
                        //效验数据
                        if (wDataSize != Marshal.SizeOf(typeof(CMD_S_StatusScore))) return false;
                        CMD_S_StatusScore pStatusScore = SerializationUnit.BytesToStruct<CMD_S_StatusScore>(pBuffer);
                        if (mGameFrameSink != null) mGameFrameSink.OnGameSceneScore(pStatusScore);

                    }
                    else if (m_bGameStatus == CMD_Trench.GS_WK_PLAYING)
                    {
                        //效验数据
                        if (wDataSize != Marshal.SizeOf(typeof(CMD_S_StatusPlay))) return false;
                        CMD_S_StatusPlay pStatusPlay = SerializationUnit.BytesToStruct<CMD_S_StatusPlay>(pBuffer);
                        if (mGameFrameSink != null) mGameFrameSink.OnGameScenePlaying(pStatusPlay);

                    }
                    else if (m_bGameStatus == CMD_Trench.GS_WK_TRENCH)
                    {
                        ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_S_StatusBlankTrench)));
                        if (wDataSize != Marshal.SizeOf(typeof(CMD_S_StatusBlankTrench))) return false;

                        //变量定义
                        CMD_S_StatusBlankTrench pStatusBlankTrench = SerializationUnit.BytesToStruct<CMD_S_StatusBlankTrench>(pBuffer);
                        if (mGameFrameSink != null) mGameFrameSink.OnGameSceneTrench(pStatusBlankTrench);
                    }
                    else
                    {
                        Debuger.Instance.LogError("场景状态错误！");
                        return false;
                    }
                    return true;
                }
            case GlobalFrame.SUB_GF_LOOKON_CONTROL:	//旁观控制
                {
                    //效验参数
                    ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_GF_LookonControl)));
                    if (wDataSize != Marshal.SizeOf(typeof(CMD_GF_LookonControl))) return false;

                    //消息处理
                    if (m_bLookonMode == true)
                    {
                        //变量定义
                        CMD_GF_LookonControl pLookonControl = SerializationUnit.BytesToStruct<CMD_GF_LookonControl>(pBuffer);

                        //事件处理
                        m_bAllowLookon = (pLookonControl.bAllowLookon == 1) ? true : false;

                        ////旁观处理
                        //m_pIClientKernelSink->OnEventLookonChanged(true,NULL,0);

                        ////获取目录
                        //CString strSectionKey=TEXT("");
                        //CString strProcessName=m_ServerAttribute.szProcessName;
                        //TCHAR szPath[MAX_PATH]=TEXT("");
                        //TCHAR szFileName[MAX_PATH]=TEXT("");
                        //bool bUserGIF=0;
                        ////GetCurrentDirectory(sizeof(szPath), szPath);
                        //GetModuleFileName(NULL, szPath, MAX_PATH); 
                        //(_tcsrchr(szPath, _T('\\')))[1] = 0;
                        //int pos=strProcessName.ReverseFind(':');
                        //int len=strProcessName.GetLength();
                        //strProcessName=strProcessName.Right(len-pos-1);
                        //len=strProcessName.GetLength();
                        //strProcessName=strProcessName.Left(len-4);
                        //_snprintf(szFileName,sizeof(szFileName),TEXT("%s\\%s\\Config.INI"),szPath,strProcessName);
                        //bUserGIF=GetPrivateProfileInt("GameOption","UserGIF",1,szFileName);

                        ////提示消息
                        //if (m_bAllowLookon==true) m_pIMessageProxy->InsertSystemString(TEXT("您被允许观看玩家游戏"),0,bUserGIF);
                        //else m_pIMessageProxy->InsertSystemString(TEXT("您被禁止观看玩家游戏"),0,bUserGIF);

                        if (mGameFrameSink != null) mGameFrameSink.OnLookOnControl(pLookonControl);
                    }

                    return true;
                }
            case GlobalFrame.SUB_GF_USER_CHAT:		//聊天信息
                {
                    //效验参数
                    CMD_GF_UserChat pUserChat = SerializationUnit.BytesToStruct<CMD_GF_UserChat>(pBuffer);
                    //ASSERT(wPacketSize>=(sizeof(CMD_GF_UserChat)-sizeof(pUserChat->szChatMessage)));
                    //ASSERT(wPacketSize==(sizeof(CMD_GF_UserChat)-sizeof(pUserChat->szChatMessage)+pUserChat->wChatLength));
                    //if (wPacketSize<(sizeof(CMD_GF_UserChat)-sizeof(pUserChat->szChatMessage))) return false;
                    //if (wPacketSize!=(sizeof(CMD_GF_UserChat)-sizeof(pUserChat->szChatMessage)+pUserChat->wChatLength)) return false;

                    ////寻找用户
                    //tagUserData * pRecvUserData=NULL;
                    //tagUserData * pSendUserData=SearchUserItemByUserID(pUserChat->dwSendUserID);
                    //if (pSendUserData==NULL) return true;
                    //if (pUserChat->dwTargetUserID!=0L) pRecvUserData=SearchUserItemByUserID(pUserChat->dwTargetUserID);

                    ////消息过滤
                    //if ((pUserChat->dwSendUserID!=m_dwUserID)&&(pSendUserData->cbCompanion==enCompanion_Detest)) return true;

                    ////显示消息
                    //m_pIClientKernelSink->OnUserChatMessage(pSendUserData,pRecvUserData,pUserChat->szChatMessage,pUserChat->crFontColor);

                    if (mGameFrameSink != null) mGameFrameSink.OnUserChat(pUserChat);

                    return true;
                }
            case GlobalFrame.SUB_GF_MESSAGE:		//系统消息
                {
                    //效验参数
                    CMD_GF_Message pMessage = SerializationUnit.BytesToStruct<CMD_GF_Message>(pBuffer);
                    //ASSERT(wDataSize>(sizeof(CMD_GF_Message)-sizeof(pMessage->szContent)));
                    //if (wDataSize<=(sizeof(CMD_GF_Message)-sizeof(pMessage->szContent))) return false;

                    ////消息处理
                    //WORD wHeadSize=sizeof(CMD_GF_Message)-sizeof(pMessage->szContent);
                    //ASSERT(wPacketSize==(wHeadSize+pMessage->wMessageLength*sizeof(TCHAR)));
                    //if (wPacketSize!=(wHeadSize+pMessage->wMessageLength*sizeof(TCHAR))) return false;
                    //pMessage->szContent[pMessage->wMessageLength-1]=0;

                    ////获取目录
                    //CString strSectionKey=TEXT("");
                    //CString strProcessName=m_ServerAttribute.szProcessName;
                    //TCHAR szPath[MAX_PATH]=TEXT("");
                    //TCHAR szFileName[MAX_PATH]=TEXT("");
                    //bool bUserGIF=0;
                    ////GetCurrentDirectory(sizeof(szPath), szPath);
                    //GetModuleFileName(NULL, szPath, MAX_PATH); 
                    //(_tcsrchr(szPath, _T('\\')))[1] = 0;
                    //int pos=strProcessName.ReverseFind(':');
                    //int len=strProcessName.GetLength();
                    //strProcessName=strProcessName.Right(len-pos-1);
                    //len=strProcessName.GetLength();
                    //strProcessName=strProcessName.Left(len-4);
                    //_snprintf(szFileName,sizeof(szFileName),TEXT("%s\\%s\\Config.INI"),szPath,strProcessName);
                    //bUserGIF=GetPrivateProfileInt("GameOption","UserGIF",1,szFileName);
                    ////显示消息
                    //if (pMessage->wMessageType&SMT_INFO) m_pIMessageProxy->InsertSystemString(pMessage->szContent,MS_NORMAL,bUserGIF);
                    //if (pMessage->wMessageType&SMT_EJECT) 
                    //{
                    //    if(pMessage->wMessageType&SMT_CLOSE_GAME)
                    //    {
                    //        KillGameTimer(m_nTimerID);
                    //        ShowMessageBox(pMessage->szContent,2,IDMB_CLOSE);
                    //    }
                    //    else
                    //        ShowMessageBox(pMessage->szContent,2,0);
                    //    return true;
                    //}

                    ////中断判断
                    //if (pMessage->wMessageType&SMT_CLOSE_GAME) 
                    //{
                    //    KillGameTimer(m_nTimerID);
                    //    m_ChannelServiceHelper->CloseChannel(true,true);
                    //}

                    ////关闭房间
                    //if (pMessage->wMessageType&SMT_CLOSE_GAME && !(pMessage->wMessageType&SMT_EJECT))
                    //{
                    //    m_bInquire=false;
                    //    m_pIClientKernelSink->CloseGameFrame(true);
                    //}

                    if ((pMessage.wMessageType & GlobalFrame.SMT_CLOSE_GAME) != 0) mSocket.Disconnect(true);

                    if (mGameFrameSink != null) mGameFrameSink.OnSysMessage(pMessage);

                    return true;
                }
            case GlobalFrame.SUB_GF_LEVELSCORE_AWARD:
                {
                    ASSERT(Marshal.SizeOf(typeof(CMD_GF_FreeAwardResult)) == wDataSize);
                    if (wDataSize != Marshal.SizeOf(typeof(CMD_GF_FreeAwardResult))) return false;

                    CMD_GF_FreeAwardResult pFreeAward = SerializationUnit.BytesToStruct<CMD_GF_FreeAwardResult>(pBuffer);
                    if (mGameFrameSink != null) mGameFrameSink.OnHLLevelScoreAwardInfo(pFreeAward);
                    return true;
                }
            case GlobalFrame.SUB_GF_EVERYDAY_AWARD:
                {
                    ASSERT(Marshal.SizeOf(typeof(CMD_GF_FreeAwardResult)) == wDataSize);
                    if (wDataSize != Marshal.SizeOf(typeof(CMD_GF_FreeAwardResult))) return false;

                    CMD_GF_FreeAwardResult pFreeAward = SerializationUnit.BytesToStruct<CMD_GF_FreeAwardResult>(pBuffer);
                    if (mGameFrameSink != null) mGameFrameSink.OnHLEveryDayAwardInfo(pFreeAward);
                    return true;
                }
        }
        return true;
    }
    //游戏
    bool OnSocketMainGame(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        switch (Command.wSubCmdID)
        {
            case enCmdTrench.SUB_S_BLANK_TRENCH:
                {
                    //效验数据
                    ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_S_BlankTrench)));
                    if (wDataSize != Marshal.SizeOf(typeof(CMD_S_BlankTrench))) return false;
                    CMD_S_BlankTrench pCmd = SerializationUnit.BytesToStruct<CMD_S_BlankTrench>(pBuffer);

                    if (mGameFrameSink != null) mGameFrameSink.OnSubBlackTrench(pCmd);

                    return true;
                }
            case enCmdTrench.SUB_S_SEND_CARD:		//发送扑克
                {
                    //效验数据
                    ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_S_SendCard)));
                    if (wDataSize != Marshal.SizeOf(typeof(CMD_S_SendCard))) return false;
                    CMD_S_SendCard pCmd = SerializationUnit.BytesToStruct<CMD_S_SendCard>(pBuffer);

                    if (mGameFrameSink != null) mGameFrameSink.OnSubSendCard(pCmd);

                    return true;
                }
            case enCmdTrench.SUB_S_LAND_SCORE:	//用户叫分
                {
                    //效验数据
                    ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_S_LandScore)));
                    if (wDataSize != Marshal.SizeOf(typeof(CMD_S_LandScore))) return false;
                    CMD_S_LandScore pCmd = SerializationUnit.BytesToStruct<CMD_S_LandScore>(pBuffer);

                    if (mGameFrameSink != null) mGameFrameSink.OnSubLandScore(pCmd);

                    return true;
                }
            case enCmdTrench.SUB_S_GAME_START:		//游戏开始
                {
                    //效验数据
                    ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_S_GameStart)));
                    if (wDataSize != Marshal.SizeOf(typeof(CMD_S_GameStart))) return false;
                    CMD_S_GameStart pCmd = SerializationUnit.BytesToStruct<CMD_S_GameStart>(pBuffer);

                    if (mGameFrameSink != null) mGameFrameSink.OnSubGameStart(pCmd);

                    return true;
                }
            case enCmdTrench.SUB_S_OUT_CARD:		//用户出牌
                {
                    //效验数据
                    ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_S_OutCard)));
                    if (wDataSize != Marshal.SizeOf(typeof(CMD_S_OutCard))) return false;
                    CMD_S_OutCard pCmd = SerializationUnit.BytesToStruct<CMD_S_OutCard>(pBuffer);

                    if (mGameFrameSink != null) mGameFrameSink.OnSubOutCard(pCmd);

                    return true;
                }
            case enCmdTrench.SUB_S_PASS_CARD:		//放弃出牌
                {
                    //效验数据
                    ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_S_PassCard)));
                    if (wDataSize != Marshal.SizeOf(typeof(CMD_S_PassCard))) return false;
                    CMD_S_PassCard pCmd = SerializationUnit.BytesToStruct<CMD_S_PassCard>(pBuffer);

                    if (mGameFrameSink != null) mGameFrameSink.OnSubPassCard(pCmd);

                    return true;
                }
            case enCmdTrench.SUB_S_GAME_END:		//游戏结束
                {
                    //效验数据
                    ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_S_GameEnd)));
                    if (wDataSize != Marshal.SizeOf(typeof(CMD_S_GameEnd))) return false;
                    CMD_S_GameEnd pCmd = SerializationUnit.BytesToStruct<CMD_S_GameEnd>(pBuffer);

                    if (mGameFrameSink != null) mGameFrameSink.OnSubGameEnd(pCmd);

                    return true;
                }
            case enCmdTrench.SUB_C_TRUSTEE:			//玩家托管
                {
                    //效验数据
                    ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_C_UserTrustee)));
                    if (wDataSize != Marshal.SizeOf(typeof(CMD_C_UserTrustee))) return false;
                    CMD_C_UserTrustee pCmd = SerializationUnit.BytesToStruct<CMD_C_UserTrustee>(pBuffer);

                    if (mGameFrameSink != null) mGameFrameSink.OnSubGameTrustee(pCmd);

                    return true;
                }
            case enCmdTrench.SUB_S_MATCH_STATUS://比赛状态
                {
                    //效验数据
                    ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_S_MatchStatus)));
                    if (wDataSize != Marshal.SizeOf(typeof(CMD_S_MatchStatus))) return false;
                    CMD_S_MatchStatus pCmd = SerializationUnit.BytesToStruct<CMD_S_MatchStatus>(pBuffer);

                    if (mGameFrameSink != null) mGameFrameSink.OnSubMatchStatus(pCmd);

                    return true;
                }
        }
        return true;
    }
    //游戏道具
    bool OnSocketMainGameProperty(CMD_Command Command, byte[] pBuffer, WORD wDataSize)
    {
        switch (Command.wSubCmdID)
        {
            case GlobalProperty.SUB_GF_GAME_PROPERTY:		//道具属性
                {
                    tagPropertyInfo pPropertyInfo = SerializationUnit.BytesToStruct<tagPropertyInfo>(pBuffer);
                    //m_pIClientKernelSink->OnEventInitProperty(pPropertyInfo);
                    //TRACE(TEXT(">>>ID[%d]NAME[%s]\n"),pPropertyInfo->dwPropertyID,pPropertyInfo->szPropName);
                    if (mGamePropertySink != null) mGamePropertySink.OnGamePropertyInfo(pPropertyInfo);
                    return true;
                }
            case GlobalProperty.SUB_GF_GAME_PROPERTY_FINISHI://道具属性
                {
                    //TRACE(TEXT(">>>游戏道具完成\n"));
                    //m_pIClientKernelSink->OnEventInitPropertyFinish();

                    if (mGamePropertySink != null) mGamePropertySink.OnGamePropertyFinish();
                    return true;
                }
            case GlobalProperty.SUB_GF_FLOWER:		//赠送鲜花
                {
                    ////效验参数
                    //ASSERT(wPacketSize==sizeof(CMD_GF_GiftNotify));
                    //if (wPacketSize!=sizeof(CMD_GF_GiftNotify)) return false;

                    ////变量定义
                    //CMD_GF_GiftNotify * pGiftNotify=(CMD_GF_GiftNotify *)pSocketPackage->cbBuffer;

                    ////获取玩家
                    //tagUserData const *pSendUserData = NULL;
                    //tagUserData const *pRcvUserData = NULL;
                    //for ( WORD wChairID = 0; wChairID < MAX_CHAIR; ++wChairID )
                    //{
                    //	tagUserData const *pUserData = GetUserInfo( wChairID );
                    //	if ( pUserData == NULL ) continue;

                    //	if (pUserData->dwGameID == pGiftNotify->dwSendUserID )
                    //	{
                    //		pSendUserData = pUserData ;

                    //		//自己判断
                    //		if ( pGiftNotify->dwSendUserID == pGiftNotify->dwRcvUserID ) pRcvUserData = pSendUserData;
                    //	}
                    //	else if ( pUserData->dwGameID == pGiftNotify->dwRcvUserID ) pRcvUserData = pUserData;

                    //	//中断判断
                    //	if ( pSendUserData != NULL && pRcvUserData != NULL ) break;
                    //}

                    ////旁观搜索
                    //if ( pSendUserData == NULL || pRcvUserData == NULL)
                    //{
                    //	for (INT_PTR i=0;i<m_UserItemLookon.GetCount();i++)
                    //	{
                    //		tagUserData *pLookonUserData = m_UserItemLookon[i];
                    //		if ( pLookonUserData->dwGameID == pGiftNotify->dwSendUserID)
                    //		{
                    //			pSendUserData = pLookonUserData;

                    //			//自己判断
                    //			if ( pGiftNotify->dwSendUserID == pGiftNotify->dwRcvUserID ) pRcvUserData = pSendUserData;
                    //		}
                    //		else if ( pLookonUserData->dwGameID == pGiftNotify->dwRcvUserID ) pRcvUserData = pLookonUserData;

                    //		//中断判断
                    //		if ( pSendUserData != NULL && pRcvUserData != NULL ) break;
                    //	}
                    //}

                    ////结果判断
                    //ASSERT( pSendUserData != NULL );
                    //ASSERT( pRcvUserData != NULL );
                    //if ( pSendUserData == NULL || pRcvUserData == NULL ) return true;

                    ////构造消息
                    //CString strGiftMsg;
                    //int nFlowerID = pGiftNotify->wGiftID;

                    ////鲜花效果
                    //m_pIClientKernelSink->OnEventFlower(pSendUserData, pRcvUserData, nFlowerID, nFlowerID);

                    return true;
                }

            case GlobalProperty.SUB_GF_MY_PROPERTY:		//用户道具
                {
                    tagBuyPropertyInfo pBuyPropertyInfo = SerializationUnit.BytesToStruct<tagBuyPropertyInfo>(pBuffer);
                    //m_pIClientKernelSink->OnEventUserProperty(pBuyPropertyInfo);
                    //TRACE(TEXT(">>>我的道具:ID[%d]NAME[%s]\n"),pBuyPropertyInfo->dwPropertyID,pBuyPropertyInfo->PropertyInfo.szPropName);

                    if (mGamePropertySink != null) mGamePropertySink.OnUserPropertyInfo(pBuyPropertyInfo);
                    return true;
                }
            case GlobalProperty.SUB_GF_MY_PROPERTY_FINISHI:	//用户道具结束
                {
                    //m_pIClientKernelSink->OnEventUserPropertyFinish();
                    //TRACE(TEXT(">>>我的道具完成\n"));
                    if (mGamePropertySink != null) mGamePropertySink.OnUserPropertyFinish();
                    return true;
                }
            case GlobalProperty.SUB_GF_SHOPING_PROPERTY:		//商城道具
                {
                    //tagPropertyInfo * pPropertyInfo=(tagPropertyInfo *)pSocketPackage->cbBuffer;
                    //m_pIClientKernelSink->OnEventShopingProperty(pPropertyInfo);

                    return true;
                }
            case GlobalProperty.SUB_GF_SHOPING_PROPERTY_FINISHI:		//商城道具结束
                {
                    //m_pIClientKernelSink->OnEventShopingPropertyFinish();

                    return true;
                }
            case GlobalProperty.SUB_GF_BUY_PROPERTY_SUCCESS:			//购买道具成功
                {
                    //tagBuyPropertyInfo * pBuyPropertyInfo=(tagBuyPropertyInfo *)pSocketPackage->cbBuffer;
                    //m_pIClientKernelSink->OnEventUserBuyPropertySuccess(pBuyPropertyInfo);

                    return true;
                }
            case GlobalProperty.SUB_GF_USED_PROPERTY_SUCCESS:			//使用道具成功
                {
                    //tagUsedPropertyInfo * pUserUsedPropertyInfo=(tagUsedPropertyInfo *)pSocketPackage->cbBuffer;
                    //m_pIClientKernelSink->OnEventUserUsedPropertySuccess(pUserUsedPropertyInfo);

                    return true;
                }

            case GlobalProperty.SUB_GF_USED_BUGLE:		//喇叭道具
                {
                    //效验参数
                    ASSERT(wDataSize == Marshal.SizeOf(typeof(CMD_GF_BugleProperty)));
                    if (wDataSize != Marshal.SizeOf(typeof(CMD_GF_BugleProperty))) return false;

                    //类型转换
                    CMD_GF_BugleProperty pBugleProperty = SerializationUnit.BytesToStruct<CMD_GF_BugleProperty>(pBuffer);

                    //喇叭内容
                    // 						CString strSectionKey=TEXT("");
                    // 						CString strProcessName=m_ServerAttribute.szProcessName;
                    // 						TCHAR szPath[MAX_PATH]=TEXT("");
                    // 						TCHAR szFileName[MAX_PATH]=TEXT("");
                    // 						DWORD dwBugleColor=0;
                    // 						//GetCurrentDirectory(sizeof(szPath), szPath);
                    // 						GetModuleFileName(NULL, szPath, MAX_PATH); 
                    // 						(_tcsrchr(szPath, _T('\\')))[1] = 0;
                    // 						int pos=strProcessName.ReverseFind(':');
                    // 						int len=strProcessName.GetLength();
                    // 						strProcessName=strProcessName.Right(len-pos-1);
                    // 						len=strProcessName.GetLength();
                    // 						strProcessName=strProcessName.Left(len-4);
                    // 						_snprintf(szFileName,sizeof(szFileName),TEXT("%s\\%s\\Config.INI"),szPath,strProcessName);
                    // 						dwBugleColor=GetPrivateProfileInt("ChatMessage","BugleColor",0xffffff,szFileName);
                    // 
                    // 						m_pIMessageProxy->InsertAllChat(pBugleProperty->szUserName,pBugleProperty->szContext,dwBugleColor,MS_NORMAL,pBugleProperty->cbBugleType);
                    // 						const tagUserData * pMeUserData=GetMeUserInfo();
                    // 
                    // 						if ((m_dwBugleCount > 0)&&(pBugleProperty->dwGameID ==pMeUserData->dwGameID))
                    // 						{
                    // 							m_dwBugleCount = m_dwBugleCount -1;
                    // 						}

                    if (mGamePropertySink != null) mGamePropertySink.OnUsedBugle(pBugleProperty);

                    return true;
                }
        }
        return true;
    }

    #endregion

    #region 道具网络消息
    public bool RequestPropSvr(WORD wMainCmd, WORD wSubCmd, byte[] buffer, WORD wDataSize,
            System.Action<SocketClient.ConnectError> OnConnectProp,
            System.Action<WORD, WORD, byte[], WORD> OnRecievedProp)
    {
        if (mPropSvrReqLst.Count > 0) return false;
        PropSvrReq req = new PropSvrReq();
        req.wMainCmd = wMainCmd;
        req.wSubCmd = wSubCmd;
        req.cbBuffer = new byte[wDataSize];
        buffer.CopyTo(req.cbBuffer, 0);
        req.wDataSize = wDataSize;
        req.OnRecieved = OnRecievedProp;
        req.OnConnected = OnConnectProp;

        mPropSvrReqLst.Add(req);
        ConnectProp();
        return true;
    }
    #endregion



}
