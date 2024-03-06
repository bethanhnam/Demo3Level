using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
		if (GameManager.instance.goldenStar > 0)
		{
			this.Close();
			GameManager.instance.goldenStar--;
			SaveSystem.instance.SetTiket(GameManager.instance.goldenStar, GameManager.instance.purpleStar);
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
			GameManager.instance.hasUI = true;
			UIManager.instance.gamePlayPanel.timer.TimerOn = false;
			Blockpanel.gameObject.SetActive(true);
			panel.localRotation = Quaternion.identity;
			panel.DOAnchorPos(new Vector3(-351, 479, 0), .5f, false).OnComplete(() =>
			{
				closeButton.DOAnchorPos(new Vector3(-71.5f, -207.8f, 0), .5f, false).OnComplete(() => {
					Blockpanel.gameObject.SetActive(false);
				});
			});
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			Blockpanel.gameObject.SetActive(true);
			closeButton.DOAnchorPos(new Vector2(552f, -105f), 1f, false).OnComplete(() =>
			{
				panel.DORotate(new Vector3(0, 0, -10f), .3f, RotateMode.Fast).OnComplete(() =>
				{
					panel.DOAnchorPos(new Vector2(panel.transform.position.x, -1467f), .3f, true).OnComplete(() =>
					{
						this.gameObject.SetActive(false);
						GameManager.instance.hasUI = false;
						UIManager.instance.gamePlayPanel.timer.TimerOn = true;
						Blockpanel.gameObject.SetActive(false);
					});
				});
			});
		}
	}


}
