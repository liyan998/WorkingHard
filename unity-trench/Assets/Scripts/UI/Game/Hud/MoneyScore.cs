using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MoneyScore : MonoBehaviour {

    [SerializeField]
    Text
        SurplusText;
    int restCardNum = 0;


	void Start () {
	
	}

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
        }
    }
    
 
    public void SetRestNum(int num)
    {
        RestCardNum = num;     
        
    }
}
