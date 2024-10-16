using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LosMiniGamePanel : MonoBehaviour
{
    [SerializeField]
    private Animator animButton;
    [SerializeField]
    private CanvasGroup cvButton;

    public Image bannerImage;

    public TextMeshProUGUI rewardText;

    public Button replayButton;
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
    public void SetProperties()
    {
        rewardText.text = UIManagerNew.Instance.StartMiniGamePanel.reward.ToString();
        bannerImage.sprite = UIManagerNew.Instance.StartMiniGamePanel.bannerImage.sprite;
    }
    public void ShowSkipPopUp()
    {
       
    }
    
}
