using System.Collections.Generic;

public class INFO_REWARD
{
	public int Id;         //ID
    public int GroupId;    //奖励分组 关卡 任务
    public int OtherId;    //相关ID 关卡 、任务具体ID
    public int type;       //类型 0 银币 1 钻石 2 道具
    public int value;      //奖励道具ID
    public int count;      //奖励数量
    public string describe;//描述

}
