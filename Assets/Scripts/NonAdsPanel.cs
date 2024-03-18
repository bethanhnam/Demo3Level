using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonAdsPanel : MonoBehaviour
{
	public CanvasGroup canvasGroup;
	public GameObject panelBoard;
	public RectTransform Blockpanel;
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			GameManager.instance.hasUI = true;
			this.gameObject.SetActive(true);
			panelBoard.gameObject.SetActive(false);
			panelBoard.transform.localScale = new Vector3(.9f, .9f, 1f);
			Blockpanel.gameObject.SetActive(true);
			AudioManager.instance.PlaySFX("OpenPopUp");
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, .3f);
			panelBoard.gameObject.SetActive(true);
			panelBoard.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.2f).OnComplete(() =>
			{
				panelBoard.transform.DOScale(Vector3.one, 0.15f).OnComplete(() =>
				{
					Blockpanel.gameObject.SetActive(false);
				});
			});
		}
		
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			canvasGroup.alpha = 1;
			Blockpanel.gameObject.SetActive(true);
			canvasGroup.DOFade(0, .3f).OnComplete(() =>
			{
				GameManager.instance.hasUI = false;
				this.gameObject.SetActive(false);
				AudioManager.instance.PlaySFX("ClosePopUp");
				Blockpanel.gameObject.SetActive(false);
			});
			panelBoard.transform.DOScale(new Vector3(.9f, .9f, 1f), 0.3f);
		}
	}
}
