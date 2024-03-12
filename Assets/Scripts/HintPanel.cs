using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class HintPanel : MonoBehaviour
{
	public RectTransform closeButton;
	public RectTransform panel;
	public RectTransform Blockpanel;
	public RectTransform watchAdButton;
	public int numOfUsed =1;
	public bool hasUse;
	public TextMeshProUGUI numOfUsedText;
	private void Start()
	{
		numOfUsed = 1;
	}
	public void UseTicket()
	{
		if (SaveSystem.instance.purpleStar >= numOfUsed)
		{

			SaveSystem.instance.purpleStar -= numOfUsed;
			SaveSystem.instance.SetTiket(SaveSystem.instance.goldenStar, SaveSystem.instance.purpleStar);
			SaveSystem.instance.SaveData();
			numOfUsed++;
			UIManager.instance.gamePlayPanel.ShowHint();
			hasUse = true;
			this.gameObject.SetActive(false);
			
		}
	}
	public void WatchAd()
	{
		//loadAd
		numOfUsed++;
		this.gameObject.SetActive(false);
		hasUse = true;
		UIManager.instance.gamePlayPanel.ShowHint();
		


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
			if (hasUse)
			{
				UIManager.instance.gamePlayPanel.ShowHint();
			}
			else
			{

				Blockpanel.gameObject.SetActive(true);
				this.gameObject.SetActive(true);
				AudioManager.instance.PlaySFX("OpenPopUp");
				GameManager.instance.hasUI = true;
				UIManager.instance.gamePlayPanel.timer.TimerOn = false;
				panel.localRotation = Quaternion.identity;
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
					panel.DOAnchorPos(new Vector2(panel.transform.position.x, -1467f), 0.25f, true).OnComplete(() =>
					{
						this.gameObject.SetActive(false);
						AudioManager.instance.PlaySFX("ClosePopUp");
						UIManager.instance.gamePlayPanel.timer.TimerOn = true;
						GameManager.instance.hasUI = false;
						Blockpanel.gameObject.SetActive(false);
					});
				});
			});
		}

	}
}
