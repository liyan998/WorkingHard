using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public enum CharacterItemState
{
    Locked,
    OnSale,
    Available,

}

public class CharacterItem : GoodItem
{

    public TrenchCharacter character;
    [SerializeField]
    PlayerRes
        res;
    [SerializeField]
    Transform
        characterPos;
//    [SerializeField]
//    Button
//        buyBtn;
//    [SerializeField]
//    Text
//        buyLabel;
//    [SerializeField]
//    Button
//        useBtn;
//    [SerializeField]
//    Text
//        useLabel;
    CharacterAnimator characterAnim;

    public override void Init(GoodData goodData, Action<GoodData> onBtn)
    {
        base.Init(goodData, onBtn);
        if (characterAnim == null)
        {
            characterAnim = Instantiate(res.GetCharacter(TrenchCharacter.Zhang)) as CharacterAnimator;
            characterAnim.transform.SetParent(characterPos);
            characterAnim.transform.localScale = Vector3.one;
        }
    }

    public void SetBtn(CharacterItemState state)
    {
        switch (state)
        {
            case CharacterItemState.Locked:
                ToggleGood(false);
                break;
            case CharacterItemState.Available:
                ToggleGood(true);
                break;
            case CharacterItemState.OnSale:
                ToggleGood(true);
                break;
        }
    }
}
