using System;
using System.Collections.Generic;

using UnityEngineEx.Net;

//关卡记录状态
public enum StageRecordStatus
{
    Failure, //失败
    Progress,//进行
    Success  //成功
}
[Serializable]
public class UserStageRecord
{
    
    public uint dwUserId;                   //用户
    public uint dwStageId;                  //关卡记录ID
    public byte IsUsableProperty;           //道具可用状态
    public int Property;                    //关卡抽奖道具
    public int Status;                      //过关状态 0 失败 1 进行中 2 通关
    public int LastStatus;                  //
    public int BestScore;                   //最好成绩
    public int LastScore;
    public uint CondId1;
    public uint CondVal1;
    public Dictionary<string, uint> Cond2;
    public uint CondId3;
    public uint CondVal3;
}

public class PlayerStageRecord
{
    Dictionary<string, UserStageRecord> mUserStageRecord = new Dictionary<string, UserStageRecord>();


    public void Load()
    {
        string record = COMMON_FUNC.GetUserStageRecord();
        if(record != COMMON_CONST.NullString)
        {
            mUserStageRecord = record.FromJSON<Dictionary<string, UserStageRecord>>();
        }
    }

    public void WriteRecord()
    {
        if (mUserStageRecord.Count == 0) return;
        COMMON_FUNC.SetUserStageRecord(mUserStageRecord.ToJSON());
    }

    

    public void SetUserRecord(UserStageRecord record)
    {
        
        record.dwUserId = DataBase.Instance.PLAYER.mUser.dwUserID;
        if (mUserStageRecord.ContainsKey(record.dwStageId.ToString()) == false) 
        {
            record.Status = record.LastStatus;
            mUserStageRecord.Add(record.dwStageId.ToString(), record);
        }
        else
        {
            record.Status = mUserStageRecord[record.dwStageId.ToString()].Status;
            if (mUserStageRecord[record.dwStageId.ToString()].BestScore > record.BestScore)
                record.BestScore = mUserStageRecord[record.dwStageId.ToString()].BestScore;

            mUserStageRecord[record.dwStageId.ToString()] = record;
        }
    }
    /// <summary>
    /// 获取所有记录
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, UserStageRecord> GetUserAllStageRecord()
    {
        return mUserStageRecord;
    }


    public uint GetLastSuccessStage()
    {
        uint last = 0;
        foreach (var v in mUserStageRecord)
        {
            if (v.Value.Status == (int)StageRecordStatus.Success)
                last = uint.Parse(v.Key);
        }
        return last;
    }

    /// <summary>
    /// 获取关卡记录
    /// </summary>
    /// <param name="stage">关卡编号</param>
    /// <returns></returns>
    public UserStageRecord GetUserStageRecord(uint stage)
    {
        if (mUserStageRecord.ContainsKey(stage.ToString()))
            return mUserStageRecord[stage.ToString()];
        else
            return null;
    }

    /// <summary>
    /// 获取所有关卡星星数和
    /// </summary>
    /// <returns></returns>
    public int GetTotalStar()
    {
        int Stars = 0;
        foreach(var e in mUserStageRecord.Values)
        {
            if ((StageRecordStatus)e.Status == StageRecordStatus.Success)
                Stars += COMMON_FUNC.GetStarByScore(e.dwStageId, e.BestScore);
        }

        return Stars;
    }

    public int GetCompleteStageCount()
    {
        int num = 0;
        foreach (var e in mUserStageRecord.Values)
        {
            if ((StageRecordStatus)e.Status == StageRecordStatus.Success)
                num ++;
        }

        return num;
    }
}
