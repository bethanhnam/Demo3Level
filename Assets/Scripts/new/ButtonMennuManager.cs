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
                    UIManagerNew.Instance.PlayButton.gameObject.SetActive(true);
                }
            });
            if (LevelManagerNew.Instance.stage == 1 && PlayerPrefs.GetInt("Hasfixed") == 0)
            {
                GameManagerNew.Instance.conversationController.StartConversation(1, 2, "3FirstFix", () =>
                {
                    GameManagerNew.Instance.CheckForTutorFix();
                });
            }
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
                            });
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
                                UIManagerNew.Instance.NewBooster.Appear();
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
        UIManagerNew.Instance.PlayButton.gameObject.SetActive(false);
        UIManagerNew.Instance.ThresholeController.showThreshole("playButton", UIManagerNew.Instance.PlayButton.transform.localScale, UIManagerNew.Instance.PlayButton.transform);
    }
    public void activePlayButton()
    {
        UIManagerNew.Instance.PlayButton.gameObject.SetActive(true);
    }
}
