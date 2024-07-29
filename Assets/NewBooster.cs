using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBooster : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Animator animator;
    public void Appear()
    {
        canvasGroup.enabled = true;
        canvasGroup.DOFade(1, 1).OnComplete(() =>
        {
            animator.enabled = true;
        });
    }
    public void Disappear()
    {
        canvasGroup.DOFade(0, 1).OnComplete(() =>
        {
            canvasGroup.enabled = false;
            animator.enabled =  false;
        });
    }
    [Button("Showthreshole")]
    public void ShowThreshole()
    {
        if(LevelManagerNew.Instance.stage == 1)
        {
            //UIManagerNew.Instance.ThresholeController.showExtraHole();
        }
    }
}
