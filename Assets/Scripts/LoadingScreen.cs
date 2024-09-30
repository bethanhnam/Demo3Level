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

    public bool hasCallAction = false;

    public void LoadScene()
    {
        sliders[0].value = 0;
        DOVirtual.Float(0, .3f, 2f, (x) =>
        {
            sliders[0].value = x;
        }).OnComplete(()
        =>
        {
            SceneManager.LoadScene("LoadDataScene", LoadSceneMode.Additive);
        });
    }

    public void LoadSceneDone()
    {
        while (!LevelManagerNew.Instance.hasLoadDone && !DataLevelManager.Instance.hasLoadDone)
        {
            continue;
        }

        DOVirtual.DelayedCall(1, () =>
        {
            SceneManager.LoadSceneAsync("GamePlay");
        });

        DOVirtual.Float(.3f, .9f, 2f, (x) =>
        {
            sliders[0].value = x;
        }).OnComplete(()
        =>
        {
            hasCallAction = true;
            RemoteConfigController.instance.Init();
            DOVirtual.DelayedCall(1f, () =>
            {
                if (!HasFinishedStory())
                {
                    LoadingManager.instance.PlayVideo();
                    this.gameObject.SetActive(false);
                }
                else
                {
                    LoadingManager.instance.NormalInitGame();
                    this.gameObject.SetActive(false);
                }
            });
        });
    }

    private void Start()
    {
        instance = this;
        //test
        PlayerPrefs.SetString("HasFinishedStory", "true");

        loadingScreen.SetActive(true);

        DontDestroyOnLoad(this.gameObject);
        LoadScene();
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
