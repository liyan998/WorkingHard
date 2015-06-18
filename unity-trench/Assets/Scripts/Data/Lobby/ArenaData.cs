using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ArenaRoom
{
    Beginner=1001,
    Junior,
    Advanced,
    Senior,
    Expert,
    Master
}

public class ArenaData : MonoBehaviour
{

    [SerializeField]
    Sprite[]
        icons;
    [SerializeField]
    Sprite[]
        rooms;
    [SerializeField]
    Color[]
        colors;

    public Sprite GetIcon(CardType type)
    {
        if (type != CardType.None)
        {
            return icons [(int)type - 1];
        } else
        {
            return null;
        }
    }

    public Sprite GetRoom(ArenaRoom room)
    {
        return rooms [(int)room-1001];
    }

    public Color GetColor(ArenaRoom room)
    {
        return colors [(int)room-1001];
    }
}
