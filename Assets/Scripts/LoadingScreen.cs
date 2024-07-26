using DG.Tweening;
using Firebase;
using Firebase.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using static UnityEngine.Rendering.HDROutputUtils;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;
    public GameObject loadingScreen;
    public Screen gamePlayScreen;
    public Slider[] sliders;

    [SerializeField]
    public CanvasGroup cv;

    // firebase
    public bool isFirebaseInitialized;
    public void LoadingScene(int sceneId)
    {
        StartCoroutine(LoadingSceneAsync(sceneId));
    }

    IEnumerator LoadingSceneAsync(int sceneId)
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        operation.allowSceneActivation = false;
        loadingScreen.SetActive(true);
        while (sliders[0].value <= 0.9f && !operation.isDone)
        {
            if (sliders[0].value <= 0.9f)
            {
                sliders[0].value += 0.01f;
            }
            if (operation.progress >= 0.9f && sliders[0].value == 0.9f && isFirebaseInitialized)
            {
                RemoteConfigController.instance.Init();
                operation.allowSceneActivation = true;
                yield return new WaitForSecondsRealtime(0.2f);
                if (!HasFinishedStory())
                {
                    GameManagerNew.Instance.videoController.gameObject.SetActive(true);
                    GameManagerNew.Instance.videoController.videoPlayer.Prepare();
                    // Wait for the video to be prepared
                    InvokeRepeating("WaitForVideoPrepare", 2, 0.5f);
                }
                else
                {
                    normalInitGame();
                }
            }
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1f);
    }

    private void WaitForVideoPrepare()
    {
        if (GameManagerNew.Instance.videoController.videoPlayer.isPrepared)
        {
            CancelInvoke("WaitForVideoPrepare");
            AudioManager.instance.PlayMusic("story");
            GameManagerNew.Instance.PlayVideo();
            Destroy(this.gameObject, 1f);
        }
    }

    private void normalInitGame()
    {
        if (GameManagerNew.Instance.videoController != null)
        {
            GameManagerNew.Instance.videoController.gameObject.SetActive(false);
        }
        DOVirtual.DelayedCall(0.1f, () =>
        {
            if (PlayerPrefs.GetInt("CompleteLastPic") == 1)
            {
                Debug.Log("chayj vaof tao moi pic");
                PlayerPrefs.SetInt("CompleteLastPic", 0);
                if (LevelManagerNew.Instance.LevelBase != null)
                {
                    if (LevelManagerNew.Instance.LevelBase.Level + 1 < DataLevelManager.Instance.DatatPictureScriptTableObjects.Length)
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
                GameManagerNew.Instance.InitStartGame();
            }
        });
        cv.DOFade(0, 0.3f).OnComplete(() =>
        {
            if (SaveSystem.instance.powerTicket > 0 || SaveSystem.instance.magicTiket > 0)
            {
                if (PlayerPrefs.GetInt("HasTransfer") == 0)
                {
                    UIManagerNew.Instance.TransferPanel.Appear();
                }
            }
            if (UIManagerNew.Instance.ChestSLider.currentValue != UIManagerNew.Instance.ChestSLider.maxValue1)
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
                    Debug.Log("mở khi  full process và chưa nhận quà hàng ngày ");
                }
                else
                {
                    UIManagerNew.Instance.ButtonMennuManager.OpenDailyRW();
                    AudioManager.instance.PlayMusic("MenuTheme");
                    if (PlayerPrefs.GetInt("CompleteLastPic") == 1)
                    {
                        PlayerPrefs.SetInt("CompleteLastPic", 0);
                        GameManagerNew.Instance.CreatePicForNewPic();
                    }
                    Debug.Log("mở khi  full process và chưa nhận quà hàng ngày ");
                }
            }
            else
            {
                if (PlayerPrefs.GetInt("HasRecieveRW") == 1)
                {
                    string lastClaimTime = PlayerPrefs.GetString("LastClaimTime", string.Empty);
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                    if (lastClaimTime.Equals(currentDate))
                    {
                        AudioManager.instance.PlayMusic("MenuTheme");
                        UIManagerNew.Instance.ButtonMennuManager.Appear();
                        Debug.Log("mở khi chua full process và  đã nhận quà hàng ngày ");
                    }
                    else
                    {
                        UIManagerNew.Instance.ButtonMennuManager.OpenDailyRW();
                        AudioManager.instance.PlayMusic("MenuTheme");
                        Debug.Log("mở khi chua full process và đã đã nhận quà hàng ngày ");
                    }
                }
                else
                {
                    AudioManager.instance.PlayMusic("MenuTheme");
                    Debug.Log("mở khi chua full process và chưa mở quà ");
                    UIManagerNew.Instance.ButtonMennuManager.Appear();
                    if (PlayerPrefs.GetInt("CompleteLastPic") == 1)
                    {
                        PlayerPrefs.SetInt("CompleteLastPic", 0);
                        GameManagerNew.Instance.CreatePicForNewPic();
                    }
                }
            }
            if (RemoteConfigController.instance.IsShowOpenAds == 1)
            {
                AdsControl.Instance.ShowOpenAds();
            }
            Destroy(this.gameObject);
        });
    }

    private void Start()
    {
        instance = this;
        //test
        //PlayerPrefs.SetString("HasFinishedStory", "true");

        DontDestroyOnLoad(this.gameObject);
        LoadingScene(1);

        Application.targetFrameRate = 60;
    }
    public bool IsFirstOpen()
    {
        var data = PlayerPrefs.GetString("isFirstOpen", "true");

        bool isFirstOpen = JsonConvert.DeserializeObject<bool>(data);

        if (isFirstOpen)
            PlayerPrefs.SetString("isFirstOpen", "false");

        return isFirstOpen;
    }
    public bool HasFinishedStory()
    {
        var data = PlayerPrefs.GetString("HasFinishedStory", "false");
        bool HasFinishedStory = JsonConvert.DeserializeObject<bool>(data);
        return HasFinishedStory;
    }
}
