using com.adjust.sdk;
using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public static VideoController instance;
    public VideoPlayer videoPlayer; // Video Player Component

    public int videoIndex = 0;

    public bool canInit = true;
    public bool canCreate = true;
    public bool canSkip = true;

    public List<VideoClip> videoList;
    //public List<VideoClip> videoActionList;

    private void Awake()
    {
        instance = this;

    }
    private void Start()
    {
        canSkip = true;
        var x = PlayerPrefs.GetInt("videoIndex");
        videoPlayer.clip = videoList[x];
        //videoPlayer.Prepare();
    }
    private void Update()
    {
    }
    public void CheckStartVideo()
    {
        //PlayerPrefs.SetInt("videoIndex", 3);

        var x = PlayerPrefs.GetInt("videoIndex");
        PlayVideo(x, null);
        if (x == 0)
        {
            FirebaseAnalyticsControl.Instance.startTutor();
        }
    }
    public void PlayVideo(int videoIndex, Action action)
    {
        this.gameObject.SetActive(true);
        this.videoIndex = videoIndex;
        PlayerPrefs.SetInt("videoIndex", videoIndex);
        videoPlayer.clip = videoList[videoIndex];
        videoPlayer.Play();

        if (videoIndex == 0)
        {
            DOVirtual.DelayedCall(3.7f, () =>
            {
                videoPlayer.Pause();
                UIManagerNew.Instance.VideoLoaingPanel.appear(() =>
                {
                    videoPlayer.aspectRatio = VideoAspectRatio.FitOutside;
                    videoPlayer.Play();
                });
            });
        }
        else
        {
            videoPlayer.aspectRatio = VideoAspectRatio.FitOutside;
        }

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
            facebook.instance.StartFB();
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            UIManagerNew.Instance.GamePlayLoading.appear();
            DOVirtual.DelayedCall(0.65f, () =>
            {
                EventController.instance.LoadData();
                EventController.instance.CheckForWeeklyEvent();
                GameManagerNew.Instance.InitStartGame();
            });
            DOVirtual.DelayedCall(0.35f, () =>
            {
                if (PlayerPrefs.GetInt("FirstStoryBubble") == 0 && LevelManagerNew.Instance.stage == 0)
                {
                    DOVirtual.DelayedCall(2f, () =>
                    {
                        PlayerPrefs.SetInt("FirstStoryBubble", 1);
                        GameManagerNew.Instance.conversationController.CanvasGroup.alpha = 0.8f;
                        UIManagerNew.Instance.BackGroundFooter.ShowBackGroundFooter(true);
                        GameManagerNew.Instance.conversationController.StartConversation(0, 0, "1FirstConver", () =>
                        {
                            UIManagerNew.Instance.BackGroundFooter.DisappearBackGroundFooter();
                            //UIManagerNew.Instance.ButtonMennuManager.isShowingFixing = true;
                            UIManagerNew.Instance.ButtonMennuManager.HideAllUI();
                            UIManagerNew.Instance.ButtonMennuManager.starBar.gameObject.SetActive(true);
                            UIManagerNew.Instance.ButtonMennuManager.sliderBar.gameObject.SetActive(true);
                            UIManagerNew.Instance.ButtonMennuManager.starCanvas.gameObject.SetActive(true);
                            UIManagerNew.Instance.ButtonMennuManager.Appear();
                            AudioManager.instance.PlayMusic("MenuTheme");
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
                    });
                }
                else
                {
                    if (SaveSystem.instance.powerTicket > 0 || SaveSystem.instance.magicTiket > 0)
                    {
                        if (PlayerPrefs.GetInt("HasTransfer") == 0)
                        {
                            AudioManager.instance.PlayMusic("MenuTheme");
                            UIManagerNew.Instance.TransferPanel.Appear();
                        }
                        else
                        {
                            DOVirtual.DelayedCall(0.5f, () =>
                            {
                                AudioManager.instance.PlayMusic("MenuTheme");
                                UIManagerNew.Instance.ButtonMennuManager.Appear();
                            });
                        }
                    }
                    else
                    {
                        DOVirtual.DelayedCall(0.5f, () =>
                        {
                            AudioManager.instance.PlayMusic("MenuTheme");
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
                    canSkip = false;
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
            });
        }
    }
    public void SkipVideo()
    {
        if (canSkip && videoIndex != 4)
        {
            long videoLength = (long)videoList[videoIndex].frameCount;
            videoPlayer.frame = videoLength;
            videoPlayer.loopPointReached += LoadingVideo;
            canCreate = true;
        }
    }
}
