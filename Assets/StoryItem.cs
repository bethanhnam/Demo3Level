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
    int videoIndex;
    public Transform targetPos;
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
        animator.enabled = true;
        this.videoIndex = videoIndex;
        this.gameObject.SetActive(true);
        animator.Play("DisplayItem");
        DOVirtual.DelayedCall(2, () =>
        {
            
            MoveItem();
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
        //DOVirtual.DelayedCall(0.8f, () =>
        //{
        //    UIManagerNew.Instance.VideoLoaingPanel.appear(() =>
        //    {
        //    });
        //});
        itemImage.MoveToFix(this.transform.position, targetPos.position,Vector3.zero,()=>
        {
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Destroy(GameManagerNew.Instance.StoryPic.gameObject, 0.1f);
                GameManagerNew.Instance.videoController.PlayVideoAction(videoIndex);
                Disable();
            });
        });
    }
    public void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
