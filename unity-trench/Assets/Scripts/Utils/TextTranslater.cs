using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextTranslater : MonoBehaviour
{
    [SerializeField]
    Text
        label;

    void Start()
    {
        label.text = TextManager.Get(label.text);
    }
}
