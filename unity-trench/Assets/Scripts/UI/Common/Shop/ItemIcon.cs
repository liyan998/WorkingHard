using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemIcon : MonoBehaviour
{
    [ContextMenuItem("Refresh","Refresh")]
    public LevelItemType
        item;
    [SerializeField]
    ItemData
        data;
    [SerializeField]
    Image
        icon;
    [SerializeField]
    Text
        desc;

    public void Refresh()
    {
        if (item != LevelItemType.None)
        {
            //Debug.Log((int)item);
            icon.sprite = data.GetIcon(item);
            desc.text = TextManager.Get("Item" + item.ToString());
        }
    }
}
