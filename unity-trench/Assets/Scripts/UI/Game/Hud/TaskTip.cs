using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TaskTip : MonoBehaviour {

    [SerializeField]
    Text mTaskTip;

    public void SetTaskTip(string text)
    {
        if(!gameObject.activeSelf)
        {
            gameObject.SetActive(  true );
        }
        mTaskTip.text = text;
    }

    
}
