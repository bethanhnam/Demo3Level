using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class videoLoading : MonoBehaviour
{
    public Animator animator;
    public Image Image;

    public void appear(Action action)
    {
        this.gameObject.SetActive(true);
        Image.DOFade(1, 1).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.1f, () =>
            {
                action?.Invoke();
            });
            Image.DOFade(0, 1).OnComplete(() =>
            {
                
                Deactive();
            });
        });
    }
    public void Deactive()
    {
        this.gameObject.SetActive(false);
    }
}
