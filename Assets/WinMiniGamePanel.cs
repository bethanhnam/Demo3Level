using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinMiniGamePanel : MonoBehaviour
{
    [SerializeField]
    private Animator animButton;
    [SerializeField]
    private CanvasGroup cvButton;

    private int appearButton = Animator.StringToHash("appear");
    private int disappearButton = Animator.StringToHash("Disappear");

    public void Appear()
    {
        AudioManager.instance.musicSource.Stop();
        UIManagerNew.Instance.BlockPicCanvas.SetActive(true);
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        animButton.enabled = true;
        cvButton.blocksRaycasts = false;
        animButton.Play(appearButton, 0, 0);
        AudioManager.instance.PlaySFX("OpenChest");
    }

    public void Close()
    {
        UIManagerNew.Instance.BlockPicCanvas.SetActive(true);
        cvButton.blocksRaycasts = false;
        animButton.Play(disappearButton);
    }

    public void Deactive()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        UIManagerNew.Instance.BlockPicCanvas.SetActive(false);
    }

    public void ActiveCVGroup()
    {
        UIManagerNew.Instance.BlockPicCanvas.SetActive(false);
        if (!cvButton.blocksRaycasts)
        {
            cvButton.blocksRaycasts = true;
        }
    }
    public void BackToMenu()
    {
        AudioManager.instance.PlaySFX("Coins");
        if (MiniGamePlay.instance.MiniGameMaps[MiniGamePlay.instance.selectedMinimap].minigame1 == MiniGameMap.Minigame.Babycold)
        {
            MiniGamePlay.instance.MiniGameMaps[MiniGamePlay.instance.selectedMinimap].frozenImg.color = new Color(1, 1, 1, 0);
            MiniGamePlay.instance.MiniGameMaps[MiniGamePlay.instance.selectedMinimap].snowParticle.gameObject.SetActive(false);
            MiniGamePlay.instance.MiniGameMaps[MiniGamePlay.instance.selectedMinimap].snowParticle1.gameObject.SetActive(false);
            MiniGamePlay.instance.MiniGameMaps[MiniGamePlay.instance.selectedMinimap].windParticle.gameObject.SetActive(false);
        }
        SaveSystem.instance.addCoin(UIManagerNew.Instance.StartMiniGamePanel.reward);
        SaveSystem.instance.SaveData();
        this.Close();
        MiniGamePlay.instance.Disappear(() =>
        {
            UIManagerNew.Instance.GamePlayLoading.appear();
            DOVirtual.DelayedCall(0.7f, () =>
            {
                MiniGamePlay.instance.MiniGameMaps[MiniGamePlay.instance.selectedMinimap].gameObject.SetActive(false);
                if (MiniGameStage.Instance != null)
                {
                    MiniGameStage.Instance.Close(true);
                }
                if (MiniGamePlay.instance.MiniGameMaps[MiniGamePlay.instance.selectedMinimap].minigame1 == MiniGameMap.Minigame.ScaryHouse)
                {
                    MiniGamePlay.instance.MiniGameMaps[MiniGamePlay.instance.selectedMinimap].ghostSkeleton.gameObject.SetActive(false);
                }
                AudioManager.instance.PlayMusic("MenuTheme");
                UIManagerNew.Instance.ButtonMennuManager.Appear();
                DOVirtual.DelayedCall(0.85f, () =>
                {
                    if (MiniGamePlay.instance.selectedMinimap == 0)
                    {
                        PlayerPrefs.SetInt("DoneMini1", 1);
                        FirebaseAnalyticsControl.Instance.LogEventMini_Done(1);
                    }
                    if (MiniGamePlay.instance.selectedMinimap == 1)
                    {
                        PlayerPrefs.SetInt("DoneMini2", 1);
                        FirebaseAnalyticsControl.Instance.LogEventMini_Done(2);
                    }


                });
            });
        });
    }
}
