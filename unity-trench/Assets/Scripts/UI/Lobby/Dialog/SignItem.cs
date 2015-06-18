using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SignItem : MonoBehaviour
{

    [SerializeField]
    Image
        icon;
    [SerializeField]
    Text
        day;
    [SerializeField]
    Text
        reward;
    [SerializeField]
    ScaleTweener
        mark;
    [SerializeField]
    ParticleSystem shining;

    public void Init(Sprite sprite, string date, string rewardValue, bool hasGot)
    {
        icon.sprite = sprite;
        icon.SetNativeSize();
        float rate=(float)(icon.preferredHeight/GetComponent<RectTransform>().rect.height);
        //Debug.Log(rate);
        icon.transform.localScale = Vector3.one /rate * 0.5f;
        day.text = date;
        reward.text = rewardValue;
        mark.gameObject.SetActive(hasGot);
        shining.gameObject.SetActive(!hasGot);
    }

    public void ShowMark()
    {
        mark.Show();
        shining.Stop();
    }
}
