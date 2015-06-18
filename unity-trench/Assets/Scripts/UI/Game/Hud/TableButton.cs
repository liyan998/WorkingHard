using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TableButton : MonoBehaviour
{

    [SerializeField]
    Button
        btn;
    [SerializeField]
    Image
        btnBkg;
    [SerializeField]
    Text
        btnLabel;
    [SerializeField]
    Outline
        btnLabelOutLine;
//  [SerializeField]
//  Sprite disableBtnBkg;
//  [SerializeField]
//  Sprite disableBtnLabel;
//  [SerializeField]
//  Sprite normalBtnBkg;
//  [SerializeField]
//  Sprite normalBtnLabel;
    [SerializeField]
    Color
        disableColor;
    [SerializeField]
    Color
        disableOutLineColor;
    Color originalColor = Color.white;
    Color originalOutlineColor = Color.black;
    bool isInitialized = false;

    public void Init()
    {
//      normalBtnBkg = btnBkg.sprite;
//      normalBtnLabel = btnLabel.sprite;
        originalColor = btnLabel.color;

        if (btnLabelOutLine != null)
        {
            //Debug.Log(btnLabelOutLine.effectColor);
            originalOutlineColor = btnLabelOutLine.effectColor;
        }
        isInitialized = true;
    }

    public void ChangeSprite(bool isDisable)
    {
        if (!isInitialized)
        {
            Init();
        }
        if (isDisable)
        {
            btn.interactable = false;
//          btnBkg.sprite=disableBtnBkg;
            btnLabel.color = disableColor;
            if (btnLabelOutLine != null)
            {
                btnLabelOutLine.effectColor= disableOutLineColor;
            }
//            btn.enabled = false;
        } else
        {
            btn.interactable = true;
//          btnBkg.sprite=normalBtnBkg;
            btnLabel.color = originalColor;
            if (btnLabelOutLine != null)
            {
                btnLabelOutLine.effectColor = originalOutlineColor;
            }
//            btn.enabled = true;
        }


    }
}
