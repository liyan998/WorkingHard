using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class jTimeDestroyKill : MonoBehaviour
{


    float fTimeKill;

    [SerializeField]
    Image logo;

    [SerializeField]
    float[] mSecondPart;

    void Start()
    {
        logo.color = new Color(1, 1, 1, apha);
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

    float mCurrentTime;

    float invalue;
    float apha;


    IEnumerator StartLogoAni()
    {
        for (int i = 0; i < mSecondPart.Length;i++ )
        {
            fTimeKill += mSecondPart[i];
        }
        iTween.ColorTo(this.gameObject, new Color(1, 1, 1, 0), 0);
        iTween.ColorTo(this.gameObject, new Color(1, 1, 1, 1), mSecondPart[0]);
        yield return new WaitForSeconds(mSecondPart[0]);
        //iTween.ColorTo(this.gameObject, new Color(1, 1, 1, 1), mSecondPart[1]);
        yield return new WaitForSeconds(mSecondPart[1]);
        iTween.ColorTo(this.gameObject, new Color(1, 1, 1, 0), mSecondPart[2]);
        yield return new WaitForSeconds(mSecondPart[2]);

//         float dev = 20f;
//         int direct=0;
//         float currentSec;
//         bool startflag = false ;
// 
//         while (true)
//         {
//             if (apha <= 0 && startflag)
//             {
//                 break;
//             }
// 
//             logo.color = new Color(1, 1, 1, apha);
//             Debug.Log("mCurrentTime:" + mCurrentTime + "  fTimeKill:" + fTimeKill);
//             if (mCurrentTime < mSecondPart[0])
//             {
//                 direct = 1;
//                 invalue = mSecondPart[0] / dev;
//                 currentSec = mSecondPart[0];               
//             }
//             else if (mCurrentTime >= mSecondPart[0] && mCurrentTime - mSecondPart[0] < mSecondPart[1])
//             {
//                 direct = 0;
//                 invalue = mSecondPart[1] / dev;
//                 currentSec = mSecondPart[1];                
//             }  
//             else
//             {
//                 startflag = true;
//         
//                 direct = -1;
//                 invalue = mSecondPart[2] / dev;
//                 currentSec = mSecondPart[2];
//             }          
// 
//             apha += (1 / dev * direct);
//            
//             yield return new WaitForSeconds(invalue);
//             mCurrentTime += invalue;
//         }
       

        DestroyDa();

        //Time.timeScale = 2.2f;
		//logo.FromTarget ();
        //Invoke("DestroyDa", fTimeKill);
    }


    

    void DestroyDa()
    {
        //COMMON_FUNC.NDestroy(gameObject);
        //Time.timeScale = 1.0f;
       Application.LoadLevel(SceneMgr.SC_Loading);
    }
}
