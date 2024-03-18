using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class PausePanel : MonoBehaviour
{
	public RectTransform Blockpanel;
	public bool isdeleting;
	//public RectTransform title;
	//public RectTransform homeButton;
	//public RectTransform retryButton;
	public CanvasGroup canvasGroup;
	public void Home()
	{
		this.gameObject.SetActive(false);
		UIManager.instance.gamePlayPanel.backFromPause = false;
		UIManager.instance.gamePlayPanel.Close();
		GameManager.instance.hasUI = false;
		GameManager.instance.gameObject.SetActive(false);
		LevelManager.instance.gameObject.SetActive(false);
		canvasGroup.DOFade(0, .2f);
		Blockpanel.gameObject.SetActive(false);
		UIManager.instance.menuPanel.Open();
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{

			this.gameObject.SetActive(true);
			canvasGroup.alpha = 1;
			if (GameManager.instance.deleting)
			{
				isdeleting = true;
			}
			Blockpanel.gameObject.SetActive(true);
			GameManager.instance.hasUI = true;
			UIManager.instance.gamePlayPanel.Close();
			UIManager.instance.DeactiveTime();
			this.GetComponent<CanvasGroup>().alpha = 0;
			//title.transform.localPosition = new Vector3(-13, 640, 0);
			//retryButton.transform.localPosition = new Vector3(1, -36, 0);
			//homeButton.transform.localPosition = new Vector3(1, 144, 0);
			this.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
			this.GetComponent<RectTransform>().DOScale(new Vector3(1, 1, 1), 0.1f).OnComplete(() =>
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
			this.GetComponent<CanvasGroup>().DOFade(0, 0.1f);
			this.GetComponent<RectTransform>().DOScale(new Vector3(.8f, .8f, 1), 0.1f).OnComplete(() =>
			{
				canvasGroup.DOFade(0, .2f);
				Blockpanel.gameObject.SetActive(false);
				this.gameObject.SetActive(false);
				SaveSystem.instance.playingHard =  true;
				 UIManager.instance.ActiveTime();
				GameManager.instance.hasUI = false;
				UIManager.instance.gamePlayPanel.backFromPause = true;
				UIManager.instance.gamePlayPanel.Open();
			});
		}
	}
	public void Retry()
	{
		Close();
		GameManager.instance.Retry();
		UIManager.instance.gamePlayPanel.backFromPause = false;
	}
}
