using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

struct PointData
{
    public int x, y;
    public double rotation;
    
}

public class PrintGenerator : MonoBehaviour
{
    [SerializeField]
    Animator
        anim;
    [SerializeField]
    Transform
        foot;
    [SerializeField]
    Image
        footPrint;
    [SerializeField]
    float
        interval = 10f;
    bool isMoving = true;
    bool isLeft = true;

    [SerializeField]
    RectTransform mParentContext;


    List<PointData> allPoint;

    List<Image> mAllFoot;

    int[] mPart;

    int currentStage;
    void Awake()
    { 
        mAllFoot = new List<Image>();
        allPoint = new List<PointData>();
        //animation ["FootPrint"].normalizedSpeed = 0.5f;

        ReadData();

        mPart = new int[50]
        { 
            7,16,26,35,46,          54,62,68,73,81,
            92,95,100,104,110,      114,118,122,128,133,
            149,156,166,175,183,    186,191,196,199,210,
            223,227,231,240,244,    252, 256,260,265,269,
            286,289,295,299,303,    307, 312, 316,319,mAllFoot.Count -1
        };


        
    
    }

    void Start()
    {
        

//         string tdata = ""; ;
//         for(int i = 0;i < mPart.Length;i++)
//         {
//             int x = (int)mAllFoot[mPart[i]].transform.position.x + 400 + 98;
//             int y = (int)mAllFoot[mPart[i]].transform.position.y + 480 - 71;
//             tdata += x + ":" + y;
//             tdata += "\n";            
//         }
//         
// 
//         FileStream aFile = new FileStream(@"D:\Users\Administrator\Desktop\Point.txt", FileMode.OpenOrCreate);
//         StreamWriter sw = new StreamWriter(aFile);
// 
//         sw.WriteLine(tdata);
// 
//         sw.Close();
//         aFile.Dispose();
        //InitStageFoot(1);
        //StartCoroutine(StartGoToStage(50, null));
        //StartCoroutine(InitStageFoot(1));
        //GoToStage(50, null);
        //StartCoroutine(TestCreatPoint());

        //StartCoroutine(TestInitStageFoot(50));
    }
    /// <summary>
    /// 行走动画
    /// </summary>
    /// <param name="level">目标行走关卡 取值范围1~50</param>
    public void GoToStage(int level,System.Action endFun=null)
    {
        StartCoroutine(StartGoToStage(level,endFun));
    }


    void ReadData()
    {      
        string jdata = Resources.Load(COMMON_CONST.PathStagePoint).ToString();
        JsonData jd = JsonMapper.ToObject(jdata);

        for (int i = 0; i < jd.Count;i++ )
        {

            Vector3 pos = new Vector3(
                  int.Parse(jd[i]["x"].ToString())
                , int.Parse(jd[i]["y"].ToString())
                , 0);

            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, float.Parse(jd[i]["rotation"].ToString())));

            Image print = Instantiate(footPrint, pos, rotation) as Image;
            //print.gameObject.SetActive(true);
            print.color = new Color(1f, 1f, 1f, 0.2f);
            print.transform.SetParent(transform);

            if (!isLeft)
            {
                print.transform.localScale = new Vector3(-1f, 1f, 1f);
                isLeft = true;
            }
            else
            {
                isLeft = false;
            }

            mAllFoot.Add(print);
        }

        //sr.Dispose();
        //aFile.Dispose();
    }



    IEnumerator TestCreatPoint()
    {
        while (isMoving)
        {
            Image print = Instantiate(footPrint, foot.position, foot.rotation) as Image;
            print.transform.SetParent(transform);

            PointData pd = new PointData();
            pd.x = (int)foot.position.x;
            pd.y = (int)foot.position.y;

            pd.rotation = foot.eulerAngles.z;

            allPoint.Add(pd);
            

            if (!isLeft)
            {
                print.transform.localScale = new Vector3(-1f, 1f, 1f);
                isLeft = true;
            }
            else
            {
                isLeft = false;
            }

            yield return new WaitForSeconds(interval);

            Debug.Log(foot.position.x+"--"+foot.position.y);
            if ((int)foot.position.x == 2176 && (int)foot.position.y == 77)
            {
                Debug.Log("break;");
                break;
            }
        }


//         FileStream aFile = new FileStream(@"D:\Users\Administrator\Desktop\Point.txt", FileMode.OpenOrCreate);
//         StreamWriter sw = new StreamWriter(aFile);
// 
//         string jdata = JsonMapper.ToJson(allPoint);
//         sw.WriteLine(jdata);
// 
//         sw.Close();
//         aFile.Dispose();
    }


    void CreatePointByData()
    {
        for(int i = 0;i < allPoint.Count;i++)
        {
            Vector3 pos = new Vector3(
                  allPoint[i].x       
                , allPoint[i].y              
                , 0);          

            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, (float)(allPoint[i].rotation)));

            Image print = Instantiate(footPrint, pos , rotation) as Image;
            print.transform.SetParent(transform);    

            if (!isLeft)
            {
                print.transform.localScale = new Vector3(-1f, 1f, 1f);
                isLeft = true;
            }
            else
            {
                isLeft = false;
            }
        }
      
    }

    /// <summary>
    /// 初始化已行走过的脚印
    /// </summary>
    /// <param name="level">行走过的路径</param>
    /***
    public IEnumerator InitStageFoot(int level)
    {
        int index = getIndexbyLevel(level);
        currentStage = level;
        if(index == -1)
        {
            Debug.LogError("错误的关卡索引");
            yield return 0;
        }

        for (int i = 0; i <= index; i++)
        {
            Debug.Log("Index:" + i);
            mAllFoot[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }

        
    }
     * **/

    public void InitStageFoot(int level)
    {
        int index = getIndexbyLevel(level);
        if (index == -1)
        {
            Debug.LogError("错误的关卡索引");
            return;
        }
        currentStage = level;
        for (int i = 0; i <= index; i++)
        {
            //Debug.Log("Index:" + i);
            //mAllFoot[i].gameObject.SetActive(true);
            mAllFoot[i].color = new Color(1f, 1f, 1f, 1f);
        }


    }

    public IEnumerator TestInitStageFoot(int level)
    {
        int index = getIndexbyLevel(level);
        if (index == -1)
        {
            Debug.LogError("错误的关卡索引");
            yield
            return 0;
        }
        currentStage = level;
        for (int i = 0; i <= index; i++)
        {
            Debug.Log("Index:" + i);

            //yield return new WaitForSeconds(0.5f);
            mAllFoot[i].gameObject.SetActive(true);

        }


    }
    private int getIndexbyLevel(int level)
    {
        for (int i = 0; i < mPart.Length;i++ )
        {
            if (level == i + 1)
            {
                return mPart[i];
            }
        }
        return -1;
    }

    IEnumerator StartGoToStage(int targetLevel, System.Action endFun)
    {
        if(targetLevel <= currentStage)
        {
            yield return 0;
        }

        int startIndex = getIndexbyLevel(currentStage);
        int endIndex = getIndexbyLevel(targetLevel);

        for (int i = startIndex; i < endIndex;i++ )
        {
            //mAllFoot[i].gameObject.SetActive(true); 
            mAllFoot[i].color = new Color(1f, 1f, 1f, 1f);
            //yield return new WaitForSeconds(this.interval);
            yield return new WaitForSeconds(interval);
        }

        currentStage = targetLevel;
        if (endFun != null)
            endFun();
    }
}
