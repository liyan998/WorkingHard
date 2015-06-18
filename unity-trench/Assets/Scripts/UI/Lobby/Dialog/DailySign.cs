using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DailySign : Dialog
{
    [SerializeField]
    SignItem[]
        dailySignItems;
    [SerializeField]
    ParticleSystem
        gotVfx;
    [SerializeField]
    Button
        signBtn;
    [SerializeField]
    ShopData
        res;
    [SerializeField]
    SignData
        data;
    [SerializeField]
    UserData
        user;
    SignItem itemNeedToSign;
    int maxGiftNum=3;
    public override void Init()
    {
        for (int i=0; i<dailySignItems.Length; i++)
        {
            bool isThatDaySigned = false;
            isThatDaySigned = !(data.continueSignedDays <= i);
            if (itemNeedToSign == null && isThatDaySigned == false)
            {
                itemNeedToSign = dailySignItems [i];
            }
            dailySignItems [i].Init(i != 0 ? res.coinIcons [i - 1] : res.oneCoin, string.Format(TextManager.Get("Day"), i + 1), "+" + data.signInfo [i].GivenValue, isThatDaySigned);
        }
        if (data.isTodaySigned)
        {
            signBtn.interactable = false;
        } else
        {
            PlayerPrefs.SetInt("LowCoinGift", maxGiftNum);
            PlayerPrefs.SetString("ShareStates", "0,0,0");
        }
    }

    public void OnGetButton()
    {
        gotVfx.transform.position = itemNeedToSign.transform.position;
        gotVfx.Play();
        itemNeedToSign.ShowMark();
        signBtn.interactable = false;
        if (!data.isTodaySigned)
        {
            user.coin += data.signInfo [data.continueSignedDays].GivenValue;
            data.Sign();
            Init();
            user.SaveMoney();
            UserHud.inst.RefreshMoney();
        }
        SOUND.Instance.OneShotSound(Sfx.inst.reward);
    }

}
