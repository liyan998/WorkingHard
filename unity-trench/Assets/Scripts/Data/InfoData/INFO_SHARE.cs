using UnityEngine;
using System.Collections;

[System.Serializable]
public class INFO_SHARE
{
    public int Id;         //ID
    public uint Platform;     //分享平台
    public uint Scene;        //0-关卡分享 1-战斗分享
    public uint Type;       //类型0-银币，金币 1-钻石 ++道具
    public int ItemId;
    public int GivenValue;      //赠送的值
    public string ShareText;      //分享内容
    public string RewardedText;     //成功提示内容
}
