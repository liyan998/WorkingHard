using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultTip : MonoBehaviour
{
    [SerializeField]
    Image[]
        labels;
    [SerializeField]
    GameObject[]
        winBtns;
    [SerializeField]
    GameObject[]
        loseBtns;
    [SerializeField]
    ScaleTweener
        light;
    [SerializeField]
    Sprite[]
        winTips;
    [SerializeField]
    Sprite[]
        loseTips;
    [SerializeField]
    ScaleTweener
        tweener;
//  [SerializeField]
//  ParticleSystem winFirework;
//  [SerializeField]
//  ParticleSystem loseVfx;
    public void SetResultTip(bool isWin)
    {
        int i = 0;
        foreach (Image label in labels)
        {

            if (isWin)
            {
                if (i == 0)
                {
                    label.sprite = winTips [i];
                } else
                {
                    label.sprite = winTips [1];
                }
            } else
            {
                if (i == 0)
                {
                    label.sprite = loseTips [i];
                } else
                {
                    label.sprite = loseTips [1];
                }
            }
            i++;
        }
//        if (isWin)
//        {
////            light.Show();
////            btnLabels [0].text = TextManager.Get("Replay");
////            btnLabels [1].text = TextManager.Get("Flaunt");
////            btnLabels [2].text = TextManager.Get("StageContinueBut");
//
//        } else
//        {
////            light.Hide(null);
////            btnLabels [0].text = TextManager.Get("Back");
////            btnLabels [1].text = TextManager.Get("SeekHelp");
////            btnLabels [2].text = TextManager.Get("Replay");
//        }
        if (winBtns.Length > 0)
        {
            foreach (GameObject btn in winBtns)
            {
                btn.SetActive(isWin);
            }
        }
        if (loseBtns.Length > 0)
        {
            foreach (GameObject btn in loseBtns)
            {
                btn.SetActive(!isWin);
            }
        }
    }

    public void ShowResult(bool isWin)
    {
        SetResultTip(isWin);
        tweener.Show();
        if (isWin)
        {
            //winFirework.Play();
            //anim.Play("WinLabel");
            light.gameObject.SetActive(true);
            light.Show();
            
            SOUND.Instance.OneShotSound(Sfx.inst.win);
        } else
        {
            light.gameObject.SetActive(false);
            //loseVfx.Play();
            //anim.Play("Idle");
            //anim.Play("LoseLabel");
            SOUND.Instance.OneShotSound(Sfx.inst.hitTable);
        }
    }

    public void Hide(System.Action action)
    {
//      winFirework.Stop ();
//      loseVfx.Stop ();
        tweener.Hide(action);
    }
}
