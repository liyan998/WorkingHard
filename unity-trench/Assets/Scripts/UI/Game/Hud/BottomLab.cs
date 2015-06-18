using UnityEngine;
using System.Collections;
using UnityEngine.UI;

enum LabelIndex
{
	SCORE,
	RATE,
}

public class BottomLab : MonoBehaviour
{

	[SerializeField]
	Text[]
		mScore;
	[SerializeField]
	ScaleTweener
		reminderBtn;
	[SerializeField]
	ScaleTweener
		stageMissionBtn;
	[SerializeField]
	Text
		stageMissionBtnLabel;
	[SerializeField]
	Tip
		stageMissionTip;
    [SerializeField]
    Reminder reminder;

    private bool mIsBeginTime = false;

	public void SetScore (uint number)
	{
		SetTextValues ((int)LabelIndex.SCORE, number);
	}

	public void SetRate (uint number)
	{
		SetTextValues ((int)LabelIndex.RATE, number);
	}

	public void ToggleReminderBtn (bool isShow)
	{
		if (isShow) {
			reminderBtn.gameObject.SetActive (isShow);
			reminderBtn.Show ();
		} else {
			reminderBtn.Hide (null);
			Reminder.inst.Toggle (false);
		}
	}

    public void ToggleReminder(bool isShow){
        reminder.Toggle(isShow);
    }

	public void Test ()
	{
		ToggleStageMissionBtn (true, 60);
	}

	public void ToggleStageMissionBtn (bool isShow, int restTime=-1)
	{
		if (isShow) {
			stageMissionBtn.gameObject.SetActive (isShow);
			stageMissionBtn.Show ();
			if (restTime >= 0) {
                mIsBeginTime = true;
				StartCoroutine (UpdateRestTime (restTime));
			}
            SOUND.Instance.OneShotSound(Sfx.inst.show);
		} else {
			stageMissionBtn.Hide (null);
            SOUND.Instance.OneShotSound(Sfx.inst.hide);
		}
	}

	public void ShowMissionStageTip (bool isComplete, string missionTitle)
	{
		stageMissionTip.gameObject.SetActive (true);
		if (isComplete) {
			stageMissionTip.Show (TextManager.Get ("completeStageMissionTip") + missionTitle);
		} else {
			stageMissionTip.Show (TextManager.Get ("failStageMissionTip") + missionTitle);
		}
	}

	void SetTextValues (int index, uint number)
	{
		if (index < 0 || index >= mScore.Length) {
			return;
		}

		mScore [index].text = number.ToString ();
	}

    public void StopTimeCount()
    {
        mIsBeginTime = false;
    }

	

	IEnumerator UpdateRestTime (int restTime)
	{
		//Debug.Log (rest.ToString ());
        System.DateTime rest = new System.DateTime();
        rest = rest.AddSeconds(restTime);
        while (restTime > 0 && mIsBeginTime)
        {
			stageMissionBtnLabel.text = TextManager.Get ("TimeStageMissionTip") + rest.Minute+":"+rest.Second;
			restTime--;
			rest = rest.AddSeconds (-1d);
			yield return new WaitForSeconds (1f);
		}
		stageMissionBtnLabel.text = TextManager.Get ("StageMissionTip");
	}

    void Start()
    {
        stageMissionBtnLabel.text = TextManager.Get("LevelMission");
    }
}
