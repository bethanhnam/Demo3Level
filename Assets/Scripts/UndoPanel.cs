using DG.Tweening;
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

    public int numOfUsed = 1;

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
            SaveSystem.instance.SaveData();
            numOfUsed++;
            Stage.Instance.Undo();
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

            Stage.Instance.Undo();
            numOfUsed++;
            this.Close();

        });

    }
    public void CheckNumOfUse()
    {
    }
    public void Uniteractable()
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
            SaveSystem.instance.AddBooster(0, -1, 0);
            SaveSystem.instance.SaveData();
            Stage.Instance.Undo();
        }
        else
        {
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
                //if (Stage.Instance.isWining && Stage.Instance.numOfIronPlates <= 0)
                //{
                //    Stage.Instance.ScaleUpStage();
                //}
                //else
                //{
                //    GamePlayPanelUIManager.Instance.Appear();
                //    GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
                //}
                GamePlayPanelUIManager.Instance.Appear();
                Stage.Instance.checked1 = false;

                ActiveCVGroup();
                this.gameObject.SetActive(false);
                Stage.Instance.AfterPanel();
            });
        }
    }
    public void SpendCoin()
    {
        if (SaveSystem.instance.coin >= 30)
        {
            if (UIManagerNew.Instance.ThresholeController.gameObject.activeSelf)
            {
                UIManagerNew.Instance.ThresholeController.Disable();
            }
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            //FirebaseAnalyticsControl.Instance.LogEventLevelStatus(LevelManagerNew.Instance.stage,LevelStatus.undo);

            SaveSystem.instance.addCoin(-30);
            SaveSystem.instance.SaveData();
            Stage.Instance.Undo();
            DOVirtual.DelayedCall(1f, () =>
            {
                UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
                this.Close();
            });
        }
        else
        {
            UIManagerNew.Instance.ShopPanel.Open();
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
