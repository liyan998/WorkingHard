using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class jTimeDestroyKill : MonoBehaviour
{

    [SerializeField]
    Image logo;

    [SerializeField]
    float[] mSecondPart;

    void Start()
    {
        logo.color = new Color(1, 1, 1, 0);
        int GraphicSetIdx = 0;
        if (System.Int32.Parse(DeviceManager.Instance.memtotal == null ? "2097152" : DeviceManager.Instance.memtotal) < 2 * 1024 * 1024)
        {
            PlayerPrefs.SetString("GraphicQuality", "Low");
            GraphicSetIdx = 0;
        }
        else
        {
            PlayerPrefs.SetString("GraphicQuality", "Middle");
            GraphicSetIdx = 1;
        }

        QualitySettings.SetQualityLevel(GraphicSetIdx);
        StartCoroutine(StartLogoAni());
    }    


    IEnumerator StartLogoAni()
    {             
        iTween.ColorTo(this.gameObject, new Color(1, 1, 1, 1), mSecondPart[0]);
        yield return new WaitForSeconds(mSecondPart[0]);            

        yield return new WaitForSeconds(mSecondPart[1]);

        iTween.ColorTo(this.gameObject, new Color(1, 1, 1, 0), mSecondPart[2]);
        yield return new WaitForSeconds(mSecondPart[2]);

        //----------------------------------------------

        GameScene gs = GameManager.Instance.GameScene;
        gs.SetGameScene(GameScene.SCENE.SCENE_LOADING);
    }  
}
