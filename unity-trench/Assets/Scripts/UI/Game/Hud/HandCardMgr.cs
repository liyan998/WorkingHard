using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngineEx.Common;
using UnityEngineEx.LogInterface;

class SelectedCard
{
    public Card card;
    public Vector3 oldV3;

}

public enum CardControlLevel
{
    Locked,
    CanDragButCannotPlay,
    CanPlay
}

public class HandCardMgr : MonoBehaviour
{
    public List<Card> HandCards = new List<Card>();
    public Card card;
    [SerializeField]
    Transform
        dragCardsParent;
    [SerializeField]
    Transform
        playedCardsPos;
    [SerializeField]
    RuleTip
        tip;
    public CardDeck deck;
    float dragHeightParam = 0.4f;
    List<Card> selectedCards = new List<Card>(); //选中的牌
    List<Card> readyCards = new List<Card>();
    List<Card> copiedCards = new List<Card>();
    float maxDistance = 56f; //最大牌距
    float minDistance = 20f; //最小牌距

    private float distance = 0; //实际牌距

    CardControlLevel lockControlLevel = CardControlLevel.Locked;
    Vector3 defaultDragCardsParentPos;
    float cardWidth = 0;
    bool isDragging = false;
    bool isDragStarted = false;
    float sortTime = 0.5f;
    float sortInterval = 0.2f;
    Action OnACardBeSelected;

    bool IsDragging
    {
        get
        {
            return isDragging;
        }
        set
        {
            if (isDragging != value)
            {
                isDragging = value;
                if (isDragging)
                {
                    OnStartDragging();
                } else
                {
                    OnExitDragging();
                }
            }
        }
    }

    bool IsDragStarted
    {
        get
        {
            return isDragStarted;
        }
        set
        {
            if (isDragStarted != value)
            {
                isDragStarted = value;
                if (isDragStarted)
                {
                    OnDragStart();
                } else
                {
                    OnDragEnd();
                }
            }
        }
    }

    void Start()
    {
        defaultDragCardsParentPos = dragCardsParent.localPosition;
        cardWidth = card.GetComponent<RectTransform>().rect.width;
    }

    public void Run(byte[] data)
    {
        StartCoroutine(AddHandCard(data));
    }

    public void InitOnSelected(Action onSelected)
    {
        OnACardBeSelected = onSelected;
    }

    IEnumerator AddHandCard(byte[] data)
    {
        byte[] randomData = new byte[data.Length];
        data.CopyTo(randomData, 0);
        randomData.Shuffle();
        for (int i = 0; i < randomData.Length; i++)
        {
            Card obj = Instantiate(card) as Card;
            obj.SetCard(randomData [i]);
            HandCards.Add(obj);
            Sequelize(data);
            SortHands(this.GetComponent<RectTransform>().rect.width, transform, HandCards);
            yield return new WaitForSeconds(sortInterval);
        }
        yield return 0;
        SequenceHands(data);
        //InitCards();

    }

    //清除手牌
    public void ClearHandCards()
    {
        foreach (Card obj in HandCards)
        {
            Destroy(obj.gameObject);
        }

        HandCards.Clear();
        selectedCards.Clear();
        readyCards.Clear();
        foreach (Card obj in copiedCards)
        {            
            Destroy(obj.gameObject);
        }
        copiedCards.Clear();
        //sequencedCards.Clear();
    }

    //清除打出的牌
    public void ClearOutCards()
    {
        deck.ClearAllCards();
    }

    //清除单个玩家的出牌显示
    public void ClearOutCards(PlayerPanelPosition p)
    {
        deck.CleanCards(p);
    }
    //处理手牌炸弹显示
    void showHandCardsBomb(List<Card> cards)
    {
        if (GameHelper.Instance.GetStage() == GameHelper.STAGE_NORMAL)
            return;
        int cardNum = 1;
        for (int i = 0; i < cards.Count; i++)
        {
            Card obj = cards [i];
            if (obj.IsShowingBomb())
                obj.ToggleBombMark(false);
            if (i != 0)
            {
                if (obj.num == cards [i - 1].num)
                    cardNum++;
                else
                    cardNum = 1;
                if (cardNum == 4)
                {
                    for (int n = 3; n >= 0; n--)
                    {
                        cards [i - n].ToggleBombMark(true);
                    }
                    SOUND.Instance.OneShotSound(Sfx.inst.show);
                }
            }
        }
    }

    public void Sequelize(byte[] data)
    {
        List<Card> sequencedCards = new List<Card>();
        foreach (byte byteData in data)
        {
            foreach (Card theCard in HandCards)
            {
                if (byteData == theCard.GetCardByteData())
                {
                    sequencedCards.Add(theCard);
                    break;
                }
            }
        }
        HandCards = sequencedCards;
    }

    public List<Card> SequelizeCards(List<Card> targetCards)
    {
        List<Card> sequencedCards = new List<Card>();
        foreach (Card card in HandCards)
        {
            foreach (Card theCard in targetCards)
            {
                if (card.GetCardByteData() == theCard.GetCardByteData())
                {
                    sequencedCards.Add(card);
                    break;
                }
            }
        }
        return sequencedCards;
    }

    public void SequenceHands(byte[] data)
    {
        ToggleCards(CardControlLevel.Locked);
        Sequelize(data);
        SortHands(this.GetComponent<RectTransform>().rect.width, transform, HandCards);
        InitCards();
    }

    public void SortHands(float width, Transform parent, List<Card> cards)
    {
        //计算牌距
        if (cards.Count > 0)
        {
            if (cards.Count > 1)
            {
                distance = (width - cardWidth) / (float)(cards.Count + 1);
                //Debug.Log(distance);
            }
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            float handsWidth = (cards.Count - 1) * distance;

            //牌位置
            float firstPoint = -handsWidth * 0.5f;
            for (int i = 0; i <cards.Count; i++)
            {
                Card obj = cards [i];
                
                obj.transform.SetParent(parent);
                //obj.transform.localPosition = new Vector3 ((firstPoint + i * distance), 0, 0);
                if (i != 0)
                {
                    obj.transform.SetSiblingIndex(cards [i - 1].transform.GetSiblingIndex() + 1);
                }
                iTween.MoveTo(obj.gameObject, iTween.Hash("position", new Vector3((firstPoint + i * distance), 0, 0),
                                                            "islocal", true,
                                                            "easetype", iTween.EaseType.easeOutQuad,
                                                            "time", sortTime));
            }
        }
    }

    void InitCards()
    {
        for (int i = 0; i < HandCards.Count; i++)
        {
            Card obj = HandCards [i];
            obj.ActivateTrigger(false);
            obj.InitOnSelectedAction(OnCardSelected);
            //obj.InitUnSelectedAction (OnCardSelected);
            obj.InitOnTouchUpAction(OnTouchUp);
        }
        lockControlLevel = CardControlLevel.CanDragButCannotPlay;

        
        showHandCardsBomb(HandCards);
    }

    public void ToggleCards(CardControlLevel controlLevel)
    {
        lockControlLevel = controlLevel;
        bool isActive = lockControlLevel != CardControlLevel.Locked ? true : false;
        for (int i = 0; i < HandCards.Count; i++)
        {
            Card obj = HandCards [i];
            obj.ActivateTrigger(isActive);
        }
    }

    void OnCardSelected(Card selected)
    {
        if (selectedCards.Contains(selected))
        {

//          selectedCards.Remove (selected);
//          selected.ShowMask (false);
            selectedCards [selectedCards.Count - 1].ShowMask(false);
            selectedCards.Remove(selectedCards [selectedCards.Count - 1]);
        } else
        {
            Debug.Log(HandCards.IndexOf(selected));
            //for avoid missing some card by fast slip
            if (startIndex == -1)
            {
                startIndex = HandCards.IndexOf(selected);
                Debug.Log("startIndex" + startIndex);
            } else
            {
//                selectedCards.Clear();
                endIndex = HandCards.IndexOf(selected);
                if (endIndex < startIndex)
                {
                    for (int i=startIndex; i>endIndex; i--)
                    {
                        if (!selectedCards.Contains(HandCards [i]))
                        {
                            selectedCards.Add(HandCards [i]);
                        }
                        HandCards [i].ShowMask(true);
                    }
                } else if (endIndex > startIndex)
                {
                    for (int i=startIndex; i<endIndex; i++)
                    {
                        if (!selectedCards.Contains(HandCards [i]))
                        {
                            selectedCards.Add(HandCards [i]);
                        }
                        HandCards [i].ShowMask(true);
                    }
                }
            }
            selectedCards=SequelizeCards(selectedCards);
            if(endIndex < startIndex){
                selectedCards.Reverse();
            }
            //end avoid func
            selectedCards.Add(selected);
            selected.ShowMask(true);
            OnACardBeSelected();
        }
        SOUND.Instance.OneShotSound(Sfx.inst.chooseCard);
    }

    void OnTouchUp()
    {
        byte[] removableCardData = new byte[0];
        List<byte> removableCardDataList = new List<byte>();
        removableCardDataList.AddRange(removableCardData);
        foreach (Card cardObj in selectedCards)
        {
            Vector3 oriPos = new Vector3(cardObj.transform.localPosition.x, 0, cardObj.transform.localPosition.z);
            if (!MainGame.inst.mIsDelegate)
            {
                cardObj.ShowMask(false);
            }
            if (readyCards.Contains(cardObj) && !removableCardDataList.Contains(cardObj.GetCardByteData()))
            {
                //card.SetAlpha(0.5f);
                iTween.MoveTo(cardObj.gameObject, iTween.Hash("position", oriPos,
                                                         "islocal", true,
                                                         "easetype", iTween.EaseType.easeOutQuad,
                                                         "time", 0.1f));
                readyCards.Remove(cardObj);

            } else
            {
                iTween.MoveTo(cardObj.gameObject, iTween.Hash("position", oriPos + Vector3.up * 20,
                                                             "islocal", true,
                                                             "easetype", iTween.EaseType.easeOutQuad,
                                                             "time", 0.1f));
                readyCards.Add(cardObj);
                //card.SetAlpha(0.5f);
            }
        }
        MainGame.inst.OnSelectCard(GetCardsByteData(selectedCards), ref removableCardData);
        selectedCards.Clear();
        OnACardBeSelected();
    }

    void Update()
    {
        if (lockControlLevel != CardControlLevel.Locked && (selectedCards.Count > 0 || readyCards.Count > 0))
        {
            if (IsPointerOnAnySelectedCard() || IsPointerOnAnyReadyCard())
            {
                IsDragStarted = true;
            }
#if UNITY_EDITOR
            if (Input.GetMouseButton (0)) {
#else
            if (Input.touchCount > 0)
            {
#endif
                if (IsDragStarted && Input.mousePosition.y > Screen.height * dragHeightParam)
                {
                    IsDragging = true;
                    dragCardsParent.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                } else
                {
                    IsDragging = false;
                }
            } else
            {
                IsDragging = false;
                IsDragStarted = false;
            }
        } else
        {
            IsDragging = false;
            IsDragStarted = false;
        }
    }

    bool IsPointerOnAnyReadyCard()
    {
        foreach (Card readyCard in readyCards)
        {
            if (readyCard.IsPointerOn())
            {
                return true;
            }
        }
        return false;
    }

    bool IsPointerOnAnySelectedCard()
    {
        if (selectedCards.Count > 0)
        {
            foreach (Card selectedCard in selectedCards)
            {
                if (selectedCard.IsPointerOn())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void AddCardsToReadyCards(List<byte> data)
    {
        //readyCards.Clear ();

        foreach (Card cardObj in HandCards)
        {
            Vector3 oriPos = new Vector3(cardObj.transform.localPosition.x, 0, cardObj.transform.localPosition.z);
            if (data.Contains(cardObj.GetCardByteData()))
            {
                if (!readyCards.Contains(cardObj))
                {
                    readyCards.Add(cardObj);
                }
                iTween.MoveTo(cardObj.gameObject, iTween.Hash("position", oriPos + Vector3.up * 20,
                                                                    "islocal", true,
                                                                    "easetype", iTween.EaseType.easeOutQuad,
                                                                    "time", 0.1f));
            } else
            {
                if (readyCards.Contains(cardObj))
                {
                    readyCards.Remove(cardObj);
                }
                iTween.MoveTo(cardObj.gameObject, iTween.Hash("position", oriPos,
                                                                        "islocal", true,
                                                                        "easetype", iTween.EaseType.easeOutQuad,
                                                                        "time", 0.1f));

            }
        }
    }

    public Card[] GetReadyCards()
    {
        return readyCards.ToArray();
    }

    public byte[] GetReadyCardsData()
    {
        return GetCardsByteData(readyCards);
    }

    byte[] GetCardsByteData(List<Card> cards)
    {
        byte[] byteData = new byte[cards.Count];
        int i = 0;
        foreach (Card theCard in cards)
        {
            byteData [i] = theCard.GetCardByteData();
            i++;
        }
        return byteData;
    }

    void OnStartDragging()
    {
        if (copiedCards.Count > 0)
        {
            foreach (Card copyCard in copiedCards)
            {
                if (copyCard.gameObject)
                {
                    Destroy(copyCard.gameObject);
                }
            }
        }
        copiedCards.Clear();
        foreach (Card cardObj in selectedCards)
        {
            Vector3 oriPos = new Vector3(cardObj.transform.localPosition.x, 0, cardObj.transform.localPosition.z);
            if (readyCards.Contains(cardObj))
            {
//              readyCards.Remove (cardObj);
            } else
            {
                readyCards.Add(cardObj);
                iTween.MoveTo(cardObj.gameObject, iTween.Hash("position", oriPos + Vector3.up * 20,
                                                                    "islocal", true,
                                                                    "easetype", iTween.EaseType.easeOutQuad,
                                                                    "time", 0.1f));
            }
        }
        foreach (Card cardObj in HandCards)
        {
            cardObj.ActivateTrigger(false);
            cardObj.ShowMask(false);
            if (readyCards.Contains(cardObj))
            {//for sequence//|| selectedCards.Contains (cardObj)
                cardObj.SetAlpha(0.5f);
                Card copyCard = Instantiate(card, dragCardsParent.position, cardObj.transform.rotation) as Card;
                copyCard.SetCard(cardObj.GetCardByteData());
                copiedCards.Add(copyCard);
                copyCard.transform.localScale *= 0.5f;
                copyCard.transform.SetParent(dragCardsParent);
            }
        }
        SortHands(copiedCards.Count * cardWidth * 0.5f, dragCardsParent, copiedCards);
        selectedCards.Clear();
        OnACardBeSelected();
    }
  
    void OnExitDragging()
    {
        if (Input.mousePosition.y <= Screen.height * dragHeightParam || lockControlLevel != CardControlLevel.CanPlay)
        {
            OnPlaySelfCardFail();
        } else
        {
            OnACardBeSelected();
            MainGame.inst.OnPlayCard(GetCardsByteData(readyCards));
        }
    }

    //float selectStartX;
    int startIndex = -1;
    int endIndex = -1;

    void OnDragStart()
    {
        //selectStartX = Input.mousePosition.x;
        startIndex = -1;
    }

    void OnDragEnd()
    {
        startIndex = -1;
        OnTouchUp();
        if (!tip.gameObject.activeInHierarchy)
        {
            foreach (Card cardObj in HandCards)
            {
                cardObj.ActivateTrigger(true);
            }
        }
    }

    public void PlayCard(List<Card> cards, PlayerPanelPosition player, VFX vfx=VFX.None)
    {
        deck.PlayCard(cards, player, vfx);
    }

    public void OnPlaySelfCardSuccess(byte[] data, VFX vfx=VFX.None)//bool isDrag
    {

        if (copiedCards.Count == 0)
        {
            if (vfx != VFX.JQK)
            {
                foreach (Card cardObj in HandCards)
                {
                    if (readyCards.Contains(cardObj))
                    {//for sequence//|| selectedCards.Contains (cardObj)
                        cardObj.ActivateTrigger(false);
                        cardObj.ShowMask(false);
                        cardObj.SetAlpha(1f);
                        Card copyCard = Instantiate(card, cardObj.transform.position, cardObj.transform.rotation) as Card;
                        copyCard.SetCard(cardObj.GetCardByteData());
                        copiedCards.Add(copyCard);
                        copyCard.transform.localScale *= 0.5f;
                        copyCard.transform.SetParent(dragCardsParent);
                    }
                }
            }
        } else
        {
            List<Card> refCopiedCards = new List<Card>();
            refCopiedCards.AddRange(copiedCards.ToArray());
            foreach (Card copiedCardObj in refCopiedCards)
            {
                bool isPlayedCard = false;
                foreach (byte byteData in data)
                {
                    if (copiedCardObj.GetCardByteData() == byteData)
                    {
                        isPlayedCard = true;
                        break;
                    }
                }
                if (!isPlayedCard)
                {
                    Destroy(copiedCardObj.gameObject);
                    copiedCards.Remove(copiedCardObj);
                }
            }
        }

        if (vfx != VFX.JQK)
        {
            deck.PlayCard(copiedCards, PlayerPanelPosition.BOTTOM, vfx);
        } else
        {
            foreach (Card copiedCardObj in copiedCards)
            {
                Destroy(copiedCardObj.gameObject);
            }
        }
        copiedCards.Clear();
     
        foreach (Card cardObj in readyCards)
        {
            HandCards.Remove(cardObj);
            Destroy(cardObj.gameObject);
        }
        readyCards.Clear();

        SortHands(this.GetComponent<RectTransform>().rect.width, transform, HandCards);
        if (!MainGame.inst.mIsDelegate)
        {
            foreach (Card restCard in HandCards)
            {
                restCard.ShowMask(false);
            }
            ToggleCards(CardControlLevel.CanDragButCannotPlay);
        } else
        {
            foreach (Card restCard in HandCards)
            {
                restCard.ShowMask(true);
            }
            ToggleCards(CardControlLevel.Locked);
        }
       
        showHandCardsBomb(HandCards);
        //clear copiedCards
    }

    public void OnPlaySelfCardFail()
    {
        dragCardsParent.localPosition = defaultDragCardsParentPos;
        foreach (Card cardObj in HandCards)
        {
            cardObj.SetAlpha(1f);
        }
        foreach (Card copyCard in copiedCards)
        {
            Destroy(copyCard.gameObject);
        }
        copiedCards.Clear();

        PutReadyCardsBack();
        selectedCards.Clear();
        OnACardBeSelected();
    }
    
    public void PutReadyCardsBack()
    {
        foreach (Card cardObj in HandCards)
        {
            if (readyCards.Contains(cardObj))
            {
                Vector3 oriPos = new Vector3(cardObj.transform.localPosition.x, 0, cardObj.transform.localPosition.z);
                if (!MainGame.inst.mIsDelegate)
                {
                    cardObj.ShowMask(false);
                }
                readyCards.Remove(cardObj);
                iTween.MoveTo(cardObj.gameObject, iTween.Hash("position", oriPos,
                                                                  "islocal", true,
                                                                  "easetype", iTween.EaseType.easeOutQuad,
                                                                  "time", 0.1f));
            }
        }
    }

    public void RewriteCardData(Card card, byte data)
    {
        card.SetCard(data);
        if (readyCards.Contains(card))
        {
            readyCards.Remove(card);
        }
        if (selectedCards.Contains(card))
        {
            selectedCards.Remove(card);
        }
    }

    public void RewriteCardsData(Card[] cards, byte[] data)
    {
        for (int i=0; i<cards.Length; i++)
        {
            RewriteCardData(cards [i], data [i]);
        }
    }
    
    public void ShowTip(RuleTipType tipType)//, float elipse=2f)
    {
        if (tipType == RuleTipType.None)
        {
            HideTip();
        } else
        {
            foreach (Card cardObj in HandCards)
            {
                cardObj.ActivateTrigger(false);
                cardObj.ShowMask(true);
            }
            tip.gameObject.SetActive(true);
            tip.ShowRuleTip(tipType);
            if (tipType == RuleTipType.PlayWithoutRule)
            {
                Invoke("HideTip", 1.5f);
            }
        }
//        StartCoroutine(DoShowTip(tipType, elipse));
    }

    void HideTip()
    {
        tip.ShowRuleTip(RuleTipType.None);
        if (!MainGame.inst.mIsDelegate)
        {
            foreach (Card cardObj in HandCards)
            {
                cardObj.ActivateTrigger(true);
                cardObj.ShowMask(false);
            }
        } else
        {
            foreach (Card cardObj in HandCards)
            {
                cardObj.ActivateTrigger(false);
                cardObj.ShowMask(true);
            }
        }
    }
//    IEnumerator DoShowTip(RuleTipType tipType, float elipse=2f)
//    {
//        foreach (Card cardObj in HandCards)
//        {
//            cardObj.ActivateTrigger(false);
//            cardObj.ShowMask(true);
//        }
//        tip.gameObject.SetActive(true);
//        tip.ShowRuleTip(tipType);
//        yield return new WaitForSeconds(elipse);
//        tip.ShowRuleTip(RuleTipType.None);
//        foreach (Card cardObj in HandCards)
//        {
//            cardObj.ActivateTrigger(true);
//            cardObj.ShowMask(false);
//        }
//    }

    List<Card> GetCardsbByByteData(byte[] data, List<Card> cards)
    {
        List<Card> gotCards = new List<Card>();
        List<byte> dataList = new List<byte>();
        dataList.AddRange(data);
        foreach (Card theCard in cards)
        {
            if (dataList.Contains(theCard.GetCardByteData()))
            {
                gotCards.Add(theCard);
            }
        }
        return gotCards;
    }

    void MarkAsBomb(List<Card> cards, bool isShowBomb)
    {
        if (cards.Count > 0)
        {
            foreach (Card bombCard in cards)
            {
                bombCard.ToggleBombMark(isShowBomb);
            }
        }
    }

    public void MarkAsBomb(byte[] data)
    {
        if (data.Length >= 4 && HandCards.Count >= data.Length)
        {
            List<byte> dataList = new List<byte>();
            dataList.AddRange(data);
            foreach (Card bombCard in HandCards)
            {
                if (dataList.Contains(bombCard.GetCardByteData()))
                {
                    bombCard.ToggleBombMark(true);
                } else if (bombCard.IsShowingBomb())
                {
                    bombCard.ToggleBombMark(false);
                }
            }
            SOUND.Instance.OneShotSound(Sfx.inst.show);
        } else
        {
            foreach (Card bombCard in HandCards)
            {
                if (bombCard.IsShowingBomb())
                {
                    bombCard.ToggleBombMark(false);
                }
            }
            //SOUND.Instance.OneShotSound (Sfx.inst.outCard);
        }
    }
}
