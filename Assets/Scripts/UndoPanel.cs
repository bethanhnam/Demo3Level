﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UndoPanel : MonoBehaviour
{
    public RectTransform closeButton;
    public RectTransform panel;
    public rankpanel notEnoughpanel;

    public TextMeshProUGUI priceText;
    public int numOfUsed = 1;
    public bool hasWatchAd = false;


    public int numOfUseByAds = 0;
    public RectTransform watchAdButton;
    public TextMeshProUGUI numOfUsedText;

    public CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        numOfUsed = 1;
    }
    public void UseTicket()
    {
        if (SaveSystem.instance.undoPoint >= numOfUsed)
        {
            if (UIManagerNew.Instance.ThresholeController.gameObject.activeSelf)
            {
                UIManagerNew.Instance.ThresholeController.Disable();
            }
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            //FirebaseAnalyticsControl.Instance.LogEventLevelStatus(LevelManagerNew.Instance.stage,LevelStatus.undo);

            SaveSystem.instance.AddBooster(0, -numOfUsed, 0);
            SaveSystem.instance.SaveData();;
            DOVirtual.DelayedCall(0.5f, () =>
            {
                if (!Stage.Instance.isWining)
                {
                    UIManagerNew.Instance.GamePlayPanel.ShowUndoEffect(() =>
                    {
                        Stage.Instance.Undo();
                    });
                }
            });
            DOVirtual.DelayedCall(1f, () =>
            {
                UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
                this.Close();
            });
        }
        else
        {
            notEnoughpanel.ShowDialog();
        }
    }
    public void WatchAd()
    {
        AdsManager.instance.ShowRewardVideo(AddType.Booster_Undo, null, () =>
        {
            //xem qu?ng cáo 
            FirebaseAnalyticsControl.Instance.LogEventLevelItem(LevelManagerNew.Instance.stage, LevelItem.undo);
            hasWatchAd = true;
            SaveSystem.instance.AddBooster(0, 1, 0);
            SaveSystem.instance.SaveData();
            DOVirtual.DelayedCall(0.5f, () =>
            {
                if (!Stage.Instance.isWining)
                {
                    UIManagerNew.Instance.GamePlayPanel.ShowUndoEffect(() =>
                    {
                        Stage.Instance.Undo();
                    });
                }
            });
            this.Close();

        });

    }
    public void CheckNumOfUse()
    {
    }
    public void Uninteractable()
    {
        watchAdButton.GetComponent<Button>().interactable = false;
    }
    public void Interactable()
    {
        watchAdButton.GetComponent<Button>().interactable = true;
    }

    public void NewWayUse()
    {
        Stage.Instance.SetDefaultBeforeUnscrew();
        
        if (SaveSystem.instance.undoPoint >= 1)
        {
            if (UIManagerNew.Instance.ThresholeController.gameObject.activeSelf)
            {
                UIManagerNew.Instance.ThresholeController.Disable();
            }
            //FirebaseAnalyticsControl.Instance.LogEventLevelStatus(LevelManagerNew.Instance.stage,LevelStatus.undo);
            //SetMinusText('-', numOfUsed);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                if (!Stage.Instance.isWining)
                {
                    UIManagerNew.Instance.GamePlayPanel.ShowUndoEffect(() =>
                    {
                        Stage.Instance.Undo();
                    });
                }
            });
        }
        else
        {
            Stage.Instance.canInteract = false;
            UIManagerNew.Instance.GamePlayPanel.OpenUndoPanel();
        }
    }

    public void Open()
    {
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            AudioManager.instance.PlaySFX("OpenPopUp");
            canvasGroup.blocksRaycasts = false;
            SetActiveWatchButton();
            priceText.text = (30 * numOfUsed).ToString();
            panel.localScale = new Vector3(.8f, .8f, 1f);
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.1f);
            panel.transform.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.2f).OnComplete(() =>
            {
                panel.transform.DOScale(Vector3.one, 0.15f).OnComplete(() =>
                {
                    ActiveCVGroup();
                    //GamePlayPanelUIManager.Instance.Close();
                });
            });
            CheckNumOfUse();

        }
    }
    public void Close()
    {
        if (this.gameObject.activeSelf)
        {
            canvasGroup.blocksRaycasts = false;
            panel.DOScale(new Vector3(0.8f, 0.8f, 0), 0.1f).OnComplete(() =>
            {
                AudioManager.instance.PlaySFX("ClosePopUp");
                GamePlayPanelUIManager.Instance.ActiveTime();
                if (!UIManagerNew.Instance.CompleteUI.gameObject.activeSelf || !UIManagerNew.Instance.WinUI.gameObject.activeSelf)
                {
                    GamePlayPanelUIManager.Instance.Appear();
                }
                Stage.Instance.checked1 = false;

                DOVirtual.DelayedCall(0.3f, () => {
                    Stage.Instance.canInteract = true;
                });
                UIManagerNew.Instance.hasUI = false;
                ActiveCVGroup();
                this.gameObject.SetActive(false);
            });
        }
    }

    public void CloseForWin()
    {
        if (this.gameObject.activeSelf)
        {
            canvasGroup.blocksRaycasts = false;
            panel.DOScale(new Vector3(0.8f, 0.8f, 0), 0.1f).OnComplete(() =>
            {
                AudioManager.instance.PlaySFX("ClosePopUp");
                Stage.Instance.checked1 = false;
                UIManagerNew.Instance.hasUI = false;
                ActiveCVGroup();
                this.gameObject.SetActive(false);
            });
        }
    }
    public void SpendCoin()
    {
        if (SaveSystem.instance.coin >= 30 * numOfUsed)
        {
            if (UIManagerNew.Instance.ThresholeController.gameObject.activeSelf)
            {
                UIManagerNew.Instance.ThresholeController.Disable();
            }
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            //FirebaseAnalyticsControl.Instance.LogEventLevelStatus(LevelManagerNew.Instance.stage,LevelStatus.undo);
            SaveSystem.instance.addCoin(-30 * numOfUsed);
            numOfUsed += 1;
            SaveSystem.instance.SaveData();
            DOVirtual.DelayedCall(.8f, () =>
            {
                UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
                this.Close();
            });
            DOVirtual.DelayedCall(1f, () =>
            {
                if (!Stage.Instance.isWining)
                {
                    UIManagerNew.Instance.GamePlayPanel.ShowUndoEffect(() =>
                    {
                        Stage.Instance.Undo();
                    });
                }
            });
        }
        else
        {
            UIManagerNew.Instance.ShopPanel.Open();
        }
    }

    public void SetActiveWatchButton()
    {
        if (!hasWatchAd)
        {
            Interactable();
        }
        else
        {
            Uninteractable();
        }
    }

    public void ActiveCVGroup()
    {
        if (!canvasGroup.blocksRaycasts)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }
}
