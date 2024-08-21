using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertScreen : MonoBehaviour
{
    public Image alertImage;
    public Animator animator;
    public CanvasGroup canvasGroup;

    public void ShowAlert()
    {
        this.gameObject.SetActive(true);
        animator.enabled = true;
        canvasGroup.enabled = true;
    }
    public void DisableAlert()
    {
        animator.enabled = false;
        canvasGroup.alpha = 0;
        this.gameObject.SetActive(false);
    }
}
