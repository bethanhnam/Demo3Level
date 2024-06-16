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
    public int numOfUsed = 1;
    public int numOfUse = 0;
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
            ShowTutor();
            numOfUse++;
            FirebaseAnalyticsControl.Instance.LogEventGameplay_Item_Undo_1(numOfUse, LevelManagerNew.Instance.stage);

            SaveSystem.instance.AddBooster(0, -numOfUsed,0);
            SaveSystem.instance.SaveData();
            numOfUsed++;
            Stage.Instance.Undo();
            this.Close();
        }
        else
        {
            notEnoughpanel.ShowDialog();
        }
    }
    public void WatchAd()
    {
        AdsManager.instance.ShowRewardVideo(() =>
        {
            ShowTutor();
            //xem qu?ng cáo 
            numOfUseByAds++;
            FirebaseAnalyticsControl.Instance.LogEventUndo_RW_Change(numOfUse);
            numOfUse++;
            FirebaseAnalyticsControl.Instance.LogEventGameplay_Item_Undo_1(numOfUse, LevelManagerNew.Instance.stage);

            Stage.Instance.Undo();
            numOfUsed++;
            this.Close();

        });

    }
    public void CheckNumOfUse()
    {
        numOfUsedText.text = ("X" + (numOfUsed).ToString());
        if (numOfUsed == 1)
        {
            Interactable();
        }
        else
        {
            Uniteractable();
        }
    }
    public void Uniteractable()
    {
        watchAdButton.GetComponent<Button>().interactable = false;
    }
    public void Interactable()
    {
        watchAdButton.GetComponent<Button>().interactable = true;
    }
    public void Open()
    {
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            OffPoiter();
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
                if (Stage.Instance.isWining && Stage.Instance.numOfIronPlates <=0)
                {
                    Stage.Instance.ScaleUpStage();
                }
                else
                {
                    GamePlayPanelUIManager.Instance.Appear();
                    GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
                }
                GamePlayPanelUIManager.Instance.ShowPoiterAgain1();
                Stage.Instance.checked1 = false;

                ActiveCVGroup();
                this.gameObject.SetActive(false);
                Stage.Instance.AfterPanel();
            });
        }
    }
    public void ActiveCVGroup()
    {
        if (!canvasGroup.blocksRaycasts)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }
    public void ShowTutor()
    {
        if (Stage.Instance.isTutor)
        {
            GamePlayPanelUIManager.Instance.boosterBar.ShowPointer(false);
            GamePlayPanelUIManager.Instance.ActiveBlackPic(false);
            GamePlayPanelUIManager.Instance.boosterBar.InteractableBT(GamePlayPanelUIManager.Instance.boosterBar.deteleBT);
        }
    }
    public void OffPoiter()
    {
        if (Stage.Instance.isTutor)
        {
            GamePlayPanelUIManager.Instance.boosterBar.ShowPointer(false);
        }
    }
}
