using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMiniGamePanel : MonoBehaviour
{
    [SerializeField]
    private Animator animButton;
    [SerializeField]
    private CanvasGroup cvButton;

    public Sprite[] bannerSprites;
    public Image bannerImage;

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
        playButton.onClick.RemoveListener(UIManagerNew.Instance.ButtonMennuManager.PlayMiniGame);
        playButton.onClick.RemoveListener(UIManagerNew.Instance.MiniGamePlay.ReplayMinigame);
        UIManagerNew.Instance.StartMiniGamePanel.playButton.onClick.RemoveListener(UIManagerNew.Instance.StartMiniGamePanel.Close);
    }

    public void ActiveCVGroup()
    {
        UIManagerNew.Instance.BlockPicCanvas.SetActive(false);
        if (!cvButton.blocksRaycasts)
        {
            cvButton.blocksRaycasts = true;
        }
    }
    public void SetProperties(int reward,int indexBanner)
    {
        this.reward = reward;
        rewardText.text = reward.ToString();
        bannerImage.sprite = bannerSprites[indexBanner];
    }
    public void SkipMinigame()
    {
        this.Close();
        UIManagerNew.Instance.SkipPanel.rewardText.text = "x" + reward.ToString() ;
        UIManagerNew.Instance.SkipPanel.Appear();
    }
    public void ChangeText(string text)
    {
        playButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
}
