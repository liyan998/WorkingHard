using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TaskData
{
    public enum KEY
    {
        KEY_GAMETIME,           //游戏时长
        KEY_GAMECOUNT,          //游戏次数

        KEY_PLAYTIME,           //出牌时长
        KEY_WINCOUNT,           //胜利次数

        KEY_WINSQUENCOUNT,      //连续胜利次数
        KEY_BLACKWINCOUNT,      //黑挖胜利次数
        KEY_MASTERWINCOUNT,     //坑主胜利次数
        KEY_NORMBLEWINCOUNT,    //平民胜利次数

        KEY_CARDCATEGROY,       //手牌特殊类型
        KEY_CARDSCORE,          //手牌特殊牌面

        KEY_CARDCATEGROYCOUNT,  //手牌特殊牌型数目
        KEY_CARDSCORECOUNT,     //手牌特殊牌面次数

        KEY_FINALCATEGROY,      //尾牌特殊牌型
        KEY_FINALSCORE,         //尾牌特殊牌面
        KEY_FINALCATEGROYCOUNT, //尾牌特殊牌型次数

        KEY_STARTCATEGROY,      //起始特殊牌型
        KEY_STARTSCORE,         //起始特殊牌面
        KEY_STARTCATEGROYCOUNT, //起始特殊牌型次数
    }


    public Dictionary<KEY, int> mAllTaskData;

    public TaskData()
    {
        mAllTaskData = new Dictionary<KEY, int>();
    }

    
    public int GetIntValue(KEY key)
    {
        if(mAllTaskData.ContainsKey(key))
        {
            return mAllTaskData[key];
        }
        return -1;
    }

    public bool SetIntValue(KEY key , int n)
    {
        if(!mAllTaskData.ContainsKey(key))
        {
            return false;
        }        

        mAllTaskData[key] = n;
        return true;
    }

    /// <summary>
    /// 自加1
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool PlusPlusIntValue(KEY key)
    {
        if(!mAllTaskData.ContainsKey(key))
        {
            return false;
        }
        mAllTaskData[key]++;
        return false;
    }




    //-----------------------------------------------------------------------
   


    public TaskData CloneDataTemp()
    {
        TaskData temp = new TaskData();
        foreach(KeyValuePair<KEY, int> keypair in mAllTaskData)
        {
            switch(keypair.Key)
            {
                case KEY.KEY_GAMETIME:          //游戏时长
                case KEY.KEY_PLAYTIME:          //出牌时长

                    temp.mAllTaskData.Add(keypair.Key, keypair.Value);

                    break;
                case KEY.KEY_GAMECOUNT:         //游戏次数
                case KEY.KEY_WINCOUNT:          //胜利次数
                case KEY.KEY_CARDCATEGROYCOUNT: //手牌特殊牌型数目
                case KEY.KEY_CARDSCORECOUNT:    //手牌特殊牌面次数
                case KEY.KEY_WINSQUENCOUNT:     //连续胜利次数
                case KEY.KEY_BLACKWINCOUNT:     //黑挖胜利次数
                case KEY.KEY_MASTERWINCOUNT:    //坑主胜利次数
                case KEY.KEY_NORMBLEWINCOUNT:   //平民胜利次数
                case KEY.KEY_FINALCATEGROYCOUNT://尾牌特殊牌型次数
                case KEY.KEY_STARTCATEGROYCOUNT://首牌特殊牌型次数

                    temp.mAllTaskData.Add(keypair.Key, 0);

                    break;
            }
        }
        return temp;
    }
}

