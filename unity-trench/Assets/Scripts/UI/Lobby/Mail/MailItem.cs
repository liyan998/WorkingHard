using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MailItem : MonoBehaviour {

    [SerializeField]
    Image[] MailMark; //0 未读 1 已读 3 附件
    [SerializeField]
    Text MailTitle; //邮件标题
    [SerializeField]
    Text FromMan;   //发件人
    [SerializeField]
    Text FromDate;  //发件日期



    public void Init()
    { 
        
    }

}
