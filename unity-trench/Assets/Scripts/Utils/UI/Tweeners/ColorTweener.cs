using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ColorTweener : Tweener
{
    [SerializeField]
    Color
        targetColor = Color.clear;
    [SerializeField]
    bool
        isText = false;
    Color originalColor;
    Action onFinish;

    void Awake()
    {
        if (!isText)
        {
            originalColor = GetComponent<Image>().color;
        }
        else{
            originalColor = GetComponent<Text>().color;
        }
    }

    public void ColorFrom()
    {
        FromTarget();
    }

    public override void ToTarget(Action onComplete=null, float time=-1f)
    {
        float moveTime = time < 0 ? tweenTime : time;
        onFinish = onComplete;
        iTween.ColorTo(gameObject, iTween.Hash("color", targetColor,
                                                         "easetype", toEaseType,
                                                         "oncomplete", "OnFinish",
                                                 "time", moveTime));
    }
    
    public override void FromTarget(Action onComplete=null, float time=-1f)
    {
        float moveTime = time < 0 ? tweenTime : time;
        if (!isText)
        {
            GetComponent<Image>().color = originalColor;
        }
        else{
            GetComponent<Text>().color = originalColor;
        }
        
        iTween.ColorFrom(gameObject, iTween.Hash("color", targetColor,
                                                   "easetype", fromEaseType,
                                                   "time", moveTime));
    }

    void OnFinish()
    {
        if (onFinish != null)
        {
            onFinish();
        }
    }
}
