using DG.Tweening;
using Firebase;
using Firebase.Extensions;
using Newtonsoft.Json;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
            if (operation.progress >= 0.9f && sliders[0].value == 0.9f )
            {
                if (isFirebaseInitialized)
                {
                    Debug.Log("isFirebaseInitialized" + " true");
                }
                else
                {
                    Debug.Log("isFirebaseInitialized" + " false");
                }
                operation.allowSceneActivation = true;
                RemoteConfigController.instance.Init();
                yield return new WaitForSecondsRealtime(1f);
                if (!HasFinishedStory())
                {
                    if (GameManagerNew.Instance != null && GameManagerNew.Instance.videoController != null)
                    {
                        GameManagerNew.Instance.videoController.gameObject.SetActive(true);
                    }
                    float maxWaitTime = 20f; // Thời gian chờ tối đa là 20 giây
                    float currentWaitTime = 0f;

                    while (!GameManagerNew.Instance.videoController.videoPlayer.isPrepared && currentWaitTime < maxWaitTime)
                    {
                        currentWaitTime += Time.unscaledDeltaTime; // Tăng thời gian chờ
                        yield return null;
                    }

                    AudioManager.instance.PlayMusic("story");
                    GameManagerNew.Instance.PlayVideo();
                    Destroy(this.gameObject, 1f);
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
                if (PlayerPrefs.GetInt("Hasfixed") == 0 && LevelManagerNew.Instance.stage == 0)
                {
                    UIManagerNew.Instance.BackGroundFooter.ShowBackGroundFooter(true);
                    GameManagerNew.Instance.conversationController.StartConversation(0, 0, "1FirstConver", () =>
                    {
                        UIManagerNew.Instance.BackGroundFooter.DisappearBackGroundFooter();
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
                            GameManagerNew.Instance.PictureUIManager.windowObj.GetComponent<SkeletonGraphic>().material = UIManagerNew.Instance.HighLightObj;
                            GameManagerNew.Instance.PictureUIManager.windowBtnObj.transform.localScale = Vector3.zero;
                            GameManagerNew.Instance.PictureUIManager.windowBtnObj.SetActive(true);
                            GameManagerNew.Instance.PictureUIManager.windowBtnObj.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
                            {
                                UIManagerNew.Instance.ShowPoiner(GameManagerNew.Instance.PictureUIManager.windowBtnObj.transform);
                            });
                        }
                    });
                }
                else if(PlayerPrefs.GetInt("Hasfixed") == 1 && LevelManagerNew.Instance.stage == 0)
                {
                    UIManagerNew.Instance.ButtonMennuManager.HideAllUI();
                    UIManagerNew.Instance.ButtonMennuManager.starBar.gameObject.SetActive(true);
                    UIManagerNew.Instance.ButtonMennuManager.sliderBar.gameObject.SetActive(true);
                    UIManagerNew.Instance.ButtonMennuManager.starCanvas.gameObject.SetActive(true);
                    UIManagerNew.Instance.ButtonMennuManager.Appear();
                    AudioManager.instance.PlayMusic("MenuTheme");
                }
            }
        });

        cv.DOFade(0, 0.3f).OnComplete(() =>
         {
             if (SaveSystem.instance.powerTicket > 0 || SaveSystem.instance.magicTiket > 0 && PlayerPrefs.GetInt("Hasfixed") == 1 && LevelManagerNew.Instance.stage > 1)
             {
                 if (PlayerPrefs.GetInt("HasTransfer") == 0)
                 {
                     UIManagerNew.Instance.TransferPanel.Appear();
                 }
             }
             if (PlayerPrefs.GetInt("Hasfixed") == 1 && LevelManagerNew.Instance.stage != 0)
             {
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

    public bool FirstStoryBubble()
    {
        var data = PlayerPrefs.GetString("FirstStoryBubble", "false");
        bool HasFinishedStory = JsonConvert.DeserializeObject<bool>(data);
        return HasFinishedStory;
    }
    void SaveImage(byte[] imageData)
    {
        string path = Path.Combine(Application.persistentDataPath, "1v");
        File.WriteAllBytes(path, imageData);
        Debug.Log("Ảnh đã được lưu tại: " + path);
    }
}
