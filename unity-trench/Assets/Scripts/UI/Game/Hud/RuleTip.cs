using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum RuleTipType
{
    None,
    PlayWithoutRule,
    NoBiggerCard,
    Agent
}

public class RuleTip : MonoBehaviour
{
    [SerializeField]
    Text
        label;
//    [SerializeField]
//    Sprite
//        playWithoutRuleTip;
//    [SerializeField]
//    Sprite
//        noBiggerCardTip;
//    [SerializeField]
//    Sprite
//        agentTip;
    [SerializeField]
    ScaleTweener
        tweener;

    public void ShowRuleTip(RuleTipType tip)
    {
        if (tip == RuleTipType.PlayWithoutRule)
        {
            label.text = TextManager.Get("PlayWithoutRule");
            tweener.Show();
            SOUND.Instance.OneShotSound(Sfx.inst.show);
        } else if (tip == RuleTipType.NoBiggerCard)
        {
            label.text = TextManager.Get("NoBiggerCard");
            tweener.Show();
            SOUND.Instance.OneShotSound(Sfx.inst.show);
        } else if (tip == RuleTipType.Agent)
        {
            label.text = "";
            tweener.Show();
            SOUND.Instance.OneShotSound(Sfx.inst.show);
        } else
        {
            tweener.Hide(delegate()
            {
                gameObject.SetActive(false);
            });
            SOUND.Instance.OneShotSound(Sfx.inst.hide);
        }
    }

//  public void ShowRuleTip (RuleTipType tip,float elipse){
//      StartCoroutine (DoShowTip(tip,elipse));
//  }
//
//  IEnumerator DoShowTip(RuleTipType tip,float elipse){
//      ShowRuleTip (tip);
//      yield return new WaitForSeconds(elipse);
//      ShowRuleTip (RuleTipType.None);
//  }
}
