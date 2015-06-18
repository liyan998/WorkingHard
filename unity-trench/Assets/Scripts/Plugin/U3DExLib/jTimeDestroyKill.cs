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


        float dev = 20f;
        int direct=0;
        float currentSec;

        while(mCurrentTime < fTimeKill)
        {
            logo.color = new Color(1, 1, 1, apha);

            if (mCurrentTime < mSecondPart[0])
            {
                direct = 1;
                invalue = mSecondPart[0] / dev;
                currentSec = mSecondPart[0];
               
            }
            else if (mCurrentTime >= mSecondPart[0] && mCurrentTime - mSecondPart[0] < mSecondPart[1])
            {
                direct = 0;
                invalue = mSecondPart[1] / dev;
                currentSec = mSecondPart[1];
                
            }  
            else
            {
        
                direct = -1;
                invalue = mSecondPart[2] / dev;
                currentSec = mSecondPart[2];
            }
          

            apha += (1 / dev * direct);
           
            yield return new WaitForSeconds(invalue);
            mCurrentTime += invalue;
        }
       

        DestroyDa();

        //Time.timeScale = 2.2f;
		//logo.FromTarget ();
        //Invoke("DestroyDa", fTimeKill);
    }


    

    void DestroyDa()
    {
        //COMMON_FUNC.NDestroy(gameObject);
        Time.timeScale = 1.0f;
        Application.LoadLevelAsync(SceneMgr.SC_Loading);
    }
}
