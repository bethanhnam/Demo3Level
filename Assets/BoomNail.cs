using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoomNail : MonoBehaviour
{
	public RectTransform closeButton;
	public RectTransform panel;
	public rankpanel notEnoughpanel;
	public RectTransform Blockpanel;
	public int numOfUsed = 1;
	public RectTransform watchAdButton;
	public TextMeshProUGUI numOfUsedText;
	private void Start()
	{
		numOfUsed = 1;
	}
	public void UseTicket()
	{
		if (SaveSystem.instance.magicTiket >= numOfUsed)
		{
			SaveSystem.instance.addTiket(0, -numOfUsed);
			SaveSystem.instance.SaveData();
			//hasUse = true;
			GameManager.instance.deletingIron = true;
			numOfUsed++;
			UIManager.instance.gamePlayPanel.ButtonOff();
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

			//xoá nail(Đồng hồ đếm giờ dừng lại)
			GameManager.instance.deletingIron = true;
			UIManager.instance.gamePlayPanel.ButtonOff();
			numOfUsed++;
			this.Close();

		});

	}
	private void Update()
	{
		numOfUsedText.text = ("X" + (numOfUsed).ToString());
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
			Blockpanel.gameObject.SetActive(true);
			this.gameObject.SetActive(true);
			AudioManager.instance.PlaySFX("OpenPopUp");
			UIManager.instance.DeactiveTime();
			GameManager.instance.hasUI = true;
			panel.localRotation = Quaternion.identity;
			panel.localPosition = new Vector3(332f, -239.000031f, 0);
			panel.localScale = new Vector3(.8f, .8f, 1);
			closeButton.localPosition = new Vector3(364, 277.600006f, 0);
			this.GetComponent<CanvasGroup>().alpha = 0;
			this.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
			panel.DOScale(new Vector3(1, 1, 1), 0.1f).OnComplete(() =>
			{
				Blockpanel.gameObject.SetActive(false);
			});

		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			Blockpanel.gameObject.SetActive(true);
			closeButton.DOAnchorPos(new Vector2(552f, -105f), .1f, false).OnComplete(() =>
			{
				panel.DORotate(new Vector3(0, 0, -10f), 0.25f, RotateMode.Fast).OnComplete(() =>
				{
					panel.DOAnchorPos(new Vector2(panel.transform.position.x, -1467f), .25f, false).OnComplete(() =>
					{
						this.gameObject.SetActive(false);
						AudioManager.instance.PlaySFX("ClosePopUp");
						UIManager.instance.ActiveTime();
						SaveSystem.instance.playingHard = true;
						GameManager.instance.hasUI = false;
						Blockpanel.gameObject.SetActive(false);
					});
				});
			});
		}
	}

}
