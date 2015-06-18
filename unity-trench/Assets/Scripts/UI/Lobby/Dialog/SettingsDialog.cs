using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsDialog : Dialog
{
    [SerializeField]
    SettingData
        data;
    [SerializeField]
    Slider
        music;
    [SerializeField]
    Slider
        sound;
    [SerializeField]
    Slider
        vibration;
    [SerializeField]
    Toggle
        muteMode;
    
    public override void Init()
    {
        data.Init();
        music.value = data.musicVolume;
        sound.value = data.soundVolume;
        vibration.value = data.vibrationToggle ? 1 : 0;
        muteMode.isOn = data.muteMode;
    }

    public void OnMusicVolumeChange()
    {
        data.SetMusicVolume(music.value);
    }

    public void OnSoundVolumeChange()
    {
        data.SetSoundVolume(sound.value);
    }

    public void OnVibrationToggle()
    {
        data.SetVibration(vibration.value == 1 ? true : false);
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void OnMuteModeToggle()
    {
        data.SetMuteMode(muteMode.isOn);
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void OnVisitCommunity()
    {
        Application.OpenURL("http://tieba.baidu.com/f?kw=%E5%A4%A9%E5%A4%A9%E6%8C%96%E5%9D%91%E5%8D%95%E6%9C%BA%E7%89%88");
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void OnSwitchAccountBtn()
    {
        ToastDlg.inst.gameObject.SetActive(true);
        ToastDlg.inst.Toast();
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void OnServiceInfoBtn()
    {
        SOUND.Instance.OneShotSound(Sfx.inst.btn);
    }

    public void OnBindBtn()
    {
        ToastDlg.inst.gameObject.SetActive(true);
        ToastDlg.inst.Toast();
    }

    //For use it in two scenes
//    public void OnCloseBtn(){
//       // LobbyDialogManager.inst.HideDialog(LobbyDialog.Setting);
//    }
}
