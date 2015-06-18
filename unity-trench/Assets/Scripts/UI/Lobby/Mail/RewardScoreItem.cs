using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RewardScoreItem : MonoBehaviour {

    [SerializeField]
    Image[] RewardTypeMark;
    [SerializeField]
    Text RewardNum;

    void showTypeMark(int rType)
    {
        foreach (Image img in RewardTypeMark)
            img.gameObject.SetActive(false);
        if(rType < RewardTypeMark.Length)
            RewardTypeMark[rType].gameObject.SetActive(true);
    }
    public void Init(int rType,int num)
    {
        if (rType == COMMON_CONST.PriceGold)
            rType = 0;
        else if (rType == COMMON_CONST.PriceDiamonds)
            rType = 2;
        showTypeMark(rType);
        RewardNum.text = num>0?" "+num:num.ToString();
        
    }
}
