using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartMiniGamePanel : MonoBehaviour
{
    [SerializeField]
    private Animator animButton;
    [SerializeField]
    private CanvasGroup cvButton;

    public Sprite[] panelSprites;

    public Sprite[] bannerSprites;
    public Image bannerImage;
    public Image panelImage;

    public int reward;
    public TextMeshProUGUI rewardText;

    public Button playButton;
    public Button skipButton;
    public Button CloseButton;


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
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.RemoveListener(UIManagerNew.Instance.MiniGamePlay.ReplayMinigame);
        playButton.onClick.RemoveListener(UIManagerNew.Instance.StartMiniGamePanel.Close);
    }

    public void ActiveCVGroup()
    {
        UIManagerNew.Instance.BlockPicCanvas.SetActive(false);
        if (!cvButton.blocksRaycasts)
        {
            cvButton.blocksRaycasts = true;
        }
    }
    public void SetProperties(int reward, int indexBanner)
    {
        this.reward = reward;
        rewardText.text = reward.ToString();
        bannerImage.sprite = bannerSprites[indexBanner];
    }
    public void SkipMinigame()
    {
        this.Close();
        UIManagerNew.Instance.SkipPanel.rewardText.text = "x" + reward.ToString();
        UIManagerNew.Instance.SkipPanel.Appear();
    }
    public void ShowPlay()
    {
        panelImage.sprite = panelSprites[0];
        playButton.GetComponentInChildren<TextMeshProUGUI>().text = "PLAY";
        CloseButton.gameObject.SetActive(true);
    }
    public void ShowRetry()
    {
        panelImage.sprite = panelSprites[1];
        playButton.GetComponentInChildren<TextMeshProUGUI>().text = "RETRY";
        CloseButton.gameObject.SetActive(false);
    }
    public void AddReplay()
    {
        playButton.onClick.RemoveAllListeners();
        DOVirtual.DelayedCall(0.3f, () =>
        {
            playButton.onClick.AddListener(UIManagerNew.Instance.MiniGamePlay.ReplayMinigame);
            playButton.onClick.AddListener(UIManagerNew.Instance.StartMiniGamePanel.Close);
        });
    }
    public void AddPlay()
    {
        playButton.onClick.RemoveAllListeners();
        DOVirtual.DelayedCall(0.3f, () =>
        {
            playButton.onClick.AddListener(UIManagerNew.Instance.ButtonMennuManager.PlayMiniGame);
            playButton.onClick.AddListener(UIManagerNew.Instance.StartMiniGamePanel.Close);
        });
    }
}
