using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 任务
/// </summary>
public class Task
{
    public const int STATE_UNCOMPLETE   = 0;//未完成
    public const int STATE_COMLETED     = 1;//已完成
    public const int STATE_FAILED       = 2;//失败

    //////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 任务状态
    /// </summary>
    public int mState;

    /// <summary>
    /// 任务条件数据
    /// </summary>
    public TaskData mTargetTaskData;

    /// <summary>
    /// 当前任务数据
    /// </summary>
    public TaskData mCurrentTaskData;


    Dictionary<TaskData.KEY, bool> mComplete;

    /// <summary>
    /// 任务ID
    /// </summary>
    int mId;

    /// <summary>
    /// 触发概率
    /// </summary>
    float mProbabillty;

    /// <summary>
    /// 奖励倍数
    /// </summary>
    float mMulip;

    //////////////////////////////////////////////////////////////////////////////////////////

   
    /// <summary>
    /// 任务ID
    /// </summary>
    public int TaskId
    {
        get { return mId; }
        set { mId = value; }
    }

    /// <summary>
    /// 任务触发
    /// </summary>
    /// <returns></returns>
    public float TaskTrigger
    {
        get { return mProbabillty; }
        set { mProbabillty = value; }
    }    

    /// <summary>
    /// 任务文本
    /// </summary>
    /// <returns></returns>
    public string TaskText
    {
        get;
        set;
    }

    /// <summary>
    /// 游戏奖励倍数
    /// </summary>
    public float TaskWinMutlip
    {
        get { return mMulip; }
        set { mMulip = value; }
    }

    /// <summary>
    /// 任务状态
    /// </summary>
    public int TaskState
    {
        get { return mState;}
        set { mState = value; }
    }

    /// <summary>
    /// 是否包含指定的KEY
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool HasContaneKey(TaskData.KEY key)
    {
        if (mTargetTaskData.mAllTaskData.ContainsKey(key))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 是否启动计时器
    /// </summary>
    public bool HasStartTimer
    {
        get
        {
            TaskData.KEY[] allTimerSys = { TaskData.KEY.KEY_GAMETIME, TaskData.KEY.KEY_PLAYTIME };

            for (int i = 0; i < allTimerSys.Length;i++ )
            {
                if (mTargetTaskData.mAllTaskData.ContainsKey(allTimerSys[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// 设置任务数据
    /// </summary>
    /// <param name="taskdata"></param>
    public void SetTargetTask(TaskData taskdata)
    {
        mTargetTaskData = taskdata;        
    }


    /// <summary>
    /// 任务是否完成
    /// </summary>
    /// <returns></returns>
    public bool Complete()
    {           
        foreach (KeyValuePair<TaskData.KEY, bool> vvp in mComplete)
        {
            if(!vvp.Value)
            {
                return false;
            }
        }
        return true;
    }

    public void flushData()
    {        
        foreach (KeyValuePair<TaskData.KEY, int> vvp in mCurrentTaskData.mAllTaskData)
        {
            switch (vvp.Key)
            {
                case TaskData.KEY.KEY_WINCOUNT:             //胜利次数
                case TaskData.KEY.KEY_BLACKWINCOUNT:        //黑挖胜利次数
                case TaskData.KEY.KEY_NORMBLEWINCOUNT:      //农民胜利次数
                case TaskData.KEY.KEY_MASTERWINCOUNT:       //坑主胜利次数
                case TaskData.KEY.KEY_CARDCATEGROYCOUNT:    //手持特殊牌型次数
                case TaskData.KEY.KEY_FINALCATEGROYCOUNT:   //尾牌特殊牌型次数
                case TaskData.KEY.KEY_STARTCATEGROYCOUNT:   //起始特殊牌型次数

                    if (vvp.Value >= mTargetTaskData.mAllTaskData[vvp.Key])
                    {
                        mComplete[vvp.Key] = true;
                    }               

                    break;            
                
                case TaskData.KEY.KEY_GAMETIME:             //游戏时长
                case TaskData.KEY.KEY_PLAYTIME:             //玩家出牌时间

                    if (vvp.Value >= mTargetTaskData.mAllTaskData[vvp.Key])
                    {
                        mComplete[vvp.Key] = false;
                        mState = STATE_FAILED;
                    }

                    break;
            }
        }
    }



    /// <summary>
    /// 清除数据
    /// </summary>
    public void ClearData()
    {
        mState = STATE_UNCOMPLETE;

        mCurrentTaskData = mTargetTaskData.CloneDataTemp();


        if(mComplete == null)
        {
            mComplete = new Dictionary<TaskData.KEY, bool>();
        }

        mComplete.Clear();

        RestTaskComplete(mCurrentTaskData);        
    }


    void RestTaskComplete(TaskData taskdata)
    {
        foreach(TaskData.KEY kvp in taskdata.mAllTaskData.Keys)
        {
            bool isOn = GetInitState(kvp);
            
            mComplete.Add(kvp, isOn);
        }
    }

    /// <summary>
    /// 初始为true字段
    /// </summary>
    public static readonly TaskData.KEY[] allInitTrue = 
                                {
                                     TaskData.KEY.KEY_GAMETIME,     //游戏时长      --
                                     TaskData.KEY.KEY_PLAYTIME,     //玩家出牌时间   --

                                     TaskData.KEY.KEY_CARDCATEGROY, //特殊牌型      not
                                     TaskData.KEY.KEY_CARDSCORE,    //特殊牌面      not
                                     TaskData.KEY.KEY_FINALCATEGROY,//特殊牌型      not
                                     TaskData.KEY.KEY_FINALSCORE,   //特殊牌面      not
                                     TaskData.KEY.KEY_STARTCATEGROY,//起始特殊牌型   not
                                     TaskData.KEY.KEY_STARTSCORE,   //起始特殊牌面   not
                                 };

   

    private bool GetInitState(TaskData.KEY kvp)
    {
        for(int i = 0;i < allInitTrue.Length;i++)
        {
            if(kvp == allInitTrue[i])
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool UpdateData(TaskData.KEY key, int data)
    {
        if(!mCurrentTaskData.mAllTaskData.ContainsKey(key))
        {
            return false;
        }

        mCurrentTaskData.mAllTaskData[key] = data;
        return true;
    }




    /// ////////////////////////////////////////////////////////////////////////////////////////////   
    
    
    public static readonly Dictionary<TaskData.KEY, TaskData.KEY> allCountKey = new Dictionary<TaskData.KEY, TaskData.KEY>()
    { 
        { TaskData.KEY.KEY_CARDCATEGROY,   TaskData.KEY.KEY_CARDCATEGROYCOUNT},
        { TaskData.KEY.KEY_FINALCATEGROY,  TaskData.KEY.KEY_FINALCATEGROYCOUNT},
        { TaskData.KEY.KEY_STARTCATEGROY,  TaskData.KEY.KEY_STARTCATEGROYCOUNT},                            
    };

    private bool CheckCategory(TaskData.KEY key)
    {
        if (allCountKey.ContainsKey(key)
            &&
            mTargetTaskData.mAllTaskData.ContainsKey(allCountKey[key]))
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// 指定牌型， 分值 更新
    /// </summary>
    /// <param name="key">字</param>
    /// <param name="category">牌型</param>
    public void UpdateCategroyOutCard(TaskData.KEY key, byte category )
    {
        if(!CheckCategory(key))
        {
            return;
        }
        
        int targetCategroy = mTargetTaskData.GetIntValue(key);

        if(targetCategroy != category)
        {
            return;
        }

        //       
        mCurrentTaskData.PlusPlusIntValue(allCountKey[key]);
    }
}
