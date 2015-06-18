using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelCompleteDlg : Dialog
{

    bool isVictory = false;
    [SerializeField]
    GameObject
        reward;
    [SerializeField]
    Text StageScore;
    [SerializeField]
    ScaleTweener[]
        stars;
    [SerializeField]
    ScaleTweener
        victoryLabel;
    [SerializeField]
    ScaleTweener
        loseLabel;
    [SerializeField]
    RewardScoreItem[]
        rewardLabel;
    [SerializeField]
    Text
        missionLabel;
    [SerializeField]
    ResultTip
        tip;
    [SerializeField]
    VfxManager vfx;
    Action onBackBtn;
    Action onNextBtn;
    Action onReplayBtn;

    public void Init(bool isSuccess, int starNum,int stageScore, List<INFO_REWARD> rewardNum, string mission, Action onBack, Action onNext, Action onReplay)
    {
        missionLabel.text = mission;
        stars [0].gameObject.SetActive(false);
        stars [1].gameObject.SetActive(false);
        stars [2].gameObject.SetActive(false);
        victoryLabel.gameObject.SetActive(false);
        loseLabel.gameObject.SetActive(false);
        if (isSuccess)
        {
            //ResultText.text = TextManager.Get("StageResultSuccess");
            StartCoroutine(ShowStar(starNum));
            vfx.Play(VFX.FireWork);
        } else
        {
            StartCoroutine(ShowLoseLabel());
        }
        isVictory = isSuccess;
        StageScore.text = stageScore.ToString();
        if (rewardNum == null)
        {
            Debug.Log(rewardNum);
            reward.SetActive(false);
        } else
        {
            reward.SetActive(true);
            foreach (var v in rewardLabel)
                v.gameObject.SetActive(false);
            for (int i = 0; i < rewardNum.Count; i++)
            {
                rewardLabel [i].gameObject.SetActive(true);
                rewardLabel [i].Init(rewardNum [i].type, rewardNum [i].value);
            }
        }
        tip.ShowResult(isSuccess);
        onBackBtn = onBack;
        onNextBtn = onNext;
        onReplayBtn = onReplay;
    }

    public void OnReplayBtn()
    {
        onReplayBtn();
    }

    IEnumerator ShowLoseLabel()
    {
        yield return new WaitForSeconds(0.5f);
        loseLabel.gameObject.SetActive(true);
        loseLabel.Show();
    }

    IEnumerator ShowStar(int starNum)
    {
        yield return new WaitForSeconds(0.5f);
        if (starNum >= 1)
        {
            stars [0].gameObject.SetActive(true);
            stars [0].Show();
        }
        yield return new WaitForSeconds(0.3f);
        if (starNum >= 2)
        {
            stars [1].gameObject.SetActive(true);
            stars [1].Show();
        }
        yield return new WaitForSeconds(0.3f);
        if (starNum == 3)
        {
            stars [2].gameObject.SetActive(true);
            stars [2].Show();
        }
        yield return new WaitForSeconds(0.3f);
        victoryLabel.gameObject.SetActive(true);
        victoryLabel.Show();
    }
    
    public void OnShareBtn()
    {
        //GameDialog.inst.HideDialog(GameDialogIndex.STAGERESULT);
        vfx.StopFireWork();
        GameDialog.inst.ShowDialog(GameDialogIndex.Share);
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void OnNextLevelBtn()
    {
        onNextBtn();
        vfx.StopFireWork();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void OnCloseBtn()
    {
        base.Hide();
        vfx.StopFireWork();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }
}
