using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameAutoController : MonoBehaviour
{

    [SerializeField]
    Button mDelegateButton;//托管

    [SerializeField]
    Button mCancalDelegate;//取消托管


    void Start()
    {
        mDelegateButton.onClick.AddListener(SetDelegate);
        mCancalDelegate.onClick.AddListener(CancalDelegate);
    }


    public void SetDelegate(bool avable)
    {
        mDelegateButton.interactable = avable;
    }

    void CancalDelegate()
    {
        if (!MainGame.inst.SetAutoOutCard(false))
        {
            return;
        }

        mDelegateButton.interactable = true;        

    }

    void SetDelegate()
    {
        if (!MainGame.inst.SetAutoOutCard(true))
        {
            return;
        }
        mDelegateButton.interactable = false;        
    }

  
}
