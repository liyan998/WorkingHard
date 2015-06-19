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
	bool
		IsUpdateRes = false;
	[SerializeField]
	TaskSlider
		mTaskSlider;
	SingleDownMgr downMr;
	[SerializeField]
	NormalLoadingCircle
		mLoading;//loading菊花



	//------------------------------------------

	NetService mNetService;

	void Start ()
	{
		mNetService = GameManager.Instance.NetService;

		CheckNet ();
	}

	void CheckNet ()
	{
		LoadingDlgManager.inst.HideDialog (0);
		StartCoroutine (CheckNetWork ());
	}

	/// <summary>
	/// 检查网络连接
	/// </summary>
	IEnumerator CheckNetWork ()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			Debug.Log ("No Internet Conntion");
			//弹窗提示
			ShowDialogRetry ();
			yield return 0;
		}
		//登录服务器
		//打开loading

		StartLoading (true);

		Debug.Log ("Login Server Testing");
		mNetService.LoginServer ();

		yield return new WaitForSeconds (2);

		CallBackConntion (false);
	}

	void StartLoading (bool sh)
	{
		mLoading.gameObject.SetActive (sh);
		if (sh) {
			mLoading.Roll ();
		}
	}


	/// <summary>
	/// 连接响应回调
	/// </summary>
	void CallBackConntion (bool isSuccess)
	{
		//关闭loading
		mLoading.gameObject.SetActive (false);
		if (isSuccess) {
			Debug.Log ("Login Server successed!");
			GameScene gs = GameManager.Instance.GameScene;
			gs.SetGameScene (GameScene.SCENE.SCENE_NETSCENE_HALL);
		} else {
			Debug.Log ("Login Server faild!");
			ShowDialogRetry ();
		}       
	}





	/// <summary>
	/// 打开UI界面
	/// </summary>
	void ShowDialogRetry ()
	{        
		Debug.Log ("Open Dialog Retry!");

		LoadingDlgManager.inst.ShowConfirmDialog ("Retry描述", GoDownLobby, "去单机场", true, CheckNet, "重试");
	}


	/// <summary>
	/// 进入单击场
	/// </summary>
	public void GoDownLobby ()
	{
        
		LoadingDlgManager.inst.HideDialog (0);
		if (IsUpdateRes) {
			List<string> wantdownGroup = new List<string> ();
			wantdownGroup.Add ("testdown");
			ResmgrNative.Instance.BeginInit ("http://192.168.1.26/publicdown/", OnInitFinish, wantdownGroup);
			LoadingText.text = "检查资源";
		} else {
			DataBase database = DataBase.Instance;
            LoadingText.text = "努力加载中...";
			StartCoroutine (loadScence ());
		}
	}

	bool indown = false;

	void OnInitFinish (System.Exception err)
	{
		if (err == null) {
			ResmgrNative.Instance.taskState.Clear ();
			LoadingText.text = "检查资源完成";
			List<string> wantdownGroup = new List<string> ();
			wantdownGroup.Add ("testdown");
			var downlist = ResmgrNative.Instance.GetNeedDownloadRes (wantdownGroup);
			//判断需要下载的资源大小
			if (ResmgrNative.Instance.taskState.tasksize > 0) {
				foreach (var d in downlist) {
					d.Download (null);
				}
				ResmgrNative.Instance.WaitForTaskFinish (DownLoadFinish);
				indown = true;
			} else {
				LoadingText.text = "加载场景中...";
				StartCoroutine (loadScence ());
			}            
		} else
			strState = null;
	}

	void DownLoadFinish ()
	{
		indown = false;
		LoadingText.text = "更新完成";
		LoadingSlider.value = 0;
		mTaskSlider.SetPart (100);
		LoadingText.text = "加载场景中...";
		StartCoroutine (loadScence ());
	}

	//加载场景
	IEnumerator loadScence ()
	{
		async = Application.LoadLevelAsync ("Lobby");
		async.allowSceneActivation = false;

		int displayProgress = 0;
		int toProgress = 0;
		while (async.progress < .9f) {
			toProgress = (int)(async.progress * 100);

			while (displayProgress < toProgress) {
				displayProgress++;
				LoadingSlider.value = displayProgress;
				mTaskSlider.SetPart (displayProgress);
				yield return new WaitForEndOfFrame ();
			}           
		}
		toProgress = 100;
		while (displayProgress < toProgress) {
			displayProgress++;
			LoadingSlider.value = displayProgress;
			mTaskSlider.SetPart (displayProgress);
			yield return new WaitForEndOfFrame ();
		}

		mTaskSlider.SetPart (toProgress);
		LoadingText.text = "加载完成";
//        SOUND.Instance.SetMuteMode(false);
		yield return new WaitForSeconds (0.2f);
		async.allowSceneActivation = true;
	}

	void Update ()
	{
		if (IsUpdateRes) {
			ResmgrNative.Instance.Update ();
			if (indown) {
				float downingSize = ResmgrNative.Instance.taskState.downingSize / 1024f;
				float tasksize = ResmgrNative.Instance.taskState.tasksize / 1024f;

				LoadingText.text = downingSize.ToString ("f2") +
					"M / " + downingSize.ToString ("f2") + "M";
				LoadingSlider.value = downingSize / tasksize * 10;

				mTaskSlider.SetPart ((int)(LoadingSlider.value * 10));                
			}
		}
	}
}
