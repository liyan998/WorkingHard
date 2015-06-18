using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngineEx.CMD.i3778;
using System.Collections.Generic;

public enum PlayerPanelPosition
{
    LEFT,
    BOTTOM,
    RIGHT
}

public class PlayerPanel : MonoBehaviour
{
    public PlayerPanelPosition panelPosition;
    public PlayerMaster roleTitle;
//    [SerializeField]
//    Image
//        playerIcon;
    [SerializeField]
    Image
        role;
    [SerializeField]
    PlayerState
        state;
    [SerializeField]
    Text
        nickname;
    //-----------------------
    [SerializeField]
    MoneyScore money;

    [SerializeField]
    RestCard
        restCard;
    [SerializeField]
    PlayerRes
        playerData;
    [SerializeField]
    ParticleSystem 
        identifyVfx;
    [SerializeField]
    GameObject 
        warningVfx;
    [SerializeField]
    Transform
        characterPos;
    CharacterAnimator
        character;

    /// <summary>
    /// 动画枚举
    /// </summary>
    public enum PlayerAnimation
    {
        Identify,
        Win,
        Lose
    }

    /// <summary>
    /// 玩家角色状态枚举
    /// </summary>
    public enum PlayerMaster
    {
        MASTER,     //坑主
        NORMAL,     //非坑主
        NONE        //未设置       
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="playerAnim">动画索引枚举</param>
    public void StartAnimation(PlayerAnimation playerAnim)
    {
        identifyVfx.transform.position = role.transform.position;
        identifyVfx.Play();
    }

    public void SetPlayerIcon(int sex)
    {
        Dictionary<int, PlayerRes.Sex> tSexMap = new Dictionary<int, PlayerRes.Sex>();

        tSexMap.Add(GlobalDef.GENDER_BOY, PlayerRes.Sex.Male);
        tSexMap.Add(GlobalDef.GENDER_GIRL, PlayerRes.Sex.Lady);
        tSexMap.Add(GlobalDef.GENDER_NULL, PlayerRes.Sex.None);

        //playerIcon.sprite = playerData.GetPlayerIcon(tSexMap [sex]);
        if (character != null && character.character != playerData.GetCharacter(tSexMap [sex]).character)//, panelPosition).character)
        {
            Destroy(character.gameObject);
        }
        if (characterPos != null)
        {
            character = Instantiate(playerData.GetCharacter(tSexMap [sex]), characterPos.position, characterPos.rotation) as CharacterAnimator;
            character.transform.SetParent(characterPos);
            character.transform.localScale = Vector3.one;
            identifyVfx.transform.position = characterPos.position;
            identifyVfx.Play();
        }

    }

    public void PlayEmotionAnim(Emotion emotion)
    {
        if (character != null)
        {
            character.PlayEmotion(emotion);
        }
    }

    /// <summary>
    /// 设置玩家名字
    /// </summary>
    /// <param name="playerName">玩家名字</param>
    public void SetPlayerName(string playerName)
    {     
        nickname.text = playerName;
    }

    /// <summary>
    /// 设置是否是坑主
    /// </summary>
    /// <param name="ismaster">角色枚举</param>
    public void SetMaster(PlayerMaster isMaster)
    {
        roleTitle = isMaster;
        switch (isMaster)
        {
            case PlayerMaster.MASTER:
                StartAnimation(PlayerAnimation.Identify);
                ToggleRole(true, isMaster);
                break;
            case PlayerMaster.NORMAL:
                ToggleRole(true, isMaster);
                break;
            case PlayerMaster.NONE:
                ToggleRole(false, isMaster);
                break;
        }
    }

    void ToggleRole(bool isShow, PlayerMaster isMaster)
    {
        if (isShow && isMaster == PlayerMaster.MASTER)
        {
            role.gameObject.SetActive(true);
            role.GetComponent<ScaleTweener>().Show();
            //role.sprite = playerData.GetPlayerRole(isMaster);      
//            if (panelPosition == PlayerPanelPosition.RIGHT)
//            {
//                role.transform.localPosition = new Vector3(30f, 15f, 0f);
//            } else
//            {
//                role.transform.localPosition = new Vector3(-30f, 15f, 0f);
//            }
            SOUND.Instance.OneShotSound(Sfx.inst.show);
        } else
        {
            role.GetComponent<ScaleTweener>().Hide(delegate()
            {
                role.gameObject.SetActive(false);
            });
            SOUND.Instance.OneShotSound(Sfx.inst.hide);
        }
    }

    public void ToggleState(bool isShow)
    {    
        if (isShow)
        {
            state.gameObject.SetActive(true);
            state.Show();
        } else
        {
            state.Hide();
        }
        //state.gameObject.SetActive (isShow);        
    }

    public void ToggleWarningLight(bool isShow)
    {    
        if (warningVfx != null)
        {
            if (isShow)
            {
                if (!warningVfx.activeSelf)
                {
                    identifyVfx.transform.position = warningVfx.transform.position;
                    identifyVfx.Play();
                    warningVfx.SetActive(true);
                }
            } else
            {
                Debug.Log(warningVfx.activeSelf);
                if (warningVfx.activeSelf)
                {
                    identifyVfx.transform.position = warningVfx.transform.position;
                    identifyVfx.Play();
                    warningVfx.SetActive(false);
                }
            }
        } else
        {
            Debug.Log("no warningVfx!");
        }
    }

    public void SetStateType(PlayerStateText stateText)
    {    
        state.SetPlayerState(stateText);
    }

    public void SetRestCard(int cards, bool isRed=true)
    {    
        restCard.SetRestNum(cards); 
    }

    public void PlusRestCardNum()
    {
        restCard.PlusRestNum();
    }

    public Transform GetRestCardStackPos()
    {
        return restCard.GetCardBackPos();
    }

    public int GetRestCardNum()
    {
        return restCard.RestCardNum;
    }

    public void SetMoney(int num, bool isRed=true)
    {
        money.SetRestNum(num);
    }

    public void Toggle(bool isShow)
    {
        if (isShow)
        {
            //playerIcon.GetComponent<ScaleTweener>().Show();
            role.GetComponent<ScaleTweener>().Show();
            nickname.GetComponent<ScaleTweener>().Show();
            restCard.Toggle(true);
        } else
        {
            //playerIcon.GetComponent<ScaleTweener>().Hide(null);
            role.GetComponent<ScaleTweener>().Hide(null);
            nickname.GetComponent<ScaleTweener>().Hide(null);
            restCard.Toggle(false);
        }
    }

}
