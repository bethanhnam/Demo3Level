
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GamePlayLoading : MonoBehaviour
{
    public Animator animator;
    public Image Image;
    public void appear()
    {
        Image.transform.localScale = Vector3.zero;
        this.gameObject.SetActive(true);
    }
    public void DisplayLogo()
    {
        GameManagerNew.Instance.Bg.gameObject.SetActive(true);
        Image.transform.DOScale(.8f, 0.3f).OnComplete(() =>
        {
            Image.transform.DOScale(0.6f, 0.2f).OnComplete(() =>
            {
                Image.transform.DOScale(.7f, 0.2f).OnComplete(() =>
                {
                    DOVirtual.DelayedCall(0.3f, () =>
                    {
                        Image.transform.DOScale(0, 0.4f).OnComplete(() =>
                        {
                            Disapper();
                        });
                    });
                });
            });
        });

    }
    public void Disapper()
    {
        animator.Play("GamePlayLoadingOut", 0, 0);
    }
    public void Diactive()
    {
        this.gameObject.SetActive(false);
    }
}
