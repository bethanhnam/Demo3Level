using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StoryItem : MonoBehaviour
{
    public static StoryItem Instance;
    public Animator animator;
    public ItemStoryImage itemImage;
    public GameObject heartBreak;
    public GameObject chopingAxe;
    int videoIndex;
    public Transform targetPos;
    float time;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }
    public void SetImg(Sprite sprite)
    {
        if (itemImage != null)
        {
            itemImage.GetComponent<Image>().sprite = sprite;
        }
    }
    public void SetTargetPos(Transform transform)
    {
        targetPos = transform;
    }
    public void DisplayItem(int videoIndex, Action action)
    {
        itemImage.gameObject.SetActive(true);
        itemImage.transform.position = Vector3.zero;
        itemImage.transform.localScale = new Vector3(2, 2, 1);
        animator.enabled = true;
        this.videoIndex = videoIndex;
        this.gameObject.SetActive(true);
        animator.Play("DisplayItem");
        AudioManager.instance.PlaySFX("GetReward");
        if (videoIndex == 4)
        {
            GameManagerNew.Instance.videoController.PlayVideo(videoIndex, null);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                ChopingWood();
            });
            Destroy(GameManagerNew.Instance.StoryPic.gameObject);

        }
        if (videoIndex == 2)
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                MoveItem();
                time = 1f;
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    MoveItem1();
                });
            });
            Destroy(GameManagerNew.Instance.StoryPic.gameObject);

            DOVirtual.DelayedCall(.5f, () =>
            {
                GameManagerNew.Instance.videoController.PlayVideo(videoIndex, null);
            });

        }
        if (videoIndex == 3)
        {
            DOVirtual.DelayedCall(.5f, () =>
            {
                BreakHeart();
                Destroy(GameManagerNew.Instance.StoryPic.gameObject,0.1f);
                DOVirtual.DelayedCall(.3f, () =>
                {
                    GameManagerNew.Instance.videoController.PlayVideo(videoIndex, null);

                });
            });
        }
    }
    public void ChopingWood()
    {
        itemImage.gameObject.SetActive(false);
        chopingAxe.gameObject.SetActive(true);
        animator.Play("DisappearItem");
        DOVirtual.DelayedCall(1f, () =>
        {
            chopingAxe.GetComponent<Animator>().enabled = true;
            DOVirtual.DelayedCall(1f, () =>
            {
                chopingAxe.gameObject.SetActive(false);
                Disable();
            });
        });

    }
    public void BreakHeart()
    {
        itemImage.gameObject.SetActive(false);
        heartBreak.gameObject.SetActive(true);
        AudioManager.instance.PlaySFX("HeartBreak");
        heartBreak.transform.GetChild(0).DORotate(new Vector3(0, 0, 30), 0.5f);
        heartBreak.transform.GetChild(1).DORotate(new Vector3(0, 0, -30), 0.5f).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                animator.Play("DisappearItem");
                Disable();
                heartBreak.gameObject.SetActive(false);
            });
        });
    }
    public void MoveItem()
    {
        animator.Play("DisappearItem");
    }
    public void MoveItem1()
    {
        animator.enabled = false;
        itemImage.transform.localScale = new Vector3(2, 2, 1);
        itemImage.MoveToFix(time, this.transform.position, targetPos.position, Vector3.zero, () =>
        {
            DOVirtual.DelayedCall(0.3f, () =>
             {
                 Disable();
             });
        });
    }
    public void Disable()
    {
        this.gameObject.SetActive(false);
        VideoController.instance.canSkip = true;
    }
    public void SetNative()
    {
        itemImage.GetComponent<Image>().SetNativeSize();
    }
   
}
