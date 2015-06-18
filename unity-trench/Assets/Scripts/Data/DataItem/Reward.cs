using System;
using System.Collections.Generic;

public class xmlReward : BaseXmlReader<Dictionary<int, INFO_REWARD>, INFO_REWARD>
{
    protected override void OnRead(INFO_REWARD v)
    {
        m_Info.Add(v.Id, v);
    }
}

public class Reward : BaseXmlNode<Dictionary<int, INFO_REWARD>, xmlReward>
{
    public const int REWARD_STAGE = 101; //关卡奖励
    public const int REWARD_TASK = 102;  //任务奖励

    public const int REWARD_GOLD = 0; //奖励金币
    public const int REWARD_DIAMONDS = 1; //奖励钻石
    public const int REWARD_PROPERTY = 2; //奖励道具

}
