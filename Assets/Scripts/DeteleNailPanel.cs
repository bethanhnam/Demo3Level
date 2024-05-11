using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasGroup))]

public class DeteleNailPanel : MonoBehaviour
{
	public RectTransform closeButton;
	public RectTransform panel;
	public rankpanel notEnoughpanel;
	public int numOfUsed = 1;
	public RectTransform watchAdButton;
	public TextMeshProUGUI numOfUsedText;
	public CanvasGroup canvasGroup;
	public int numOfUse = 0;
	public int numOfUseByAds = 0;
	private void Start()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		numOfUsed = 1;
	}
	public void UseTicket()
	{
		if (SaveSystem.instance.magicTiket >= numOfUsed)
		{
			numOfUse++;
			FirebaseAnalyticsControl.Instance.LogEventUnscrewUsed(numOfUse);

			SaveSystem.instance.addTiket(0, -numOfUsed);
			SaveSystem.instance.SaveData();
			//hasUse = true;
			numOfUsed++;
			Stage.Instance.setDeteleting(true);
			//UIManager.instance.gamePlayPanel.ButtonOff();
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
			//xem qu?ng cáo 
			numOfUseByAds++;
			FirebaseAnalyticsControl.Instance.LogEventUnscrewByAds(numOfUseByAds);

			numOfUse++;
			FirebaseAnalyticsControl.Instance.LogEventUnscrewUsed(numOfUse);

			//xoá nail(Đồng hồ đếm giờ dừng lại)
			Stage.Instance.setDeteleting(true);
			//UIManager.instance.gamePlayPanel.ButtonOff();
			numOfUsed++;
			this.Close();

		});

	}
	private void Update()
	{
		numOfUsedText.text = ("X" +(numOfUsed).ToString());
		if (numOfUsed == 1)
		{
			watchAdButton.GetComponent<Button>().interactable = true;
		}
		else
		{
			watchAdButton.GetComponent<Button>().interactable = false;
		}
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			canvasGroup.blocksRaycasts = false;
			AudioManager.instance.PlaySFX("OpenPopUp");
			panel.localRotation = Quaternion.identity;
			panel.localPosition = new Vector3(-351, 479, 0);
			panel.localScale = new Vector3(.8f, .8f, 1);
			closeButton.localPosition = new Vector3(364, 277.600006f, 0);
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, 0.1f);
			panel.DOScale(new Vector3(1, 1, 1), 0.1f).OnComplete(() =>
			{
				GamePlayPanelUIManager.Instance.Close();
				ActiveCVGroup();
			});

		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			canvasGroup.blocksRaycasts = false;
			closeButton.DOAnchorPos(new Vector2(552f, -105f), .1f, false).OnComplete(() =>
			{
				panel.DORotate(new Vector3(0, 0, -10f), 0.25f, RotateMode.Fast).OnComplete(() =>
				{
					panel.DOAnchorPos(new Vector2(panel.transform.position.x, -1467f), .25f, false).OnComplete(() =>
					{
						this.gameObject.SetActive(false);
						AudioManager.instance.PlaySFX("ClosePopUp");
						GamePlayPanelUIManager.Instance.ActiveTime();
						GamePlayPanelUIManager.Instance.Appear();
						GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
						
						ActiveCVGroup();
					});
				});
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
}
