using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject[] noticeButtons;

    public GameObject playButton;
    public MiniGamePlayButton MiniGamePlayButton;

    private int appearButton = Animator.StringToHash("appear");
    private int disappearButton = Animator.StringToHash("disappear");

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
                if (LevelManagerNew.Instance.stage == 0)
                {
                    ShowPointer();
                }
                else
                {
                    UIManagerNew.Instance.ButtonMennuManager.playButton.gameObject.SetActive(true);
                }
            });
            if (LevelManagerNew.Instance.stage == 1 && PlayerPrefs.GetInt("Hasfixed") == 0)
            {
                GameManagerNew.Instance.conversationController.StartConversation(1, 2, "3FirstFix", () =>
                {
                    GameManagerNew.Instance.CheckForTutorFix();
                });
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
        Close();
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
            DOVirtual.DelayedCall(.7f, () =>
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
                        if (LevelManagerNew.Instance.stage == 0)
                        {
                            GamePlayPanelUIManager.Instance.boosterBar.gameObject.SetActive(false);
                            //tuto undo 
                            GameManagerNew.Instance.conversationController.StartConversation(1, 1, "2SecondConver", () =>
                            {
                                Stage.Instance.TutorLevel1();
                            }, true);
                        }
                        if (LevelManagerNew.Instance.stage == 1)
                        {
                            GamePlayPanelUIManager.Instance.boosterBar.gameObject.SetActive(false);
                            //tuto undo 
                            if (SaveSystem.instance.extraHolePoint == 0)
                            {
                                SaveSystem.instance.extraHolePoint = 1;
                                UIManagerNew.Instance.LoadData(SaveSystem.instance.unscrewPoint, SaveSystem.instance.undoPoint, SaveSystem.instance.extraHolePoint, SaveSystem.instance.coin, SaveSystem.instance.star);
                            }
                            UIManagerNew.Instance.NewBooster.SetValue(0);
                            DOVirtual.DelayedCall(0.5f, () =>
                            {
                                if (Stage.Instance != null && Stage.Instance.gameObject.activeSelf)
                                {
                                    Stage.Instance.canInteract = false;
                                }
                                UIManagerNew.Instance.NewBooster.Appear();
                            });
                        }
                    });
                    GameManagerNew.Instance.CreateLevel(level);
                }
            });
            Close();
        }
    }
    [Button("CreateMinigame")]
    public void PlayMiniGame()
    {
        DOVirtual.DelayedCall(1, () =>
        {
            int level = 0;
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            UIManagerNew.Instance.GamePlayLoading.appear();

            DOVirtual.DelayedCall(.7f, () =>
            {
                DOVirtual.DelayedCall(.95f, () =>
                    {
                        if (LevelManagerNew.Instance.stage == 4)
                        {
                            level = 0;
                        }
                        if (LevelManagerNew.Instance.stage == 7)
                        {
                            level = 1;
                        }
                    });
                GameManagerNew.Instance.CreateMiniGame(level);
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
        if(LevelManagerNew.Instance.stage == 4 && PlayerPrefs.GetInt("GiveAwayBooster") == 1)
        {
            ConversationController.instance.StartConversation(0, 6, "minigame1", () =>
            {
                MiniGamePlayButton.SetQuestButton(0, "Help me");
            });
        }
        if (LevelManagerNew.Instance.stage == 7)
        {
            ConversationController.instance.StartConversation(0, 8, "minigame2", () =>
            {
                MiniGamePlayButton.SetQuestButton(1, "Help me");
            });
        }
    }
    public void ChangePlayButton()
    {
        playButton.gameObject.SetActive(false);
        MiniGamePlayButton.gameObject.SetActive(true);
    }
}
