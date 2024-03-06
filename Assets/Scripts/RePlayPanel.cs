using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePlayPanel : MonoBehaviour
{
	public RectTransform closeButton;
	public RectTransform panel;
	public RectTransform Blockpanel;
	public void UseTicket()
	{
		if(GameManager.instance.goldenStar > 0)
		{
			this.Close();
			GameManager.instance.goldenStar--;
			SaveSystem.instance.SetTiket(GameManager.instance.goldenStar, GameManager.instance.purpleStar);
			GameManager.instance.Replay();
		}
	}
	public void WatchAd()
	{
		// load ad 
		this.Close();
		GameManager.instance.Replay();
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			Blockpanel.gameObject.SetActive(true);
			this.gameObject.SetActive(true);
			GameManager.instance.hasUI = true;
			panel.localRotation = Quaternion.identity;
			panel.DOAnchorPos(new Vector3(-351, 479, 0), 1f, false).OnComplete(() =>
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
					panel.DOAnchorPos(new Vector2(panel.transform.position.x, -1467f), .5f, false).OnComplete(() =>
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
