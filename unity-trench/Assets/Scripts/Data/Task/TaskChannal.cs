using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using UnityEngine;


public class TaskChannal
    : SingleClass<TaskChannal>
{
//     private static TaskChannal mInstran;
// 
//     private TaskChannal()
//     {
//     }
// 
//     public static TaskChannal Instance
//     {
//         get
//         {
//             if(mInstran == null)
//             {
//                 mInstran = new TaskChannal();
//             }
//             return mInstran;        
//         }
//     }    

    //----------------------------------------
    
    /// <summary>
    /// 任务数据池
    /// </summary>
    TaskFactory mTaskFactory;
    
    /// <summary>
    /// 目标任务
    /// </summary>
    Task        mTarget;

    /// <summary>
    /// 开关
    /// </summary>
    bool mTrun;

    /// <summary>
    /// 当前结算分数
    /// </summary>
    public int mCurrentSettle;

    /// <summary>
    /// 
    /// </summary>
    System.Timers.Timer timer;

    /// <summary>
    /// 游戏时长
    /// </summary>
    int timeSecond;

    /// <summary>
    /// 玩家出牌时长
    /// </summary>
    int playTime;

    public bool Trun
    {
        get { return mTrun; }
        set { mTrun = value; }
    }

    public Task TargetTask
    {
        get{ return mTarget;}       
    }

    public void initData(string xmldata)
    {
        mTrun = false;
        
        mTaskFactory = new TaskFactory();
        mTaskFactory.initData(xmldata);

        timer = new System.Timers.Timer(1000);
        timer.Elapsed += new ElapsedEventHandler(OnTimeCounter);
    }

    public void CreateTask()
    {
        if(mTrun)
        {
            mTarget = mTaskFactory.RandTask();
            mTarget.ClearData();
            ClearTaskData(); 
        }       
    }

    public void ClearTaskData()
    {
        mCurrentSettle  = 0;

        timeSecond      = 0;
        playTime        = 0;

        isFirstOut      = false;
        isStartPlayTime = false;

        StopTimer(); 
    }

    public void DestoryTask()
    {
        mTarget = null;
        ClearTaskData();
        timer.Close();
    }

    //-------------------------------------------------

    /// <summary>
    /// 场次任务结算
    /// </summary>
    /// <param name="scenScore">场次低分</param>
    /// <param name="isWin">是否输赢</param>
    /// <returns>结算分数</returns>
    public int TaskSettle(int scenScore, bool isWin)
    {
        if(!isWin)
        {
            return 0;
        }
        
        if (mTarget.mState != Task.STATE_COMLETED)
        {
            return 0;
        }

        mCurrentSettle = (int)(mTarget.TaskWinMutlip * scenScore);
        return mCurrentSettle;
    }

    //---------------------------------------------------

    /// <summary>
    /// 游戏检查开始
    /// </summary>
    public void InGameStart(int playerid)
    {
        if (!mTrun)
        {
            return;
        }

        if(mTarget.HasStartTimer)
        {
            StartTimer();
        }
                       
        if (mTarget.HasContaneKey(TaskData.KEY.KEY_STARTCATEGROY))
        {
            isFirstOut = (playerid == 1); 
            if(!isFirstOut)
            {
                mTarget.mState = Task.STATE_FAILED;                
            }
        }        
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    public void InGameRandCard(byte[] allCard, Dictionary<ushort, List<byte>> allPlayerCard)
    {
        Debug.Log("Task Config Card");           
    }
    
    /// <summary>
    /// 玩家出牌
    /// </summary>
    /// <param name="outlist"></param>
    public void InGamePlayerOutCard(byte[] outlist)
    {
        if (!mTrun || mTarget.mState != Task.STATE_UNCOMPLETE)
        {
            return;
        }

        OutFirstCategroy(outlist);
        OutLastCategroy(outlist);
        OutCategroyScore(outlist);
        //
        CheckTaskComplete();
    }


    
    private void OutFirstCategroy(byte[] outlist)
    {
        if (!mTarget.HasContaneKey(TaskData.KEY.KEY_STARTCATEGROY))
        {
            return;
        }

        if(!isFirstOut)
        {
            return;
        }

        byte outcardCategroy = GameHelper.Instance.GetOutCardType(outlist);

        if (mTarget.HasContaneKey(TaskData.KEY.KEY_STARTSCORE))
        {
            byte cardscore = (byte)mTarget.mTargetTaskData.GetIntValue(TaskData.KEY.KEY_STARTSCORE);
            if (GameLogicUtil.ContaneCardByScore(outlist, cardscore))
            {
                mTarget.UpdateCategroyOutCard(TaskData.KEY.KEY_STARTCATEGROY, outcardCategroy);
            }
        }
        else
        {
            mTarget.UpdateCategroyOutCard(TaskData.KEY.KEY_STARTCATEGROY, outcardCategroy);
        } 

    }

    /// <summary>
    /// 手牌指定牌型，包含指定分值 记录
    /// </summary>
    /// <param name="outlist"></param>
    private void OutCategroyScore(byte[] outlist)
    {
        if (!mTarget.HasContaneKey(TaskData.KEY.KEY_CARDCATEGROY))
        {
            return;
        }

        byte outcardCategroy = GameHelper.Instance.GetOutCardType(outlist);

        if (mTarget.HasContaneKey(TaskData.KEY.KEY_CARDSCORE))
        {
            byte cardscore = (byte)mTarget.mTargetTaskData.GetIntValue(TaskData.KEY.KEY_CARDSCORE);
            if (GameLogicUtil.ContaneCardByScore(outlist, cardscore))
            {
                mTarget.UpdateCategroyOutCard(TaskData.KEY.KEY_CARDCATEGROY, outcardCategroy);
            }
        }
        else
        {
            mTarget.UpdateCategroyOutCard(TaskData.KEY.KEY_CARDCATEGROY, outcardCategroy);
        } 
    }

    /// <summary>
    /// 尾牌出牌 指定牌型，分值记录
    /// </summary>
    /// <param name="outlist"></param>
    private void OutLastCategroy(byte[] outlist)
    {
        if (!mTarget.HasContaneKey(TaskData.KEY.KEY_FINALCATEGROY))
        {
            return;
        }
        //Debug.Log("最后一手牌" + GameHelper.Instance.PlayerHandCard[1].Count);
        if (GameHelper.Instance.PlayerHandCard[1].Count != 0)
        {
            //最后一手牌
            return;
        }

        byte outcardCategroy = GameHelper.Instance.GetOutCardType(outlist);
        if (mTarget.HasContaneKey(TaskData.KEY.KEY_FINALSCORE))
        {
            byte cardscore = (byte)mTarget.mTargetTaskData.GetIntValue(TaskData.KEY.KEY_FINALSCORE);
            if (GameLogicUtil.ContaneCardByScore(outlist, cardscore))
            {
                mTarget.UpdateCategroyOutCard(TaskData.KEY.KEY_FINALCATEGROY, outcardCategroy);
            }
        }
        else
        {
            mTarget.UpdateCategroyOutCard(TaskData.KEY.KEY_FINALCATEGROY, outcardCategroy);
        } 

    }
    private bool isFirstOut;
    private bool isStartPlayTime;

    /// <summary>
    /// 玩家出牌开始
    /// </summary>
    public void InGamePlayerStart()
    {
        if (!mTrun || mTarget.mState == Task.STATE_FAILED)
        {
            return;
        }
        //Debug.Log("Player Start");
        isStartPlayTime = true;
        playTime = 0;
    }

    /// <summary>
    /// 玩家未出牌
    /// </summary>
    public void InGamePlayerEnd()
    {
        if (!mTrun || mTarget.mState == Task.STATE_FAILED)
        {
            return;
        }
        //Debug.Log("Player End");
        isStartPlayTime = false;        
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    /// <param name="isWin"></param>
    /// <param name="categroy"></param>
    public void InGameEnd(bool isWin, int categroy)
    {
        StopTimer();

        if (!mTrun || mTarget.mState != Task.STATE_UNCOMPLETE)
        {
            return;
        }

        if(!isWin)
        {
            return;
        }

        mTarget.mCurrentTaskData.PlusPlusIntValue(TaskData.KEY.KEY_WINCOUNT);
        switch (categroy)
        {
            case GameHelper.PLAYER_CATEGROY_BLACKMASTER:
                mTarget.mCurrentTaskData.PlusPlusIntValue(TaskData.KEY.KEY_BLACKWINCOUNT);
                break;
            case GameHelper.PLAYER_CATEGROY_MASTER:
                mTarget.mCurrentTaskData.PlusPlusIntValue(TaskData.KEY.KEY_MASTERWINCOUNT);
                break;
            case GameHelper.PLAYER_CATEGROY_NORMAB:
                mTarget.mCurrentTaskData.PlusPlusIntValue(TaskData.KEY.KEY_NORMBLEWINCOUNT);
                break;
        }       

        CheckTaskComplete();       
    }

    //---------------------------------------------------------------

    void CheckTaskComplete()
    {
        mTarget.flushData();

        if(mTarget.mState == Task.STATE_FAILED)
        {
            Debug.Log(mTarget.TaskText + " STATE_STATE_FAILED ");
        }
        else if (mTarget.Complete())
        {
            Debug.Log(mTarget.TaskText + " STATE_COMLETED ");
            mTarget.mState = Task.STATE_COMLETED;
        }
    }    

    void CheckPlayerTime()
    {
        if (!isStartPlayTime)
        {
            return;
        }
        playTime++;
        Debug.Log(playTime + " playTime ");
        mTarget.mCurrentTaskData.SetIntValue(TaskData.KEY.KEY_PLAYTIME, playTime);
    }

    void CheckGameTime()
    {
        timeSecond++;
        mTarget.mCurrentTaskData.SetIntValue(TaskData.KEY.KEY_GAMETIME, timeSecond);
        Debug.Log("!!!!  " + timeSecond);
    }

    //------------------------------------------------------------

    void StartTimer()
    {
        timeSecond      = 0;
        playTime        = 0;

        timer.Enabled   = true;       
    }

    void OnTimeCounter(object source, ElapsedEventArgs e)
    {
        if (mTarget.mState != Task.STATE_UNCOMPLETE)
        {
            StopTimer();   
            return;
        }

        CheckGameTime();
        CheckPlayerTime();           
        
        CheckTaskComplete();
    }

    void StopTimer()
    {
        timer.Enabled = false;
    }

    //------------------------------------------------------------

    
}

