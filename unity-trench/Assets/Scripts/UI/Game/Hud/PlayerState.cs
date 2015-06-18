﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerState : MonoBehaviour {

	[SerializeField]
	Text
		label;
	[SerializeField]
	ScaleTweener
		tweener;
//	[Tooltip("0:scorePass,1:1,2:2,3:3,4:ready,5:playPass,6,black,7:none")]
	
	public void SetPlayerState (PlayerStateText state)
	{
		if (state == PlayerStateText.None) {
			Hide ();
		} else {
            label.text = TextManager.Get(state.ToString());
			Show();
		}
	}

	public void Hide(){
		tweener.Hide (null);
		SOUND.Instance.OneShotSound (Sfx.inst.hide);
	}

	public void Show(){
		tweener.Show ();
		SOUND.Instance.OneShotSound (Sfx.inst.show);
	}
}
