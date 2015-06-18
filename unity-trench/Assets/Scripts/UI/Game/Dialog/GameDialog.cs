using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public enum GameDialogIndex
{
    CONFIRM,    //
    FINAL,      //结算
    TASKALERT,  //任务提示
    STAGEMARK,  //关卡进度
    STAGERESULT, //关卡结果
    MissionState,
    Share,
    Help
}

public class GameDialog : DialogManager
{
    [SerializeField]
    ConfirmDialog
        mConfigDialog;
    [SerializeField]
    LevelCompleteDlg
        levelCompleteDialog;
    [SerializeField]
    MoveTweener
        curtain;
    //---------------------------------------

    /// <summary>
    /// 结算面板类型 普通场，炸弹场
    /// </summary>
    public const int CATEGORY_NORMALE = 1;
    /// <summary>
    /// 结算面板类型 关卡
    /// </summary>
    public const int CATEGORY_STAGE = 2;
    public static GameDialog inst;

    void Awake()
    {
        inst = this;
    }
    
    void Start()
    {
        curtain.FromTarget();
    }

    public void OnConfirmExit()
    {        
//        mConfigDialog.Init(MainGame.inst.GetBreakButtonTip(), ExitGameScene);
//        mConfigDialog.gameObject.SetActive(true);
//        mConfigDialog.Show(GameDialogIndex.CONFIRM);
        ShowConfirmDialog(MainGame.inst.GetBreakButtonTip(), ExitGameScene);
    }

    public void ShowConfirmDialog(string descString, Action onConfirmed, string confirmDescString=null, bool hasCancelBtn=true, Action onCancelled=null, string cancelDescString=null, string title=null)
    {
        mConfigDialog.Init(descString, onConfirmed, confirmDescString, hasCancelBtn, onCancelled, cancelDescString, title);
//        mConfigDialog.gameObject.SetActive(true);
//        mConfigDialog.Show();
        ShowDialog(GameDialogIndex.CONFIRM);
    }

    void ExitGameScene()
    {
        curtain.ToTarget(OnExitGame);
    }

    public void OnExitGame()
    {
        Debug.Log("exit Game!!!!!!!!");
        MainGame.inst.OnExitGame();
        
    }

    public void AlertTaskText(string contentStr)
    {
        TaskAlert ta = dialogs [(int)GameDialogIndex.TASKALERT] as TaskAlert;
        ta.gameObject.SetActive(true);
        ta.ShowText(contentStr);
    }
    
    /// <summary>
    /// 设置结算面板 
    /// </summary>
    /// <param name="category">CATEGORY_STAGE | CATEGORY_NORMALE</param>
    /// <param name="isWin">是否 获胜</param>
    /// <param name="allData">显示分数</param>
    /// <param name="action">继续回调</param>
    public void SetFinalWin(int category, bool isWin, int[] allData, System.Action action1=null,
        System.Action action2 = null, System.Action action3 = null)
    {
        HideDialog((int)GameDialogIndex.MissionState);
        ShowDialog((int)GameDialogIndex.FINAL);
        if (GetDialog(currentDialog) is FinalDialog)
        {
            FinalDialog finaldlg = GetDialog(currentDialog) as FinalDialog;
            
//            finaldlg.SetWin(isWin);
//            finaldlg.SetConuite(action);

            switch (category)
            {
                case CATEGORY_NORMALE:
                    finaldlg.Init(isWin, true, allData, action1, action2, action3);
                    break;
                case CATEGORY_STAGE:
                    finaldlg.Init(isWin, false, allData, action1, action2, action3);
                    break;
            }
        }
    }

    public void ShowStageResult(bool isSuccess, int starNum, int score,string target, List<INFO_REWARD> reward,
                                Action onBack,
                                Action onNext, Action onReplay)
    {
        HideDialog((int)GameDialogIndex.MissionState);
        ShowDialog((int)GameDialogIndex.STAGERESULT);
        levelCompleteDialog.Init(isSuccess, starNum,score, reward, target, onBack, onNext, onReplay);
//      GateFinalDialog dig = currentDialog as GateFinalDialog;
//      if (dig != null) {
//          dig.InitFinal (isSuccess, starNum, target, score,reward);
//            dig.SetFinalConfirmation(butTitle, delegate
//            {
//                HideDialog((int)GameDialogIndex.STAGERESULT);
//                butAction();
//            });
//            dig.SetFinalCancel(butTitle2, delegate
//            {
//                HideDialog((int)GameDialogIndex.STAGERESULT);
//                butAction2();
//            });
//      }
    }

    int last = 0;

    public void ShowDialog(GameDialogIndex dialog)
    {
        ShowDialog((int)dialog);
    }

    public void HideDialog(GameDialogIndex dialog)
    {
        HideDialog((int)dialog);
        last = (int)dialog;
    }

//    public void ShowLastDialog()
//    {
//        ShowDialog((int)last);
//    }


}
