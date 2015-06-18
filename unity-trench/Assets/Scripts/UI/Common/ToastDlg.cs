using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToastDlg : Dialog
{
    public static ToastDlg inst;
    [SerializeField]
    Text
        label;
    [SerializeField]
    ColorTweener
        hideColorTweener;
    [SerializeField]
    ColorTweener
        textColorTweener;
    [SerializeField]
    MoveTweener
        hidePosTweener;

    void Awake()
    {
        inst = this;
        gameObject.SetActive(false);
    }

    public void InvokeToast(string content)
    {
        Toast(content);
    }

    public void Toast(string content=null, float showTime=2f)
    {
        hidePosTweener.FromTarget(null, 0);
        hideColorTweener.FromTarget(null, 0);
        textColorTweener.FromTarget(null, 0);
        if (!string.IsNullOrEmpty(content))
        {
            label.text = content;
        } else
        {
            label.text = TextManager.Get("ComingSoon");
        }
        Show();
        Invoke("Hide", showTime);
    }

    public override void Hide()
    {
        hideColorTweener.ToTarget();
        textColorTweener.ToTarget();
        hidePosTweener.ToTarget(delegate
        {
            gameObject.SetActive(false);
        });
    }
}
