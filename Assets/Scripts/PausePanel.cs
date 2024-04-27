using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

[RequireComponent(typeof(CanvasGroup))]
public class PausePanel : MonoBehaviour
{
	public bool isdeleting;
	public bool isdeletingIron;
	public CanvasGroup canvasGroup;
	public void Home()
	{
		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.ingame_pause, () =>
		{
			this.gameObject.SetActive(false);
			Stage.Instance.ResetBooster();
			GameManagerNew.Instance.PictureUIManager.Open();
			UIManagerNew.Instance.ButtonMennuManager.Appear();
			canvasGroup.DOFade(0, 0.5f).OnComplete(() =>
			{
				Destroy(GameManagerNew.Instance.CurrentLevel.gameObject);
			});
		}, null);
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{

			this.gameObject.SetActive(true);
			canvasGroup.alpha = 1;
			if (Stage.Instance.isDeteleting)
			{
				isdeleting = true;
			}
			//if (Stage.Instance.deletingIron)
			//{
			//	isdeletingIron = true;
			//}
			//UIManager.instance.DeactiveTime();
			this.GetComponent<CanvasGroup>().alpha = 0;
			this.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
			this.GetComponent<RectTransform>().DOScale(new Vector3(1, 1, 1), 0.1f).OnComplete(() =>
			{
				
			});

		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			this.GetComponent<CanvasGroup>().DOFade(0, 0.1f);
			GamePlayPanelUIManager.Instance.AppearForReOpen();
			this.GetComponent<RectTransform>().DOScale(new Vector3(.8f, .8f, 1), 0.1f).OnComplete(() =>
			{
				canvasGroup.DOFade(0, .2f);
				this.gameObject.SetActive(false);
				GamePlayPanelUIManager.Instance.ActiveTime();
				//UIManager.instance.ActiveTime();
				//UIManager.instance.gamePlayPanel.backFromPause = true;
			});
			
		}
	}
	public void Retry()
	{
		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.ingame_pause, () =>
		{
			Close();
			Stage.Instance.ResetBooster();
			GamePlayPanelUIManager.Instance.AppearForReOpen();
			GameManagerNew.Instance.Replay();
			//UIManager.instance.gamePlayPanel.backFromPause = false;
		},null);
		
	}
}
