using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Emotion
{
    Normal=0,
    Smile,
    Happy,
    Shock,
    Morose,
    Sad
}

public class CharacterAnimator : MonoBehaviour
{
    public TrenchCharacter character;
    [SerializeField]
    BlinkEye
        blinkEye;
    [Header("Normal,Smile,Happy,Shock,Morose,Sad")]
    //    [SerializeField]
//    ColorTweener[]
//        eyes;
//    [SerializeField]
//    ColorTweener[]
//        mouths;
    [SerializeField]
    Animator
        anim;
    [SerializeField]
    AnimationClip[]
        anims;
//    ColorTweener currentEye;
//    ColorTweener currentMouth;
//    Emotion lastEmotion;

    void Start()
    {
        blinkEye.StartIdleAnim();
    }

    public void PlayEmotion(int emotion)
    {
        PlayEmotion((Emotion)emotion);
    }

    public void PlayEmotion(Emotion emotion)
    {
//        if (lastEmotion != emotion)
//        {
//            if (emotion == Emotion.Normal)
//            {
//                blinkEye.StartIdleAnim();
//            } else
//            {
//                blinkEye.StopIdleAnim();
////                eyes [(int)emotion].gameObject.SetActive(true);
////                eyes [(int)emotion].FromTarget();
////                mouths [(int)emotion].gameObject.SetActive(true);
////                mouths [(int)emotion].FromTarget();
//            }
//            if (currentEye != null)
//            {
//                currentEye.ToTarget(delegate()
//                {
//                    currentEye.gameObject.SetActive(false);
//                });
//            }
//
//            if (currentMouth != null)
//            {
//                currentMouth.ToTarget(delegate()
//                {
//                    currentMouth.gameObject.SetActive(false);
//                });
//            }

//            lastEmotion = emotion;
//        }
        anim.CrossFade(anims [(int)emotion].name, 0.0f);
    }
}
