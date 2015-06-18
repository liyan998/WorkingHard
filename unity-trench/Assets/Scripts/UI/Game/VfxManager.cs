using UnityEngine;
using System;
using System.Collections;

public enum VFX
{
    Dust,
    Straight,
    Bomb,
    Win,
    Lose,
    JQK,
    InkSplash,
    PairStraight,
    FireWork,
    None,
}

public class VfxManager : MonoBehaviour
{
    
    [SerializeField]
    VfxPlayer
        straightVfx;
    [SerializeField]
    GameObject
        straightLightVfx;
    [SerializeField]
    VfxPlayer
        pairStraightVfx;
    [SerializeField]
    ParticleSystem
        dustVfx;
    [SerializeField]
    ParticleSystem
        bombExploVfx;
    [SerializeField]
    GameObject
        bomb;
    [SerializeField]
    VfxPlayer
        winVfx;
    [SerializeField]
    VfxPlayer
        loseVfx;
    [SerializeField]
    JQKvfx
        jqkVfx;
    [SerializeField]
    VfxPlayer
        jqkTextVfx;
    [SerializeField]
    GameObject
        pairStraightAnim;
    [SerializeField]
    ParticleSystem
        inkSplashVfx;
    [SerializeField]
    Transform
        front;
    [SerializeField]
    VfxPlayer[]
        itemVfxes;
    [SerializeField]
    ParticleSystem[]
        fireworks;
    Vector3 defaultInkPos;
    Vector3 defaultDustPos;

    void Start()
    {
        if (inkSplashVfx != null)
        {
            defaultInkPos = inkSplashVfx.transform.position;
            defaultDustPos = dustVfx.transform.position;
        }
    }

    public void Test()
    {
        Play(VFX.Straight);
        //Play(VFX.Bomb);
        //Play(VFX.PairStraight);
        //Play(LevelItemType.Bomb);
    }

    public void Play(LevelItemType item)
    {
        if (item != LevelItemType.None)
        {
            VfxPlayer itemVfx = itemVfxes [(int)item - 10001];
            itemVfx.gameObject.SetActive(true);
            itemVfx.Play(0);
        }
    }

    public void Play(VFX vfx, Action onVfxEnd=null, int id=-1, byte[] outCardData=null, Transform pos=null)
    {
        float delay = 0;
        //onVfxEndCallBack = onVfxEnd;
        inkSplashVfx.transform.position = defaultInkPos;
        dustVfx.transform.position = defaultInkPos;
        switch (vfx)
        {
            case VFX.Dust:
                if (pos != null)
                {
                    dustVfx.transform.position = pos.position;
                }
                dustVfx.Play();
                delay = dustVfx.duration;
                break;
            case VFX.Bomb:
                Animator missile = (Instantiate(bomb) as GameObject).GetComponent<Animator>();
                Debug.Log(id);
                if (id >= 0)
                {
                    switch ((PlayerPanelPosition)id)
                    {
                        case PlayerPanelPosition.BOTTOM:
                            missile.CrossFade("Eject", 0f);
                            break;
                        case PlayerPanelPosition.LEFT:
                            missile.CrossFade("EjectLeft", 0f);
                            break;
                        case PlayerPanelPosition.RIGHT:
                            missile.CrossFade("EjectRight", 0f);
                            break;
                    }
                } else
                {
                    missile.CrossFade("Eject", 0f);
                }
                StartCoroutine(DoBombExplo());
                delay = 1.2f + bombExploVfx.duration;
                break;
            case VFX.Straight:
                VfxPlayer newStraightText = null;
                if (pos != null)
                {
                    //straightVfx.transform.position = new Vector3(straightVfx.transform.position.x, pos.position.y, straightVfx.transform.position.z);
                    newStraightText = Instantiate(straightVfx, new Vector3(straightVfx.transform.position.x, pos.position.y, straightVfx.transform.position.z), straightVfx.transform.rotation) as VfxPlayer;
                    Instantiate(straightLightVfx, new Vector3(straightLightVfx.transform.position.x, pos.position.y, straightLightVfx.transform.position.z), straightLightVfx.transform.rotation);
                } else
                {
                    newStraightText = Instantiate(straightVfx) as VfxPlayer;
                    Instantiate(straightLightVfx);
                }
                newStraightText.transform.SetParent(transform);
                newStraightText.gameObject.SetActive(true);
                newStraightText.Play(0, null, false);
                delay = straightVfx.showTime;
                break;
            case VFX.PairStraight:
                GameObject pairStraight = Instantiate(pairStraightAnim) as GameObject;
                pairStraight.transform.SetParent(front);
                pairStraight.transform.localScale = Vector3.one * (Screen.width / 800f) * 0.8f;
                pairStraightVfx.gameObject.SetActive(true);
                pairStraightVfx.Play();
                delay = pairStraightVfx.showTime;
                break;
            case VFX.Win:
                winVfx.gameObject.SetActive(true);
                winVfx.Play(0, null, false);
                delay = winVfx.showTime;
                break;
            case VFX.Lose:
                loseVfx.gameObject.SetActive(true);
                loseVfx.Play(0, null, false);
                delay = loseVfx.showTime;
                break;
            case VFX.JQK:
                jqkVfx.gameObject.SetActive(true);
                jqkVfx.Play(id, outCardData);
                StartCoroutine(DoJqkTextVfx());
                delay = 3f;
                break;
            case VFX.InkSplash:
                if (pos != null)
                {
                    inkSplashVfx.transform.position = pos.position;
                }
                inkSplashVfx.Play();
                delay = inkSplashVfx.duration;
                break;
            case VFX.FireWork:
                foreach (ParticleSystem ptc in fireworks)
                {
                    ptc.Play();
                    delay = ptc.duration;
                }
                break;
        }
        //Invoke("OnVfxEnd", delay);
        StartCoroutine(DoVfxEnd(delay, onVfxEnd));
    }

    public void StopFireWork()
    {
        foreach (ParticleSystem ptc in fireworks)
        {
            ptc.Stop();
        }
    }

    public void HideWinLabel(){
        winVfx.Hide();
    }

    public void HideLoseLabel(){
        loseVfx.Hide();
    }

    IEnumerator DoJqkTextVfx()
    {
        yield return new WaitForSeconds(1f);
        jqkTextVfx.gameObject.SetActive(true);
        jqkTextVfx.Play();
    }

    IEnumerator DoVfxEnd(float delay, Action callBack)
    {
        yield return new WaitForSeconds(delay);
        if (callBack != null)
        {
            callBack();
        }
        //jqkVfx.gameObject.SetActive(false);
    }

    IEnumerator DoBombExplo()
    {
        yield return new WaitForSeconds(1.2f);
        bombExploVfx.Play();
    }
}
