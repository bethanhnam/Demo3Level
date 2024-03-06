using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
	public CanvasGroup canvasGroup;
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			GameManager.instance.hasUI = true;
			this.gameObject.SetActive(true);
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, 1f).OnComplete(() =>
			{
			});
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			canvasGroup.alpha = 1;
			canvasGroup.DOFade(0, 1f).OnComplete(() =>
			{
				GameManager.instance.hasUI = false;
				this.gameObject.SetActive(false);
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
