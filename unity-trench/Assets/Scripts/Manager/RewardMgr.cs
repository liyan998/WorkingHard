using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class RewardMgr
{
    Reward mReward = new Reward();

    public void LoadReward()
    {
        string data = Resources.Load(COMMON_CONST.PathRewardInfo).ToString();
        mReward.XmlLoad(data, "RewardInfo");
    }


    /// <summary>
    /// 获取奖励信息
    /// </summary>
    /// <param name="id">奖励编号</param>
    /// <returns></returns>
    public INFO_REWARD GetReward(int id)
    {
        return mReward.INFO[id];
    }

    /// <summary>
    /// 获取所有奖励
    /// </summary>
    /// <returns></returns>
    public List<INFO_REWARD> GetAllReward()
    {
        List<INFO_REWARD> list = new List<INFO_REWARD>();
        list.AddRange(mReward.INFO.Values);
        return list;
    }

    /// <summary>
    /// 获取关卡奖励
    /// </summary>
    /// <param name="stageId">关卡ID</param>
    /// <returns></returns>
    public List<INFO_REWARD> GetStageReward(uint stageId)
    {
        List<INFO_REWARD> list = GetAllReward();
        return list.FindAll((x)=>x.GroupId==Reward.REWARD_STAGE
            && x.OtherId == stageId);
    }

}
