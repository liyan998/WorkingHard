using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RewardProItem : MonoBehaviour {

    [SerializeField]
    int[] PropID;   //道具ID
    [SerializeField]
    Sprite[] PropSprite;

    [SerializeField]
    Image PropImg;
    [SerializeField]
    Text PropNum;


    public void Init(int propID,int num)
    {
        PropNum.text = num.ToString();
    }
}
