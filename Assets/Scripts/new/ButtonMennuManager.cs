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
		AudioManager.instance.PlayMusic("MenuTheme");
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
		Close();
		UIManagerNew.Instance.DailyRWUI.Appear();
	}
	public void OpenSettingPanel()
	{
		Close();
		UIManagerNew.Instance.SettingPanel.Open();
	}
	public void OpenNonAdsPanel()
	{
		Close();
		UIManagerNew.Instance.NonAdsPanel.Open();
	}
	public void OpenShopPanel()
	{
		if(GameManagerNew.Instance.CurrentLevel != null)
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
}
