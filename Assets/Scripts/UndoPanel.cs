using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UndoPanel : MonoBehaviour
{
	public RectTransform closeButton;
	public RectTransform panel;
	public RectTransform Blockpanel;
	public bool hasUse;
	public void UseTicket()
	{
		if (GameManager.instance.goldenStar > 0)
		{
			GameManager.instance.goldenStar--;
			hasUse = true;
			SaveSystem.instance.SetTiket(GameManager.instance.goldenStar, GameManager.instance.purpleStar);
			InputManager.instance.Undo();
			this.Close();
		}
	}
	public void WatchAd()
	{
		//xem qu?ng cáo 
		InputManager.instance.Undo();
		hasUse = true;
		this.Close();
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
						UIManager.instance.gamePlayPanel.timer.TimerOn = true;
						GameManager.instance.hasUI = false;
						Blockpanel.gameObject.SetActive(false);
					});
				});
			});
		}
	}

}
