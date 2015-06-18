using System;
using UnityEngine;
using UnityEngineEx.CMD.i3778;
using UnityEngineEx.LogInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngineEx.Common;
using UnityEngine.UI;

/// <summary>
/// 主游戏流程
/// </summary>
public class MainGame : MonoBehaviour, IGameSink
{   
    public static MainGame inst;
    [SerializeField]
    TableController
        mGameView;
    [SerializeField]
    GameDialog
        mGameDialog;
    /// <summary>
    /// 底部bar
    /// </summary>
    [SerializeField]
    BottomLab
        mBottomLab;
    [SerializeField]
    LobbyData
        mLobbyData;
    [SerializeField]
    GameProperty
        mProperty;
    [SerializeField]
    GameAutoController
        mGameAuto;
    [SerializeField]
    TaskTip
        mTaskTip;

    [SerializeField]
    WindowData
        Windata;           //返回窗口数据

    //游戏关卡处理逻辑类
    GameStage gameStage;
    //------------------------------------------------------------------------------------

    GameHelper mGameHelper;//= GameHelper.Instance;                 //游戏辅助类
    Player[] mPlayers = new Player[CMD_Trench.GAME_PLAYER];         //同桌用户

    //timer id
    uint DalayGameStart = Timer.NormalTimeIDBase + 1;               //游戏延迟开始定时器ID
    uint DalayNewTurn = Timer.NormalTimeIDBase + 2;                 //游戏延迟开始定时器ID

    uint GameOutCatdSeconds = 25;                                   //游戏出牌定时器时间
    int callScoreTime = 25;                                         //叫分超时
  
    ushort SELT_VIEW_CHAIR = 1;                                     //视图自己座位  

    public List<byte> SearchCards = new List<byte>();
    int mState;
    public const int STATE_INIT = 0;
    public const int STATE_READY = 1;
    public const int STATE_RUN = 2;//游戏中
    public const int STATE_STATISTICS = 3;//结算



    public bool mIsDelegate;                   //是否托管


    public void SetState(int sta)
    {
        this.mState = sta;
    }

    public int GetState()
    {
        return mState;
    }

    //这里使用视图椅子号，单机版可以忽略
    void Awake()
    {
        

        inst = this;
        mGameHelper = GameHelper.Instance;
        gameStage = gameObject.GetComponent<GameStage>();

        AllocChair(1, DataBase.Instance.PLAYER);
        mGameHelper.PerformSitDownAction(1, mPlayers [1], this);
        mPlayers [1].ConfigLoseCount = 0;

        AllocChair(0, mGameHelper.ANDROID_USER_MGR.ActiveAndroid());  //记得要还回去
        mGameHelper.PerformSitDownAction(0, mPlayers [0], (AndroidUser)mPlayers [0]);

        AllocChair(2, mGameHelper.ANDROID_USER_MGR.ActiveAndroid());
        mGameHelper.PerformSitDownAction(2, mPlayers [2], (AndroidUser)mPlayers [2]);

        //----------------------------------------------------
       

        
      
    }

    void Start()
    {      
        switch (mGameHelper.GetStage())
        {
            case GameHelper.STAGE_GATE:
                //DataBase.Instance.PLAYER.Property = PropertyMgr.PROPERTY_SCORE2;
                Debug.Log("DataBase.Instance.PLAYER.Property:" + DataBase.Instance.PLAYER.Property);
                break;
            default:
                Debug.Log("DataBase.Instance.PLAYER.Property:" + DataBase.Instance.PLAYER.Property);
                break;
        }
        flushBottomBar();
        //开速开始游戏
        if (DataBase.Instance.IsJustStartPlay)
        {
            DataBase.Instance.IsJustStartPlay = false;
            OnReady();
        }
    }

    public void ResetData()
    {

    }


    void flushBottomBar()
    {
        mBottomLab.SetRate((uint)mGameHelper.lCellScore);
        mBottomLab.SetScore((uint)DataBase.Instance.PLAYER.Gold);
    }
  
    //安排座位
    bool AllocChair(ushort wChair, Player p)
    {
        if (COMMON_FUNC.IsInvalidChair(wChair))
        {
            Debuger.Instance.LogError(COMMON_CONST.ErrChair);
            return false;
        }
        mPlayers [wChair] = p;
        return true;
    }

    /// <summary>
    /// 退出游戏场景，应该调用
    /// </summary>
    void FreeAllChair()
    {
        for (ushort wChair = 0; wChair < CMD_Trench.GAME_PLAYER; wChair++)
        {
            if (mPlayers [wChair].mIsAndroid)
            {
                mGameHelper.ANDROID_USER_MGR.RealseAndroid((AndroidUser)mPlayers [wChair]);
            }
            mPlayers [wChair] = null;
        }
    }

    /// <summary>
    /// 命令处理
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="o"></param>
    public void OnGameMessage(GlobalDef.CMD_Head cmd, object o) //游戏处理函数
    {
        //Debuger.Instance.Log (cmd.CommandInfo.wMainCmdID + " : " + cmd.CommandInfo.wSubCmdID);
        switch (cmd.CommandInfo.wMainCmdID)
        {
            case CMD_Game.MDM_GR_USER:      //用户命令
                OnUserCommand(cmd, o);
                break;
            case GlobalFrame.MDM_GF_GAME:   //游戏命令
                OnGameCommand(cmd, o);
                break;
        }
    }

    /// <summary>
    /// 游戏命令处理
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="o"></param>
    private void OnGameCommand(GlobalDef.CMD_Head cmd, object o)
    {
        //Debuger.Instance.Log(cmd.CommandInfo.wSubCmdID);
        switch (cmd.CommandInfo.wSubCmdID)
        {
            case enCmdTrench.SUB_S_SEND_CARD:       //发牌
                OnSendCard((CMD_S_SendCard)o);
                break;
            case enCmdTrench.SUB_S_LAND_SCORE:      //叫分
                OnLandScore((CMD_S_LandScore)o);
                break;
            case enCmdTrench.SUB_S_GAME_START:      //游戏开始
                StartCoroutine(OnGameStart((CMD_S_GameStart)o));
                break;
            case enCmdTrench.SUB_S_OUT_CARD:        //出牌
                OnSubGameOutCard((CMD_S_OutCard)o);
                break;
            case enCmdTrench.SUB_S_PASS_CARD:       //过牌
                OnSubGamePassCard((CMD_S_PassCard)o);
                break;
            case enCmdTrench.SUB_S_GAME_END:        //游戏结束
                OnSubGameEnd((CMD_S_GameEnd)o);
                break;
        }
    }

    /// <summary>
    /// 玩家命令处理
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="o"></param>
    private void OnUserCommand(GlobalDef.CMD_Head cmd, object o)
    {
        switch (cmd.CommandInfo.wSubCmdID)
        {
            case CMD_Game.SUB_GR_USER_STATUS:
                OnUStatus((CMD_Game.CMD_GR_UserStatus)o);
                break;
        }
    }

    //------------------------------------------------------------------


    public string GetBreakButtonTip()
    {
        string tip = TextManager.Get("backLobbyTip");

        if (mGameHelper.GetStage() == GameHelper.STAGE_GATE)
        {
            tip = TextManager.Get("quitLevelTip");
            return tip;
        }

        switch (GetState())
        {           
            case STATE_READY:
            case STATE_RUN:
           
                int cutScore = EconomicSystem.GameBreakScore(new int[] { mGameHelper.lCellScore });
                tip = string.Format(TextManager.Get("LeaveGameTip"),cutScore);
                break;
        }
        return tip;
    }

    /// <summary>
    /// 就绪
    /// </summary>
    public void OnReady()
    {
        SetState(STATE_INIT);
        Debug.Log("OnReady!!!!!!!!!!!!!!");
        if (mGameHelper.GetStage() != GameHelper.STAGE_GATE)
        {
            //判断用户金币
            var roomData = DataBase.Instance.CurRoom;
            if (DataBase.Instance.PLAYER.Gold < roomData.dwMinBonus)
            {
                var database = DataBase.Instance;
                //金币不足先看能领低保
                int getLowCoinGiftRest = database.PLAYER.GetLowCoinGiftRest;
                if (roomData.bRelief == true && getLowCoinGiftRest > 0)
                {
                    mGameDialog.ShowConfirmDialog(
                        string.Format(TextManager.Get("lowCoinGiftTip"),
                        (COMMON_CONST.MaxGiftNum - getLowCoinGiftRest + 1).ToString()),
                        delegate {
                            database.PLAYER.OnLowCoinGiftGet();
                            OnReady();
                        },
                        null, false, null, null, TextManager.Get("lowCoinGiftTitle"));
                }
                else
                {
                    enRoomType roomType =
                    GameHelper.Instance.GetStage() == GameHelper.STAGE_BOM ? enRoomType.enBomb : enRoomType.enNormal;

                    INFO_ROOM room = database.ROOM_MGR.GetCanPlayRoom(roomType, database.PLAYER.Gold);
                    if (room.dwMinBonus > database.PLAYER.Gold)
                    {

                        mGameDialog.ShowConfirmDialog(
                            string.Format(TextManager.Get("lowCoinRoomTip"), roomData.dwMinBonus),
                            delegate
                            {
                                MainGame.inst.OnExitGame();
                                DataBase.Instance.SCENE_MGR.SetScene(SceneMgr.SC_Hall);
                            },
                            TextManager.Get("GiveUp"),
                            true,
                            delegate
                            {
                                Windata.LobbyWin = LobbyState.Shop;
                                MainGame.inst.OnExitGame();
                                DataBase.Instance.SCENE_MGR.SetScene(SceneMgr.SC_Hall);
                        
                            },
                            TextManager.Get("Buy"),
                        TextManager.Get("lowCoinGiftTitle")
                        );

                    }
                    else
                    { 
                        //低倍场可去
                        mGameDialog.ShowConfirmDialog(
                            string.Format(TextManager.Get("lowCoinEndTip"), roomData.dwMinBonus),
                            delegate
                            {
                                OnExitGame();
                                database.EnterNomalRoom(room.wServerID);
                            },
                            TextManager.Get("GoToLowBetRoom"),
                            true,
                            delegate
                            {
                                Windata.LobbyWin = LobbyState.Shop;
                                MainGame.inst.OnExitGame();
                                DataBase.Instance.SCENE_MGR.SetScene(SceneMgr.SC_Hall);

                            },
                            TextManager.Get("Buy"),
                        TextManager.Get("lowCoinGiftTitle")
                        );
                    }
                }
                
                return;
            }
            else if (DataBase.Instance.PLAYER.Gold >= roomData.dwMaxBonus)
            {
                mGameDialog.ShowConfirmDialog(TextManager.Get("highCoinRoomTip"),
                    delegate {
                        Windata.LobbyWin = LobbyState.Arena;
                        MainGame.inst.OnExitGame();
                        DataBase.Instance.SCENE_MGR.SetScene(SceneMgr.SC_Hall);
                    }
                , null, false,null,null,TextManager.Get("highCoinTitle"));
                return;
            }
        }
        TaskInit();
        //黑挖
        if (mGameHelper.IsBlendTrench)
        {            
            mGameView.TimeCallBack(5, ClockPosition.BlackCallScore, OnGameStart);
            mGameView.SwitchGameState(TableState.Black);
            mGameView.SetBlackCallBack(BlackCallBack, OnGameStart);
        } else
        {            
            OnGameStart();
        }
        ClearDelegate();
        SetState(STATE_READY);
        flushBottomBar();
    }

    private void ClearBlackCallBar()
    {        
        mGameView.ClockStop();
        mGameView.SwitchGameState(TableState.None);
    }

    private void BlackCallBack()
    {        
        ClearBlackCallBar();
        mGameHelper.BlackCallScore();  
    }

    private void OnGameStart()
    {
        ClearBlackCallBar();
    
        gameStage.BeginStage(mGameView);
        mGameHelper.Ready(1);

        if (mGameHelper.GetStage() == GameHelper.STAGE_GATE 
            && 
            DataBase.Instance.PLAYER.Property == PropertyMgr.PROPERTY_BOM)
        {
            mProperty.StartSendBom();
        }
    }

    //重新开始
    public void ReStart()
    {
        mGameView.ReStart();
        PropertyClearStopCard();
        ClearDelegate();
    }
    
    int mCurrentUser;
    int mBlackCallScore;
    /// <summary>
    /// 发牌
    /// </summary>
    /// <param name="cmd"></param>
    private void OnSendCard(CMD_S_SendCard cmd)
    {
        List<byte> cards = new List<byte>();
        foreach (var e in cmd.bCardData)
        {
            if (e != 0)
            {
                cards.Add(e);
            }
        }
        mGameView.ToggleCards(CardControlLevel.Locked);
        
        mGameView.SetCardStackCallBack(CompleteDispatchCard);
        mGameView.StartDispatchCards(cards.ToArray());


        
        Debuger.Instance.Log("叫分玩家:" + cmd.wCurrentUser);
        mCurrentUser = cmd.wCurrentUser;
        mBlackCallScore = cmd.wBlankTrenchUser;
    }

    /// <summary>
    /// 完成发牌回调
    /// </summary>
    public void CompleteDispatchCard()
    {
        
        mGameView.SetCardStackCallBack(null);

        if (mBlackCallScore == 1 && mCurrentUser == 1)
        {
            mGameHelper.BlackScoreStart();
            return;
        }
       
        
        PropertyChangeCard();//换下家牌        

        PropertyGetNextCard();//获得下家手牌
    }

    private void CallBackCallScore()
    {
        CallScore(-1);
    }

    /// <summary>
    /// 玩家叫分
    /// </summary>
    public void PlayerCallScore()
    {
        //叫分
        mGameView.ToggleCards(CardControlLevel.Locked);
        int id = -1;
        if (mCurrentUser == 1)
        {
            PropertyGetLastCard();
            ShowIdentify(0);
            mGameView.ClockStart(callScoreTime, CallBackCallScore, id);
        } else
        {
            mGameView.ClockStart(callScoreTime, CallBackCallScore, mCurrentUser);
        }
    }

    public void GameActionCallScore()
    {
        for (int i = 0; i < mPlayers.Length; i++)
        {
            Player player = mPlayers [i];
            if (i == 1)
            {
                PlayerCallScore();
            } else
            {
                player.ActionCallScore();
            }
        }
    }


    /// <summary>
    /// 叫分
    /// </summary>
    /// <param name="cmd"></param>
    void OnLandScore(CMD_S_LandScore cmd)
    {
        Dictionary<int, PlayerStateText> restable = new Dictionary<int, PlayerStateText>();

        restable.Add(255, PlayerStateText.ScorePass);
        restable.Add(1, PlayerStateText.Score1);
        restable.Add(2, PlayerStateText.Score2);
        restable.Add(3, PlayerStateText.Score3);
        restable.Add(4, PlayerStateText.Black);

        //--------------------------------------------------
        //cmd.bLandScore = 2;

        mGameView.ClockStop();
        mGameView.SetState(cmd.bLandUser, restable [cmd.bLandScore]);
        bool isFemale = GameHelper.Instance.mPlayers [cmd.bLandUser].Gender == GlobalDef.GENDER_GIRL ? true : false;
        //      Debug.Log (score);

        int score = (int)restable [cmd.bLandScore];
//        score = score > 3 ? 3 : score;
        if(score <= 3){
        SOUND.Instance.OneShotSound(Sfx.inst.GetVoice(VoiceType.CallScore, score, isFemale));
        }

        if (COMMON_FUNC.IsInvalidChair(cmd.wCurrentUser))
        {   //完成叫分

            Timer.Instance.SetTimer(DalayGameStart, 1.0f, delegate
            {
                for (int i = 0; i < 3; i++)
                {
                    mGameView.SetState(i, PlayerStateText.None);
                }
            });

            return;
        }

        //---------------------------------------------------------

		switch (cmd.wCurrentUser)
        {
		case 1:
			PropertyGetLastCard ();
            ShowIdentify(cmd.bCurrentScore);          
			mGameView.ClockStart (callScoreTime, CompleteClock);
			break;
		case 0:
		case 2:                
			mGameView.ClockStart (callScoreTime, CallBackAndroidScore, cmd.wCurrentUser);
			break;
		}
	}

    /// <summary>
    /// 显示叫分 交互
    /// </summary>
    /// <param name="showButtonparm"></param>
    private void ShowIdentify(int showButtonparm)
    {
        int showCallButton = showButtonparm;
        byte[] tempData = GameLogicUtil.ConverListToByte(mGameHelper.PlayerHandCard [1]);
        bool hasMustCall = GameLogicUtil.HasMustCallScore(tempData);
        if (hasMustCall)
        {
            Debug.Log("Must Call Score");
            
            if (showCallButton == 2)
            {
                showCallButton = 4;
            } else
            {
                showCallButton = 3;
            }       
        }
        Debug.Log("Show Buttons " + showCallButton);
        mGameView.ToggleBtns(TableState.Identify, true, showCallButton);
    }

    private void OnUStatus(CMD_Game.CMD_GR_UserStatus userstatus)
    {     
        Dictionary<byte, PlayerStateText> restable = new Dictionary<byte, PlayerStateText>();
        restable.Add(3, PlayerStateText.Ready);

        switch ((GlobalDef.enUserStatus)userstatus.cbUserStatus)
        {
            case GlobalDef.enUserStatus.US_SIT://坐下
                mGameView.SetInforPanel(userstatus.wChairID, mPlayers [userstatus.wChairID]);

                if (userstatus.wChairID == 1)
                {                
                    mGameView.SwitchGameState(TableState.Ready);                    

                    //道具 禁手清除上次状态
                    PropertyClearStopCard();
                }
                
                break;
            case GlobalDef.enUserStatus.US_READY://准备
                if (restable.ContainsKey(userstatus.cbUserStatus) && userstatus.wChairID != (ushort)GlobalDef.Deinfe.INVALID_CHAIR)
                {
                    mGameView.SetState(userstatus.wChairID, restable [userstatus.cbUserStatus]);
                }
                break;
        }
    }

    private IEnumerator OnGameStart(CMD_S_GameStart gscmd)
    {
        Debuger.Instance.Log("GameStart");
        //设置坑主
        mGameView.SetMaster(gscmd.wLandUser);
        if (gscmd.wLandUser == SELT_VIEW_CHAIR)
            //坑主
            gameStage.SetPlayerType(COMMON_CONST.RoleLand);
        else
            gameStage.SetPlayerType(COMMON_CONST.RoleNoLand);
   
        byte[] tempBuffer = new byte[ gscmd.bMeCardData.Length];
        System.Array.Copy(gscmd.bMeCardData, tempBuffer, gscmd.bMeCardData.Length);

        byte[] tempBackBuffer = new byte[gscmd.bBackCard.Length];
        System.Array.Copy(gscmd.bBackCard, tempBackBuffer, gscmd.bBackCard.Length);

   
     

        yield return new WaitForSeconds(2f);


        mGameView.AddPitCard((CurrentPlayer)gscmd.wLandUser, tempBackBuffer, tempBuffer);
        //底牌动画需要3秒
        yield return new WaitForSeconds(3f);
        TableController.ClockCallBack call = null;


        //
        SetState(STATE_RUN);//
        //自己判断
        if (gscmd.wCurrentUser == SELT_VIEW_CHAIR)
        {
            //玩家开始出牌
            gameStage.BeginOutCard(gscmd.wCurrentUser);
            call = CallBackOutCard;
            //显示出牌和不出按钮

            mGameView.ToggleCards(CardControlLevel.CanPlay);
            SetActivePlay(PLAY_SENDBUTTON);
        } else
        {
            mGameView.ToggleCards(CardControlLevel.CanDragButCannotPlay);
        }
        //记牌器
        PropertyRecord();

        NextPlayer(gscmd.wCurrentUser);

        TaskChannal.Instance.InGameStart(gscmd.wCurrentUser);
        
        //Debuger.Instance.Log("自己坑主数据:" + gscmd.bMeCardData.ToString<byte>());
        mGameView.ClockStart((int)GameOutCatdSeconds, call, (int)gscmd.wCurrentUser);       
    }
 
    bool OnSubGameOutCard(CMD_S_OutCard cmd)
    {        
        //状态校验
        if (GameHelper.Instance.GameStatus != CMD_Trench.GS_WK_PLAYING)
            return true;
        mGameView.ClockStop();

        //---------------------------------------------

        byte[] temp = new byte[cmd.bCardCount];
        System.Array.Copy(cmd.bCardData, temp, cmd.bCardCount);

        //清除当前玩家上次出牌    
        if (!COMMON_FUNC.IsInvalidChair(cmd.wCurrentUser))
            mGameView.ClearOutCards((PlayerPanelPosition)cmd.wCurrentUser); 

        UiRenderOutCard(cmd.wOutCardUser, temp);

        //移动定时器
        if (!COMMON_FUNC.IsInvalidChair(cmd.wCurrentUser))
        {
            TableController.ClockCallBack call = null;
            if (cmd.wCurrentUser == SELT_VIEW_CHAIR)
            {
                call = CallBackOutCard;

                gameStage.BeginOutCard(cmd.wCurrentUser);
                        
                mGameHelper.ClearPrompt();
                //

                if (mIsDelegate)
                {
                    StartCoroutine(AutoCard(0));
                } else
                {
                    PlayerContrl();
                }
            } else
            {
                if (!mIsDelegate)
                {                
                    mGameView.ToggleCards(CardControlLevel.CanDragButCannotPlay);
                }               
            }
            mGameView.ClockStart((int)GameOutCatdSeconds, call, cmd.wCurrentUser);
        }

        NextPlayer(cmd.wCurrentUser);
        return true;
    }

    void PlayerContrl()
    {
        //检测是否有大过上家的牌              
        if (mGameHelper.SearchOutCard(1))
        {
            SetActivePlay(PLAY_ALLBUTTON);
            mGameView.ToggleCards(CardControlLevel.CanPlay);
        } else
        {
            //没有可出的牌
            SetActivePlay(PLAY_PASS);
            PropertyDisableStopButton();
            mGameView.ShowTip(RuleTipType.NoBiggerCard);
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="round"></param>
    /// <returns></returns>
    IEnumerator AutoCard(int round)
    {
        mGameView.ToggleCards(CardControlLevel.Locked);
        SetActivePlay(PLAY_CLOSE);

        Debuger.Instance.Log("--------------Auto Card:" + mGameHelper.TurnOutCard.Count);

        yield return new WaitForSeconds(2);


        round = mGameHelper.TurnOutCard.Count == 0 ? 1 : 0;

        if (round == 1)
        {
            //首次出牌

            byte[] carddata;
            mGameHelper.GetAutoFirestOut(out carddata);
            mGameHelper.OutCard(SELT_VIEW_CHAIR, carddata);
        } else
        {
            List<byte> allCard = mGameHelper.GetHelpCard();

            yield return new WaitForSeconds(1);

            if (allCard.Count == 0)
            {
                //pass
                OnChoice((int)ChoiceCommand.CHOICE_PASS);
            } else
            {
                //out
                mGameHelper.OutCard(SELT_VIEW_CHAIR, GameLogicUtil.ConverListToByte(allCard));                
            }
        }       
    }


    /// <summary>
    /// 渲染出牌特效，音效
    /// </summary>
    /// <param name="setid"></param>
    /// <param name="cardlist"></param>
    void UiRenderOutCard(int setid, byte[] cardlist)
    {
        VFX vfx = ShowCardVxf(setid, cardlist, delegate
        {
            Debug.Log("FinishVfx");
        });
        //显示出牌数据 
        if (setid == mPlayers [SELT_VIEW_CHAIR].mUser.wChairID)
        {
            //判断是否最后一手
            if (mGameHelper.PlayerHandCard [SELT_VIEW_CHAIR].Count == 0)
            {
                gameStage.EndOutCard(cardlist, mGameHelper.GetOutCardType(cardlist), true); //任务出牌判断 
            } else
            {
                gameStage.EndOutCard(cardlist, mGameHelper.GetOutCardType(cardlist));
            }

            mGameView.OnPlaySelfCardSuccess(vfx, cardlist);

            TaskChannal.Instance.InGamePlayerOutCard(cardlist);

            PropertySetStopOut ();
            PropertyCloseStopButton ();
        } 
        else 
        {           
            mGameView.PlayOthersCard(cardlist, (PlayerPanelPosition)setid, false, vfx);
            Reminder.inst.Decrease(cardlist);
        }    
    }
    
    /// <summary>
    /// 托管
    /// </summary>
    public bool SetAutoOutCard(bool isAutoOut)
    {
        if (GetState() != STATE_RUN)
        {           
            return false;
        }            

        if (isAutoOut)
        {
            Debuger.Instance.Log("Auto Out Card");

            //mGameView.ToggleCards(CardControlLevel.Locked);
            mGameView.ToggleAgent(true);
            mIsDelegate = true; 

            //----------------------

            if (mCurrentUser == 1)
            {
                StartCoroutine(AutoCard(0));
            }

            //----------------------


            return true;
        } else
        {      
            Debuger.Instance.Log("Auto Out Card Canncal");
            ClearDelegate();
            return true;
        }        
    }

    void ClearDelegate()
    {
        mIsDelegate = false;
        mGameView.ToggleAgent(false);
        mGameAuto.SetDelegate(true);
    }


    /// <summary>
    /// 出牌特效
    /// </summary>
    /// <param name="cardList">牌集</param>
    int lastSetID;
    byte[] lastCardList;
    OutCardsType lastCardsType;

    VFX ShowCardVxf(int setid, byte[] cardList, Action onVfxEnd=null)
    {
        VFX vfx = VFX.None;
        OutCardsType type = GameLogicUtil.GetOutCardType(cardList);
        bool isFemale = GameHelper.Instance.mPlayers [setid].Gender == GlobalDef.GENDER_GIRL ? true : false;
        //Debug.Log(setid+" "+GameHelper.Instance.mPlayers [setid].Gender);
        switch (type)
        {
            case OutCardsType.Single:
                if (Card.GetTranslatedDataNum(Card.TranslateData(cardList [0])) == 3)
                {
                    //mGameView.PlayInkSplashVfx();
                    vfx = VFX.InkSplash;
                }
                SOUND.Instance.OneShotSound(Sfx.inst.GetVoice(VoiceType.Single, GameLogicUtil.GetVoiceIndex(cardList [0]), isFemale));
                break;
            case OutCardsType.Pair:
                if (Card.GetTranslatedDataNum(Card.TranslateData(cardList [0])) == 3)
                {
                    //mGameView.PlayInkSplashVfx();
                    vfx = VFX.InkSplash;
                }
                SOUND.Instance.OneShotSound(Sfx.inst.GetVoice(VoiceType.Pair, GameLogicUtil.GetVoiceIndex(cardList [0]), isFemale));
                break;
            case OutCardsType.Triple:
                if (Card.GetTranslatedDataNum(Card.TranslateData(cardList [0])) == 3)
                {
                    //mGameView.PlayInkSplashVfx();
                    vfx = VFX.InkSplash;
                }
                SOUND.Instance.OneShotSound(Sfx.inst.GetVoice(VoiceType.Triple, GameLogicUtil.GetVoiceIndex(cardList [0]), isFemale));
                break;
            case OutCardsType.Bomb:
                if (mGameHelper.GetStage() == GameHelper.STAGE_NORMAL)
                {
                    if (Card.GetTranslatedDataNum(Card.TranslateData(cardList [0])) == 3)
                    {
                        //mGameView.PlayInkSplashVfx();
                        vfx = VFX.InkSplash;
                    } else
                    {
                        vfx = VFX.Bomb;
                    }
                    break;
                }
                mGameView.PlayBombVfx(setid);
                vfx = VFX.Bomb;
                SOUND.Instance.OneShotSound(Sfx.inst.GetVoice(VoiceType.Quad, GameLogicUtil.GetVoiceIndex(cardList [0]), isFemale));
                SOUND.Instance.OneShotSound(Sfx.inst.GetVoice(VoiceType.Bom));
                SOUND.Instance.PlayBGM(Bgm.inst.battle,false,Bgm.inst.game);
                break;
            case OutCardsType.Straight:
                if (Card.GetTranslatedDataNum(Card.TranslateData(cardList [0])) == 13)
                {
                    vfx = VFX.InkSplash;
                }
                if (cardList.Length == 3 && Card.GetTranslatedDataNum(Card.TranslateData(cardList [0])) == 13)
                {
                    mGameView.PlayJQKVfx(setid, onVfxEnd, cardList);
                    vfx = VFX.JQK;
                }
                if (cardList.Length >= 5)
                {
                    mGameView.PlayStraightVfx(setid);
                    vfx = VFX.Straight;
                }
                if (lastCardList != null && lastCardList.Length > 0 && lastSetID != setid && lastCardList.Length == cardList.Length && lastCardsType == OutCardsType.Straight)
                {
                    SOUND.Instance.OneShotSound(Sfx.inst.GetVoice(VoiceType.Bigger, UnityEngine.Random.Range(0, 3), isFemale));
                } else
                {
                    SOUND.Instance.OneShotSound(Sfx.inst.GetVoice(VoiceType.Straight, GameLogicUtil.GetVoiceIndex(cardList [0]), isFemale));
                }
                break;
            case OutCardsType.PairStraight:
                mGameView.PlayPairStraightVfx();
                vfx = VFX.PairStraight;
                SOUND.Instance.OneShotSound(Sfx.inst.GetVoice(VoiceType.Straight, GameLogicUtil.GetVoiceIndex(cardList [0]), isFemale));
                
                SOUND.Instance.OneShotSound(Sfx.inst.GetVoice(VoiceType.PairStraight));
                break;
            case OutCardsType.TripleStraight:
                SOUND.Instance.OneShotSound(Sfx.inst.GetVoice(VoiceType.Straight, GameLogicUtil.GetVoiceIndex(cardList [0]), isFemale));
                break;
            case OutCardsType.BombStraight:
                SOUND.Instance.OneShotSound(Sfx.inst.GetVoice(VoiceType.Straight, GameLogicUtil.GetVoiceIndex(cardList [0]), isFemale));
                break;
            default:
                Debug.LogError("pass");
                break;
        }

        lastSetID = setid;
        lastCardList = cardList;
        lastCardsType = type;

        return vfx;
    }   


    //--------------------------------------------------------------------------------------------


    bool OnSubGamePassCard(CMD_S_PassCard cmd)
    {        
        //效验数据
        CMD_S_PassCard pPassCard = (CMD_S_PassCard)cmd;        

        //清除当前玩家上次出牌
        mGameView.ClearOutCards((PlayerPanelPosition)cmd.wCurrentUser);
        //删除定时器
        if (GameHelper.Instance.GameStatus != CMD_Trench.GS_WK_PLAYING)
            return true;
        mGameView.ClockStop();

        //玩家设置
        if ((pPassCard.wPassUser != mPlayers [SELT_VIEW_CHAIR].mUser.wChairID))
        {
            //设置该玩家门前牌数据 
            ActionPassCard(pPassCard.wPassUser);
        } else
        {
            gameStage.PassCard();
        }
        NextPlayer(cmd.wCurrentUser);
        //一轮判断
        if (pPassCard.bNewTurn == 1)
        {
            //m_bTurnCardCount = 0;
            //m_bTurnOutType = CT_INVALID;
            //m_cbZhaDanCnt = 0;
            //memset(m_bTurnCardData, 0, sizeof(m_bTurnCardData));
            //清理界面数据

            //mGameView.ClearOutCards();

            Timer.Instance.SetTimer(DalayNewTurn, 0.5f, delegate
            {
                for (int i = 0; i < CMD_Trench.GAME_PLAYER; i++)
                {
                    if (i != cmd.wCurrentUser)
                        mGameView.ClearOutCards((PlayerPanelPosition)i);
                }
            });
        }

        //设置界面
        //mGameView.SetState(cmd.wPassUser, PlayerStateText.PlayPass);    //设置不出
        //ActionPassCard(cmd.wPassUser);
        
        TableController.ClockCallBack call = null;

        //玩家设置
        if (pPassCard.wCurrentUser == mPlayers [SELT_VIEW_CHAIR].mUser.wChairID)
        {
            gameStage.BeginOutCard(cmd.wCurrentUser);
            call = CallBackOutCard;
            mGameView.SetState(pPassCard.wCurrentUser, PlayerStateText.None);

            if (mIsDelegate)
            {
                StartCoroutine(AutoCard(pPassCard.bNewTurn));
            } else
            {
                PlayerContrlPass(pPassCard.bNewTurn);
            }                       
        } else
        {
            if (!mIsDelegate)
            {
                mGameView.ToggleCards(CardControlLevel.CanDragButCannotPlay);
            }           
        }
        

        //播放声音
        if (pPassCard.wPassUser != mPlayers [SELT_VIEW_CHAIR].mUser.wChairID)
        {
            //播放放弃声音 需补充
            //if (IsEnableSound()) PlayPassCardSound(pPassCard->wPassUser);

        }

        //设置时间
        mGameView.ClockStart((int)GameOutCatdSeconds, call, pPassCard.wCurrentUser);

        //没有打过上家牌在这里进行判断
        if (mPlayers [SELT_VIEW_CHAIR].mUser.wChairID == pPassCard.wCurrentUser)
        {
            //NoBigCardTip();
        }

        return true;
    }

    void PlayerContrlPass(int round)
    {
    

        mGameView.ToggleCards(CardControlLevel.CanPlay);
        if (round == 1)
        {
            Debuger.Instance.Log("首次出牌");
            SetActivePlay(PLAY_SENDBUTTON);
            PropertyClearAndroidPass();
        } else
        {
            //检测是否有大过上家的牌           
            mGameHelper.ClearPrompt();
            if (mGameHelper.SearchOutCard(SELT_VIEW_CHAIR))
            {
                //SearchCards.AddRange(outCards);
                SetActivePlay(PLAY_ALLBUTTON);
                //mGameView.ToggleCards(CardControlLevel.CanPlay);
            } else
            {
                //没有可出的牌
                SetActivePlay(PLAY_PASS);
                mGameView.ShowTip(RuleTipType.NoBiggerCard);

            }
            //SetActivePlay(PLAY_ALLBUTTON);
        }
    }

    bool OnSubGameEnd(CMD_S_GameEnd cmd)
    {
        SetState(STATE_STATISTICS);
        switch (mGameHelper.GetStage())
        {
            case GameHelper.STAGE_GATE:
                mGameView.ShowRusult(cmd.bGameResult [1], delegate()
                {
                    StageGameEnd(cmd);
                });
                break;
            case GameHelper.STAGE_BOM:
            case GameHelper.STAGE_NORMAL:            
                mGameView.ShowRusult(cmd.bGameResult [1], delegate()
                {
                    //GameFinal1(cmd);
                    GameFinal2(cmd);
                });
                break;
        }
        flushBottomBar();
        ShowLosePlayerCard(cmd.bCardCount, cmd.bCardData);
        DataBase.Instance.PLAYER.SaveUserInfo();
        ClearDelegate();//20150611.fix 713
        return true;
    }

    private void StageGameEnd(CMD_S_GameEnd cmd)
    {
        if (cmd.bGameResult[SELT_VIEW_CHAIR])
            PropertyScore2(cmd.lGameScore);
        gameStage.EndStageGame(cmd.bGameResult [SELT_VIEW_CHAIR], cmd.lGameScore [SELT_VIEW_CHAIR]);
        //
    }

    private void GameFinal1(CMD_S_GameEnd cmd)
    {
        //写入数据

        //全部翻牌显示 需补充
        int index = 0;

        for (int i = 0; i < cmd.bCardCount.Length; i++)
        {
            byte[] cards = new byte[cmd.bCardCount [i]];
            System.Array.Copy(cmd.bCardData, index, cards, 0, cmd.bCardCount [i]);
            if (cards.Length > 0 && cards [0] > 0) //这块传的牌数据有问题，占时判断处理
                mGameView.PlayOthersCard(cards, (PlayerPanelPosition)i, true);
            index += cmd.bCardCount [i];
        }
        //显示游戏结果 需补充
        mGameView.ShowRusult(cmd.bGameResult [SELT_VIEW_CHAIR]);
        mGameView.ClockStop();
        SetActivePlay(PLAY_CLOSE);
        //显示准备按钮
        mGameView.ToggleBtns(TableState.Result, true, 0);
    }

    private void ShowLosePlayerCard(byte[] cardnum, byte[] allCard)
    {
        int index = 0;

        //---------------------------------------------------

        string bufferstr = "";
        for (int i = 0; i < cardnum.Length;i++ )
        {
            bufferstr += cardnum[i];
            bufferstr += ", ";
        }
        bufferstr += "\n";
        bufferstr += "\n";
        for (int i = 0; i < allCard.Length; i++)
        {
            bufferstr += allCard[i];
            bufferstr += ", ";
        }
        bufferstr += "\n";
// 
//         Debuger.Instance.Log(bufferstr);

//         for (int i = 0; i < cardnum.Length; i++)
//         {
//             byte[] cards = new byte[cardnum[i]];
//             System.Array.Copy(allCard, index, cards, 0, cardnum[i]);
// 
//             if (cards.Length > 0 && cards[0] > 0) //这块传的牌数据有问题，占时判断处理
//                 mGameView.PlayOthersCard(cards, (PlayerPanelPosition)i, true);
//             index += cardnum[i];
//         }

        for(int i = 0;i < cardnum.Length;i++)
        {
            if(cardnum[i] == 0)
            {
                continue;
            }

            byte[] cards = new byte[cardnum[i]];

            for(int j = 0;j < cards.Length;j++)
            {
                cards[j] = allCard[index++];
            }
            mGameView.PlayOthersCard(cards, (PlayerPanelPosition)i, true);
        }
    }

    private void GameFinal2(CMD_S_GameEnd cmd)
    {
        mGameView.ClockStop();
        //mGameDialog.ShowDialog(1);
        
        //------------------------------------------------------------------

        int[] gamescore = new int[4];
        System.Array.Copy(cmd.lGameScore, gamescore, cmd.lGameScore.Length);    
   
        TaskFinal(gamescore);

        //-------------------------------------------------------

        mGameDialog.SetFinalWin
        (
            GameDialog.CATEGORY_NORMALE,
            cmd.bGameResult [SELT_VIEW_CHAIR],
            new int[] { cmd.lGameScore[SELT_VIEW_CHAIR] },
            // true,
            //  new int[]{12,34,45},
            null,
            //delegate{
                //Windata.LobbyWin = LobbyState.Arena;
                //Windata.RoomType = mGameHelper.GetStage();
                //MainGame.inst.OnExitGame();
                //DataBase.Instance.SCENE_MGR.SetScene(SceneMgr.SC_Hall);
            //}
            null,
            delegate()
            {
                mGameDialog.HideCurrentDialog();
                mGameView.ReStart();
                //mGameView.ToggleBtns(TableState.Ready, true, 0);        
            }
        );
        SetActivePlay(PLAY_CLOSE);
    }

    //--------------------------------------------------------------

    void CallBackAndroidScore()
    {
        mGameView.ClockStop();       
    }

    /// <summary>
    /// 用户计时器超时回调
    /// </summary>
    void CallBackOutCard()
    {
        //调用自己的出牌按钮   需补充

        //自己第一个出牌必须出一个
        if (mGameHelper.TurnOutCard.Count == 0)
        {
            Debuger.Instance.Log("min number out");

            byte[] selectData = mGameView.GetPlayingCardsData();

            if (selectData != null && VerdictOutCard(selectData))
            {
                mGameHelper.OutCard(SELT_VIEW_CHAIR, selectData);
                return;
            }

            //min carddate
            int index = mGameHelper.PlayerHandCard [mPlayers [SELT_VIEW_CHAIR].mUser.wChairID].Count;
            byte carddata = mGameHelper.PlayerHandCard [mPlayers [SELT_VIEW_CHAIR].mUser.wChairID] [index - 1];
            
            mGameHelper.OutCard(SELT_VIEW_CHAIR, new byte[] { carddata });     
         
        } else
        {//可以不出
            Debuger.Instance.Log("default pass");
            //
            ActionPassCard(SELT_VIEW_CHAIR);
            mGameHelper.PassCard(SELT_VIEW_CHAIR);
        }
    }

    void ActionPassCard(ushort id)
    {
        mGameView.SetState(id, PlayerStateText.PlayPass);
        if (SELT_VIEW_CHAIR == id)
        {
            mGameView.PutReadyCardsBack();
            if (!mIsDelegate)
            {
                mGameView.ShowTip(RuleTipType.None);
            }
        }else{
            mGameView.PlayEmotion((PlayerPanelPosition)id,Emotion.Morose);
        }
        bool isFemale = GameHelper.Instance.mPlayers [id].Gender == GlobalDef.GENDER_GIRL ? true : false;
        SOUND.Instance.OneShotSound(Sfx.inst.GetVoice(VoiceType.Pass, UnityEngine.Random.Range(0, 4), isFemale));
        //StartCoroutine(DisablePassCard(id));
    }

    IEnumerator DisablePassCard(int id)
    {
        yield return new WaitForSeconds(2);
        mGameView.SetState(id, PlayerStateText.None);
    }

    /// <summary>
    /// 
    /// </summary>
    public void CompleteClock()
    {
        CallScore(-1);
    }
    /// <summary>
    /// 叫分接口 
    /// </summary>
    /// <param name="score">-1 不叫， 1， 2，3</param>
    public void CallScore(int score)
    {
        mGameView.ClockStop();
        bool isPlayerFemale = DataBase.Instance.PLAYER.Gender == GlobalDef.GENDER_GIRL ? true : false;
        switch (score)
        {
            case -1:
                
                mGameHelper.CallScore(1, 255);
                mGameView.ClockStart(15, CallBackAndroidScore, 2);
            //SOUND.Instance.OneShotSound (Sfx.inst.GetVoice(VoiceType.CallScore,0,isPlayerFemale));
                break;

            case 1:
            //SOUND.Instance.OneShotSound (Sfx.inst.GetVoice(VoiceType.CallScore,1,isPlayerFemale));
                mGameHelper.CallScore(1, (byte)score);
                break;
            case 2:
            //SOUND.Instance.OneShotSound (Sfx.inst.GetVoice(VoiceType.CallScore,2,isPlayerFemale));
                mGameHelper.CallScore(1, (byte)score);
                break;
            case 3:
            //SOUND.Instance.OneShotSound (Sfx.inst.GetVoice(VoiceType.CallScore,3,isPlayerFemale));
                mGameHelper.CallScore(1, (byte)score);
                break;
            default:
                break;
        }


        PropertyDisableLastCard();//道具底牌 消失
        mGameView.ToggleBtns(TableState.Identify, false, 0);
    }

    private void NextPlayer(ushort currentid)
    {
        Debuger.Instance.Log("CurrentUser: " + currentid);

        TaskPlayerChange(currentid);

        mCurrentUser = currentid;
     
        if (currentid != 1)
        {
            return;
        }

        isOnChoice = true;
        PropertyStopOut();      
    }
    



    /// <summary>
    /// ui 拖拽回调
    /// </summary>
    /// <param name="cardList"></param>
    /// <returns></returns>
    public bool OnPlayCard(byte[] cardList)
    {
        Debuger.Instance.Log("Select Card :" + cardList.Length);

        if (VerdictOutCard(cardList))
        {
            mGameView.ClockStop();        
            SetActivePlay(PLAY_CLOSE);         
            mGameHelper.OutCard(SELT_VIEW_CHAIR, cardList);
            return true;
        } else
        {
            mGameView.ShowTip(RuleTipType.PlayWithoutRule);
            mGameView.OnPlaySelfCardFail();
            return false;
        }        
    }

	public void OnSelectCard (byte[] cardList, ref byte[] outCardList)
	{
		//Debuger.Instance.Log ("OnSelectCard~~~~~~~~~~~~");
		if (mCurrentUser == 1) {
			PropertyStopOut ();
		}
		PropertyChangeButton ();
	}

    public const int PLAY_CLOSE         = 0;
    public const int PLAY_ALLBUTTON     = 1;
    public const int PLAY_SENDBUTTON    = 2;
    public const int PLAY_PASS          = 4;


    /// <summary>
    /// 设置玩家按钮显示
    /// </summary>
    /// <param name="isActive"></param>
    public void SetActivePlay(int isActive)
    {
        switch (isActive)
        {
            case PLAY_ALLBUTTON:
                mGameView.ToggleBtns(TableState.Play, true, 1);
                break;
            case PLAY_CLOSE:                            
                mGameView.ToggleBtns(TableState.Play, false, 0);              
                break;
            case PLAY_SENDBUTTON:
                mGameView.ToggleBtns(TableState.Play, true, PLAY_SENDBUTTON);
                break;
            case PLAY_PASS:               
                mGameView.ToggleBtns(TableState.Play, true, 0);
                break;

        }
    }

    bool isOnChoice;
    
    /// <summary>
    /// 按钮选择
    /// </summary>
    /// <param name="i"></param>
    public void OnChoice(int i)
    {       

        if(!isOnChoice)
        {
            return;
        }
        switch ((ChoiceCommand)i)
        {
            case ChoiceCommand.CHOICE_PASS:

                Debuger.Instance.Log("OnChoice:Pass");

                ActionPassCard(SELT_VIEW_CHAIR);

                mGameHelper.PassCard(SELT_VIEW_CHAIR);

                SetActivePlay(PLAY_CLOSE);
                PropertyCloseStopButton();
                break;
            case ChoiceCommand.CHOICE_OUTCARD:
                //
                byte[] data = mGameView.GetPlayingCardsData();

                if (data.Length == 0)
                {
                    return;
                }

                if (VerdictOutCard(data))
                {                    
                    //允许出牌
                    mGameHelper.OutCard(SELT_VIEW_CHAIR, data);
                    //mGameView.OnPlaySelfCardSuccess(false);
                    SetActivePlay(PLAY_CLOSE);
                    isOnChoice = false;
                } else
                {                    
                    //错误不允许出牌
                    mGameView.OnPlaySelfCardFail();
                    mGameView.ShowTip(RuleTipType.PlayWithoutRule);
                    isOnChoice = true;
                }                
                break;
            case ChoiceCommand.CHOICE_TIPE://提示
                break;
        }

        
    }

    //提示出牌处理
    public List<byte> SearchOutCard()
    {
        //byte[] outCards = 
        return mGameHelper.GetHelpCard();
    }

    /// <summary>
    /// 出牌判断
    /// </summary>
    /// <param name="shootcards">选中的牌</param>
    /// <returns></returns>
    bool VerdictOutCard(byte[] shootcards)
    {
        byte[] m_bTurnCardData = mGameHelper.TurnOutCard.ToArray();
        byte m_bTurnCardCount = (byte)m_bTurnCardData.Length;
        //获取扑克

        //出牌判断
        if (shootcards.Length > 0L)
        {
            //牌型扑克
            byte[] bCardData = new byte[shootcards.Length];
            shootcards.CopyTo(bCardData, 0);
            mGameHelper.mGameLogic.SortCardList(bCardData, (byte)bCardData.Length);

            //分析类型
            byte bCardType = mGameHelper.mGameLogic.GetCardType(bCardData, (byte)bCardData.Length);

            //类型判断
            if (bCardType == GLogicDef.CT_INVALID)
                return false;

            //跟牌判断
            if (m_bTurnCardCount == 0)
                return true;
            return mGameHelper.mGameLogic.CompareCard(m_bTurnCardData, bCardData, m_bTurnCardCount, (byte)bCardData.Length);
        }

        return false;
    }

    /// <summary>
    /// 游戏退出
    /// </summary>
    public void OnExitGame()
    {
        GameBreakCut();

        TaskChannal.Instance.DestoryTask();
        //----------------------------------------

        Player me = mPlayers [1];
        FreeAllChair();
        mGameHelper.PerformStandUpAction(me);

        DataBase.Instance.SCENE_MGR.SetScene(SceneMgr.SC_Hall);
    }

    /// <summary>
    /// 游戏退出扣分
    /// </summary>
    public void GameBreakCut()
    {
        int stageCategory = mGameHelper.GetStage();
        if (stageCategory == GameHelper.STAGE_GATE)
        {
            return;
        }

        //-------------------------------------------

        if (GetState() != STATE_READY)
        {
            return;
        }
        Debuger.Instance.Log(" GameBreakCut ");
        //-------------------------------------------

        switch (stageCategory)
        {
            case GameHelper.STAGE_BOM:
            case GameHelper.STAGE_NORMAL:

                int cutScore = EconomicSystem.GameBreakScore(new int[] { mGameHelper.lCellScore });
                mPlayers [1].Gold = (uint)(mPlayers [1].Gold < cutScore ? 0 : mPlayers [1].Gold - cutScore);
                mPlayers [1].WriteUserInfo();

                break;
        }
        
    }

    /////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 道具 偷窥底牌
    /// </summary>
    private void PropertyGetLastCard()
    {
        if (mGameHelper.GetStage() != GameHelper.STAGE_GATE)
        {
            return;
        }
        if (DataBase.Instance.PLAYER.Property != PropertyMgr.PROPERTY_GETLASTCARD)
        {
            return;
        }


        mProperty.SetBottomCard(mGameHelper.mBackCard);
    }

    /// <summary>
    /// 清理道具 偷窥底牌
    /// </summary>
    private void PropertyDisableLastCard()
    {
        if (mGameHelper.GetStage() != GameHelper.STAGE_GATE)
        {
            return;
        }
        if (DataBase.Instance.PLAYER.Property != PropertyMgr.PROPERTY_GETLASTCARD)
        {
            return;
        }

        mProperty.SetBottomDisable();
    }

    //--------------------------------------

    /// <summary>
    /// 道具 偷窥手牌
    /// </summary>
    private void PropertyGetNextCard()
    {
        if (mGameHelper.GetStage() != GameHelper.STAGE_GATE)
        {
            return;
        }
        if (DataBase.Instance.PLAYER.Property != PropertyMgr.PROPERTY_GETNEXTCARD)
        {
            return;
        }
        StartCoroutine(mProperty.SetNextPlayerCards(mGameHelper.PlayerHandCard [2]));
    }

    //--------------------------------------------

    /// <summary>
    /// 道具禁手按钮
    /// </summary>
    private void PropertyStopOut()
    {
        if (mGameHelper.GetStage() != GameHelper.STAGE_GATE)
        {
            return;
        }
        if (!DataBase.Instance.PLAYER.hasAndroidPass())
        {
            return;
        }

     
       
        mProperty.SetStopButton(true);
        
        byte[] data = mGameView.GetPlayingCardsData();
        Debug.Log("禁手~~!" + data.Length);
        if (data.Length > 0)
        {
            mProperty.SetStopButtonState(PropertyStopButton.State.STATE_ENABLE);
        } else if (data.Length == 0)
        {
            mProperty.SetStopButtonState(PropertyStopButton.State.STATE_DISABLED);
        }
    }

    private void PropertyCloseStopButton()
    {
        if (mGameHelper.GetStage() != GameHelper.STAGE_GATE)
        {
            return;
        }
        if (DataBase.Instance.PLAYER.Property != PropertyMgr.PROPERTY_STOPOUT)
        {
            return;
        }
        mProperty.SetStopButton(false);
    }

    private void PropertyDisableStopButton()
    {
        if (mGameHelper.GetStage() != GameHelper.STAGE_GATE)
        {
            return;
        }
        if (DataBase.Instance.PLAYER.Property != PropertyMgr.PROPERTY_STOPOUT)
        {
            return;
        }
        mProperty.SetStopButtonState(PropertyStopButton.State.STATE_DISABLED);
    }

    private void PropertySetStopOut()
    {
        if (mGameHelper.GetStage() != GameHelper.STAGE_GATE)
        {
            return;
        }
        if (mProperty.GetStopButtonState() == PropertyStopButton.State.STATE_SELECTED)
        {
            Player player = DataBase.Instance.PLAYER;
            if (player.hasAndroidPass())
            {
                player.HasStopOut = true;
                player.StopOutCount++;
                mGameView.PlayItemVfx(LevelItemType.Forbid);
                mProperty.SetStopButtonState(PropertyStopButton.State.STATE_HIDE);
            }            
        }
    }

    private bool hasShowStopButton()
    {
        Player player = DataBase.Instance.PLAYER;

        if (player.Property == PropertyMgr.PROPERTY_STOPOUT && player.StopOutCount < 1)
        {
            return true;
        }
        return false;
    }

    private void PropertyClearAndroidPass()
    {
        if (mGameHelper.GetStage() != GameHelper.STAGE_GATE)
        {
            return;
        }
        Player player = DataBase.Instance.PLAYER;

        if (player.Property == PropertyMgr.PROPERTY_STOPOUT && player.StopOutCount == 1)
        {
            Debug.Log("Clear Player stop button");
            player.HasStopOut = false;
        }
    }

    private void PropertyClearStopCard()
    {
        if (mGameHelper.GetStage() != GameHelper.STAGE_GATE)
        {
            return;
        }
        Player player = DataBase.Instance.PLAYER;
        if (player.Property != PropertyMgr.PROPERTY_STOPOUT)
        {
            return;
          
        }

        player.StopOutCount = 0;
        player.HasStopOut = false;
    }

    //------------------------------------------------
    /// <summary>
    /// 道具偷梁换柱
    /// </summary>
    private void PropertyChangeCard()
    {
        if (mGameHelper.GetStage() == GameHelper.STAGE_GATE
            &&
            DataBase.Instance.PLAYER.Property == PropertyMgr.PROPERTY_CHANGE)
        {
            mGameView.ToggleCards(CardControlLevel.CanDragButCannotPlay);
            mProperty.ShowChangeDialog(true);
            mProperty.SetChangeButtonAction(PropertyChangeAction, PropertyChangCancal);
        } else
        {
            //叫分
            GameActionCallScore();
        }        
    }

    private void PropertyChangeAction()
    {
        List<byte> allCard = mGameHelper.PlayerHandCard [2];

        byte[] mycard = mGameView.GetPlayingCardsData();        

        byte temp = allCard [0];
        allCard [0] = mycard [0];

        List<byte> refcard = mGameHelper.PlayerHandCard [1];
        //Debug.Log(refcard.Count);
        for (int i = 0; i < refcard.Count; i++)
        {
            if (refcard [i] == mycard [0])
            {
                refcard [i] = temp;
                break;
            }
        }

        mGameHelper.SortCardList(refcard, (byte)refcard.Count);
        mGameHelper.UpdateUserCard();

        byte[] overlist = new byte[refcard.Count];
        for (int i = 0; i < refcard.Count; i++)
        {
            overlist [i] = refcard [i];
        }      
        byte[] changedData = {temp};
        mGameView.ChangeReadyCards(changedData, overlist);

        mProperty.ShowChangeDialog(false);
        mProperty.SetChangeButtonAction(null, null);

        GameActionCallScore();
    }

    private void PropertyChangCancal()
    {
        mProperty.ShowChangeDialog(false);
        mProperty.SetChangeButtonAction(null, null);
        //叫分
        GameActionCallScore();
    }

    private void PropertyChangeButton()
    {
        if (mGameHelper.GetStage() != GameHelper.STAGE_GATE)
        {
            return;
        }
        if (DataBase.Instance.PLAYER.Property != PropertyMgr.PROPERTY_CHANGE)
        {
            return;
        }


        byte[] temp = mGameView.GetPlayingCardsData();

        if (temp.Length != 1)
        {
            mProperty.SetChangeButton(false);
        } else if (temp.Length == 1)
        {
            mProperty.SetChangeButton(true);
        }
    }

    //------------------------------------------------------------
    /// <summary>
    /// 道具记牌器
    /// </summary>
    private void PropertyRecord()
    {
        if (mGameHelper.GetStage() != GameHelper.STAGE_GATE)
        {
            return;
        }
        if (DataBase.Instance.PLAYER.Property != PropertyMgr.PROPERTY_RECORD)
        {
            return;
        }
        mGameView.ToggleReminder(true);
        mGameView.PlayItemVfx(LevelItemType.Recorder);
        Reminder.inst.Decrease(mGameHelper.PlayerHandCard [1].ToArray());
    }
    
    private void PropertyScore2(int[] allScore)
    {
        if (mGameHelper.GetStage() != GameHelper.STAGE_GATE)
        {
            return;
        }
        if (DataBase.Instance.PLAYER.Property != PropertyMgr.PROPERTY_SCORE2)
        {
            return;
        }
        //增加分数特效
        mGameView.PlayItemVfx(LevelItemType.DoubleScore);
    }

    /////////////////////////////////////////////////////////////////////////////////////////


    private void TaskInit()
    {
        if(mGameHelper.GetStage() == GameHelper.STAGE_GATE)
        {
            TaskChannal.Instance.Trun = false;
        }
        if (!TaskChannal.Instance.Trun)
        {
            return;
        }
        TaskChannal.Instance.CreateTask();
        mTaskTip.SetTaskTip(TaskChannal.Instance.TargetTask.TaskText);
    }
    
    private void TaskFinal(int[] array)
    {
        if (!TaskChannal.Instance.Trun)
        {
            return;
        }
        array [3] = TaskChannal.Instance.mCurrentSettle;
        mTaskTip.gameObject.SetActive(false);
    }
    
    private void TaskPlayerChange(int position)
    {

        if (mGameHelper.GetStage() == GameHelper.STAGE_GATE)
        {
            return;
        }

        if (!TaskChannal.Instance.Trun)
        {
            return;
        }


        if (position == 1)
        {
            TaskChannal.Instance.InGamePlayerStart();
        } else
        {
            TaskChannal.Instance.InGamePlayerEnd();
        }
    }
    
}
