using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewsBar : MonoBehaviour
{
    [SerializeField]
    MoveTweener
        tweener;
    [SerializeField]
    Text
        news;

    public void Toggle(bool isOpen, string text=null)
    {
        if (!string.IsNullOrEmpty(text))
        {
            news.text = text;
        }

        if (isOpen)
        {
            tweener.FromTarget();
        } else
        {
            tweener.ToTarget();
        }
    }
}
