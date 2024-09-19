
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
        AudioManager.instance.musicSource.Stop();
        AudioManager.instance.sfxSource.Stop();
        Image.transform.localScale = Vector3.zero;
        this.gameObject.SetActive(true);
    }
    public void DisplayLogo()
    {
        AudioManager.instance.PlaySFX("GamePlayLoading");
        GameManagerNew.Instance.Bg.gameObject.SetActive(true);
        Image.transform.DOScale(.8f, 0.3f).OnComplete(() =>
        {
            Image.transform.DOScale(0.6f, 0.2f).OnComplete(() =>
            {
                Image.transform.DOScale(.7f, 0.2f).OnComplete(() =>
                {
                    DOVirtual.DelayedCall(0.2f, () =>
                    {
                        Image.transform.DOScale(0, 0.2f);
                        DOVirtual.DelayedCall(0.2f, () =>
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
        AudioManager.instance.PlaySFX("GamePlayLoading");
    }
    public void Diactive()
    {
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
