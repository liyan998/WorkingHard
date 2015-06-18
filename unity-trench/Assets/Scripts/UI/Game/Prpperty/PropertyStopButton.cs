using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PropertyStopButton : MonoBehaviour
{

    [SerializeField]
    Button
        mStopOut;
    [SerializeField]
    ScaleTweener
        tweener;
    [SerializeField]
    Image icon;
    [SerializeField]
    Image
        selectedMark;
    [SerializeField]
    Sprite[] icons;
    State mState;


    public enum State
    {
        STATE_DISABLED,//不可用
        STATE_ENABLE,//可选
        STATE_SELECTED,//已经选择
        STATE_HIDE,//不
    }


    void Start()
    {

        mStopOut.onClick.AddListener(ChangeButtonState);
        //SetState(State.STATE_ENABLE);
    }

    public void SetState(State state)
    {
        switch (state)
        {
            case State.STATE_DISABLED:
                mStopOut.interactable = false;
                icon.sprite=icons[0];
                break;
            case State.STATE_HIDE:
                mStopOut.interactable = false;
                tweener.Hide(null);
                SOUND.Instance.OneShotSound(Sfx.inst.hide);
                break;
            case State.STATE_ENABLE:
               // mStopOut.image.sprite = mAllButtonState[1];
                selectedMark.gameObject.SetActive(false);
                if (!mStopOut.interactable)
                {
                    mStopOut.interactable = true;
                    tweener.Show();
                    icon.sprite=icons[1];
                    SOUND.Instance.OneShotSound(Sfx.inst.show);
                }
                break;
            case State.STATE_SELECTED:
                //mStopOut.image.sprite = mAllButtonState[0];
                selectedMark.gameObject.SetActive(true);
                break;
          
        }
        this.mState = state;
    }

    public State GetState()
    {
        return this.mState;
    }
    
    public void ChangeButtonState()
    {
        switch (mState)
        {
            case State.STATE_ENABLE:
                SetState(State.STATE_SELECTED);               
                break;
            case State.STATE_SELECTED:
                SetState(State.STATE_ENABLE);
                break;
                
        }

        Debug.Log("STATE:" + mState);
    }    
}
