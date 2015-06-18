using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RewardItem : MonoBehaviour
{
    public LevelItemType item;
    [SerializeField]
    Text
        RewardName;
    [SerializeField]
    Image
        icon;
    [SerializeField]
    Image
        bkg;
    [SerializeField]
    Sprite
        unselected;
    [SerializeField]
    Sprite
        selected;

    public void SetReward(string name, Sprite image, LevelItemType type)
    {
        item = type;
        this.RewardName.text = name;
        icon.sprite = image;
    }

//    public void SetType(LevelItemType type){
//    
//    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            bkg.sprite = selected;
        } else
        {
            bkg.sprite = unselected;
        }
    }
}
