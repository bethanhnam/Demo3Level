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
        //PlayerPrefs.SetInt("videoIndex",3);
        var x = PlayerPrefs.GetInt("videoIndex");
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
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            UIManagerNew.Instance.GamePlayLoading.appear();
            DOVirtual.DelayedCall(0.8f, () =>
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
