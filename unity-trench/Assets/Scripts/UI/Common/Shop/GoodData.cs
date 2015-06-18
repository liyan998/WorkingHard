using UnityEngine;
using System.Collections;

public enum GoodType
{
    Diamond,
    Coin,
    Character,
    Item1,
    Item2
}

public enum PriceType
{
    Coin,
    Diamond,
    Cash
}

public enum GoodState
{
    OnSale,
    BigSale,
    Cheap,
    Discount,
    Gone,
}

public class GoodData
{
    public string goodName;
    public GoodType type;
    public float price;
    public PriceType priceType;
    public string desc;
    public Sprite icon;
    public Sprite stateMark;
    public Sprite priceIcon;
    public int id;
    public string pid;
    public bool isOnSale;
    public int lifeHour;
    public int plusNum;
    public GoodState state=GoodState.OnSale;
}
