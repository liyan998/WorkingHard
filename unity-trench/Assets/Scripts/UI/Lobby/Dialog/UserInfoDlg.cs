using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserInfoDlg : Dialog
{
    [SerializeField]
    UserData
        data;
    [SerializeField]
    InputField
        input;
    [SerializeField]
    Toggle
        maleToggle;
    [SerializeField]
    Toggle
        femaleToggle;
    [SerializeField]
    Text
        title;
//    [SerializeField]
//    Text
//        diamond;
//    [SerializeField]
//    Text
//        coin;
    [SerializeField]
    Text
        levelComplete;
    [SerializeField]
    Text
        winRate;
    [SerializeField]
    Text
        maxWin;
    [SerializeField]
    Text
        maxScore;
    [SerializeField]
    GameObject[]
        characters;
    [SerializeField]
    PlayerRes
        res;
    [SerializeField]
    Button
        changeBtn;
    [SerializeField]
    Button
        rankingBtn;
    [SerializeField]
    TextFilter
        filter;

    public override void Init()
    {
        if (!string.IsNullOrEmpty(data.uiName))
        {
            input.text = data.uiName;
        }
        maleToggle.isOn = !data.isFemale;
        femaleToggle.isOn = data.isFemale;
//        changeBtn.interactable = false;
//        head.sprite = res.GetPlayerIcon(data.isFemale ? PlayerRes.Sex.Lady : PlayerRes.Sex.Male);
        characters[0].SetActive(!data.isFemale);
        characters[1].SetActive(data.isFemale);
        title.text = data.title;
//        diamond.text = data.diamond.ToString();
//        coin.text = data.coin.ToString();
        levelComplete.text = DataBase.Instance.PLAYER.GetCompleteStageCount() + "/" + DataBase.Instance.STAGE_MGR.GetAllStage().Count;
        
        
        int totalCount = DataBase.Instance.PLAYER.TotalPlayCount;

        if(totalCount == 0)
        {
            float winrate = (float)DataBase.Instance.PLAYER.TotalWinCount / totalCount * 100f;
            winRate.text = "0.0" + "%";        
        }
        else
        {
            float winrate = (float)DataBase.Instance.PLAYER.TotalWinCount / totalCount * 100f;
            winRate.text = winrate.ToString("#0.0") + "%";//DataBase.Instance.PLAYER.TotalWinCount+"/"+DataBase.Instance.PLAYER.TotalPlayCount;
        
        }
        
        
        UserHud.inst.Init();
    }

    public void JudgeChangeBtn()
    {
        if (data.uiName != input.text || data.isFemale != femaleToggle.isOn)
        {
            changeBtn.interactable = true;
        } else
        {
            changeBtn.interactable = false;
        }
    }

    public void OnChangeBtn()
    {
        if (input.text.Length > 0 && input.text.Length <= 16)
        {
            if (filter.IsLegal(input.text))
            {
                data.SetUserInfo(input.text, femaleToggle.isOn);
                Init();
                JudgeChangeBtn();
                //changeBtn.interactable = false;
                SOUND.Instance.OneShotSound(Sfx.inst.btn);
                ToastDlg.inst.gameObject.SetActive(true);
                ToastDlg.inst.Toast(TextManager.Get("UserInfoChangeSuccess"));
            } else
            {
                ToastDlg.inst.gameObject.SetActive(true);
                ToastDlg.inst.Toast(TextManager.Get("UserNameIllegal"));
            }
        } else
        {
            ToastDlg.inst.gameObject.SetActive(true);
            ToastDlg.inst.Toast(TextManager.Get("UserInfoSaveFail"));
        }
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
        input.DeactivateInputField();
    }

    public void OnFemaleToggle()
    {
        maleToggle.isOn = !femaleToggle.isOn;
        data.SetUserInfo(input.text, femaleToggle.isOn);
        Init();
        JudgeChangeBtn();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void OnMaleToggle()
    {
        femaleToggle.isOn = !maleToggle.isOn;
        data.SetUserInfo(input.text, femaleToggle.isOn);
        Init();
        JudgeChangeBtn();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    //切换账号
    public void OnSwitchBut()
    {
        DataBase.Instance.PLAYER.LogoutUser();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    //绑定手机号
    public void OnBindPhone()
    {
        LobbyDialogManager.inst.ShowDialog(LobbyDialog.BindPhone);
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }
}
