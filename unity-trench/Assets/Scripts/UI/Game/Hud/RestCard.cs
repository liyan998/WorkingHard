using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RestCard : MonoBehaviour
{

    [SerializeField]
    ScaleTweener
        back;
    [SerializeField]
    Text
        SurplusText;
    int restCardNum = 0;
    float numTweenInterval = 0.01f;
    Color defaultColor;

    public int RestCardNum
    {
        get
        {
            return restCardNum;
        }
        set
        {
            //Debug.Log("restCardNum  " + restCardNum + " value " + value);
            restCardNum = value;
            SurplusText.text = restCardNum.ToString();
            if (SurplusText.gameObject.activeInHierarchy)
            {
                SurplusText.gameObject.SendMessage("Show");
            }
        }
    }
    
    void Awake()
    {
        defaultColor = SurplusText.color;
    }

    //设置剩余牌数
    public void SetRestNum(int num)
    {
        //RestCardNum = num;
        iTween.ValueTo(gameObject, iTween.Hash("from", RestCardNum, "to", num, "time", Mathf.Clamp(Mathf.Abs(RestCardNum - num) * numTweenInterval,1,3), "onupdate", "SetRestCardNum"));
        if (num <= 3 && restCardNum > num)
        {
            SurplusText.color = Color.red;
        } else
        {
            SurplusText.color = defaultColor;
        }
    }

    public void PlusRestNum()
    {
        RestCardNum++;
        SurplusText.color = defaultColor;
    }

    void SetRestCardNum(int num)
    {
        RestCardNum = num;
    }

    public Transform GetCardBackPos()
    {
        return back.transform;
    }

    public void Toggle(bool isShow)
    {
        if (isShow)
        {
            SurplusText.GetComponent<ScaleTweener>().Show();
            back.Show();
        } else
        {
            SurplusText.GetComponent<ScaleTweener>().Hide(null);
            back.Hide(null);
        }
    }
}
