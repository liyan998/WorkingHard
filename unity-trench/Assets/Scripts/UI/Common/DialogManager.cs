using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{

    [SerializeField]
    protected GameObject
        coverObject;
    [SerializeField]
    protected Obstacle
        obstacle;
    [SerializeField]
    protected Dialog
        blur;
    [SerializeField]
    protected Dialog[]
        dialogs;
    protected int currentDialog = -1;
    protected int lastDialog = -1;
    public List<int> openedDialogs = new List<int>();

    public Dialog GetDialog(int dialog)
    {
        return dialogs [dialog];
    }

    public void ShowLastDialog()
    {
//        if (lastDialog > 0)
//        {
//            ShowDialog(lastDialog);
//        }
        if (openedDialogs.Count > 0)
        {
            ShowDialog(openedDialogs [openedDialogs.Count - 1], true);
        }
    }

    public void ShowCover(bool isShow=true)
    {
        coverObject.SetActive(isShow);
    }

    public void ShowDialog(int dialog)
    {
        ShowDialog(dialog, false);
    }

    public void ShowDialog(int dialog, bool isRestoreLast)
    {
        //Debug.Log(dialog + " " + isRestoreLast);
        if (!openedDialogs.Contains(dialog) || isRestoreLast)
        {
            obstacle.gameObject.SetActive(true);
            obstacle.Show();
            blur.gameObject.SetActive(true);
            blur.Show();
            dialogs [dialog].gameObject.SetActive(true);
            dialogs [dialog].Init();
            dialogs [dialog].Show();
            currentDialog = dialog;
            if (!isRestoreLast)
            {
                if (openedDialogs.Count > 0)
                {
                    HideDialog(openedDialogs [openedDialogs.Count - 1], true);
                }
                openedDialogs.Add(dialog);
            }
            OnShow();
            SOUND.Instance.OneShotSound(Sfx.inst.btn);
        }

    }

    public virtual void OnShow()
    {
    
    }

    public void HideDialog(int dialog)
    {
        HideDialog(dialog, false);
    }

    public void HideDialog(int dialog, bool isTemp)
    {

//        lastDialog = currentDialog;
        dialogs [dialog].Hide();
        if (!isTemp)
        {
            obstacle.Hide();
            blur.Hide();
            openedDialogs.Remove(dialog);
            //Debug.Log(openedDialogs.Count);
            lastDialog = currentDialog;
            if (openedDialogs.Count > 0)
            {
                ShowLastDialog();
            } else
            {
                currentDialog = -1;
            }
        }
        OnHide();
        SOUND.Instance.OneShotSound(Sfx.inst.outCard);
    }

    public virtual void OnHide()
    {
        
    }

    public void HideCurrentDialog()
    {
//        obstacle.Hide();
//        blur.Hide();
        Debug.Log(currentDialog);
        if (currentDialog > 0)
        {
            HideDialog(currentDialog);
            //currentDialog = -1;
        }
//        OnHide();
    }

//    public void HideCurrentDialogTemp()
//    {
//        if (currentDialog > 0)
//        {
//            dialogs [currentDialog].Hide();
//        }
//    }

    public int GetOpeningDialogCount()
    {
        return openedDialogs.Count;
    }
}
