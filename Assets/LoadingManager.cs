using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public void NormalInitGame()
    {
        DOVirtual.DelayedCall(0.1f, HandleNormalInitGame);
        LoadingScreen.instance.cv.DOFade(0, 0.3f).OnComplete(HandleLoadingScreenFadeComplete);
    }

    private void HandleNormalInitGame()
    {
        int completeLastPic = PlayerPrefs.GetInt("CompleteLastPic");

        if (completeLastPic == 1)
        {
            Debug.Log("Creating new picture");
            PlayerPrefs.SetInt("CompleteLastPic", 0);
            var levelBase = LevelManagerNew.Instance.LevelBase;
            if (levelBase != null)
            {
                if (levelBase.Level + 1 < DataLevelManager.Instance.DatatPictureScriptTableObjects.Length)
                {
                    GameManagerNew.Instance.CreateForNewPic();
                }
                else
                {
                    PlayerPrefs.SetInt("CompleteLastPic", 1);
                    GameManagerNew.Instance.InitStartGame();
                }
            }
        }
        else
        {
            HandleNonCompleteLastPic();
        }
    }

    private void HandleNonCompleteLastPic()
    {
        GameManagerNew.Instance.InitStartGame();
        if (PlayerPrefs.GetInt("Hasfixed") == 0 && LevelManagerNew.Instance.stage == 0)
        {
            UIManagerNew.Instance.BackGroundFooter.ShowBackGroundFooter(true);
            GameManagerNew.Instance.conversationController.StartConversation(0, 0, "1FirstConver", () =>
            {
                UIManagerNew.Instance.BackGroundFooter.DisappearBackGroundFooter();
                SetupInitialUI();
            });
        }
        else if (PlayerPrefs.GetInt("Hasfixed") == 1 && LevelManagerNew.Instance.stage == 0)
        {
            SetupUIForFixedState();
        }
    }

    private void SetupInitialUI()
    {
        UIManagerNew.Instance.ButtonMennuManager.isShowingFixing = true;
        UIManagerNew.Instance.ButtonMennuManager.HideAllUI();
        UIManagerNew.Instance.ButtonMennuManager.starBar.gameObject.SetActive(true);
        UIManagerNew.Instance.ButtonMennuManager.sliderBar.gameObject.SetActive(true);
        UIManagerNew.Instance.ButtonMennuManager.starCanvas.gameObject.SetActive(true);
        UIManagerNew.Instance.ButtonMennuManager.Appear();

        if (PlayerPrefs.GetInt("Hasfixed") == 0 && LevelManagerNew.Instance.LevelBase.Level == 0)
        {
            if (SaveSystem.instance.star == 0)
            {
                SaveSystem.instance.addStar(1);
            }
            var windowBtnObj = GameManagerNew.Instance.PictureUIManager.windowBtnObj;
            var highlightMaterial = UIManagerNew.Instance.HighLightObj;

            GameManagerNew.Instance.PictureUIManager.windowObj.GetComponent<SkeletonGraphic>().material = highlightMaterial;
            windowBtnObj.transform.localScale = Vector3.zero;
            windowBtnObj.SetActive(true);
            windowBtnObj.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
            {
                UIManagerNew.Instance.ShowPoiner(windowBtnObj.transform);
            });
        }
    }

    private void SetupUIForFixedState()
    {
        UIManagerNew.Instance.ButtonMennuManager.isShowingFixing = true;
        UIManagerNew.Instance.ButtonMennuManager.HideAllUI();
        UIManagerNew.Instance.ButtonMennuManager.starBar.gameObject.SetActive(true);
        UIManagerNew.Instance.ButtonMennuManager.sliderBar.gameObject.SetActive(true);
        UIManagerNew.Instance.ButtonMennuManager.starCanvas.gameObject.SetActive(true);
        UIManagerNew.Instance.ButtonMennuManager.Appear();
        AudioManager.instance.PlayMusic("MenuTheme");
    }

    private void HandleLoadingScreenFadeComplete()
    {
        if (SaveSystem.instance.powerTicket > 0 || SaveSystem.instance.magicTiket > 0 && PlayerPrefs.GetInt("Hasfixed") == 1 && LevelManagerNew.Instance.stage > 1)
        {
            if (PlayerPrefs.GetInt("HasTransfer") == 0)
            {

                UIManagerNew.Instance.TransferPanel.Appear();
            }
        }

        EventController.instance.LoadData();

        EventController.instance.CheckForWeeklyEvent();

        if (PlayerPrefs.GetInt("Hasfixed") == 1 && LevelManagerNew.Instance.stage != 0)
        {

            HandleHasFixedState();
        }

        if (RemoteConfigController.instance.IsShowOpenAds == 1)
        {

            AdsControl.Instance.ShowOpenAds();
        }

        Destroy(LoadingScreen.instance.gameObject);
    }

    private void HandleHasFixedState()
    {
        var chestSlider = UIManagerNew.Instance.ChestSLider;
        if (chestSlider.currentValue != chestSlider.maxValue1)
        {
            HandleDailyReward();
        }
        else
        {
            HandleRewardReceivedState();
        }
    }

    private void HandleDailyReward()
    {
        string lastClaimTime = PlayerPrefs.GetString("LastClaimTime", string.Empty);
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

        if (lastClaimTime.Equals(currentDate))
        {
            AudioManager.instance.PlayMusic("MenuTheme");
            UIManagerNew.Instance.ButtonMennuManager.Appear();
            if (PlayerPrefs.GetInt("CompleteLastPic") == 1)
            {
                PlayerPrefs.SetInt("CompleteLastPic", 0);
                GameManagerNew.Instance.CreatePicForNewPic();
            }
            UIManagerNew.Instance.BlockPicCanvas.SetActive(false);
            Debug.Log("Full process and daily reward claimed.");
        }
        else
        {
            if (LevelManagerNew.Instance.stage >= 4)
            {
                UIManagerNew.Instance.ButtonMennuManager.OpenDailyRW();
            }
            else
            {
                UIManagerNew.Instance.ButtonMennuManager.Appear();
            }
            AudioManager.instance.PlayMusic("MenuTheme");
            if (PlayerPrefs.GetInt("CompleteLastPic") == 1)
            {
                PlayerPrefs.SetInt("CompleteLastPic", 0);
                GameManagerNew.Instance.CreatePicForNewPic();
            }
            Debug.Log("Full process and daily reward not claimed.");
        }
    }

    private void HandleRewardReceivedState()
    {
        if (PlayerPrefs.GetInt("HasRecieveRW") == 1)
        {
            string lastClaimTime = PlayerPrefs.GetString("LastClaimTime", string.Empty);
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            if (lastClaimTime.Equals(currentDate))
            {
                AudioManager.instance.PlayMusic("MenuTheme");
                UIManagerNew.Instance.ButtonMennuManager.Appear();
                Debug.Log("Incomplete process but daily reward claimed.");
            }
            else
            {
                if (LevelManagerNew.Instance.stage >= 4)
                {
                    UIManagerNew.Instance.ButtonMennuManager.OpenDailyRW();
                }
                else
                {
                    UIManagerNew.Instance.ButtonMennuManager.Appear();
                }
                AudioManager.instance.PlayMusic("MenuTheme");
                Debug.Log("Incomplete process and daily reward not claimed.");
            }
        }
        else
        {
            AudioManager.instance.PlayMusic("MenuTheme");
            Debug.Log("Incomplete process and no reward opened.");
            UIManagerNew.Instance.ButtonMennuManager.Appear();
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
            if (PlayerPrefs.GetInt("CompleteLastPic") == 1)
            {
                PlayerPrefs.SetInt("CompleteLastPic", 0);
                GameManagerNew.Instance.CreatePicForNewPic();
            }
        }
    }

    public void PlayVideo()
    {
        if (GameManagerNew.Instance != null )
        {
            Debug.LogError("có GameManagerNew");
        }
        if(GameManagerNew.Instance.videoController != null)
        {
            Debug.LogError("có videoController");
        }    
        if (GameManagerNew.Instance != null && GameManagerNew.Instance.videoController != null)
        {
            Debug.LogError("chạy vào playvideo bth");
            GameManagerNew.Instance.videoController.gameObject.SetActive(true);
            AudioManager.instance.PlayMusic("story");
            GameManagerNew.Instance.videoController.CheckStartVideo();
            Debug.LogError("chạy vào playvideo bth done ");

        }
        else
        {
            StartCoroutine(LoadVideo(() =>
                    {
                        Debug.LogError("chạy vào playvideo ko bth");
                        GameManagerNew.Instance.videoController.CheckStartVideo();
                        AudioManager.instance.PlayMusic("story");
                        Debug.LogError("chạy vào playvideo ko bth done");
                    }));
        }
    }

    private IEnumerator LoadVideo(Action onComplete)
    {
        float maxWaitTime = 3f;
        float currentWaitTime = 0f;

        while (!GameManagerNew.Instance.videoController.videoPlayer.isPrepared && currentWaitTime < maxWaitTime)
        {
            currentWaitTime += Time.unscaledDeltaTime;
            yield return null;
        }

        if (onComplete != null)
        {
            onComplete.Invoke();
        }
    }
}
