using System;
using UnityEngineEx.CMD.i3778;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineEx;
using UnityEngineEx.Net;
using UnityEngineEx.LogInterface;
using UnityEngineEx.Common;
using System.Collections;


using WORD = System.UInt16;
using DWORD = System.UInt32;
using EncodeTans = System.Text.Encoding;
using System.Runtime.InteropServices;

public class SignInMgr
{

    //网络通信失败
    public Action NetConnectError;
    //获取签到奖励列表
    public void GetSignList(Action<GlobalProperty.CMD_LoginAwardListResult> refun)
    {
        uint userId = RefactorData.Instance.NETPLAYER.GlobalUserData.dwUserID;
        GlobalProperty.CMD_LoginAwardList requstSignList;
        requstSignList.dwUserID = userId;
        byte[] sendInfo = SerializationUnit.StructToBytes(requstSignList);
        NetGameHelper.Instance.RequestPropSvr(

                GlobalProperty.MDM_GF_SERIAL_LOGIN,
                GlobalProperty.SUB_GF_AWARD_LIST_REQ,
                sendInfo,
                (WORD)sendInfo.Length,
                (SocketClient.ConnectError err) =>
                {
                    if (err != SocketClient.ConnectError.Conntected)
                    {
                        Debuger.Instance.Log("发送验证码网络连接异常..");
                        if (NetConnectError != null)
                            NetConnectError();
                    }

                },
                (WORD mainCmd, WORD subCmd, byte[] buf, WORD dataSize) =>
                {
                    if (mainCmd == GlobalProperty.MDM_GF_SERIAL_LOGIN
                        && subCmd == GlobalProperty.SUB_GF_AWARD_LIST_RET)
                    {
                        if (dataSize >= Marshal.SizeOf(typeof(GlobalProperty.CMD_LoginAwardListResult)))
                        {
                            var sendResult = SerializationUnit.BytesToStruct<GlobalProperty.CMD_LoginAwardListResult>(buf);
                            if (refun != null)
                            {
                                refun(sendResult);
                            }
                        }
                    }
                }

            );
    }

    //获取签到奖励
    public void GetSignReward(Action<GlobalProperty.CMD_LoginAwardResult> refun)
    {
        uint userId = RefactorData.Instance.NETPLAYER.GlobalUserData.dwUserID;
        GlobalProperty.CMD_LoginAward requstSignList;
        requstSignList.dwUserID = userId;
        byte[] sendInfo = SerializationUnit.StructToBytes(requstSignList);
//         NetGameHelper.Instance.RequestPropSvr(
// 
//                 GlobalProperty.MDM_GF_SERIAL_LOGIN,
//                 GlobalProperty.SUB_GF_LOGIN_AWARD_REQ,
//                 sendInfo,
//                 (WORD)sendInfo.Length,
//                 (SocketClient.ConnectError err) =>
//                 {
//                     if (err != SocketClient.ConnectError.Conntected)
//                     {
//                         Debuger.Instance.Log("发送验证码网络连接异常..");
//                         if (NetConnectError != null)
//                             NetConnectError();
//                     }
// 
//                 },
//                 (WORD mainCmd, WORD subCmd, byte[] buf, WORD dataSize) =>
//                 {
//                     if (mainCmd == GlobalProperty.MDM_GF_SERIAL_LOGIN
//                         && subCmd == GlobalProperty.SUB_GF_LOGIN_AWARD_RET)
//                     {
//                         if (dataSize >= Marshal.SizeOf(typeof(GlobalProperty.CMD_LoginAwardResult)))
//                         {
//                             var sendResult = SerializationUnit.BytesToStruct<GlobalProperty.CMD_LoginAwardResult>(buf);
//                             if (refun != null)
//                             {
//                                 refun(sendResult);
//                             }
//                         }
//                     }
//                 }
// 
//             );
    }

}
