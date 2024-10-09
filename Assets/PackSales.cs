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
        DateTime timeRemaining = time;

        TimeSpan timeUntilNextEvent = timeRemaining - now;

        // Convert the TimeSpan to a DateTime starting from a baseline
        DateTime timeUntilNextEventAsDateTime = DateTime.MinValue.Add(timeUntilNextEvent);

        int days = (int)timeUntilNextEvent.TotalDays;
        int hours = timeUntilNextEvent.Hours;
        int minutes = timeUntilNextEvent.Minutes;
        int seconds = timeUntilNextEvent.Seconds;

        if (days < 0 && hours < 0 && minutes < 0 && seconds < 0)
        {
            days = 0;
            hours = 0;
            minutes = 0;
            seconds = 0;
        }
        if (days > 0)
        {
            timeText = $"{(int)days:D2}d{(int)hours:D2}h";
        }
        else
        {
            if (hours > 0)
            {
                timeText = $"{(int)hours:D2}h{(int)minutes:D2}m";
            }
            else
            {
                timeText = $"{(int)minutes:D2}m{(int)seconds:D2}";
                if (seconds < 0)
                {
                    return null;
                }
            }
        }
        return timeText;
    }
}
