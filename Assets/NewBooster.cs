using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBooster : MonoBehaviour
{
    public Image Image;
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
    public void ShowThreshole()
    {
        if(LevelManagerNew.Instance.stage == 1)
        {
            UIManagerNew.Instance.ThresholeController.showThreshole("extrahole");
        }
        if (LevelManagerNew.Instance.stage == 3)
        {
            UIManagerNew.Instance.ThresholeController.showThreshole("unscrew");
        }
    }
}
