using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackSales : MonoBehaviour
{
    public Animator animator;
    public CanvasGroup group;
    public Animator[] items;

    private int appear = Animator.StringToHash("PackSaleHalloWeen");
    private int Disappear = Animator.StringToHash("PackSaleHalloWeenDisappear");

    [Button("Appear")]
    public void Appear()
    {
        if (this.gameObject.activeSelf == false)
        {
            this.gameObject.SetActive(true);
        }
        animator.Play(appear);
        DOVirtual.DelayedCall(1, () =>
        {
            PlayItemIdleAnim(0);
        });
    }
    public void ActiveCanvasGroup()
    {
        group.blocksRaycasts = true;
    }
    public void DeactiveCanvasGroup()
    {
        group.blocksRaycasts = false;
    }

    public void PlayItemIdleAnim(int i)
    {
        if (i < items.Length)
        {
            items[i].enabled = true;
            DOVirtual.DelayedCall(1.1f, () =>
            {
                //items[i].enabled = false;
                i++;
                PlayItemIdleAnim(i);
            });
        }
        else
        {
            i = 0;
            PlayItemIdleAnim(i);
        }
    }
}
