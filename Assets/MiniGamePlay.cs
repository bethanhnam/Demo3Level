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
        MiniGameMaps[selectedMinimap].ghostSkeleton.skeletonGraphic.gameObject.SetActive(false);
        collectValue = 0;
        MiniGameMaps[selectedMinimap].collectSlider.value = collectValue;
        SetMaxValue(Maxcollectvalue);
        for (int i = 0; i < miniItem.Length; i++)
        {
            miniItem[i].SetImage(MiniGameMaps[selectedMinimap].sprite);
            miniItem[i].itemImg.SetNativeSize();
        }
        DOVirtual.DelayedCall(0.5f, () =>
        {
            MiniGameMaps[selectedMinimap].collectText.text = collectValue.ToString();
            if (selectedMinimap == 0)
            {
                MiniGameMaps[selectedMinimap].ghostSkeleton.gameObject.SetActive(true);
                MiniGameMaps[selectedMinimap].ghostSkeleton.RestartTimer();
                MiniGameMaps[selectedMinimap].characterStepBack.RestartTimer();

                MiniGameMaps[selectedMinimap].skeleton.gameObject.SetActive(true);
                MiniGameMaps[selectedMinimap].ghostSkeleton.hasReached = false;

                MiniGameMaps[selectedMinimap].itemImage.gameObject.SetActive(true);

                DOVirtual.DelayedCall(0.3f, () =>
                {
                    MiniGameMaps[selectedMinimap].characterStepBack.StartTimer();
                    
                    MiniGameMaps[selectedMinimap].ghostSkeleton.StartTimer();
                });
            }
            if (selectedMinimap == 1)
            {
                var particleSystem = MiniGameMaps[selectedMinimap].UIParticle;
                if (particleSystem != null)
                {
                    var mainModule = particleSystem.main;
                    mainModule.startSize = 0.6f;
                }
                MiniGameMaps[selectedMinimap].HeartSlider.RestartTimer();
                MiniGameMaps[selectedMinimap].skeleton.AnimationState.SetAnimation(0, "idle_sad", true);

                DOVirtual.DelayedCall(0.3f, () =>
                {
                    MiniGameMaps[selectedMinimap].HeartSlider.StartTimer();
                });
            }
            MiniGameMaps[selectedMinimap].clockFill.RestartTimer();
            MiniGameMaps[selectedMinimap].clockFill.StartTimer();
        });
    }
    public void ChangeCollectSliderValue()
    {
        AudioManager.instance.PlaySFX("FillUpSlider");
        MiniGameMaps[selectedMinimap].collectSlider.value = collectValue;
        MiniGameMaps[selectedMinimap].collectSlider.DOValue(MiniGameMaps[selectedMinimap].collectSlider.value + 1, 0.3f).OnComplete(() =>
        {
            if (selectedMinimap == 1)
            {
                // Tăng kích thước hạt
                var particleSystem = MiniGameMaps[selectedMinimap].UIParticle;
                if (particleSystem != null)
                {
                    var mainModule = particleSystem.main;
                    mainModule.startSize = new ParticleSystem.MinMaxCurve(mainModule.startSize.constant + .5f);
                }
                else
                {
                    Debug.LogError("ParticleSystem not found on selected minimap.");
                }
            }
            collectValue += 1;
            MiniGameMaps[selectedMinimap].collectText.text = collectValue.ToString();
            if (collectValue >= MiniGameMaps[selectedMinimap].collectSlider.maxValue)
            {
                //win minigame
                MiniGameMaps[selectedMinimap].hasDone = true;
                MiniGameMaps[selectedMinimap].clockFill.StopTimer();
                if (selectedMinimap == 0)
                {
                    MiniGameMaps[selectedMinimap].itemImage.gameObject.SetActive(false);
                    MiniGameMaps[selectedMinimap].ghostSkeleton.StopTimer();
                }
                if (selectedMinimap == 1)
                {
                    MiniGameMaps[selectedMinimap].HeartSlider.StopTimer();
                }
                Debug.Log("win minigames");
                if (selectedMinimap == 1)
                {
                    MiniGameMaps[selectedMinimap].skeleton.AnimationState.SetAnimation(0, "happy", false);
                    DOVirtual.DelayedCall(0.3f, () =>
                    {
                        MiniGameMaps[selectedMinimap].skeleton.AnimationState.SetAnimation(0, "idle_happy", true);
                        UIManagerNew.Instance.WinMiniGamePanel.Appear();
                    });
                }
                if (selectedMinimap == 0)
                {
                    MiniGameMaps[selectedMinimap].characterStepBack.GoToWin(() =>
                    {
                        UIManagerNew.Instance.WinMiniGamePanel.Appear();
                    });
                    MiniGameMaps[selectedMinimap].ghostSkeleton.gameObject.SetActive(false);
                }
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
    public void CloseMinigame()
    {
        UIManagerNew.Instance.GamePlayLoading.appear();
        DOVirtual.DelayedCall(0.3f, () =>
        {
            if (MiniGameMaps[selectedMinimap].ghostSkeleton != null && MiniGameMaps[selectedMinimap].ghostSkeleton.gameObject.activeSelf)
            {
                MiniGameMaps[selectedMinimap].ghostSkeleton.gameObject.SetActive(false);
            }
            Disappear(() =>
            {
                MiniGameMaps[selectedMinimap].gameObject.SetActive(false);
                DOVirtual.DelayedCall(1f, () =>
                {
                    if (MiniGameStage.Instance != null)
                    {
                        MiniGameStage.Instance.Close(true);
                    }
                    UIManagerNew.Instance.ButtonMennuManager.Appear();
                });
            });
        });
    }
    public void ReplayMinigame()
    {
        UIManagerNew.Instance.GamePlayLoading.appear();
        if (UIManagerNew.Instance.ButtonMennuManager.gameObject.activeSelf) {
            UIManagerNew.Instance.ButtonMennuManager.Close();
        }
        DOVirtual.DelayedCall(0.5f, () =>
        {
            if (UIManagerNew.Instance.StartMiniGamePanel.gameObject.activeSelf)
            {
                UIManagerNew.Instance.StartMiniGamePanel.Close();
            }
            if (UIManagerNew.Instance.SkipPanel.gameObject.activeSelf)
            {
                UIManagerNew.Instance.SkipPanel.Close();
            }
        });
        GameManagerNew.Instance.ReCreateMiniGame(UIManagerNew.Instance.ButtonMennuManager.levelMinigame);
    }
}
