using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThresholeController : MonoBehaviour
{
    public GameObject[] thresholes;
    public CanvasGroup CanvasGroup;

    private int thresholeIndex;

    public void Appear(Action action = null)
    {
        this.gameObject.SetActive(true);
        CanvasGroup.enabled = true;
        CanvasGroup.DOFade(1, 1).OnComplete(() =>
        {
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
            action?.Invoke();
        });
    }
    public void Disable()
    {
        thresholes[thresholeIndex].gameObject.SetActive(false);
        CanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.enabled = false;
            this.gameObject.SetActive(false);
        });
    }
    [Button("Show threshole")]
    public void showThreshole(int thresholeIndex)
    {
        this.thresholeIndex = thresholeIndex;
        Appear();
        thresholes[thresholeIndex].gameObject.SetActive(true);
    }
}
