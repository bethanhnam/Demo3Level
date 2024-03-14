using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
	public CanvasGroup canvasGroup;
	public RectTransform Blockpanel;
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			Blockpanel.gameObject.SetActive(true);
			GameManager.instance.hasUI = true;
			AudioManager.instance.PlaySFX("OpenPopUp");
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, .3f).OnComplete(() =>
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
			canvasGroup.alpha = 1;
			canvasGroup.DOFade(0, .3f).OnComplete(() =>
			{
				GameManager.instance.hasUI = false;
				this.gameObject.SetActive(false);
				AudioManager.instance.PlaySFX("ClosePopUp");
				Blockpanel.gameObject.SetActive(false);
			});
			
		}
	}
	public void BackToMenu()
	{
		Close();
		UIManager.instance.menuPanel.canvasGroup.alpha = 0;
		UIManager.instance.menuPanel.canvasGroup.DOFade(1, .7f).OnComplete(() =>
		{
		});
		//UIManager.instance.menuPanel.Open();
	}
}
