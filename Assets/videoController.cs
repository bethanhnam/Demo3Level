using com.adjust.sdk;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public static VideoController instance;
    public VideoPlayer videoPlayer; // Video Player Component

    public int videoIndex = 0;

    public bool canInit = true;
    public bool canCreate = true;

    public List<VideoClip> videoList;

    [System.Serializable]
    public class Frame
    {
        public string sprite;
        public float duration;
    }

    [System.Serializable]
    public class AnimationData
    {
        public List<Frame> frames;
    }

    private AnimationData animationData;

    private void Awake()
    {
        instance = this;
    }
    public void CheckStartVideo()
    {
        var x = PlayerPrefs.GetInt("videoIndex");
        PlayVideo(x, null);
    }
    public void PlayVideo(int videoIndex, Action action)
    {
        Debug.Log(("play video " + videoIndex).ToString());
        this.gameObject.SetActive(true);
        this.videoIndex = videoIndex;
        PlayerPrefs.SetInt("videoIndex",videoIndex);
        videoPlayer.clip = videoList[videoIndex];
        videoPlayer.Play();
        videoPlayer.loopPointReached += LoadingVideo;
        canCreate = true;
    }
    void LoadingVideo(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= LoadingVideo;
        if (this.videoIndex == videoList.Count - 1)
        {
            PlayerPrefs.SetString("HasFinishedStory", "true");
            UIManagerNew.Instance.GamePlayLoading.appear();
            RemoteConfigController.instance.Init();
            DOVirtual.DelayedCall(0.5f, () =>
            {
                GameManagerNew.Instance.InitStartGame();
            });
            if (SaveSystem.instance.powerTicket > 0 || SaveSystem.instance.magicTiket > 0)
            {
                if (PlayerPrefs.GetInt("HasTransfer") == 0)
                {
                    UIManagerNew.Instance.TransferPanel.Appear();
                }
                else
                {
                    DOVirtual.DelayedCall(0.7f, () =>
                    {
                        UIManagerNew.Instance.ButtonMennuManager.Appear();
                    });
                }
            }
            else
            {
                DOVirtual.DelayedCall(0.7f, () =>
                {
                    UIManagerNew.Instance.ButtonMennuManager.Appear();
                });
            }
        }
        else
        {
            UIManagerNew.Instance.VideoLoaingPanel.appear(() =>
            {

                if (canCreate)
                {
                    if (videoIndex == 0)
                    {
                        GameManagerNew.Instance.InitStartStoryPic(0);
                        canCreate = false;
                    }
                    if (videoIndex == 1)
                    {
                        GameManagerNew.Instance.InitStartStoryPic(1);
                        canCreate = false;
                    }
                }
                //PlayerPrefs.SetString("HasFinishedStory","true");
            });
        }
        DOVirtual.DelayedCall(1f, () =>
        {
            this.gameObject.SetActive(false);
        });
    }
    public void NormalInit()
    {
        RemoteConfigController.instance.Init();
        DOVirtual.DelayedCall(0.1f, () =>
        {
            GameManagerNew.Instance.InitStartGame();
        });
        DOVirtual.DelayedCall(0.03f, () =>
        {
            {
                if (SaveSystem.instance.powerTicket > 0 || SaveSystem.instance.magicTiket > 0)
                {
                    if (PlayerPrefs.GetInt("HasTransfer") == 0)
                    {
                        UIManagerNew.Instance.TransferPanel.Appear();
                    }
                }
            }
        });
    }
}
