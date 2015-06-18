using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShareMgr
{

    Participation share = new Participation();
    
    public void Load()
    {
        string data = Resources.Load(COMMON_CONST.PathShare).ToString();
        //Debug.Log (data);
        share.XmlLoad(data, "Participation");
        //Debug.Log (sign.INFO.Values.Count);
    }
    
    public List<INFO_SHARE> GetAllShareData()
    {
        List<INFO_SHARE> list = new List<INFO_SHARE>();
        list.AddRange(share.INFO.Values);
        return list;
    }
    
    public INFO_SHARE GetShareData(int shareID)
    {
        foreach (var e in share.INFO.Values)
        {
            if (e.Id == shareID)
                return e;
        }
        
        return null;
    }
}
