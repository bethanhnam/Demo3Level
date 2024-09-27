using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Timers;
using System.Runtime.InteropServices.WindowsRuntime;
using Sirenix.OdinInspector;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class LosePanel : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public bool hasUse = false;

    public Button spendCoinButton;
    public Button watchAdButton;

    public Sprite bttn_green;
    public Sprite bttn_orange;
    public Sprite bttn_gray;

    public Image titleImage;
    public FailOffScroll FailOffScroll;

    public TextMeshProUGUI[] spendCoinButtonTexts;
    public TextMeshProUGUI[] watchAdButtonTexts;

    public TMP_FontAsset greenFontAsset;
    public TMP_FontAsset grayFontAsset;
    public TMP_FontAsset orangeFontAsset;

    // Start is called before the first frame update
    void Start()
    {
        watchAdButton.interactable = true;
        SetTitlePost();
        SetScrollPost();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        if (hasUse)
        {
            watchAdButton.interactable = false;
        }
        else
        {
            watchAdButton.interactable = true;
            watchAdButton.GetComponent<Image>().sprite = bttn_orange;
        }

        if (SaveSystem.instance.coin < 100)
        {
            spendCoinButton.interactable = false;
            spendCoinButton.GetComponent<Image>().sprite = bttn_gray;
        }
        else
        {
            spendCoinButton.interactable = true;
            spendCoinButton.GetComponent<Image>().sprite = bttn_green;
        }
        SetTextStroke();
    }

    public void SetTextStroke()
    {
        if (hasUse)
        {
            for (int i = 0; i < watchAdButtonTexts.Length; i++)
            {
                watchAdButtonTexts[i].font = grayFontAsset;
            }
        }
        else
        {
            for (int i = 0; i < watchAdButtonTexts.Length; i++)
            {
                watchAdButtonTexts[i].font = orangeFontAsset;
            }
        }
        if (SaveSystem.instance.coin < 100)
        {
            for (int i = 0; i < spendCoinButtonTexts.Length; i++)
            {
                spendCoinButtonTexts[i].font = grayFontAsset;
            }
        }
        else
        {
            for (int i = 0; i < spendCoinButtonTexts.Length; i++)
            {
                spendCoinButtonTexts[i].font = greenFontAsset;
            }
        }
    }

    public void WatchAd()
    {
        // load ad 
        if (!hasUse)
        {
            AdsManager.instance.ShowRewardVideo(() =>
            {
                Close();

                GamePlayPanelUIManager.Instance.timer.SetTimer(61f);
                GamePlayPanelUIManager.Instance.ActiveTime();
                GamePlayPanelUIManager.Instance.Appear();
                GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
                hasUse = true;
                watchAdButton.interactable = false;
                watchAdButton.GetComponent<Image>().sprite = bttn_gray;
                Stage.Instance.CheckDoneLevel();
                DOVirtual.DelayedCall(1.3f, () =>
                {
                    UIManagerNew.Instance.GamePlayPanel.timer.TimerText.transform.DOScale(1.2f, 0.3f).OnComplete(() =>
                    {
                        UIManagerNew.Instance.GamePlayPanel.timer.TimerText.transform.DOScale(1f, 0.2f);
                    });
                });
                FirebaseAnalyticsControl.Instance.LogEventLevelStatus(LevelManagerNew.Instance.stage, LevelStatus.revive);

            });

        }

    }
    public void Replay()
    {
        // load ad 
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
        AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_lose, () =>
        {
            Close();
            ConversationController.instance.Disappear();
            GameManagerNew.Instance.Retry();
            hasUse = false;
        }, null);
    }
    public void Open()
    {
        //neu da win thi khong mo lose
        if (Stage.Instance.numOfIronPlates <= 0)
        {
            return;
        }
        else
        {
            FirebaseAnalyticsControl.Instance.LogEventLevelStatus(LevelManagerNew.Instance.stage, LevelStatus.fail);
            if (!this.gameObject.activeSelf)
            {
                this.gameObject.SetActive(true);
                AudioManager.instance.PlaySFX("LosePop");
                canvasGroup.alpha = 0;
                canvasGroup.DOFade(1, .3f).OnComplete(() =>
                {

                });
            }
        }
    }
    public void Close()
    {
        if (this.gameObject.activeSelf)
        {
            canvasGroup.alpha = 1;
            canvasGroup.DOFade(0, .3f).OnComplete(() =>
            {
                this.gameObject.SetActive(false);
                AudioManager.instance.PlaySFX("ClosePopUp");
                Stage.Instance.checked1 = false;
            });
        }
    }

    public void SpendCoin()
    {
        if (SaveSystem.instance.coin >= 100)
        {
            SaveSystem.instance.addCoin(-100);
            SaveSystem.instance.SaveData();
            Close();

            GamePlayPanelUIManager.Instance.timer.SetTimer(45f);
            GamePlayPanelUIManager.Instance.ActiveTime();
            GamePlayPanelUIManager.Instance.Appear();
            GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);

            spendCoinButton.interactable = false;
            spendCoinButton.GetComponent<Image>().sprite = bttn_gray;
            Stage.Instance.CheckDoneLevel();
            DOVirtual.DelayedCall(1.1f, () =>
            {
                UIManagerNew.Instance.GamePlayPanel.timer.TimerText.transform.DOScale(1.2f, 0.3f).OnComplete(() =>
            {
                UIManagerNew.Instance.GamePlayPanel.timer.TimerText.transform.DOScale(1f, 0.2f);
            });
            });
            FirebaseAnalyticsControl.Instance.LogEventLevelStatus(LevelManagerNew.Instance.stage, LevelStatus.revive);
        }
        else
        {
            spendCoinButton.transform.DORotate(new Vector3(0, 0, 2), 0.2f).OnComplete(() =>
            {
                spendCoinButton.transform.DORotate(new Vector3(0, 0, -2), 0.2f).OnComplete(() =>
                {
                    spendCoinButton.transform.DORotate(new Vector3(0, 0, 0), 0.2f).OnComplete(() =>
                    {
                        spendCoinButton.interactable = false;
                        spendCoinButton.GetComponent<Image>().sprite = bttn_gray;
                    });
                });
            });
        }
    }
    public void SetTitlePost()
    {
        RectTransform rectTransform = titleImage.GetComponent<RectTransform>();
        RectTransform referenceRect = this.GetComponent<RectTransform>();

        // Tính toán vị trí 2/3 chiều cao của reference image
        float targetYPosition = referenceRect.rect.height * (2f / 5.0f);

        // Đặt vị trí của titleImage dựa trên reference image
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, targetYPosition);
    }
    public void SetScrollPost()
    {
        RectTransform rectTransform = FailOffScroll.GetComponent<RectTransform>();
        RectTransform referenceRect = this.GetComponent<RectTransform>();

        // Tính toán vị trí 2/3 chiều cao của reference image
        float targetYPosition = referenceRect.rect.height * (-1.32f / 5.0f);

        // Đặt vị trí của titleImage dựa trên reference image
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, targetYPosition);
    }
    public void BackToHome()
    {
        Close();
        UIManagerNew.Instance.GamePlayLoading.appear();
        DOVirtual.DelayedCall(.7f, () =>
        {
            GameManagerNew.Instance.PictureUIManager.Open();
            this.gameObject.SetActive(false);
            Stage.Instance.ResetBooster();
            UIManagerNew.Instance.ButtonMennuManager.Appear();
            canvasGroup.DOFade(0, 0.5f).OnComplete(() =>
            {
                GameManagerNew.Instance.Bg.gameObject.SetActive(false);
                Destroy(GameManagerNew.Instance.CurrentLevel.gameObject);
            });
        });
    }
}
