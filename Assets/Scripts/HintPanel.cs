using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintPanel : MonoBehaviour
{
	public RectTransform closeButton;
	public RectTransform panel;
	public RectTransform Blockpanel;
	public void UseTicket()
	{
		if (GameManager.instance.purpleStar > 0)
		{
			this.gameObject.SetActive(false);
			GameManager.instance.purpleStar--;
			SaveSystem.instance.SetTiket(GameManager.instance.goldenStar, GameManager.instance.purpleStar);
			UIManager.instance.gamePlayPanel.ShowHint();
		}
	}
	public void WatchAd()
	{
		//loadAd
		this.gameObject.SetActive(false);
		UIManager.instance.gamePlayPanel.ShowHint();


	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			Blockpanel.gameObject.SetActive(true);
			this.gameObject.SetActive(true);
			GameManager.instance.hasUI = true;
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
						UIManager.instance.gamePlayPanel.timer.TimerOn = true;
						GameManager.instance.hasUI = false;
						Blockpanel.gameObject.SetActive(false);
					});
				});
			});
		}

	}
}
