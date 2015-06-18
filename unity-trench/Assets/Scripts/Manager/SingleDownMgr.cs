using System;
using UnityEngine;
using UnityEngineEx.CMD.i3778;
using UnityEngineEx.LogInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngineEx.Common;


public class SingleDownMgr : SingleClass<SingleDownMgr>
{

    void Start()
    {
        StartCoroutine(InitDownload());
    }
    IEnumerator InitDownload()
    {
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            //wifi
            List<string> wantdownGroup = new List<string>();
            wantdownGroup.Add("testdown");
            ResmgrNative.Instance.BeginInit("http://192.168.1.26/publicdown/", OnInitFinish, wantdownGroup);
            indown = true;
            StartCoroutine(DownUpdate());
        }
        yield return 0;
    }

    bool indown = false;
    void OnInitFinish(System.Exception err)
    {
        if (err == null)
        {
            StartCoroutine(DownList());
        }
        else
        {
            indown = false;
        }
    }
    IEnumerator DownList()
    {
        List<string> wantdownGroup = new List<string>();
        wantdownGroup.Add("testdown");
        var downlist = ResmgrNative.Instance.GetNeedDownloadRes(wantdownGroup);
        foreach (var d in downlist)
        {
            d.Download(null);
        }
        ResmgrNative.Instance.WaitForTaskFinish(DownLoadFinish);
        yield return 0;
    }
    void DownLoadFinish()
    {
        indown = false;
    }

    IEnumerator DownUpdate()
    {
        while (indown)
        {
            Debug.Log("--------------------------------------------");
            ResmgrNative.Instance.Update();
            yield return 0;
        }
        yield return 0;
    }

    //获取Bundle
    public void GetBundle(string strElement, System.Type type, Action<UnityEngine.Object> RetrunBundle)
    {
        
        var files = ResmgrNative.Instance.verLocal.groups["testdown"].listfiles;
        foreach(KeyValuePair<string,LocalVersion.ResInfo> temp in files)
        {

            string s = temp.Key;
        }
        if (files != null && files.ContainsKey(strElement))
        {
            indown = true;
            StartCoroutine(DownUpdate());
            try { 
                files[strElement].BeginLoadAssetBundle((bundle, tag) =>
                {
                    //UnityEngine.Object[] objes = bundle.LoadAll();
					UnityEngine.Object obj = bundle.LoadAsset("xx", type);
                    bundle.Unload(false);
                    RetrunBundle(obj);
                    indown = true;
                });
            }
            catch {

                indown = false;
            }
            
        }
    }
    
}
