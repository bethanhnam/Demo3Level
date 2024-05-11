using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMennuManager : MonoBehaviour
{
	[SerializeField]
	private Animator animButton;
	[SerializeField]
	private CanvasGroup cvButton;

	private int appearButton = Animator.StringToHash("appear");
	private int disappearButton = Animator.StringToHash("disappear");

	public void Appear()
	{
		if (GameManagerNew.Instance.PictureUIManager.gameObject.activeSelf == false)
		{
			GameManagerNew.Instance.PictureUIManager.Open();
		}
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		cvButton.blocksRaycasts = false;
		animButton.Play(appearButton, 0, 0);
	}
	public void Close()
	{
		cvButton.blocksRaycasts = false;
		animButton.Play(disappearButton);
		
	}

	public void Deactive()
	{
		if (gameObject.activeSelf)
		{
			gameObject.SetActive(false);
		}
	}

	public void ActiveCVGroup()
	{
		if (!cvButton.blocksRaycasts)
		{
			cvButton.blocksRaycasts = true;
		}
		if (AudioManager.instance.musicSource.isPlaying)
		{
			if (AudioManager.instance.musicSource.clip.name != "MenuTheme")
			{
				AudioManager.instance.PlayMusic("MenuTheme");
			}
		}

		FirebaseAnalyticsControl.Instance.LogEventMenuPanelAccessSuccessfully(1);
	}
	public void DiactiveCVGroup()
	{
		if (cvButton.blocksRaycasts)
		{
			cvButton.blocksRaycasts = false;
		}
	}
	public void OpenDailyRW()
	{
		//GameManagerNew.Instance.ClosePicture(false);
		Close();
		UIManagerNew.Instance.DailyRWUI.Appear();
	}
	public void OpenCongratPanel()
	{
		//GameManagerNew.Instance.ClosePicture(false);
		Close();
		UIManagerNew.Instance.CongratPanel.Open();
	}
	public void OpenCompletePanel()
	{
		//GameManagerNew.Instance.ClosePicture(false);
		Close();
		UIManagerNew.Instance.CompletePanel.Open();
	}
	public void OpenSettingPanel()
	{
		//GameManagerNew.Instance.ClosePicture(false);
		Close();
		UIManagerNew.Instance.SettingPanel.Open();
	}
	public void OpenRattingPanel()
	{
		//GameManagerNew.Instance.ClosePicture(false);
		Close();
		UIManagerNew.Instance.RattingPanel.Open();
	}
	public void OpenNonAdsPanel()
	{
		//GameManagerNew.Instance.ClosePicture(false);
		Close();
		UIManagerNew.Instance.NonAdsPanel.Open();
	}
	public void OpenShopPanel()
	{
		//GameManagerNew.Instance.ClosePicture(false);
		if (GameManagerNew.Instance.CurrentLevel != null)
		{
			GameManagerNew.Instance.CloseLevel(false);
		}
		Close();
		UIManagerNew.Instance.ShopPanel.Open();
	}
	public void DisappearDailyRW()
	{
		Appear();
		UIManagerNew.Instance.DailyRWUI.Close();
	}

	public void DisplayWin()
	{
		UIManagerNew.Instance.CompleteImg.gameObject.SetActive(true);
	}
	public void PlayButton() {
		int level = DataLevelManager.Instance.GetLevel();
		GameManagerNew.Instance.CreateLevel(level);
		Close();
	}
	public void DisPlayPresent()
	{
		Vector3 startPos = UIManagerNew.Instance.ChestSLider.present.transform.position;
		UIManagerNew.Instance.ChestSLider.present.transform.DOMove(Vector3.zero, 1f).OnComplete(() =>
		{
			UIManagerNew.Instance.ChestSLider.returnPos();
			UIManagerNew.Instance.ButtonMennuManager.OpenCongratPanel();
		});
	}
}
