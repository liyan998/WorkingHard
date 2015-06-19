using UnityEngineEx.CMD.i3778;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineEx;
using UnityEngineEx.Net;
using UnityEngineEx.LogInterface;
using UnityEngineEx.Common;
using System.Runtime.InteropServices;

using WORD = System.UInt16;
using DWORD = System.UInt32;
using EncodeTans = System.Text.Encoding;


/// <summary>
///  Player
/// </summary>
public class NetPlayer //: MonoBehaviour
{
    #region 公有属性
    public GlobalDef.tagGlobalUserData GlobalUserData { get { return mGlobalUser_; } set { mGlobalUser_ = value; } }            //全局用户(网络版)
    public bool SignState { get { return mSignStatus; } set { mSignStatus = value; } }                                         //签到状态
    public WORD VipLv { get { return mVipLevel; } set { mVipLevel = value; } }                                                 //Vip等级
    
    #endregion

    #region 保护属性    
    protected GlobalDef.tagGlobalUserData mGlobalUser_;                                 //全局用户信息
    #endregion

    #region 私有属性
    bool mSignStatus = false;			//设置签到状态
    WORD mVipLevel = 0;
    #endregion    
   


    public NetPlayer()
	{
        
        mGlobalUser_ = new GlobalDef.tagGlobalUserData();

	}
    //用户发送验证码倒计时
    public int SendSeconds = 0;
    //网络通信失败
    public Action NetConnectError;
    //用户发送验证码
    public void SendVericode(string phone,string dev,byte type,Action<string> refun)
    {
        GlobalProperty.CMD_BindVerityCode sendVericode;
        sendVericode.szAccounts = phone.ToBytes();
        sendVericode.szUserDevice = dev.ToBytes();
        sendVericode.szMessageType = type;
        byte[] sendInfo = SerializationUnit.StructToBytes(sendVericode);
        NetGameHelper.Instance.RequestPropSvr(

                GlobalProperty.MDM_GF_USER_DEVICE_BINDING,
                GlobalProperty.SUB_GF_MOBILE_BIND_CODE_REQ,
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
                    if (mainCmd == GlobalProperty.MDM_GF_USER_DEVICE_BINDING
                        && subCmd == GlobalProperty.SUB_GF_MOBILE_BIND_CODE_RET)
                    {
                        if (dataSize >= Marshal.SizeOf(typeof(GlobalProperty.CMD_VerityCodeResult)))
                        {
                            var sendResult = SerializationUnit.BytesToStruct<GlobalProperty.CMD_VerityCodeResult>(buf);
                            if (refun != null)
                            {
                                refun(sendResult.tszDescribe.ToAnsiString());
                            }
                        }
                    }
                }

            );
    }
    //用户绑定手机
    public void BindPhone(string account, string pwd, string veriCode, Action<int, string> BindPhoneResult)
    {
        string enPwd = UnityEngineEx.Net.EncryptHelper.ToMySqlEncrypt(pwd);
        GlobalProperty.CMD_UserDeviceBinding userBind;
        userBind.szAccounts = account.ToBytes();
        userBind.szPassWord = enPwd.ToBytes();
        userBind.szVerifiyCode = veriCode.ToBytes();
        userBind.szUserDevice = DeviceManager.Instance.deviceId.ToBytes();
        byte[] bindInfo = SerializationUnit.StructToBytes(userBind);

        NetGameHelper.Instance.RequestPropSvr(

                GlobalProperty.MDM_GF_USER_DEVICE_BINDING,
                GlobalProperty.SUB_GF_DEVICE_BINGING_REQ,
                bindInfo,
                (WORD)bindInfo.Length,
                (SocketClient.ConnectError err) =>
                {
                    if (err != SocketClient.ConnectError.Conntected) 
                    {
                        Debuger.Instance.Log("绑定手机网络连接异常..");
                        if (NetConnectError != null)
                            NetConnectError();
                    }
                        
                },
                (WORD mainCmd, WORD subCmd, byte[] buf, WORD dataSize) => {
                    if (mainCmd == GlobalProperty.MDM_GF_USER_DEVICE_BINDING
                        && subCmd == GlobalProperty.SUB_GF_DEVICE_BINGING_RET)
                    {
                        if (dataSize >= Marshal.SizeOf(typeof(GlobalProperty.CMD_DeviceBindingResult)))
                        {
                            var bindInfoResult = SerializationUnit.BytesToStruct<GlobalProperty.CMD_DeviceBindingResult>(buf);
                            if (BindPhoneResult != null)
                            {
                                BindPhoneResult(bindInfoResult.nResult, bindInfoResult.tszDescribe.ToAnsiString());
                            }
                        }
                    }
                }

            );

    }

    //用户重置密码
    public void ResetPwd(string account,string pwd,string Vericode,Action<int,string> reFun)
    {

        GlobalProperty.CMD_ModifyPassWord mPwd;
        string enPwd = UnityEngineEx.Net.EncryptHelper.ToMySqlEncrypt(pwd);
        mPwd.szAccounts = account.ToBytes();
        mPwd.szPassWord = enPwd.ToBytes();
        mPwd.szVerifiyCode = Vericode.ToBytes();
        byte[] sendInfo = SerializationUnit.StructToBytes(mPwd);
        NetGameHelper.Instance.RequestPropSvr(

                GlobalProperty.MDM_GF_MODIFY_USER_INFO,
                GlobalProperty.SUB_GF_MODIFY_PASSWORD_REQ,
                sendInfo,
                (WORD)sendInfo.Length,
                (SocketClient.ConnectError err) =>
                {
                    if (err != SocketClient.ConnectError.Conntected)
                    {
                        Debuger.Instance.Log("用户重置密码网络连接异常..");
                        if (NetConnectError != null)
                            NetConnectError();
                    }

                },
                (WORD mainCmd, WORD subCmd, byte[] buf, WORD dataSize) =>
                {
                    if (mainCmd == GlobalProperty.MDM_GF_MODIFY_USER_INFO
                        && subCmd == GlobalProperty.SUB_GF_MODIFY_PASSWORD_RET)
                    {
                        if (dataSize >= Marshal.SizeOf(typeof(GlobalProperty.CMD_ModifyPassWordResult)))
                        {
                            var sendResult = SerializationUnit.BytesToStruct<GlobalProperty.CMD_ModifyPassWordResult>(buf);
                            if (reFun != null)
                            {
                                reFun(sendResult.nResult,sendResult.tszDescribe.ToAnsiString());
                            }
                        }
                    }
                }

            );
    }

    /// <summary>
    /// 修改用户信息
    /// </summary>
    /// <param name="account">用户名</param>
    /// <param name="nickname">昵称</param>
    /// <param name="sex">性别</param>
    /// <param name="reFun"></param>
    public void EditUserInfo(string account,string nickname,byte sex,Action<string> reFun)
    {
        GlobalProperty.CMD_ModifyUserInfo userInfo;
        userInfo.szAccounts = account.ToBytes();
        userInfo.szNickName = nickname.ToBytes();
        userInfo.cbUserSex = sex;

        byte[] sendInfo = SerializationUnit.StructToBytes(userInfo);
        NetGameHelper.Instance.RequestPropSvr(

                GlobalProperty.MDM_GF_MODIFY_USER_INFO,
                GlobalProperty.SUB_GF_MODIFY_USER_INFO_REQ,
                sendInfo,
                (WORD)sendInfo.Length,
                (SocketClient.ConnectError err) =>
                {
                    if (err != SocketClient.ConnectError.Conntected)
                    {
                        Debuger.Instance.Log("修改用户信息--网络连接异常..");
                        if (NetConnectError != null)
                            NetConnectError();
                    }

                },
                (WORD mainCmd, WORD subCmd, byte[] buf, WORD dataSize) =>
                {
                    if (mainCmd == GlobalProperty.MDM_GF_MODIFY_USER_INFO
                        && subCmd == GlobalProperty.SUB_GF_MODIFY_USER_INFO_RET)
                    {
                        if (dataSize >= Marshal.SizeOf(typeof(GlobalProperty.CMD_ModifyUserInfoResult)))
                        {
                            var sendResult = SerializationUnit.BytesToStruct<GlobalProperty.CMD_ModifyUserInfoResult>(buf);
                            if (reFun != null)
                            {
                                reFun(sendResult.tszDescribe.ToAnsiString());
                            }
                        }
                    }
                }

            );
    }
    
}
