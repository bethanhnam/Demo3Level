using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailMiniGame : MonoBehaviour
{
    public Image failImage;
    public CanvasGroup canvasGroup;

    public void Appear(Action action)
    {
        canvasGroup.alpha = 0;
        failImage.transform.localScale = Vector3.one * 3;
        canvasGroup.DOFade(1, 1);
        failImage.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            DOVirtual.DelayedCall(2, () =>
            {
                Disapper(action);
            });
        });
    }
    public void Disapper(Action action)
    {
        canvasGroup.alpha = 1;
        failImage.transform.localScale = Vector3.one;
        canvasGroup.DOFade(0, 1).OnComplete(() =>
        {
            action();
            failImage.transform.DOScale(Vector3.one * 3, 1f).SetEase(Ease.InElastic);
        });
       
    }
}
