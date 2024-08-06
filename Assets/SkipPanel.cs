using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkipPanel : MonoBehaviour
{
    [SerializeField]
    private Animator animButton;
    [SerializeField]
    private CanvasGroup cvButton;

    public int reward;
    public TextMeshProUGUI rewardText;

    public Button playButton;
    public Button skipButton;

    private int appearButton = Animator.StringToHash("MinigameAppear");
    private int disappearButton = Animator.StringToHash("MinigameDisappear");

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
    public void SkipMinigame()
    {
        var level = 0;
        if (LevelManagerNew.Instance.stage == 4)
        {
            level = 0;
        }
        if (LevelManagerNew.Instance.stage == 7)
        {
            level = 1;
        }
        UIManagerNew.Instance.MiniGamePlay.MiniGameMaps[level].hasDone = true;
        if (UIManagerNew.Instance.StartMiniGamePanel.gameObject.activeSelf)
        {
            UIManagerNew.Instance.StartMiniGamePanel.Close();
        }
        this.Close();
        DOVirtual.DelayedCall(0.3f, () =>
        {
            UIManagerNew.Instance.ButtonMennuManager.Appear();
        });
    }
}
