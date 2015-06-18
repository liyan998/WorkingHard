using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CardDeck : MonoBehaviour
{
    [SerializeField]
    RectTransform
        LeftDeck;
    [SerializeField]
    RectTransform
        RightDeck;
    [SerializeField]
    RectTransform
        MyDeck;
    float outCardScale = 0.5f;//0.625f;
    float distanceParam = 0.6f;//0.35f;

    public void ClearAllCards()
    {
        CleanCards(PlayerPanelPosition.LEFT);
        CleanCards(PlayerPanelPosition.RIGHT);
        CleanCards(PlayerPanelPosition.BOTTOM);
    }

    public void CleanCards(PlayerPanelPosition p)
    {
        Transform currTransfrom = GetDeck(p);
        foreach (Transform child in currTransfrom)
        {
            if (child.GetComponent<Card>() != null)
            {
                child.GetComponent<Card>().Gone();
            } else
            {
                Destroy(child.gameObject);
            }
        }
    }

    public Transform GetDeck(PlayerPanelPosition player)
    {
        if (player == PlayerPanelPosition.BOTTOM)
        {
            return MyDeck;
        } else if (player == PlayerPanelPosition.LEFT)
        {
            return LeftDeck;
        } else if (player == PlayerPanelPosition.RIGHT)
        {
            return RightDeck;
        } else
        {
            return null;
        }
    }

    public void PlayCard(List<Card> Cards, PlayerPanelPosition p, VFX vfx=VFX.None)
    { 
        
        //计算牌位置起点
        if (Cards.Count > 0)
        {
            //牌实际宽度 牌缩放到0.625;
            float cardWidth = Cards [0].GetComponent<RectTransform>().rect.width * outCardScale;
            float cardHeight = Cards [0].GetComponent<RectTransform>().rect.height * outCardScale;
            //牌间距
            float distance = cardWidth * distanceParam;

            float cardDeckWidth = 0;
            //大于10张牌分两行显示
            if (Cards.Count > 10)
                cardDeckWidth = distance * 9 + cardWidth;
            else
                cardDeckWidth = distance * (Cards.Count - 1) + cardWidth;


            //牌堆起始位置
            float begin = 0;
            RectTransform currTransfrom = MyDeck;
            if (p == PlayerPanelPosition.BOTTOM)
            {
                begin = -cardDeckWidth / 2 + 0.5f * cardWidth;
                currTransfrom = MyDeck;
            } else if (p == PlayerPanelPosition.LEFT)
            {
                begin = -cardDeckWidth / 2 + 0.5f * cardWidth;//0.5f * cardWidth;
                currTransfrom = LeftDeck;

            } else if (p == PlayerPanelPosition.RIGHT)
            {
                begin = -cardDeckWidth / 2 + 0.5f * cardWidth;//-cardDeckWidth + 0.5f * cardWidth;
                currTransfrom = RightDeck;
            }
            currTransfrom.SetSiblingIndex(2);
            
            Vector3 beginV3 = new Vector3(0f, 0f, 0f);

            //删除所有子对象
            foreach (Transform child in currTransfrom.transform)
            {
                if (child.GetComponent<Card>() != null)
                {
                    child.GetComponent<Card>().Gone();
                } else
                {
                    Destroy(child.gameObject);
                }
            }
            //排列牌堆
            for (int i = 0; i < Cards.Count; i++)
            {
                beginV3.x = begin;
                Cards [i].transform.SetParent(currTransfrom.transform);
                if (i < 10)
                {
                    beginV3.x += i * distance;
                } else
                {
                    beginV3.x += (i - 10) * distance;
                    beginV3.y = -0.5f * cardHeight;
                }
                if (vfx == VFX.InkSplash)
                {
                    beginV3.x = i * distance;
                    StartCoroutine(DropToDeck(Cards [i].gameObject, p, beginV3, i * 0.1f));
                    Cards [i].transform.localScale = Vector3.one * 3f;
                } else
                {
                    iTween.MoveTo(Cards [i].gameObject, iTween.Hash("position", beginV3,
                                                         "islocal", true,
                                                         "easetype", iTween.EaseType.easeOutQuad,
                                                         "time", 0.5f));
                    iTween.ScaleTo(Cards [i].gameObject, iTween.Hash("scale", new Vector3(outCardScale, outCardScale),
                                                         "easetype", iTween.EaseType.easeOutQuad,
                                                         "time", 0.5f));
                }
                SOUND.Instance.OneShotSound(Sfx.inst.outCard);
                Cards [i].SetNumScale(true);
                //Cards [i].ChangeNumScale(true);
            }
        }

    }

    IEnumerator DropToDeck(GameObject obj, PlayerPanelPosition p, Vector3 pos, float delay=0f)
    {
        Transform currTransfrom = MyDeck;
        if (p == PlayerPanelPosition.BOTTOM)
            currTransfrom = MyDeck;
        else if (p == PlayerPanelPosition.LEFT)
            currTransfrom = LeftDeck;
        else if (p == PlayerPanelPosition.RIGHT)
            currTransfrom = RightDeck;
        obj.transform.position = currTransfrom.transform.position + pos * 5f + Vector3.forward * transform.position.z;
        obj.transform.localScale = Vector3.one * 3f;
        Vector3 dir = Vector3.down;
        switch (p)
        {
            case PlayerPanelPosition.BOTTOM:
                dir = Vector3.down;
                break;
            case PlayerPanelPosition.LEFT:
                dir = Vector3.left;
                break;
            case PlayerPanelPosition.RIGHT:
                dir = Vector3.right;
                break;
        }
        iTween.MoveFrom(obj, iTween.Hash("position", dir * 1000f + pos + Vector3.forward * transform.position.z,
                                                          //"islocal", true,
                                                          "easetype", iTween.EaseType.easeOutQuad,
                                                            "time", 0.5f + delay));
        iTween.RotateAdd(obj, iTween.Hash("amount", Vector3.forward * 360,
                                                           "islocal", true,
                                                           "easetype", iTween.EaseType.easeOutQuad,
                                                            "time", 0.5f + delay));
        yield return new WaitForSeconds(0.5f);

//        iTween.ScaleTo(obj, iTween.Hash("scale", Vector3.one * 1f,
//                                                                        "islocal", true,
//                                                                        "easetype", iTween.EaseType.easeInExpo,
//                                                                        "time", 0.5f));
//        iTween.MoveTo(obj, iTween.Hash("position", currTransfrom.transform.position + pos + Vector3.forward * transform.position.z,
//                                         //"islocal", true,
//                                       "easetype", iTween.EaseType.easeInExpo,
//                                         "time", 0.5f));
//        yield return new WaitForSeconds(1f);
        iTween.MoveTo(obj, iTween.Hash("position", pos,
                                                        "islocal", true,
                                                        "easetype", iTween.EaseType.easeInExpo,
                                                        "time", 0.5f));
        iTween.ScaleTo(obj, iTween.Hash("scale", new Vector3(outCardScale, outCardScale),
                                                         "easetype", iTween.EaseType.easeInExpo,
                                                         "time", 0.5f));

    }
}
