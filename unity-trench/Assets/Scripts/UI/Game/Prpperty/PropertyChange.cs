using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PropertyChange : MonoBehaviour {

    [SerializeField]
    Button[] mButton;

    [SerializeField]
    VfxManager vfx;
	
	public void ChanageButton(bool onoff)
    {       

        mButton[0].interactable = onoff;
    }


    public void ShowChangeDialog(bool onoff)
    {
        gameObject.SetActive(onoff);
        if (onoff)
        {
            vfx.Play(LevelItemType.Exchange);
        }
    }



    public void SetCallBack(Action action, Action action1)
    {
        if(action == null)
        {
            //mButton[0].onClick.AddListener(null);
			mButton[0].onClick.RemoveAllListeners();
        }else
        {
            mButton[0].onClick.AddListener(action.Invoke);
        }

        if (action1 == null)
        {
            //mButton[1].onClick.AddListener(null);
			mButton[0].onClick.RemoveAllListeners();
        }
        else
        {
            mButton[1].onClick.AddListener(action1.Invoke);
        }
       
    }
}
