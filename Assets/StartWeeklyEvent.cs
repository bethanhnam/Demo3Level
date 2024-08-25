using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartWeeklyEvent : MonoBehaviour
{
    public Animator Animator;

    public Image bigCollectionImg;
    public Image smallCollectionImg;
    public Button continueButton;

    public void Appear()
    {
        UIManagerNew.Instance.BlockPicCanvas.SetActive(true);
        this.gameObject.SetActive(true);
        Animator.enabled = true;
        Animator.Play("Appear");
    }

    public void Active()
    {
        UIManagerNew.Instance.BlockPicCanvas.SetActive(false);
        Animator.enabled = false;
    }


    public void Disappear()
    {
        UIManagerNew.Instance.BlockPicCanvas.SetActive(true);
        Animator.enabled = true;
        Animator.Play("Disappear");
    }

    public void Deactive()
    {
        this.gameObject.SetActive(false);
        Animator.enabled = false;
        DOVirtual.DelayedCall(0.7f, () =>
        {
            UIManagerNew.Instance.WeeklyEventPanel.Appear();
        });
    }

    public void SetCollectImg(Sprite sprite)
    {
        bigCollectionImg.sprite = sprite;
        smallCollectionImg.sprite = sprite;
    }

}
