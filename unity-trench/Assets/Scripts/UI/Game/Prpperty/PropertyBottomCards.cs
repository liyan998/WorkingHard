using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PropertyBottomCards : MonoBehaviour
{
    [SerializeField]
    Card
        mCard;
    [SerializeField]
    VfxManager
        vfx;
    List<Card> mAllCards;

    const int INTERVAL = 50;//间距

    public void SetBottomCard(byte[] cards)
    {
        if (mAllCards == null)
        {
            mAllCards = new List<Card>();
        }
        this.gameObject.SetActive(true);
//        mStart.Play();
        vfx.Play(LevelItemType.PeepPit);
        for (int i = 0; i < cards.Length; i++)
        {
            Card card = Instantiate(mCard) as Card;
            card.transform.SetParent(this.transform);

            RectTransform rt = card.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(i * INTERVAL, 0, 0);

            card.SetCard(cards [i]);
            mAllCards.Add(card);
        }
    }

    public void SetBottomDisable()
    {
        this.gameObject.SetActive(false);
        for (int i = 0; i < mAllCards.Count; i++)
        {
            mAllCards [i].transform.SetParent(null);
            Destroy(mAllCards [i].gameObject);
        }

        mAllCards.Clear();
    }
}
