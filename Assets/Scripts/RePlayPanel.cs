using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class RePlayPanel : MonoBehaviour
{
	public RectTransform closeButton;
	public RectTransform panel;
	public RectTransform Blockpanel;
	public RectTransform watchAdButton;
	public int numOfUsed =1;
	public TextMeshProUGUI numOfUsedText;
	private void Start()
	{
		numOfUsed = 1;
	}
	public void UseTicket()
	{
		if (SaveSystem.instance.powerTicket >= numOfUsed)
		{

			SaveSystem.instance.addTiket(-numOfUsed, 0);
			SaveSystem.instance.SaveData();
			numOfUsed++;
			GameManager.instance.Replay();
			this.Close();
			
		}
	}
	public void WatchAd()
	{
		// load ad 
		AdsManager.instance.ShowRewardVideo(() =>
		{
			this.Close();
			GameManager.instance.Replay();
			numOfUsed++;
		});
		
		
	}
	private void Update()
	{
			numOfUsedText.text = (numOfUsed).ToString();
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
			GameManager.instance.hasUI = true;
			panel.localRotation = Quaternion.identity;
			UIManager.instance.gamePlayPanel.timer.TimerOn = false;
			this.GetComponent<CanvasGroup>().alpha = 0;
			panel.localPosition = new Vector3(-351, 479, 0);
			panel.localScale = new Vector3(.8f, .8f, 0);
			closeButton.localPosition = new Vector3(364, 277.600006f, 0);
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
					panel.DOAnchorPos(new Vector2(panel.transform.position.x, -1467f), 0.25f, false).OnComplete(() =>
					{
						this.gameObject.SetActive(false);
						AudioManager.instance.PlaySFX("ClosePopUp");
						GameManager.instance.hasUI = false;
						UIManager.instance.gamePlayPanel.timer.TimerOn = true;
						Blockpanel.gameObject.SetActive(false);
					});
				});
			});
		}
	}
}
