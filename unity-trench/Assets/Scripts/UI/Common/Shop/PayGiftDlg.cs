using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngineEx.LogInterface;
using UnityEngine.UI;

public class PayGiftDlg : MonoBehaviour {
    [SerializeField]
    Object mItemNode;
    [SerializeField]
    GameObject mSplit;
    [SerializeField]
    GameObject mItemParent;
    [SerializeField]
    ItemData id;
    [SerializeField]
    Text AwardValue;
    [SerializeField]
    Text mTitle;

    List<PayGift> mPayGifts = new List<PayGift>();

    Vector3[] pos = new Vector3[3]{
      new Vector3(0.0f,0.0f,0.0f),
      new Vector3(120.0f,0.0f,0.0f),
      new Vector3(240.0f,0.0f,0.0f)
    };
	// Use this for initialization
	void Start () {
        AddItem(LevelItemType.DoubleScore);
        AddItem(LevelItemType.Recorder);
        SetAwardNum("25462金币");
        SetTitle("冲个毛毛的金币");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddItem(LevelItemType type)
    {
        GameObject o = GameObject.Instantiate(mItemNode) as GameObject;
        PayGift p = o.GetComponent<PayGift>();
        p.transform.SetParent(mItemParent.transform);
        p.transform.localPosition = pos[mPayGifts.Count >= pos.Length ? pos.Length - 1 : mPayGifts.Count];
        mPayGifts.Add(p);

        // = gameObject.GetComponent<ItemData>();
        p.SetInfo(id.GetIcon(type), TextManager.Get("Item"+type.ToString()));

        if(mPayGifts.Count == 1)
        {
            mSplit.SetActive(false);
            mPayGifts[0].transform.localPosition = pos[1];
        }
        else if(mPayGifts.Count == 2)
        {
            mSplit.SetActive(true);
            mPayGifts[0].transform.localPosition = pos[0];
            mPayGifts[1].transform.localPosition = pos[2];
        }
        else
        {
            Debuger.Instance.LogError("超过了最大限制");
        }

    }

    public void SetAwardNum(string v)
    {
        AwardValue.text = v;
    }

    public void SetTitle(string v)
    {
        mTitle.text = v;
    }
}
