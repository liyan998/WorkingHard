using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GoodItem : MonoBehaviour
{
    [SerializeField]
    Text
        nameLabel;
    [SerializeField]
    Image
        icon;
    [SerializeField]
    Text
        desc;
    [SerializeField]
    Text
        price;
    [SerializeField]
    Button
        btn;
    [SerializeField]
    Image
        onSaleMark;
    [SerializeField]
    Text
        onSaleState;
    [SerializeField]
    Image
        priceIcon;
    GoodData data;
    Action<GoodData> onPurchaseBtn;

    public virtual void Init(GoodData goodData, Action<GoodData> onBtn)
    {
        data = goodData;
        nameLabel.text = data.goodName;
        onPurchaseBtn = onBtn;
        if (icon != null)
        {
            icon.sprite = data.icon;
            icon.SetNativeSize();
            float rate = (float)(icon.preferredHeight / GetComponent<LayoutElement>().preferredHeight);
//            Debug.Log(icon.preferredHeight);
//            Debug.Log(GetComponent<LayoutElement>().preferredHeight);
//            Debug.Log(rate);
            icon.transform.localScale = Vector3.one / rate * 0.25f;
        }
        desc.text = data.desc;
        price.text = data.price.ToString();
        if (data.isOnSale)
        {
            onSaleMark.gameObject.SetActive(true);
            onSaleMark.sprite = data.stateMark;
            onSaleState.text = TextManager.Get(data.state.ToString());
        } 
        else
        {
            onSaleMark.gameObject.SetActive(false);
        }
        priceIcon.sprite = data.priceIcon;
        ToggleGood(data.isOnSale);
    }

    public void ToggleGood(bool isActive)
    {
        btn.interactable = isActive;
    }

    public void OnPurchaseBtn()
    {
        onPurchaseBtn(data);
       
        //SOUND.Instance.OneShotSound(Sfx.inst.btn); added on shop
    }

    public GoodData GetData()
    {
        return data;
    }

//  public void ToggleBtn(bool isActive){
//      btn.interactable = isActive;
//  }
}
