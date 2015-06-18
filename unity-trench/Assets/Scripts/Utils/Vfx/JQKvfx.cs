using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JQKvfx : MonoBehaviour
{
    [SerializeField]
    Card[]
        cards;
    [SerializeField]
    Animator
        anim;
    [SerializeField]
    CardDeck
        deck;
    List<Card> copyCards = new List<Card>();

    public void Test(int id){
        Play(id,new byte[]{1,1,1});
    }

    public void Play(int id, byte[] data)
    {
        StartCoroutine(DoJQKVfx(id, data));
    }
    
    IEnumerator DoJQKVfx(int id, byte[] data)
    {
        foreach (Card card in cards)
        {
            card.gameObject.SetActive(true);
        }
        if (data != null)
        {
            int i = 0;
            foreach (Card card in cards)
            {
                card.SetCard(data [i]);
                card.SetAlpha(1f);
                i++;
            }
        }
//        yield return new WaitForSeconds(0.45f);
//        Camera.main.gameObject.SendMessage ("Shake");
//        vfx.Play (VFX.Dust);
//        SOUND.Instance.OneShotSound (Sfx.inst.hitTable);
//        PlayVibration ();
//        yield return new WaitForSeconds(0.08f);
//        Camera.main.gameObject.SendMessage ("Shake");
//        vfx.Play (VFX.Dust);
//        SOUND.Instance.OneShotSound (Sfx.inst.hitTable);
//        PlayVibration ();
//        yield return new WaitForSeconds(0.05f);
//        Camera.main.gameObject.SendMessage ("Shake");
//        vfx.Play (VFX.Dust);
//        SOUND.Instance.OneShotSound (Sfx.inst.hitTable);
//        PlayVibration ();
        //anim.CrossFade("Idle",0);
        switch ((PlayerPanelPosition)id)
        {
            case PlayerPanelPosition.BOTTOM:
                anim.Play("JQK");
                break;
            case PlayerPanelPosition.LEFT:
                anim.Play("JQK_L");
                //anim.CrossFade("JQK",0.1f);
                break;
            case PlayerPanelPosition.RIGHT:
                anim.Play("JQK_R");
                //anim.CrossFade("JQK",0.1f);
                break;
        }
        //Debug.Log( anim.GetCurrentAnimatorStateInfo(0).length);

        yield return new WaitForSeconds(1f);
        copyCards.Clear();
        foreach (Card card in cards)
        {
            copyCards.Add(Instantiate(card)as Card);
            copyCards[copyCards.Count-1].transform.SetParent(deck.transform);
            //card.SetAlpha(0f);
            card.gameObject.SetActive(false);
        }
//        yield return 0;
//        foreach (Card card in cards)
//        {
//            card.gameObject.SetActive(false);
//        }
        yield return new WaitForSeconds(0.2f);
        if (id >= 0 && id <= 2)
        {
            deck.PlayCard(copyCards, (PlayerPanelPosition)id);
        }
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
