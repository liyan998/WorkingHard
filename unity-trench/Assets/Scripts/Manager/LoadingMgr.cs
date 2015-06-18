using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadingMgr : MonoBehaviour
{

    public Slider LoadingSlider;
    public Text LoadingText;
    AsyncOperation async;
    string strState = "";

    [SerializeField]
    bool IsUpdateRes = false;

    [SerializeField]
    TaskSlider mTaskSlider;

    SingleDownMgr downMr;

    DataBase database;

	void Start() 
    {
        database = DataBase.Instance;
//        SOUND.Instance.SetMuteMode(true);
        //downMr = SingleDownMgr.Instance;
        if (IsUpdateRes)
        { 
            List<string> wantdownGroup = new List<string>();
            wantdownGroup.Add("testdown");
            ResmgrNative.Instance.BeginInit("http://192.168.1.26/publicdown/", OnInitFinish, wantdownGroup);
            LoadingText.text = "检查资源";
        }
        else
            StartCoroutine(loadScence());
        
	}

    void OnDestroy()
    {
        
    
    }

    bool indown = false;
    void OnInitFinish(System.Exception err)
    {
        if (err == null)
        {
            ResmgrNative.Instance.taskState.Clear();
            LoadingText.text = "检查资源完成";
            List<string> wantdownGroup = new List<string>();
            wantdownGroup.Add("testdown");
            var downlist = ResmgrNative.Instance.GetNeedDownloadRes(wantdownGroup);
            //判断需要下载的资源大小
            if (ResmgrNative.Instance.taskState.tasksize > 0)
            {
                foreach (var d in downlist)
                {
                    d.Download(null);
                }
                ResmgrNative.Instance.WaitForTaskFinish(DownLoadFinish);
                indown = true;
            }
            else 
            {
                LoadingText.text = "加载场景中...";
                StartCoroutine(loadScence());
            }
            
        }
        else
            strState = null;
    }
    void DownLoadFinish()
    {
        indown = false;
        LoadingText.text = "更新完成";
        LoadingSlider.value = 0;
        mTaskSlider.SetPart(100);
        LoadingText.text = "加载场景中...";
        StartCoroutine(loadScence());
    }

    //加载场景
    IEnumerator loadScence()
    {
        async = Application.LoadLevelAsync("Lobby");
        async.allowSceneActivation = false;

        int displayProgress = 0;
        int toProgress = 0;
        while (async.progress < .9f)
        {

            toProgress = (int)(async.progress * 100);

            while (displayProgress < toProgress)
            {
                displayProgress++;
                LoadingSlider.value = displayProgress;
                mTaskSlider.SetPart(displayProgress);
                yield return new WaitForEndOfFrame();
            }
           
        }


        toProgress = 100;
        while (displayProgress < toProgress)
        {
            displayProgress++;
            LoadingSlider.value = displayProgress;
            mTaskSlider.SetPart(displayProgress);
            yield return new WaitForEndOfFrame();
        }


        mTaskSlider.SetPart(toProgress);
        LoadingText.text = "加载完成";
//        SOUND.Instance.SetMuteMode(false);
        yield return new WaitForSeconds(0.2f);
        async.allowSceneActivation = true;
    }



	void Update () {
        if (IsUpdateRes)
        {
            ResmgrNative.Instance.Update();
            if(indown)
            {
                float downingSize = ResmgrNative.Instance.taskState.downingSize / 1024f;
                float tasksize = ResmgrNative.Instance.taskState.tasksize / 1024f;


                LoadingText.text = downingSize.ToString("f2") +
                                   "M / " + downingSize.ToString("f2") + "M";
                LoadingSlider.value = downingSize / tasksize * 10;

                mTaskSlider.SetPart((int)(LoadingSlider.value * 10));
                
            }
        }


        
 
	}
}
