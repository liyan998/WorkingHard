using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PayGift : MonoBehaviour {

    [SerializeField]
    Image itemicon;
    [SerializeField]
    Text itemname;

    public void SetInfo(Sprite icon,string name)
    {
        itemicon.sprite = icon;
        itemname.text = name;
    }
}
