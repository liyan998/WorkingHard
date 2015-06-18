using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System;

public class TimerComponet : MonoBehaviour {

    [SerializeField]
    Text mTimeText;

    void Start()
    {
        StartCoroutine(SetTime());
    }


    IEnumerator SetTime()
    {

        while(true)
        {
            DateTime dt = DateTime.Now;

            mTimeText.text = dt.ToShortTimeString().ToString();
            yield return new WaitForSeconds(1);

        }
    }
}
