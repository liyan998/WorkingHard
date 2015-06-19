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

public class MailMgr : MonoBehaviour {

    
    //网络通信失败
    public Action NetConnectError;
    private void ConnectErrorFun(SocketClient.ConnectError err,string msg)
    {
        if (err != SocketClient.ConnectError.Conntected)
        {
            Debuger.Instance.Log(msg);
            if (NetConnectError != null)
                NetConnectError();
        }
    }
    //获取未读邮件数
    public void GetNotReadCount(uint userId, Action<GlobalProperty.CMD_MailsReadRet> refun)
    {

        GlobalProperty.CMD_MailsReadReq requestData;
        requestData.dwReceiverID = userId;
        byte[] sendData = SerializationUnit.StructToBytes(requestData);
//         DataBase.Instance.NetHelper.RequestPropSvr(
// 
//                 GlobalProperty.MDM_GF_MAILS,
//                 GlobalProperty.SUB_GF_MAILS_READ_REQ,
//                 sendData,
//                 (WORD)sendData.Length,
//                 (SocketClient.ConnectError err) =>
//                 {
//                     ConnectErrorFun(err, "获取未读邮件数--网络连接失败");
// 
//                 },
//                 (WORD mainCmd, WORD subCmd, byte[] buf, WORD dataSize) =>
//                 {
//                     if (mainCmd == GlobalProperty.MDM_GF_MAILS
//                         && subCmd == GlobalProperty.SUB_GF_MAILS_READ_RET)
//                     {
//                         if (dataSize >= Marshal.SizeOf(typeof(GlobalProperty.CMD_MailsReadRet)))
//                         {
//                             var sendResult = SerializationUnit.BytesToStruct<GlobalProperty.CMD_MailsReadRet>(buf);
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
    
    //获取邮件列表
    public void GetMailList(uint userId, Action<GlobalProperty.CMD_MailsListRet> refun)
    {

        GlobalProperty.CMD_MailsListReq requestData;
        requestData.dwReceiverID = userId;
        byte[] sendData = SerializationUnit.StructToBytes(requestData);
//         DataBase.Instance.NetHelper.RequestPropSvr(
// 
//                 GlobalProperty.MDM_GF_MAILS,
//                 GlobalProperty.SUB_GF_MAILS_LIST_REQ,
//                 sendData,
//                 (WORD)sendData.Length,
//                 (SocketClient.ConnectError err) =>
//                 {
//                     ConnectErrorFun(err,"邮件列表获取--网络连接失败");
// 
//                 },
//                 (WORD mainCmd, WORD subCmd, byte[] buf, WORD dataSize) =>
//                 {
//                     if (mainCmd == GlobalProperty.MDM_GF_MAILS
//                         && subCmd == GlobalProperty.SUB_GF_MAILS_LIST_RET)
//                     {
//                         if (dataSize >= Marshal.SizeOf(typeof(GlobalProperty.CMD_MailsListRet)))
//                         {
//                             var sendResult = SerializationUnit.BytesToStruct<GlobalProperty.CMD_MailsListRet>(buf);
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

    //获取邮件内容
    public void GetMailContent(uint userId, uint mailId, Action<GlobalProperty.CMD_MailsContentRet> refun)
    {

        GlobalProperty.CMD_MailsContentReq requestData;
        requestData.dwReceiverID = userId;
        requestData.dwMailsID = mailId;
        byte[] sendData = SerializationUnit.StructToBytes(requestData);
//         DataBase.Instance.NetHelper.RequestPropSvr(
// 
//                 GlobalProperty.MDM_GF_MAILS,
//                 GlobalProperty.SUB_GF_MAILS_CONTENT_REQ,
//                 sendData,
//                 (WORD)sendData.Length,
//                 (SocketClient.ConnectError err) =>
//                 {
//                     ConnectErrorFun(err, "邮件内容获取--网络连接失败");
// 
//                 },
//                 (WORD mainCmd, WORD subCmd, byte[] buf, WORD dataSize) =>
//                 {
//                     if (mainCmd == GlobalProperty.MDM_GF_MAILS
//                         && subCmd == GlobalProperty.SUB_GF_MAILS_CONTENT_RET)
//                     {
//                         if (dataSize >= Marshal.SizeOf(typeof(GlobalProperty.CMD_MailsContentRet)))
//                         {
//                             var sendResult = SerializationUnit.BytesToStruct<GlobalProperty.CMD_MailsContentRet>(buf);
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

    //获取邮件附件奖励
    public void GetMailReward(uint userId, uint mailId, Action<GlobalProperty.CMD_MailsAwardRet> refun)
    {

        GlobalProperty.CMD_MailsAwardReq requestData;
        requestData.dwReceiverID = userId;
        requestData.dwMailsID = mailId;
        byte[] sendData = SerializationUnit.StructToBytes(requestData);
//         DataBase.Instance.NetHelper.RequestPropSvr(
// 
//                 GlobalProperty.MDM_GF_MAILS,
//                 GlobalProperty.SUB_GF_MAILS_AWARD_REQ,
//                 sendData,
//                 (WORD)sendData.Length,
//                 (SocketClient.ConnectError err) =>
//                 {
//                     ConnectErrorFun(err, "邮件附件获取--网络连接失败");
// 
//                 },
//                 (WORD mainCmd, WORD subCmd, byte[] buf, WORD dataSize) =>
//                 {
//                     if (mainCmd == GlobalProperty.MDM_GF_MAILS
//                         && subCmd == GlobalProperty.SUB_GF_MAILS_AWARD_RET)
//                     {
//                         if (dataSize >= Marshal.SizeOf(typeof(GlobalProperty.CMD_MailsAwardRet)))
//                         {
//                             var sendResult = SerializationUnit.BytesToStruct<GlobalProperty.CMD_MailsAwardRet>(buf);
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
