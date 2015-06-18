using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PageManager : MonoBehaviour
{

    [SerializeField]
    GameObject[]
        pages;
    [SerializeField]
    Button[]
        pageBtns;
    [SerializeField]
    GameObject[]
        activeMark;
    [SerializeField]
    GameObject[]
        disableMark;

    public void SwitchPage(int pageIndex)
    {
        for (int i=0; i<pages.Length; i++)
        {
            pages [i].SetActive(false);
            pageBtns [i].interactable = true;
            if (activeMark.Length > 0)
            {
                activeMark [i].SetActive(false);
            }
            if (disableMark.Length > 0)
            {
                disableMark [i].SetActive(true);
            }
        }
        pages [pageIndex].SetActive(true);
        pageBtns [pageIndex].interactable = false;
        if (activeMark.Length > 0)
        {
            activeMark [pageIndex].SetActive(true);
        }
        if (disableMark.Length > 0)
        {
            disableMark [pageIndex].SetActive(false);
        }
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }
}
