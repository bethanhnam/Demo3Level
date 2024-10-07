using DG.Tweening;
using Facebook.Unity;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class ButtonMennuManager : MonoBehaviour
{
    [SerializeField]
    private Animator animButton;
    [SerializeField]
    private CanvasGroup cvButton;
    [SerializeField]
    private RewardMove rewardMove;
    [SerializeField]

    public StarMove starMove;

    public GameObject starBar;
    public GameObject coinBar;
    public GameObject sliderBar;

    public Canvas starCanvas;

    public GameObject[] noticeButtons;
    public int levelMinigame;

    public GameObject playButton;
    public GameObject fixButton;
    public MiniGamePlayButton MiniGamePlayButton;

    public Image[] playButtonMaterial;
    public bool HasCallTween = false;

    private int appearButton = Animator.StringToHash("appear");
    private int disappearButton = Animator.StringToHash("disappear");
    private int disappearFixing = Animator.StringToHash("HideFixingUI");

    public bool isShowingFixing = false;


    //weeklyevent
    public TextMeshProUGUI NumOfCollect;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI timeRemaining;
    public Slider weeklyEventSlider;

    public Image unlockAtLevel;
    public TextMeshProUGUI unLockAtLevelText;

    public Image rewardImage;
    public Transform rewardDefautTransform;

    public Image collectImage;
    public Image fixNoticeImg;

    //HalloWeen
    public Image slideBarImage;
    public List<Sprite> slideSprites;

    public GameObject halloWeenTreat;
    public GameObject halloWeenPack;
    public TextMeshProUGUI halloWeenPackTimeText;


    private void Awake()
    {
        //// datatest
        //PlayerPrefs.SetInt("GiveAwayBooster", 1);
    }
    private void Update()
    {
        if (this.gameObject.activeSelf)
        {
            if (EventController.instance.isHalloWeen)
            {
                halloWeenPackTimeText.text = UIManagerNew.Instance.HalloWeenPack.SetText(EventController.instance.halloWeenEventConfig.EndTime);
            }
            if (EventController.instance.weeklyEvent != null)
            {
                timeRemaining.text = UIManagerNew.Instance.WeeklyEventPanel.CauculateTimeRemaining();
                if (CheckForChangeDataWeekly())
                {
                    //halloWeen
                    DOVirtual.DelayedCall(1, () =>
                    {
                        PlayerPrefs.SetString("FirstWeeklyEvent", "false");
                        EventController.instance.CheckTimeForWeeklyEvent();
                        EventController.instance.CheckForWeeklyEvent();
                        UIManagerNew.Instance.StartWeeklyEvent.Appear();
                        CheckForHalloWeen();
                        PlayerPrefs.SetString("FirstWeeklyEvent", "true");
                        Debug.Log("CheckForHalloWeen update");
                    });
                }
            }
        }
    }
    private void Start()
    {
        LevelManagerNew.Instance.SetConfigData();
        PlayButtonShinning();
    }
    public void Appear()
    {
        if (GameManagerNew.Instance.PictureUIManager != null)
        {
            if (GameManagerNew.Instance.PictureUIManager.gameObject.activeSelf == false)
            {
                GameManagerNew.Instance.PictureUIManager.Open();
            }
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            // fix button noti 
            fixNoticeImg.gameObject.SetActive(false);
            checkForAbleToFix();

            cvButton.blocksRaycasts = false;
            animButton.Play(appearButton, 0, 0);
            SaveSystem.instance.LoadData();
            DOVirtual.DelayedCall(1f, () =>
            {
                UIManagerNew.Instance.ButtonMennuManager.playButton.gameObject.SetActive(true);
            });

            //EventController.instance.CheckForWeeklyEvent();
            // new ui

            if (LevelManagerNew.Instance.stage < 2 && PlayerPrefs.GetInt("Hasfixed") == 1)
            {
                UIManagerNew.Instance.ButtonMennuManager.isShowingFixing = true;
                UIManagerNew.Instance.ButtonMennuManager.HideAllUI();
                UIManagerNew.Instance.ButtonMennuManager.starBar.gameObject.SetActive(true);
                UIManagerNew.Instance.ButtonMennuManager.sliderBar.gameObject.SetActive(true);
                UIManagerNew.Instance.ButtonMennuManager.starCanvas.gameObject.SetActive(true);
                GameManagerNew.Instance.PictureUIManager.ChangeItemOnly(LevelManagerNew.Instance.LevelBase.Level);
            }
            UIManagerNew.Instance.ThresholeController.SetSecondItemButton();
            CheckForWeeklyEventMenu();
            CheckForHalloWeen();

            if (LevelManagerNew.Instance.stage == 1 && PlayerPrefs.GetInt("Hasfixed") == 0)
            {
                GameManagerNew.Instance.conversationController.StartConversation(1, 2, "3FirstFix", () =>
                {
                    GameManagerNew.Instance.CheckForTutorFix();
                });
            }
            if (LevelManagerNew.Instance.stage <= 3)
            {
                noticeButtons[0].gameObject.SetActive(false);
            }
            else
            {
                noticeButtons[0].gameObject.SetActive(true);
            }
            CheckDailyNotice();
            CheckForMinigame();


            if (HasCallTween == false)
            {
                HasCallTween = true;
            }
            UIManagerNew.Instance.ChestSLider.SetCurrentValue(LevelManagerNew.Instance.LevelBase.CountLevelWin);
        }
    }

    public void CheckForWeeklyEventMenu()
    {
        if (!isShowingFixing && LevelManagerNew.Instance.stage >= 8 && EventController.instance.weeklyEvent != null)
        {
            LoadSliderValue();
            unlockAtLevel.gameObject.SetActive(false);
            weeklyEventSlider.gameObject.SetActive(true);
            sliderBar.gameObject.SetActive(false);
            collectImage.sprite = EventController.instance.weeklyEventItemSprite;
        }
        else
        {
            weeklyEventSlider.gameObject.SetActive(false);
            sliderBar.gameObject.SetActive(true);
        }
        if (LevelManagerNew.Instance.stage >= 2 && LevelManagerNew.Instance.stage < 8)
        {
            unLockAtLevelText.text = "Unlock at level 9";
            unlockAtLevel.gameObject.SetActive(true);
            weeklyEventSlider.gameObject.SetActive(false);
        }
    }
    public void Close()
    {
        cvButton.blocksRaycasts = false;
        animButton.Play(disappearButton);
    }

    public void Deactive()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public void ActiveCVGroup()
    {
        if (!GameManagerNew.Instance.CheckSliderValue())
        {
            if (!cvButton.blocksRaycasts)
            {
                cvButton.blocksRaycasts = true;
            }
        }
        if (AudioManager.instance.musicSource.isPlaying)
        {
            if (AudioManager.instance.musicSource.clip.name != "MenuTheme2")
            {
                AudioManager.instance.PlayMusic("MenuTheme");
            }
        }
        DisplayLevelText();


        FirebaseAnalyticsControl.Instance.Screen_Home();
    }

    public void CheckForEvent()
    {
        if (UIManagerNew.Instance.WeeklyEventPanel.WeeklyItemCollect.numOfCollection > 0)
        {
            UIManagerNew.Instance.BlockPicCanvas.SetActive(true);
        }
    }
    public void DiactiveCVGroup()
    {
        cvButton.blocksRaycasts = false;
    }
    public void OpenNotEnoughStar()
    {
        UIManagerNew.Instance.NotEnoughStarPanel.Open();
    }
    public void OpenDailyRW()
    {

        FirebaseAnalyticsControl.Instance.click_dailyRw();
        //GameManagerNew.Instance.ClosePicture(false);
        Close();
        UIManagerNew.Instance.DailyRWUI.Appear();
    }
    public void OpenCongratPanel()
    {
        //GameManagerNew.Instance.ClosePicture(false);
        //Close();
        UIManagerNew.Instance.CongratPanel.Open();
    }
    public void OpenSettingPanel()
    {
        //GameManagerNew.Instance.ClosePicture(false);
        Close();
        UIManagerNew.Instance.SettingPanel.Open();
    }
    public void OpenRattingPanel()
    {
        //GameManagerNew.Instance.ClosePicture(false);
        Close();
        UIManagerNew.Instance.RattingPanel.Open();
    }
    public void OpenNonAdsPanel()
    {
        //GameManagerNew.Instance.ClosePicture(false);
        Close();
        UIManagerNew.Instance.NonAdsPanel.Open();
    }
    public void OpenHalloWeenTreat()
    {
        Close();
        UIManagerNew.Instance.HalloWeenTreat.Appear();
        UIManagerNew.Instance.BlockPicCanvas.SetActive(false);
    }
    public void OpenHalloWeenPack()
    {
        Close();
        UIManagerNew.Instance.HalloWeenPack.Appear();
    }
    public void OpenShopPanel()
    {
        //GameManagerNew.Instance.ClosePicture(false);
        //if (GameManagerNew.Instance.CurrentLevel != null)
        //{
        //    GameManagerNew.Instance.CloseLevel(false);
        //}
        Close();
        UIManagerNew.Instance.ShopPanel.Open();
    }
    public void OpenShopPanelViaBooster()
    {
        //GameManagerNew.Instance.ClosePicture(false);
        if (GameManagerNew.Instance.CurrentLevel != null)
        {
        }
        Close();
        UIManagerNew.Instance.ShopPanel.Open();
    }
    public void OpenRW()
    {
        rewardMove.gameObject.SetActive(true);
    }
    public void DisappearDailyRW()
    {
        Appear();
        Debug.Log("thông qua dailyRW");
        GameManagerNew.Instance.SetCompleteStory();
        Debug.Log("thông qua SetCompleteStory");
        UIManagerNew.Instance.DailyRWUI.Close();
    }

    public void OpenStartMinigame()
    {
        Close();
        UIManagerNew.Instance.StartMiniGamePanel.Appear();
        if (UIManagerNew.Instance != null)
        {
            UIManagerNew.Instance.ButtonMennuManager.CheckForMinigame();
        }
    }

    public void DisplayWin()
    {
        UIManagerNew.Instance.CompleteImg.gameObject.SetActive(true);
    }
    public void PlayButton()
    {
        int level = LevelManagerNew.Instance.GetStage();
        //GameManagerNew.Instance.CreateLevel(level);
        if (GameManagerNew.Instance.CheckLevelStage())
        {
            PlayerPrefs.SetInt("HasCompleteLastLevel", 1);
        }
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
        UIManagerNew.Instance.GamePlayLoading.appear();
        DOVirtual.DelayedCall(.6f, () =>
        {
            UIManagerNew.Instance.GamePlayPanel.AppearForCreateLevel();
            if (PlayerPrefs.GetInt("HasCompleteLastLevel") == 1)
            {
                int replayLevel = UnityEngine.Random.Range(10, LevelManagerNew.Instance.stageList.Count - 1);
                GameManagerNew.Instance.CreateLevel(replayLevel);
            }
            else
            {
                DOVirtual.DelayedCall(.95f, () =>
                {
                    if (LevelManagerNew.Instance.stage == 1)
                    {
                        GamePlayPanelUIManager.Instance.boosterBar.gameObject.SetActive(false);
                        //tuto undo 
                        DOVirtual.DelayedCall(0.5f, () =>
                        {
                            if (Stage.Instance != null && Stage.Instance.gameObject.activeSelf)
                            {
                                Stage.Instance.canInteract = false;
                            }
                        });
                    }
                });
                GameManagerNew.Instance.CreateLevel(level);
            }
        });
        Close();
    }
    public void PlayMiniGame()
    {
        DOVirtual.DelayedCall(1, () =>
        {
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            UIManagerNew.Instance.GamePlayLoading.appear();
            DOVirtual.DelayedCall(.5f, () =>
                {
                    if (LevelManagerNew.Instance.stage == 3)
                    {
                        this.levelMinigame = 0;
                    }
                    if (LevelManagerNew.Instance.stage == 7)
                    {
                        this.levelMinigame = 1;
                    }
                    GameManagerNew.Instance.CreateMiniGame(levelMinigame);
                });
            Close();
        });
    }

    public void DisPlayPresent()
    {
        Vector3 startPos = UIManagerNew.Instance.ChestSLider.present.transform.position;
        UIManagerNew.Instance.ChestSLider.present.transform.DOMove(Vector3.zero, 1f).OnComplete(() =>
        {
            UIManagerNew.Instance.ChestSLider.returnPos();
            UIManagerNew.Instance.ButtonMennuManager.OpenCongratPanel();
            DOVirtual.DelayedCall(0.3f, () =>
            {
                UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
            });
        });
    }
    public void DisplayLevelText()
    {
        if (PlayerPrefs.GetInt("HasCompleteLastLevel") == 1)
        {
            UIManagerNew.Instance.playBTLevelTexts.text = "Level " + (LevelManagerNew.Instance.displayLevel + 1).ToString();
        }
        else
        {
            if (LevelManagerNew.Instance.stage + 1 <= LevelManagerNew.Instance.stageList.Count)
            {
                UIManagerNew.Instance.playBTLevelTexts.text = "Level " + (LevelManagerNew.Instance.stage + 1).ToString();
            }
            else
            {
                UIManagerNew.Instance.playBTLevelTexts.text = "Level " + (LevelManagerNew.Instance.displayLevel + 1).ToString();
            }
        }
    }
    public void ShowPointer()
    {
        if (UIManagerNew.Instance.GamePlayPanel.gameObject.activeSelf)
        {
            UIManagerNew.Instance.GamePlayPanel.DeactiveTime();
        }
        if (Stage.Instance != null && Stage.Instance.gameObject.activeSelf)
        {
            Stage.Instance.canInteract = false;
        }
        UIManagerNew.Instance.ThresholeController.showThreshole("playButton", UIManagerNew.Instance.ButtonMennuManager.playButton.transform.localScale, UIManagerNew.Instance.ButtonMennuManager.playButton.transform);
    }
    public void activePlayButton()
    {
        UIManagerNew.Instance.ButtonMennuManager.playButton.gameObject.SetActive(true);
    }
    public void ShowNoticeIcon(int i, bool status)
    {
        noticeButtons[i].transform.GetChild(0).gameObject.SetActive(status);
    }
    public void CheckDailyNotice()
    {
        string lastTime = PlayerPrefs.GetString("LastClaimTime");
        DateTime lastclaimTime;
        if (!string.IsNullOrEmpty(lastTime))
        {
            lastclaimTime = DateTime.Parse(lastTime); // Đảm bảo thời gian UTC
        }
        else
        {
            lastclaimTime = DateTime.MinValue;
        }

        // Kiểm tra với thời gian UTC hiện tại
        if (DateTime.Today > lastclaimTime)
        {
            ShowNoticeIcon(0, true);
        }
        else
        {
            ShowNoticeIcon(0, false);
        }
    }
    public void CheckForMinigame()
    {
        if (LevelManagerNew.Instance.stage == 3)
        {
            GameManagerNew.Instance.isMinigame = true;
            if (!UIManagerNew.Instance.MiniGamePlay.MiniGameMaps[0].hasDone && PlayerPrefs.GetInt("DoneMini1") == 0)
            {
                UIManagerNew.Instance.StartMiniGamePanel.SetProperties(50, 0);
                MiniGamePlayButton.SetQuestButton(0, "Help me");

                UIManagerNew.Instance.StartMiniGamePanel.ShowPlay();
                UIManagerNew.Instance.MiniGamePlay.isReplay = false;

                UIManagerNew.Instance.StartMiniGamePanel.AddPlay();

            }
            else
            {
                GameManagerNew.Instance.isMinigame = false;
                MiniGamePlayButton.SetToPlayButton();
            }
        }
        if (LevelManagerNew.Instance.stage == 7)
        {
            GameManagerNew.Instance.isMinigame = true;
            if (!UIManagerNew.Instance.MiniGamePlay.MiniGameMaps[1].hasDone && PlayerPrefs.GetInt("DoneMini2") == 0)
            {
                UIManagerNew.Instance.StartMiniGamePanel.SetProperties(50, 1);
                MiniGamePlayButton.SetQuestButton(1, "Help me");
                UIManagerNew.Instance.StartMiniGamePanel.ShowPlay();
                UIManagerNew.Instance.MiniGamePlay.isReplay = false;

                UIManagerNew.Instance.StartMiniGamePanel.AddPlay();
            }
            else
            {
                GameManagerNew.Instance.isMinigame = false;
                MiniGamePlayButton.SetToPlayButton();
            }
        }
    }
    public void HideAllUI()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void ShowAllUI()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void showFixingUI()
    {
        if (GameManagerNew.Instance.PictureUIManager != null)
        {
            if (UIManagerNew.Instance.ButtonMennuManager.isShowingFixing)
            {
                GameManagerNew.Instance.PictureUIManager.DisplayButton();
            }
            else
            {
                GameManagerNew.Instance.PictureUIManager.HiddenButton();
            }
        }
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
    }
    public void showFixing()
    {
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
        isShowingFixing = true;
        if (!isShowingFixing && LevelManagerNew.Instance.stage >= 8)
        {
            LoadSliderValue();

        }
        else
        {

        }
        animButton.Play("ShowFixingUI");
    }

    public void HideFixing()
    {
        checkForAbleToFix();
        isShowingFixing = false;
        showFixingUI();
        if (!isShowingFixing && LevelManagerNew.Instance.stage >= 8)
        {
            LoadSliderValue();
        }
        else
        {
        }
        animButton.Play(disappearFixing);

    }

    public void PlayAppearAnim()
    {
        UIManagerNew.Instance.ChestSLider.SetCurrentValue(LevelManagerNew.Instance.LevelBase.CountLevelWin);
        animButton.Play(appearButton);
    }

    public void LoadSliderValue()
    {
        if (EventController.instance != null)
        {
            if (EventController.instance.weeklyEvent != null)
            {
                if (UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent)
                {
                    weeklyEventSlider.maxValue = EventController.instance.weeklyEvent.numToLevelUp;
                    weeklyEventSlider.value = EventController.instance.weeklyEvent.numOfCollection;
                    NumOfCollect.text = EventController.instance.weeklyEvent.numOfCollection.ToString() + "/" + EventController.instance.weeklyEvent.numToLevelUp;
                    rewardImage.sprite = UIManagerNew.Instance.WeeklyEventPanel.rewardImage.sprite;
                    rewardText.text = UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].numOfReward.text;
                    collectImage.sprite = UIManagerNew.Instance.WeeklyEventPanel.ItemToCollect.sprite;
                    timeRemaining.text = UIManagerNew.Instance.WeeklyEventPanel.CauculateTimeRemaining();
                    ChangeRewardImage();
                    UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.CompleteEvent();
                }
                else
                {
                    weeklyEventSlider.maxValue = EventController.instance.weeklyEvent.numToLevelUp;
                    weeklyEventSlider.value = EventController.instance.weeklyEvent.numOfCollection;
                    NumOfCollect.text = EventController.instance.weeklyEvent.numOfCollection.ToString() + "/" + EventController.instance.weeklyEvent.numToLevelUp;
                    rewardImage.sprite = UIManagerNew.Instance.WeeklyEventPanel.rewardImage.sprite;
                    rewardText.text = UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].numOfReward.text;
                    collectImage.sprite = UIManagerNew.Instance.WeeklyEventPanel.ItemToCollect.sprite;
                    timeRemaining.text = UIManagerNew.Instance.WeeklyEventPanel.CauculateTimeRemaining();
                    ChangeRewardImage();
                    UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.UpdateData();
                }
            }
        }
    }

    public void ChangeRewardImage()
    {
        if (UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardType1 == weeklyReward.rewardType.gold)
        {
            rewardImage.sprite = UIManagerNew.Instance.WeeklyEventPanel.rewardSprites[0];
        }
        if (UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardType1 == weeklyReward.rewardType.unscrew)
        {
            rewardImage.sprite = UIManagerNew.Instance.WeeklyEventPanel.rewardSprites[1];
        }
        if (UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardType1 == weeklyReward.rewardType.undo)
        {
            rewardImage.sprite = UIManagerNew.Instance.WeeklyEventPanel.rewardSprites[2];
        }
        if (UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardType1 == weeklyReward.rewardType.drill)
        {
            rewardImage.sprite = UIManagerNew.Instance.WeeklyEventPanel.rewardSprites[3];
        }
    }

    public void checkForAbleToFix()
    {
        try
        {
            if (GameManagerNew.Instance.PictureUIManager != null)
            {
                for (int j = 0; j < GameManagerNew.Instance.PictureUIManager.stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].listObjLock.Count; j++)
                {
                    if (DataLevelManager.Instance.DataLevel.Data[GameManagerNew.Instance.PictureUIManager.level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].DataItmeLevel[j].IsUnlock == false)
                    {
                        if (SaveSystem.instance.star >= DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].Item[j].Star)
                        {
                            fixNoticeImg.gameObject.SetActive(true);
                            return;
                        }
                    }
                }
            }
        }
        catch { }
        fixNoticeImg.gameObject.SetActive(false);
    }
    public void PlayButtonShinning()
    {
        DOVirtual.Float(0, 1, 2f, (x) =>
        {
            playButtonMaterial[0].material.SetFloat("_ShineLocation", x);
        }).OnComplete(() =>
        {
            DOVirtual.DelayedCall(1.5f, () =>
            {
                PlayButtonShinning();
            });
        });
    }

    public void CheckForHalloWeen()
    {
        if (EventController.instance.isHalloWeen)
        {
            halloWeenPack.gameObject.SetActive(true);
            halloWeenTreat.gameObject.SetActive(true);
            halloWeenPackTimeText.text = UIManagerNew.Instance.HalloWeenPack.SetText(EventController.instance.halloWeenEventConfig.EndTime);
        }
        else
        {
            halloWeenPack.gameObject.SetActive(false);
            halloWeenTreat.gameObject.SetActive(false);
            halloWeenPackTimeText.text = UIManagerNew.Instance.HalloWeenPack.SetText(DateTime.Now);
        }
    }

    public bool CheckForChangeDataWeekly()
    {
        bool result = false;
        if (EventController.instance.weeklyEvent != null)
        {
            DateTime endTime;
            try
            {
                var x = JsonConvert.DeserializeObject<long>(EventController.instance.weeklyEvent.endEventDate);
                endTime = new DateTime(x);
            }
            catch (Exception ex)
            {
                var x = DateTime.Parse(EventController.instance.weeklyEvent.endEventDate);
                endTime = x;
                EventController.instance.weeklyEvent.endEventDate = x.Ticks.ToString();
                EventController.instance.SaveData("WeeklyEvent", EventController.instance.weeklyEvent);
            }

            if (EventController.instance.HasTimeExpired(endTime))
            {
                result = true;
            }
        }
        return result;
    }
    private void OnDisable()
    {

    }
}
