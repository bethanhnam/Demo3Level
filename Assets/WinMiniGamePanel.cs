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
        UIManagerNew.Instance.BlockPicCanvas.SetActive(true);
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        animButton.enabled = true;
        cvButton.blocksRaycasts = false;
        animButton.Play(appearButton, 0, 0);
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
        SaveSystem.instance.addCoin(UIManagerNew.Instance.StartMiniGamePanel.reward);
        SaveSystem.instance.SaveData();
        this.Close();
        MiniGamePlay.instance.Disappear(() =>
        {
            MiniGamePlay.instance.MiniGameMaps[MiniGamePlay.instance.selectedMinimap].gameObject.SetActive(false);
            UIManagerNew.Instance.GamePlayLoading.appear();
            if (MiniGameStage.Instance != null)
            {
                MiniGameStage.Instance.Close(true);
            }
            DOVirtual.DelayedCall(0.7f, () =>
            {
                UIManagerNew.Instance.ButtonMennuManager.Appear();
                DOVirtual.DelayedCall(0.85f, () =>
                {
                    if (MiniGamePlay.instance.selectedMinimap == 0)
                    {
                        ConversationController.instance.StartConversation(1, 7, "AfterMinigame1", () =>
                        {
                            ConversationController.instance.StartConversation(1, 9, "AfterMinigame2", () => { 
                            });
                        });
                    }
                });
            });
        });
    }
}
