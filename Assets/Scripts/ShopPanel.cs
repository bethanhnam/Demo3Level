using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
	public CanvasGroup canvasGroup;
	public RestorePanel restorePanel;

	public Animator animator;
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			FirebaseAnalyticsControl.Instance.LogEventShopPanelAccessSuccessfully(1);
			if (GameManagerNew.Instance.CurrentLevel != null)
			{
				GamePlayPanelUIManager.Instance.DeactiveTime();
			}
			AudioManager.instance.PlaySFX("OpenPopUp");
			canvasGroup.alpha = 0;
			//animator.Play("appear", 0, 0);
			canvasGroup.DOFade(1, .3f).OnComplete(() =>
			{
                UIManagerNew.Instance.LoadData(SaveSystem.instance.magicTiket, SaveSystem.instance.powerTicket, SaveSystem.instance.coin, SaveSystem.instance.star);
            });
		}
	}
	public void Close()
	{
		if (GameManagerNew.Instance.CurrentLevel != null)
		{
            Stage.Instance.checked1 = false;
            BackToGame();
		}
		else
		{
			if (this.gameObject.activeSelf)
			{
				canvasGroup.alpha = 1;
				canvasGroup.DOFade(0, .3f).OnComplete(() =>
				{
					this.gameObject.SetActive(false);
					AudioManager.instance.PlaySFX("ClosePopUp");
					UIManagerNew.Instance.ButtonMennuManager.Appear();
				});


			}
		}
	}
	public void BackToGame()
	{
		if (this.gameObject.activeSelf)
		{
			canvasGroup.alpha = 1;
			canvasGroup.DOFade(0, .3f).OnComplete(() =>
			{
				this.gameObject.SetActive(false);
				AudioManager.instance.PlaySFX("ClosePopUp");

			});
			GamePlayPanelUIManager.Instance.ActiveTime();
			GamePlayPanelUIManager.Instance.Appear();
			GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
		}
	}
	public void ExchangeTicket()
	{
		if(SaveSystem.instance.powerTicket >= 2)
		{
			SaveSystem.instance.powerTicket -=2;
			SaveSystem.instance.magicTiket += 1;
			SaveSystem.instance.SaveData();
		}
	}
}
