using Coffee.UIExtensions;
using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MiniGamePlay : MonoBehaviour
{
    public MiniGameMap[] MiniGameMaps;
    public int selectedMinimap;

    public static MiniGamePlay instance;

    public MiniItem[] miniItem;

    public float collectValue;
    public float heartValue;

    public CanvasGroup canvasGroup;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    public void Appear(Action action)
    {
        this.gameObject.SetActive(true);
        canvasGroup.DOFade(1, 0.9f).OnComplete(() =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            action();
        });
    }
    public void Disappear(Action action)
    {
        canvasGroup.DOFade(0, 0.35f).OnComplete(() =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            this.gameObject.SetActive(false);
            action();
        });
    }
    public void SetItem(int miniGameMapIndex, int Maxcollectvalue)
    {
        selectedMinimap = miniGameMapIndex;
        collectValue = 0;
        MiniGameMaps[selectedMinimap].collectSlider.value = collectValue;
        SetMaxValue(Maxcollectvalue);
        for (int i = 0; i < miniItem.Length; i++)
        {
            miniItem[i].SetImage(MiniGameMaps[selectedMinimap].sprite);
            miniItem[i].itemImg.SetNativeSize();
        }
        DOVirtual.DelayedCall(0.2f, () =>
        {
            MiniGameMaps[selectedMinimap].clockFill.StartTimer();
            if (MiniGameMaps[selectedMinimap].HeartSlider != null)
            {
                MiniGameMaps[selectedMinimap].HeartSlider.StartTimer();

            }
            if (MiniGameMaps[selectedMinimap].ghostSkeleton != null)
            {
                MiniGameMaps[selectedMinimap].ghostSkeleton.StartTimer();
            }
        });
    }
    public void ChangeCollectSliderValue()
    {
        AudioManager.instance.PlaySFX("FillUpSlider");
        MiniGameMaps[selectedMinimap].collectSlider.DOValue(MiniGameMaps[selectedMinimap].collectSlider.value + 1, 0.7f).OnComplete(() =>
        {
            if (selectedMinimap == 0)
            {
                MiniGameMaps[selectedMinimap].UIParticle.scale += 5;
            }
            collectValue += 1;
            MiniGameMaps[selectedMinimap].collectText.text = collectValue.ToString();
            if (collectValue >= MiniGameMaps[selectedMinimap].collectSlider.maxValue)
            {
                //win minigame
                MiniGameMaps[selectedMinimap].clockFill.StopTimer();
                if (selectedMinimap == 1)
                {
                    MiniGameMaps[selectedMinimap].itemImage.gameObject.SetActive(false);
                    MiniGameMaps[selectedMinimap].ghostSkeleton.StopTimer();
                }
                if (selectedMinimap == 0)
                {
                    MiniGameMaps[selectedMinimap].HeartSlider.StopTimer();
                }
                Debug.Log("win minigames");
                MiniGameMaps[selectedMinimap].skeleton.AnimationState.SetAnimation(0, "happy", false);
                DOVirtual.DelayedCall(0.3f, () =>
                {
                    MiniGameMaps[selectedMinimap].skeleton.AnimationState.SetAnimation(0, "idle_happy", true);
                    UIManagerNew.Instance.WinMiniGamePanel.Appear();
                });
            }
        });

    }
    public void SetMaxValue(int numOfItem)
    {
        MiniGameMaps[selectedMinimap].collectSlider.maxValue = numOfItem;
        MiniGameMaps[selectedMinimap].maxCollectText.text = numOfItem.ToString();
    }
    public void StopMinigame()
    {
        MiniGameMaps[selectedMinimap].clockFill.StopTimer();

        if (MiniGameMaps[selectedMinimap].HeartSlider != null)
        {
            MiniGameMaps[selectedMinimap].HeartSlider.StopTimer();

        }
        if (MiniGameMaps[selectedMinimap].ghostSkeleton != null)
        {
            MiniGameMaps[selectedMinimap].ghostSkeleton.StopTimer();
        }
    }
}
