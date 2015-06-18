﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopData : MonoBehaviour
{

    List<INFO_PROPERTY> goodInfo = new List<INFO_PROPERTY>();
    public List<GoodData> goodData = new List<GoodData>();
//    public List<Sprite> itemIcons = new List<Sprite>();
    public ItemData itemData;
    public List<Sprite> diamondIcons = new List<Sprite>();
    public List<Sprite> coinIcons = new List<Sprite>();
    public List<Sprite> priceIcons = new List<Sprite>();
    public List<Sprite> onSaleStates = new List<Sprite>();
    public Sprite oneCoin;

    public void Init()
    {
        goodInfo = DataBase.Instance.PROP_MGR.GetAllProperty();
        //Debug.Log (goodInfo.Count);
        goodData.Clear();
        foreach (INFO_PROPERTY info in goodInfo)
        {
            GoodData good = new GoodData();
            good.id = info.Id;
            good.desc = info.Describe;
            good.goodName = info.Name;
            //Debug.Log(info.Id);
            good.isOnSale = info.Nullity == 0 ? true : false;
            good.lifeHour = (int)info.Period;
            good.price = info.Price;
            good.priceType = (PriceType)info.PriceType;
            good.priceIcon = priceIcons [info.PriceType];
            if (info.GivenType == 0)
            {
                good.icon = coinIcons [info.Id - 30001];
                good.type = GoodType.Coin;
                good.plusNum = info.GivenValue;
            } else if (info.GivenType == 1)
            {
                good.icon = diamondIcons [info.Id - 20001];
                good.type = GoodType.Diamond;
                good.plusNum = info.GivenValue;
            } else
            {
                good.icon = itemData.GetIcon((LevelItemType)info.Id);// itemIcons [info.Id - 10001];
                good.type = (GoodType)(info.GivenValue + 3);
                good.plusNum = info.GivenCount;
            }
            good.pid = good.id.ToString();
            good.state = (GoodState)info.Nullity;
            good.stateMark = onSaleStates [(int)good.state];
            goodData.Add(good);
        }
    }
}
