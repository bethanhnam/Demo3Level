using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PausePanel : MonoBehaviour
{
	public RectTransform Blockpanel;
	public RectTransform title;
	public RectTransform homeButton;
	public RectTransform retryButton;
	public CanvasGroup canvasGroup;
	public void Home()
	{
		this.Close();
		UIManager.instance.gamePlayPanel.backFromPause =false;
		UIManager.instance.gamePlayPanel.Close();
		UIManager.instance.menuPanel.Open();
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{

			this.gameObject.SetActive(true);
			canvasGroup.alpha = 1;
			Blockpanel.gameObject.SetActive(true);
			GameManager.instance.hasUI = true;
			UIManager.instance.gamePlayPanel.Close();
			title.transform.localPosition = new Vector3(-13f, 1476f, 0);
			retryButton.transform.localPosition = new Vector3(1324f, -36f, 0);
			homeButton.transform.localPosition = new Vector3(-1400f, 144f, 0);
			title.DOAnchorPos(new Vector3(-13f, -262f, 0), 0.3f, false).OnComplete(() =>
			{
				homeButton.DOAnchorPos(new Vector3(1, 144, 0), .3f, false).OnComplete(() => {
					retryButton.DOAnchorPos(new Vector3(1, -36, 0), .3f, false).OnComplete(() =>
					{
						Blockpanel.gameObject.SetActive(false);
					});
				});
			});
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			Blockpanel.gameObject.SetActive(true);
			canvasGroup.alpha = 1;
				retryButton.DOAnchorPos(new Vector3(1324f, -36f, 0), .3f, false).OnComplete(() =>
				{
					homeButton.DOAnchorPos(new Vector3(-1400f, 144f, 0), .3f, false).OnComplete(() => {
						title.DOAnchorPos(new Vector3(-13f, 1476f, 0), .1f, false).OnComplete(() =>
						{
							canvasGroup.DOFade(0, .2f);
							Blockpanel.gameObject.SetActive(false);
							this.gameObject.SetActive(false);
							UIManager.instance.gamePlayPanel.timer.TimerOn = true;
							GameManager.instance.hasUI = false;
							UIManager.instance.gamePlayPanel.backFromPause = true;
							UIManager.instance.gamePlayPanel.Open();
						});
					});
				});
		}
	}
	public void Retry()
	{
		Close();
		GameManager.instance.Replay();
		UIManager.instance.gamePlayPanel.backFromPause = false;
	}
}
