using Coffee.UIExtensions;
using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    public Notice notice;
    public Button replayButton;
    public Button closeBbutton;

    public AlertScreen alertImage;
    public FailMiniGame FailMiniGame;

    public Image noticeImage;

    public CanvasGroup canvasGroup;

    public bool isReplay = false;

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

    public void ShowNotice(bool status)
    {
        if (status)
        {
            notice.canAppear = true;
            notice.NoticeAppear();
        }
        else
        {
            notice.canAppear = false;
            notice.NoticeDisappear();
        }
    }

    public void SetItem(int miniGameMapIndex, int Maxcollectvalue)
    {
        selectedMinimap = miniGameMapIndex;
        UIManagerNew.Instance.MiniGamePlay.closeBbutton.gameObject.SetActive(true);
        UIManagerNew.Instance.MiniGamePlay.replayButton.gameObject.SetActive(false);
        if (MiniGameMaps[selectedMinimap].minigame1 == MiniGameMap.Minigame.ScaryHouse)
        {
            ChangeDefaultPos();
            MiniGameMaps[selectedMinimap].ghostSkeleton.dark.gameObject.SetActive(false);
            MiniGameMaps[selectedMinimap].ghostSkeleton.skeletonGraphic.gameObject.SetActive(false);
        }
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
            if (MiniGameMaps[selectedMinimap].minigame1 == MiniGameMap.Minigame.ScaryHouse)
            {
                MiniGameMaps[selectedMinimap].ghostSkeleton.gameObject.SetActive(true);
                MiniGameMaps[selectedMinimap].ghostSkeleton.RestartTimer();
                MiniGameMaps[selectedMinimap].characterStepBack.RestartTimer();

                MiniGameMaps[selectedMinimap].skeleton.gameObject.SetActive(true);
                MiniGameMaps[selectedMinimap].ghostSkeleton.hasReached = false;

                MiniGameMaps[selectedMinimap].itemImage.gameObject.SetActive(true);

                DOVirtual.DelayedCall(0.3f, () =>
                {
                    if (!isReplay)
                    {
                        UIManagerNew.Instance.BackGroundFooter.ShowBackGroundFooter(false);
                        MiniGameStage.Instance.canInteract = false;
                        ConversationController.instance.StartConversation(0, 15, "startminigame1", () =>
                        {
                            MiniGameStage.Instance.canInteract = true;
                            UIManagerNew.Instance.BackGroundFooter.DisappearBackGroundFooter();
                            MiniGameMaps[selectedMinimap].clockFill.RestartTimer();
                            MiniGameMaps[selectedMinimap].characterStepBack.StartTimer();
                            AudioManager.instance.PlayMusic("ScraryHouse");

                            MiniGameMaps[selectedMinimap].ghostSkeleton.StartTimer();
                            MiniGameMaps[selectedMinimap].clockFill.StartTimer();
                        }, false);
                    }
                    else
                    {
                        MiniGameMaps[selectedMinimap].clockFill.RestartTimer();
                        MiniGameMaps[selectedMinimap].characterStepBack.StartTimer();
                        AudioManager.instance.PlayMusic("ScraryHouse");
                        MiniGameMaps[selectedMinimap].ghostSkeleton.StartTimer();
                        MiniGameMaps[selectedMinimap].clockFill.StartTimer();
                    }
                });
            }
            if (MiniGameMaps[selectedMinimap].minigame1 == MiniGameMap.Minigame.Babycold)
            {
                MiniGameMaps[selectedMinimap].floor.DOFade(0, 0f);
                MiniGameMaps[selectedMinimap].snowParticle.Play();
                MiniGameMaps[selectedMinimap].snowParticle1.Play();
                MiniGameMaps[selectedMinimap].windParticle.Play();
                var particleSystem = MiniGameMaps[selectedMinimap].UIParticle;
                if (particleSystem != null)
                {
                    var mainModule = particleSystem.main;
                    mainModule.startSize = 0.6f;
                }
                MiniGameMaps[selectedMinimap].HeartSlider.RestartTimer();
                MiniGameMaps[selectedMinimap].skeleton1.AnimationState.SetAnimation(0, "tremble", true);

                DOVirtual.DelayedCall(0.3f, () =>     
                {
                    AudioManager.instance.PlayMusic("BabyClodTheme");
                    MiniGameMaps[selectedMinimap].frozenImg.color = new Color(1, 1, 1, 0);
                    
                    MiniGameMaps[selectedMinimap].windParticle.gameObject.SetActive(true);
                    DOVirtual.DelayedCall(0.3f, () =>
                    {
                        MiniGameMaps[selectedMinimap].snowParticle1.gameObject.SetActive(true);
                    });
                    DOVirtual.DelayedCall(0.3f, () =>
                    {
                        MiniGameMaps[selectedMinimap].snowParticle.gameObject.SetActive(true);
                    });
                    MiniGameMaps[selectedMinimap].HeartSlider.StartTimer();
                    MiniGameMaps[selectedMinimap].clockFill.RestartTimer();
                    MiniGameMaps[selectedMinimap].clockFill.StartTimer();
                });
            }

        });
    }
    public void ChangeCollectSliderValue()
    {
        AudioManager.instance.PlaySFX("FillUpSlider");
        MiniGameMaps[selectedMinimap].collectSlider.DOValue(collectValue + 1, 0.05f).OnComplete(() =>
        {
            collectValue += 1;
            if (MiniGameMaps[selectedMinimap].minigame1 == MiniGameMap.Minigame.Babycold)
            {
                // Tăng kích thước hạt
                var particleSystem = MiniGameMaps[selectedMinimap].UIParticle;
                if (particleSystem != null)
                {
                    var mainModule = particleSystem.main;
                    mainModule.startSize = 3;
                }
                else
                {
                    Debug.LogError("ParticleSystem not found on selected minimap.");
                }
                MiniGameMaps[selectedMinimap].skeleton1.AnimationState.SetAnimation(0, "win", false);
                DOVirtual.DelayedCall(0.4f, () =>
                {
                    MiniGameMaps[selectedMinimap].skeleton1.AnimationState.SetAnimation(0, "idle_win", true);
                });
                MiniGameMaps[selectedMinimap].floor.DOFade(1, 1f);
            }
            MiniGameMaps[selectedMinimap].collectText.text = collectValue.ToString();
            if (collectValue >= MiniGameMaps[selectedMinimap].collectSlider.maxValue)
            {
                //win minigame
                if (selectedMinimap == 0)
                {
                    PlayerPrefs.SetInt("DoneMini1", 1);
                }
                else if (selectedMinimap == 1)
                {
                    PlayerPrefs.SetInt("DoneMini2", 1);
                }
                MiniGameMaps[selectedMinimap].hasDone = true;
                MiniGameMaps[selectedMinimap].clockFill.StopTimer();

                Debug.Log("win minigames");
                if (MiniGameMaps[selectedMinimap].minigame1 == MiniGameMap.Minigame.Babycold)
                {
                    Color newColor = new Color(1, 1, 1, 1);
                    MiniGameMaps[selectedMinimap].floor.gameObject.SetActive(false);
                    MiniGameMaps[selectedMinimap].snowParticle.Stop();
                    MiniGameMaps[selectedMinimap].snowParticle1.Stop();
                    MiniGameMaps[selectedMinimap].windParticle.Stop();
                    MiniGameMaps[selectedMinimap].completeImg.DOColor(newColor, 1f);
                    MiniGameMaps[selectedMinimap].HeartSlider.StopTimer();
                    MiniGameMaps[selectedMinimap].skeleton1.AnimationState.SetAnimation(0, "win", false);
                    DOVirtual.DelayedCall(0.4f, () =>
                    {
                        MiniGameMaps[selectedMinimap].skeleton1.AnimationState.SetAnimation(0, "idle_win", true);
                    });
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        MiniGameStage.Instance.canInteract = false;
                        UIManagerNew.Instance.WinMiniGamePanel.Appear();
                    });
                }
                if (MiniGameMaps[selectedMinimap].minigame1 == MiniGameMap.Minigame.ScaryHouse)
                {
                    MiniGameMaps[selectedMinimap].itemImage.gameObject.SetActive(false);
                    MiniGameMaps[selectedMinimap].ghostSkeleton.StopTimer();
                    MiniGameMaps[selectedMinimap].gateImage.gameObject.SetActive(true);
                    MiniGameMaps[selectedMinimap].characterStepBack.GoToWin(() =>
                    {
                        MiniGameStage.Instance.canInteract = false;
                        UIManagerNew.Instance.WinMiniGamePanel.Appear();
                    });
                    MiniGameMaps[selectedMinimap].ghostSkeleton.MoveGhost(false);
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
        if (MiniGameStage.Instance.numOfIronPlates <= 0)
        {
            return;
        }
        else
        {
            MiniGameMaps[selectedMinimap].clockFill.RestartTimer();
            UIManagerNew.Instance.GamePlayLoading.appear();
            alertImage.DisableAlert();
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
                        AudioManager.instance.PlayMusic("MenuTheme");
                        UIManagerNew.Instance.ButtonMennuManager.Appear();
                    });
                });
            });
        }
    }
    public void ReplayMinigame()
    {
        int levelminigame1 =0;
        if (LevelManagerNew.Instance.stage == 3)
        {
            levelminigame1 = 0;
        }
        if (LevelManagerNew.Instance.stage == 7)
        {
            levelminigame1 = 1;
        }
        isReplay = true;
        UIManagerNew.Instance.GamePlayLoading.appear();
        if (UIManagerNew.Instance.ButtonMennuManager.gameObject.activeSelf)
        {
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
            if (UIManagerNew.Instance.MiniGamePlay.alertImage.gameObject.activeSelf)
            {
                UIManagerNew.Instance.MiniGamePlay.alertImage.DisableAlert();
            }
            MiniGameMaps[selectedMinimap].clockFill.RestartTimer();
        });
        GameManagerNew.Instance.ReCreateMiniGame(levelminigame1);
    }

    public void StopAllActionInMiniGame()
    {
        if (MiniGameMaps[selectedMinimap].minigame1 == MiniGameMap.Minigame.ScaryHouse)
        {
            MiniGameMaps[selectedMinimap].ghostSkeleton.StopTimer();
            MiniGameMaps[selectedMinimap].characterStepBack.GotCatched();
            MiniGameMaps[selectedMinimap].characterStepBack.StopTimer();
        }
    }

    public void ChangeDefaultPos()
    {
        if (MiniGameMaps[selectedMinimap].minigame1 == MiniGameMap.Minigame.ScaryHouse)
        {
            Debug.Log(MiniGameMaps[selectedMinimap].minX.transform.position.x);
            Debug.Log(MiniGameMaps[selectedMinimap].characterStepBack.transform.position);
            Debug.Log(MiniGameMaps[selectedMinimap].characterStepBack.transform.position);

            MiniGameMaps[selectedMinimap].ghostSkeleton.defaultTransform = new Vector3(MiniGameMaps[selectedMinimap].maxX.transform.position.x, 0, 1);

            MiniGameMaps[selectedMinimap].characterStepBack.winTargetTransform = new Vector3(MiniGameMaps[selectedMinimap].minX.transform.position.x, 0, 1);

            MiniGameMaps[selectedMinimap].characterStepBack.RestartTimer();
            MiniGameMaps[selectedMinimap].ghostSkeleton.RestartTimer();
        }
    }

    public void ChangeForNotAction()
    {
        if (MiniGameMaps[selectedMinimap].minigame1 == MiniGameMap.Minigame.Babycold)
        {
            // Tăng kích thước hạt
            var particleSystem = MiniGameMaps[selectedMinimap].UIParticle;
            if (particleSystem != null)
            {
                var mainModule = particleSystem.main;
                mainModule.startSize = new ParticleSystem.MinMaxCurve(0.6f, 0.9f);
            }
            else
            {
                Debug.LogError("ParticleSystem not found on selected minimap.");
            }
            MiniGameMaps[selectedMinimap].floor.DOFade(0, 0.5f);
            MiniGameMaps[selectedMinimap].skeleton1.AnimationState.SetAnimation(0, "tremble", true);
        }
    }
}
