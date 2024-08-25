using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    public GameObject sliderBar;

    public Canvas starCanvas;

    public GameObject[] noticeButtons;
    public int levelMinigame;

    public GameObject playButton;
    public MiniGamePlayButton MiniGamePlayButton;

    private int appearButton = Animator.StringToHash("appear");
    private int disappearButton = Animator.StringToHash("disappear");

    public bool isShowingFixing = false;


    //weeklyevent
    public TextMeshProUGUI NumOfCollect;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI timeRemaining;
    public Slider weeklyEventSlider;
    public Image rewardImage;

    private void Awake()
    {
        //// datatest
        //PlayerPrefs.SetInt("GiveAwayBooster", 1);
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
            cvButton.blocksRaycasts = false;
            animButton.Play(appearButton, 0, 0);
            SaveSystem.instance.LoadData();
            DOVirtual.DelayedCall(1f, () =>
            {
                UIManagerNew.Instance.ButtonMennuManager.playButton.gameObject.SetActive(true);
            });

            if (!isShowingFixing && LevelManagerNew.Instance.stage >=8)
            {
                weeklyEventSlider.gameObject.SetActive(true);
            }
            else
            {
                weeklyEventSlider.gameObject.SetActive(false);
            }
            if (LevelManagerNew.Instance.stage == 1 && PlayerPrefs.GetInt("Hasfixed") == 0)
            {
                GameManagerNew.Instance.conversationController.StartConversation(1, 2, "3FirstFix", () =>
                {
                    GameManagerNew.Instance.CheckForTutorFix();
                });
            }
            if (LevelManagerNew.Instance.stage >= 1 && PlayerPrefs.GetInt("Hasfixed") == 1)
            {
                isShowingFixing = false;
                GameManagerNew.Instance.PictureUIManager.HiddenButton();
            }
            CheckDailyNotice();
            if (LevelManagerNew.Instance.stage <= 3)
            {
                noticeButtons[0].gameObject.SetActive(false);
            }
            else
            {
                noticeButtons[0].gameObject.SetActive(true);
            }
            CheckForMinigame();
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
    public void DiactiveCVGroup()
    {
        if (cvButton.blocksRaycasts)
        {
            cvButton.blocksRaycasts = false;
        }
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
        Close();
        UIManagerNew.Instance.CongratPanel.Open();
    }
    public void OpenCompletePanel()
    {
        //GameManagerNew.Instance.ClosePicture(false);
        Close();
        UIManagerNew.Instance.CompletePanel.Open();
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
    public void OpenShopPanel()
    {
        //GameManagerNew.Instance.ClosePicture(false);
        if (GameManagerNew.Instance.CurrentLevel != null)
        {
            GameManagerNew.Instance.CloseLevel(false);
        }
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
        DOVirtual.DelayedCall(1, () =>
        {
            if (LevelManagerNew.Instance.stage >= 8)
            {
                if (!EventController.instance.FirstWeeklyEvent())
                {
                    UIManagerNew.Instance.StartWeeklyEvent.Appear();
                    PlayerPrefs.SetString("FirstWeeklyEvent", "true");
                }
            }
        });
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
            UIManagerNew.Instance.ButtonMennuManager.OpenCompletePanel();
        }
        else
        {
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            UIManagerNew.Instance.GamePlayLoading.appear();
            DOVirtual.DelayedCall(.8f, () =>
            {
                UIManagerNew.Instance.GamePlayPanel.AppearForCreateLevel();
                if (PlayerPrefs.GetInt("HasCompleteLastLevel") == 1)
                {
                    int replayLevel = UnityEngine.Random.Range(5, 29);
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
                    if (LevelManagerNew.Instance.stage == 6)
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
            UIManagerNew.Instance.playBTLevelTexts.text = LevelManagerNew.Instance.stageList.Count.ToString();
        }
        else
        {
            if (LevelManagerNew.Instance.stage + 1 <= LevelManagerNew.Instance.stageList.Count)
            {
                UIManagerNew.Instance.playBTLevelTexts.text = (LevelManagerNew.Instance.stage + 1).ToString();
            }
            else
            {
                UIManagerNew.Instance.playBTLevelTexts.text = (LevelManagerNew.Instance.stage).ToString();
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
        string lastClaimTime = PlayerPrefs.GetString("LastClaimTime", string.Empty);
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
        if (lastClaimTime.Equals(currentDate))
        {
            ShowNoticeIcon(0, false);
        }
        else
        {
            ShowNoticeIcon(0, true);
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
        if (LevelManagerNew.Instance.stage == 6)
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
    }
    public void showFixing()
    {
        isShowingFixing = true;
        animButton.Play("ShowFixingUI");
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
                    rewardImage.sprite = UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardImg.sprite;
                    rewardText.text = UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].numOfReward.text;
                    timeRemaining.text = UIManagerNew.Instance.WeeklyEventPanel.CauculateTimeRemaining();
                    UIManagerNew.Instance.WeeklyEventPanel.ChangeRewardImage();
                    UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.CompleteEvent();
                }
                else
                {
                    weeklyEventSlider.maxValue = EventController.instance.weeklyEvent.numToLevelUp;
                    weeklyEventSlider.value = EventController.instance.weeklyEvent.numOfCollection;
                    NumOfCollect.text = EventController.instance.weeklyEvent.numOfCollection.ToString() + "/" + EventController.instance.weeklyEvent.numToLevelUp;
                    rewardImage.sprite = UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardImg.sprite;
                    rewardText.text = UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].numOfReward.text;
                    timeRemaining.text = UIManagerNew.Instance.WeeklyEventPanel.CauculateTimeRemaining();
                    UIManagerNew.Instance.WeeklyEventPanel.ChangeRewardImage();
                    UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.UpdateData();
                }
            }
        }
    }

}
