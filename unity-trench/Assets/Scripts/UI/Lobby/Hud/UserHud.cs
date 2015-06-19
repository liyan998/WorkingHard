using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserHud : MonoBehaviour
{
	public static UserHud inst;
	[SerializeField]
	UserData
		data;
	[SerializeField]
	Image
		head;
	[SerializeField]
	Text
		userName;
	[SerializeField]
	Text
		coin;
	[SerializeField]
	Text
		diamond;
	[SerializeField]
	Tweener
		backBtn;
//    [SerializeField]
//    Tweener SettingBtn;
	[SerializeField]
	Tweener
		shopBtn;
	[SerializeField]
	Tweener
		mailBtn;
	[SerializeField]
	Tweener
		userHead;
	bool isBackBtnToggled = false;

	void Awake ()
	{
		inst = this;
	}

	public void Init ()
	{
		head.sprite = data.head;
		userName.text = data.uiName;
		coin.text = data.coin.ToString ();
		diamond.text = data.diamond.ToString ();
		if (PlayerPrefs.GetInt ("IsLobby", 0) == 2) {
			shopBtn.ToTarget ();
			mailBtn.gameObject.SetActive (true);
			mailBtn.FromTarget ();
		}
	}

	public void ToggleBackBtn (bool isShow)
	{
		if (isShow != isBackBtnToggled) {
			if (isShow) {
				backBtn.gameObject.SetActive (true);
				backBtn.FromTarget ();
				userHead.ToTarget ();
			} else {
				backBtn.ToTarget ();
				userHead.FromTarget ();
			}
			isBackBtnToggled = isShow;
		}
	}

	public void RefreshMoney ()
	{
		data.RefreshMoney ();
		if (coin) {
			coin.text = data.coin.ToString ();
		}
		if (diamond) {
			diamond.text = data.diamond.ToString ();
		}
	}

	public void OnBackBtn ()
	{
		//Debug.Log(LobbyDialogManager.inst.GetOpeningDialogCount());
		if (LobbyDialogManager.inst.GetOpeningDialogCount () != 0) {
			LobbyDialogManager.inst.HideCurrentDialog ();
			if (LobbyDialogManager.inst.GetOpeningDialogCount () != 0) {
				ToggleBackBtn (true);
			} else {
				if (LobbyController.inst.backStates.Count > 0) {
					ToggleBackBtn (true);
				} else {
					ToggleBackBtn (false);
				}
				//LobbyController.inst.BackToLastState();
			}
		} else {
			//Debug.Log(LobbyController.inst.currentState);
			if (LobbyController.inst.currentState == LobbyState.Dialog || LobbyController.inst.currentState == LobbyState.Shop) {
				LobbyController.inst.BackToLastState ();
			} else {
				LobbyController.inst.SwitchState (LobbyState.MainMenu);
			}
//            if (LobbyController.inst.backState != LobbyState.MainMenu)
//            {
//                ToggleBackBtn(true);
//            } else
//            {
//                ToggleBackBtn(false);
//            }
		}
		SOUND.Instance.OneShotSound (Sfx.inst.btn);
	}
}
