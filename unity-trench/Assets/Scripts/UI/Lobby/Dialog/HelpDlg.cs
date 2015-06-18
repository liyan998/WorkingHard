using UnityEngine;
using System.Collections;

public class HelpDlg : Dialog
{

    public void OnServiceBtn()
    {
        ToastDlg.inst.gameObject.SetActive(true);
        ToastDlg.inst.Toast();
    }
}
