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
    //public List<VideoClip> videoActionList;

    private void Awake()
    {
        instance = this;

    }
    private void Start()
    {
        var x = PlayerPrefs.GetInt("videoIndex");
        videoPlayer.clip = videoList[x];
        videoPlayer.Prepare();
    }
    public void CheckStartVideo()
    {
        PlayerPrefs.SetInt("videoIndex", 3);
        var x = PlayerPrefs.GetInt("videoIndex");
        if (x == 0)
        {
            FirebaseAnalyticsControl.Instance.startTutor();
        }
        PlayVideo(x, null);
    }
    public void PlayVideo(int videoIndex, Action action)
    {
        this.gameObject.SetActive(true);
        this.videoIndex = videoIndex;
        PlayerPrefs.SetInt("videoIndex", videoIndex);
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
            FirebaseAnalyticsControl.Instance.completeTutor();
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            UIManagerNew.Instance.GamePlayLoading.appear();
            DOVirtual.DelayedCall(0.5f, () =>
            {
                GameManagerNew.Instance.InitStartGame();
            });
            DOVirtual.DelayedCall(0.35f, () =>
            {
                if (PlayerPrefs.GetInt("FirstStoryBubble") == 0)
                {
                    DOVirtual.DelayedCall(1.3f, () =>
                    {
                        PlayerPrefs.SetInt("FirstStoryBubble", 1);
                        GameManagerNew.Instance.conversationController.CanvasGroup.alpha = 0.8f;
                        GameManagerNew.Instance.conversationController.StartConversation(0,0, "1FirstConver", () =>
                        {
                            GameManagerNew.Instance.conversationController.CanvasGroup.alpha = 0.4f;
                            UIManagerNew.Instance.ButtonMennuManager.Appear();
                        });
                    });
                }
                else
                {
                    if (SaveSystem.instance.powerTicket > 0 || SaveSystem.instance.magicTiket > 0)
                    {
                        if (PlayerPrefs.GetInt("HasTransfer") == 0)
                        {
                            UIManagerNew.Instance.TransferPanel.Appear();
                        }
                        else
                        {
                            DOVirtual.DelayedCall(0.5f, () =>
                            {
                                UIManagerNew.Instance.ButtonMennuManager.Appear();
                            });
                        }
                    }
                    else
                    {
                        DOVirtual.DelayedCall(0.5f, () =>
                        {
                            UIManagerNew.Instance.DailyRWUI.Appear();
                        });
                    }
                }
            });
            DOVirtual.DelayedCall(.7f, () =>
            {
                GameManagerNew.Instance.isStory = false;
                this.gameObject.SetActive(false);
            });
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
                        UIManagerNew.Instance.StoryItem.SetImg(DataLevelStoryPic.instance.listJson[0].itemSpite);
                        UIManagerNew.Instance.StoryItem.SetTargetPos(DataLevelStoryPic.instance.listJson[0].targetTransform);
                        canCreate = false;
                    }
                    if (videoIndex == 1)
                    {
                        GameManagerNew.Instance.InitStartStoryPic(1);
                        UIManagerNew.Instance.StoryItem.SetImg(DataLevelStoryPic.instance.listJson[1].itemSpite);
                        UIManagerNew.Instance.StoryItem.SetTargetPos(DataLevelStoryPic.instance.listJson[1].targetTransform);
                        canCreate = false;
                    }
                    if (videoIndex == 2)
                    {
                        GameManagerNew.Instance.InitStartStoryPic(2);
                        UIManagerNew.Instance.StoryItem.SetImg(DataLevelStoryPic.instance.listJson[2].itemSpite);
                        UIManagerNew.Instance.StoryItem.SetTargetPos(DataLevelStoryPic.instance.listJson[2].targetTransform);
                        canCreate = false;
                    }
                }
                //PlayerPrefs.SetString("HasFinishedStory","true");
            });
        }

    }
}
