using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BindPhone : Dialog {


    [SerializeField]
    Text PhoneAccount;
    [SerializeField]
    Text UserPwd;
    [SerializeField]
    Text VeriCode;

    [SerializeField]
    Text AccountMsg;
    [SerializeField]
    Text PwdMsg;
    [SerializeField]
    Text VerMsg;

    [SerializeField]
    Text SendText;
	// Use this for initialization
	void Start () {
        SendText.text = TextManager.Get("SendIdentifyingCode");
	}

    int sendSeconds = 60;
    public void OnSendVeriCode(GameObject but)
    {
        Button sendBut = but.GetComponent<Button>();
        sendBut.interactable = false;
        sendSeconds = 60;
        //执行短信发送命令
        StartCoroutine(sendText(sendBut));
    }

    
    IEnumerator sendText(Button but)
    {

        while (sendSeconds > -1)
        { 
            SendText.text = string.Format(TextManager.Get("ResendIdentifyingCode"),sendSeconds.ToString());
            yield return new WaitForSeconds(1);
            sendSeconds--;
        }
        SendText.text = TextManager.Get("SendIdentifyingCode");
        but.interactable = true;
        yield return 0;
    }

    
    public void OnBindPhone()
    {
        string account = PhoneAccount.text;
        string pwd = UserPwd.text;
        string vericode = VeriCode.text;
        if (!string.IsNullOrEmpty(account))
        { 
            AccountMsg.text = TextManager.Get("TeleNumVoid");
            return;
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(account, @"^[1]+\d{10}"))
        {
            AccountMsg.text = TextManager.Get("TeleNumInvalid");
            return;
        }

        if(pwd.Length < 6 || pwd.Length > 20)
        {
            AccountMsg.text = TextManager.Get("PasswordInvalid");
            return;
        }
        if (!System.Text.RegularExpressions.Regex.IsMatch(account, @"^\d{6}"))
        {
            AccountMsg.text = TextManager.Get("IdentifyingCodeInvalid");
            return;
        }
        
    }


    //绑定命令返回回调
}
