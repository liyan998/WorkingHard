using UnityEngine;
using System;
using System.Collections;

public class LoadingDlgManager : DialogManager {

	public static LoadingDlgManager inst;
	public ConfirmDialog confirmDialog;

	void Awake()
	{
		inst = this;
	}

//	public  Dialog GetDialog(LobbyDialog dialog)
//	{
//		return dialogs [(int)dialog];
//	}
//	
//	public  void ShowDialog(LobbyDialog dialog)
//	{
//		ShowDialog((int)dialog);
//	}
	
	public void ShowConfirmDialog(string descString, Action onConfirmed, string confirmDescString=null, bool hasCancelBtn=true, Action onCancelled=null, string cancelDescString=null, string title=null)
	{
		confirmDialog.Init(descString, onConfirmed, confirmDescString, hasCancelBtn, onCancelled, cancelDescString, title);
		ShowDialog(0);
	}
	
//	public void HideDialog(LobbyDialog dialog)
//	{
//		HideDialog((int)dialog);
//	}
}
