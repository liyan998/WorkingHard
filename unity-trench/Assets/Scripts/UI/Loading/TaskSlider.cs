using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TaskSlider : MonoBehaviour {

    [SerializeField]
    Card CCard;

    byte[] cardlist = {0x23, 0x22, 0x21, 0x2D, 0x2C, 0x2B, 0x2A, 0x29, 0x28, 0x27, 0x26, 0x25, 0x24};
    int part;

    List<Card> allCard;



    void Awake()
    {
        
  

    }


    void initCardList()
    {
        allCard = new List<Card>();

        for (int i = 0; i < cardlist.Length; i++)
        {
            Card card = Instantiate(CCard) as Card;
            card.transform.SetParent(this.transform);
            card.SetCard(cardlist[i]);
            card.SetCard(CardType.Back, 0);

            RectTransform rt = card.GetComponent<RectTransform>();
            //rt.rotation = Quaternion.Euler(new Vector3(0f, 0f, 340f));
            rt.localScale = new Vector3(0.26f, .26f, .26f);
            rt.localPosition = new Vector3(-212f + i * 36, -84.1f, 0f);

            allCard.Add(card);
        }
    }
  
	void Start ()
    {
        initCardList();

        this.gameObject.SetActive(false);
	}
	
	public void SetPart(int part)
    {
        if(!gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
        }

        this.part = part;
       // Debug.Log("TaskSlider:"+ part);
        flushPart();
    }

    int currentindex =-1; 
    public void flushPart()
    {
        
        float partvalue = 1f / allCard.Count * 100;
        //Debug.Log(1f / allCard.Count * 100);
        int completepart = (int)(part / partvalue);
        //Debug.Log(completepart);

        if(currentindex == completepart || completepart >= cardlist.Length)
        {
            return;
        }
        currentindex = completepart;

        for (int i = 0; i <= currentindex; i++)
        {           
            Card card = allCard[i];
            if (card.type != CardType.Back || card.isFliping)
            {
                continue;
            }
            card.FlipForward();
        }     
    }

   

    public IEnumerator TestSlider()
    {
        int i = 0;

        while(i++ < 100)
        {
            SetPart(i);

            yield return new WaitForSeconds(0.02f);

        }

        yield return 0;
    }
}
