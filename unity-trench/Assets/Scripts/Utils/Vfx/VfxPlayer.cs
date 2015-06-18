using UnityEngine;
using System;
using System.Collections;

public class VfxPlayer : MonoBehaviour
{

    [SerializeField]
    Tweener
        tweener;
    [SerializeField]
    Animator
        anim;
    [SerializeField]
    ParticleSystem[]
        vfxs;
    [SerializeField]
    GameObject
        generateVfx;
    [SerializeField]
    bool
        isAnimShow = false;
    public float showTime = 2f;

    public void Play(float delay=0, Action onEnd=null, bool isAutoHide=true)
    {
        StartCoroutine(DoPlay(delay, onEnd, isAutoHide));
    }

    IEnumerator DoPlay(float delay=0, Action onEnd=null, bool isAutoHide=true)
    {
        yield return new WaitForSeconds(delay);
        if (!isAnimShow)
        {
            tweener.FromTarget();
            SOUND.Instance.OneShotSound(Sfx.inst.show);
        } else
        {
            anim.enabled = true;
            anim.Play(0);
        }
        foreach (ParticleSystem vfx in vfxs)
        {
            vfx.Play();
        }
        if (generateVfx != null)
        {
            Instantiate(generateVfx);
        }
        if (isAutoHide)
        {
            yield return new WaitForSeconds(showTime);
            Hide();
        }
        if (onEnd != null)
        {
            onEnd();
        }
    }

    public void Hide()
    {
        if (isAnimShow)
        {
            anim.enabled = false;
        }
        tweener.ToTarget(null);
        foreach (ParticleSystem vfx in vfxs)
        {
            vfx.Stop();
        }
        SOUND.Instance.OneShotSound(Sfx.inst.hide);
    }
}
