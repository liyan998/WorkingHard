using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum LevelItemType
{
    None=0,
    Bomb=10001,
    Recorder,
    PeepNext,
    Forbid,
    Exchange,
    PeepPit,
    DoubleScore
}

public class ItemData : MonoBehaviour
{

    [SerializeField]
    Sprite[]
        icons;

    public Sprite GetIcon(LevelItemType type)
    {
        if (type != LevelItemType.None)
        {
            return icons [(int)type - 10001];
        } else
        {
            return null;
        }
    }
}
