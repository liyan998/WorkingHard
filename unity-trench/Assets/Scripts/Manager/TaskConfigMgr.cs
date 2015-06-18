using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml;

public class TaskConfigMgr
{
    public void LoadData()
    {
        string xmldata = Resources.Load(COMMON_CONST.PathTaskInfo).ToString();

        TaskChannal.Instance.initData(xmldata);     
    }




    

   
}
