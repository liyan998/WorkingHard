using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
/// <summary>
/// 
/// </summary>
class TaskFactory
{

    List<Task> mAllTask;

    public TaskFactory()
    {
        mAllTask = new List<Task>();

        //----------------------------------------

        Task task1          = new Task();

        task1.TaskId        = 1;
        task1.TaskText      = "取得游戏胜利";
        task1.TaskTrigger   = 0.2f;
        task1.TaskWinMutlip = 1000;

        //--------------------------------------

        TaskData taskdata = new TaskData();      

        int[] resdata = new int[18];
        for (int i = 0; i < resdata.Length;i++ )
        {
            resdata[i] = -1;
        }

        //resdata[(int)TaskData.KEY.KEY_GAMETIME]                   = 180;
        //resdata[(int)TaskData.KEY.KEY_PLAYTIME]                   = 10;
        //resdata[(int)TaskData.KEY.KEY_WINCOUNT]                   = 1;  
        //resdata[(int)TaskData.KEY.KEY_BLACKWINCOUNT]              = 1;
        //手牌牌型 分值
        //resdata[(int)TaskData.KEY.KEY_CARDCATEGROY]               = GLogicDef.CT_DOUBLE;
        //resdata[(int)TaskData.KEY.KEY_CARDCATEGROYCOUNT]          = 1;
        //resdata[(int)TaskData.KEY.KEY_CARDSCORE]                  = 4;
        //尾牌
        //resdata[(int)TaskData.KEY.KEY_FINALCATEGROY]              = GLogicDef.CT_DOUBLE;
        //resdata[(int)TaskData.KEY.KEY_FINALCATEGROYCOUNT]         = 1;
        //首次出牌
        //resdata[(int)TaskData.KEY.KEY_STARTCATEGROY]                = GLogicDef.CT_DOUBLE;
        //resdata[(int)TaskData.KEY.KEY_STARTCATEGROYCOUNT]           = 1;

        for (int i = 0; i < resdata.Length;i++ )
        {
            if (resdata[i] != -1)
            {
                taskdata.mAllTaskData[(TaskData.KEY)i] = resdata[i];               
            }
        }

        //task1.SetTargetTask(taskdata);

        //mAllTask.Add(task1);
    }


   

    public Task RandTask()
    {
        return mAllTask[0];
    }


    public void initData(string xmldata)
    {
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(xmldata);

        XmlElement xmlroot = xmldoc.DocumentElement;
        XmlNodeList xmlNodeList = xmlroot.SelectNodes("/TaskRoot/Task");

        for (int i = 0; i < xmlNodeList.Count; i++)
        {
            XmlNode xn = xmlNodeList.Item(i);
            Task task = GetTaskFromXml(xn.OuterXml);
            //Debug.Log(task.TaskId);

            mAllTask.Add(task);
        }
    }    

    private Task GetTaskFromXml(string xmlstr)
    {
        Task task = new Task();

        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(xmlstr);
        XmlElement xmlroot = xmldoc.DocumentElement;

        task.TaskId         = GetIntFromItem(xmlroot,   "/Task/TaskID");
        task.TaskText       = GetStringFromItem(xmlroot,"/Task/TaskText");
        task.TaskTrigger    = GetFloatFromItem(xmlroot, "/Task/Probability");
        task.TaskWinMutlip  = GetIntFromItem(xmlroot,   "/Task/RewardRatio");

        //task.SetTargetTask(GetTaskDataFromXml(xmlroot.SelectSingleNode("/Task/TaskData").OuterXml));
        task.SetTargetTask(GetTaskData(xmlroot));
        return task;
    }
    

    public static  int GetIntFromItem(XmlElement xe, string itemtext)
    {
        return int.Parse( GetStringFromItem( xe, itemtext ) );
    }

    public static string GetStringFromItem(XmlElement xe, string itemtext)
    {
        return xe.SelectSingleNode(itemtext).InnerText;
    }

    public static float GetFloatFromItem(XmlElement xe, string itemtext)
    {
        return float.Parse(GetStringFromItem(xe, itemtext));
    }


    private TaskData GetTaskData(XmlElement xe)
    {
        TaskData td = new TaskData();
        string labtask = "/Task/";
        foreach (KeyValuePair<string, TaskData.KEY> kv in mKeyMap)
        {
            string valstr = GetStringFromItem(xe, labtask + kv.Key);
            if(valstr != "-1")
            {
                td.mAllTaskData.Add(kv.Value, int.Parse(valstr));
            }
        }
        return td;
    }


    readonly static Dictionary<string, TaskData.KEY> mKeyMap = new Dictionary<string,TaskData.KEY>()
                    { 
                    {"GameTime",                TaskData.KEY.KEY_GAMETIME},
                    {"GameNumber",              TaskData.KEY.KEY_GAMECOUNT},
                    {"PlayTime",                TaskData.KEY.KEY_PLAYTIME},
                    {"VictoryNumber",           TaskData.KEY.KEY_WINCOUNT},
                    {"ForVicNum",               TaskData.KEY.KEY_WINSQUENCOUNT},
                    {"DiggingVicNum",           TaskData.KEY.KEY_BLACKWINCOUNT},
                    {"MasterVicNum",            TaskData.KEY.KEY_MASTERWINCOUNT},
                    {"FarmersVicNum",           TaskData.KEY.KEY_NORMBLEWINCOUNT},

                    {"HandCarTyp",              TaskData.KEY.KEY_CARDCATEGROY},
                    {"HandCard",                TaskData.KEY.KEY_CARDSCORE},
                    {"HandCarTypNum",           TaskData.KEY.KEY_CARDSCORECOUNT},
                    {"HandCarTypRound",         TaskData.KEY.KEY_CARDCATEGROYCOUNT},

                    {"DeviceCarTyp",            TaskData.KEY.KEY_FINALCATEGROY},
                    {"DeviceCard",              TaskData.KEY.KEY_FINALSCORE},
                    {"DeviceCarTypRound",       TaskData.KEY.KEY_FINALCATEGROYCOUNT},

                    {"FirstBrandCarTyp",        TaskData.KEY.KEY_STARTCATEGROY},
                    {"FirstBrandCard",          TaskData.KEY.KEY_STARTSCORE},
                    {"FirstBrandCarTypRound",   TaskData.KEY.KEY_STARTCATEGROYCOUNT},
                    };


   


    private TaskData GetTaskDataFromXml(string xmlstr)
    {
        TaskData td = new TaskData();

        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(xmlstr);

        XmlElement xmlroot = xmldoc.DocumentElement;
        XmlNodeList xnl = xmlroot.ChildNodes;

        for (int i = 0; i < xnl.Count; i++)
        {
            XmlNode xn = xnl.Item(i);
            if (mKeyMap.ContainsKey(xn.Name))
            {
                string tempstr = xn.InnerText;
                if(tempstr != "")
                {
                    td.mAllTaskData.Add(mKeyMap[xn.Name], int.Parse(xn.InnerText));
                }                
            }          
        }

        return td;
    }





}

