using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PackSales : MonoBehaviour
{
    public Animator animator;
    public CanvasGroup group;
    public Animator[] items;

    private int appear = Animator.StringToHash("PackSaleHalloWeen");
    private int disappear = Animator.StringToHash("PackSaleHalloWeenDisappear");

    public void Appear()
    {
        if (this.gameObject.activeSelf == false)
        {
            this.gameObject.SetActive(true);
        }
        animator.Play(appear);
        DOVirtual.DelayedCall(1, () =>
        {
            for (int i = 0; i < items.Length; i++)
            {
                PlayItemIdleAnim(1.1f * i, i, "HalloWeenPackItem", null);
            }
        });
    }

    public void Disappear()
    {
        DeactiveCanvasGroup();
        animator.Play(disappear);
        UIManagerNew.Instance.ButtonMennuManager.Appear();
    }
    public void Deative()
    {
        this.gameObject.SetActive(false);
    }
    public void ActiveCanvasGroup()
    {
        group.blocksRaycasts = true;
    }
    public void DeactiveCanvasGroup()
    {
        group.blocksRaycasts = false;
    }
    public void PlayItemIdleAnim(float delayTime, int i, String t, Action action)
    {
        if (i < items.Length)
        {
            DOVirtual.DelayedCall(delayTime, () =>
            {
                items[i].enabled = true;
                items[i].Play(t);
            });
        }
        else
        {
            if (action != null)
            {
                DOVirtual.DelayedCall(3, () =>
                {
                    action();
                });
            }
        }
    }

    public string SetText(DateTime time)
    {
        string timeText = string.Empty;

        DateTime now = DateTime.Now;
        TimeSpan timeUntilNextEvent = time - now;

        // Ensure no negative values are displayed
        if (timeUntilNextEvent < TimeSpan.Zero)
        {
            return "00m00s"; // Default to "00m00s" if time is in the past
        }

        int days = (int)timeUntilNextEvent.TotalDays;
        int hours = timeUntilNextEvent.Hours;
        int minutes = timeUntilNextEvent.Minutes;
        int seconds = timeUntilNextEvent.Seconds;

        // Display days and hours if days are greater than 0
        if (days > 0)
        {
            timeText = $"{days:D2}d{hours:D2}h";
        }
        else if (hours > 0)
        {
            timeText = $"{hours:D2}h{minutes:D2}m";
        }
        else
        {
            timeText = $"{minutes:D2}m{seconds:D2}s";
        }

        return timeText;
    }
}
