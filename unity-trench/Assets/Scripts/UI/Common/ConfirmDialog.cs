﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ConfirmDialog : Dialog
{
    [SerializeField]
    Text
        title;
    [SerializeField]
    Text
        desc;
    [SerializeField]
    Text
        confirmDesc;
    [SerializeField]
    Text
        cancelDesc;
    [SerializeField]
    Button
        confirmBtn;
    [SerializeField]
    Button
        cancelBtn;
//    [SerializeField]
//    Button
//        closeBtn;
    [SerializeField]
    Button
        backBtn;
    Action onConfirm;
    Action onCancel;
    float btnX = 125f;

    public void Init(string descString, Action onConfirmed, string confirmDescString=null, bool hasCancelBtn=true, Action onCancelled=null, string cancelDescString=null, string titleString=null)
    {
        title.text = titleString;
        desc.text = descString;
        onConfirm = onConfirmed;
        onCancel = onCancelled;
        if (!string.IsNullOrEmpty(confirmDescString))
        {
            confirmDesc.text = confirmDescString;
        } else
        {
            confirmDesc.text = TextManager.Get("Confirm");
        }
        if (!string.IsNullOrEmpty(cancelDescString))
        {
            cancelDesc.text = cancelDescString;
        } else
        {
            cancelDesc.text = TextManager.Get("Cancel");
        }
        if (hasCancelBtn)
        {
            cancelBtn.gameObject.SetActive(true);
//            closeBtn.gameObject.SetActive(true);
            backBtn.enabled = true;
            cancelBtn.transform.localPosition = new Vector3(btnX, cancelBtn.transform.localPosition.y, cancelBtn.transform.localPosition.z);
            confirmBtn.transform.localPosition = new Vector3(-btnX, confirmBtn.transform.localPosition.y, confirmBtn.transform.localPosition.z);
        } else
        {
            cancelBtn.gameObject.SetActive(false);
//            closeBtn.gameObject.SetActive(false);
            backBtn.enabled = false;
            confirmBtn.transform.localPosition = new Vector3(0, confirmBtn.transform.localPosition.y, confirmBtn.transform.localPosition.z);
        }
    }

    public void OnConfirmBtn()
    {
        if (onConfirm != null)
        {
            onConfirm();
        }
        //Hide();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);

  
    }

    public void OnCancelBtn()
    {
        if (onCancel != null)
        {
            onCancel();
        }

        //Hide();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }
}
