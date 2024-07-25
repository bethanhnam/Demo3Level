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

    //pointer
    public GameObject playBTPoinnter;

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
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        DisplayPointer();
                    });
                }
                else
                {
                    DisablePointer();
                }
            });
        }
    }
    public void Close()
    {
        cvButton.blocksRaycasts = false;
        animButton.Play(disappearButton);
        DisablePointer();
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
        DisablePointer();
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
                    GameManagerNew.Instance.CreateLevel(level);
                }
            });
            Close();
        }
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
    public void DisplayReward()
    {

    }

    public void DisplayPointer()
    {
        playBTPoinnter.gameObject.SetActive(true);
    }
    public void DisablePointer()
    {
        playBTPoinnter.gameObject.SetActive(false);
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
}
