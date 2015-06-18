using UnityEngine;
using System.Collections;

public enum TrenchCharacter
{
    Gao,
    Zhang,
    Zhou,
    Cheng
}

public class PlayerRes : MonoBehaviour
{

    public enum Sex
    {
        Male,
        Lady,
        None
    }

    /// <summary>
    /// 坑主 平民
    /// </summary>
    [SerializeField]
    Sprite[]
        mRole;

    /// <summary>
    /// ICON
    /// </summary>
    [SerializeField]
    Sprite[]
        mIcon;
    [SerializeField]
    CharacterAnimator[]
        characters;
//    [SerializeField]
//    CharacterAnimator[]
//        nextCharacters;

    public Sprite GetPlayerIcon(Sex sex)
    {
        return mIcon [(int)sex];
    }

    public CharacterAnimator GetCharacter(TrenchCharacter character)
    {
        return characters[(int)character];
    }

    public CharacterAnimator GetCharacter(Sex sex){
        return characters[(int)sex];
    }
//    public CharacterAnimator GetCharacter(Sex sex, PlayerPanelPosition pos)
//    {
//        if (pos == PlayerPanelPosition.LEFT)
//        {
//            return lastCharacters [(int)sex];
//        }
//        if (pos == PlayerPanelPosition.RIGHT)
//        {
//            return nextCharacters [(int)sex];
//        }
//        return null;
//    }

    public Sprite GetPlayerRole(PlayerPanel.PlayerMaster role)
    {
        return mRole [(int)role];
    }
}
