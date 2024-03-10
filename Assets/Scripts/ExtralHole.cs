using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class ExtralHole : MonoBehaviour
{
	public string layerName = "Hole";
	public ExtraHoleButton extraHoleButton;
	public RectTransform closeButton;
	public RectTransform panel;
	public RectTransform Blockpanel;
	private void Update()
	{
		try
		{
			extraHoleButton = FindFirstObjectByType<ExtraHoleButton>();
		}
		catch { }
	}
	public void UseTicket()
	{
		if (GameManager.instance.GoldenStar > 0)
		{
			this.Close();
			GameManager.instance.GoldenStar--;
			SaveSystem.instance.SetTiket(GameManager.instance.GoldenStar, GameManager.instance.PurpleStar);
			SaveSystem.instance.SaveData();
			Level.instance.ChangeLayer();
			extraHoleButton.gameObject.SetActive(false);
		}
	}
	public void WatchAd()
	{
		// load ad 
		this.Close();
		Level.instance.ChangeLayer();
		extraHoleButton.gameObject.SetActive(false);
		
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			AudioManager.instance.PlaySFX("OpenPopUp");
			GameManager.instance.hasUI = true;
			UIManager.instance.gamePlayPanel.timer.TimerOn = false;
			Blockpanel.gameObject.SetActive(true);
			panel.localRotation = Quaternion.identity;
			this.GetComponent<CanvasGroup>().alpha = 0;
			panel.localPosition = new Vector3(-351, 479, 0);
			panel.localScale = new Vector3(.8f, .8f, 0);
			closeButton.localPosition = new Vector3(359.100006f, 275.600006f, 0);
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
					panel.DOAnchorPos(new Vector2(panel.transform.position.x, -1467f), 0.25f, true).OnComplete(() =>
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
