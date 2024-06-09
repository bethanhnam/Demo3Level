using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
	[SerializeField]
	private Animator animButton;
	[SerializeField]
	private CanvasGroup cvButton;
	[SerializeField]
	private TextMeshProUGUI TimeText;

    private int appearButton = Animator.StringToHash("appear");
	private int disappearButton = Animator.StringToHash("Disappear");

	[SerializeField]
	private RectTransform posImage;

	public RectTransform PosImage { get => posImage; }

	public void Appear()
	{
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		cvButton.blocksRaycasts = false;
		animButton.Play(appearButton, 0, 0);
		//displayTime();
		DisplayPicture();
		RecivedRw();

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
	}
	public void displayTime()
	{
		int minutes = Mathf.FloorToInt((181 - GamePlayPanelUIManager.Instance.timer.TimeLeft) / 60);
		int seconds = Mathf.FloorToInt((181 - GamePlayPanelUIManager.Instance.timer.TimeLeft) % 60);
		TimeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
	}
	public void DisplayPicture()
	{
		GameManagerNew.Instance.PictureUIManager.HiddenButton();
		GameManagerNew.Instance.PictureUIManager.Open();

	}
	public void RecivedRw()
	{
		SaveSystem.instance.addCoin(10);
		SaveSystem.instance.addStar(1);
		SaveSystem.instance.SaveData();
	}
	public void MoveToAddRW()
	{

	}
	public void ContinueBT()
	{
		//Deactive();
		UIManagerNew.Instance.WinUI.Close();
		UIManagerNew.Instance.ButtonMennuManager.DiactiveCVGroup();
		AudioManager.instance.PlayMusic("MenuTheme");
        UIManagerNew.Instance.ButtonMennuManager.OpenRW();
        GameManagerNew.Instance.PictureUIManager.ChangeItemOnly(LevelManagerNew.Instance.LevelBase.Level,false);
        if (!UIManagerNew.Instance.ButtonMennuManager.gameObject.activeSelf)
        {
            UIManagerNew.Instance.ButtonMennuManager.Appear();
            if (GameManagerNew.Instance.PictureUIManager.picTutor != null)
            {
                GameManagerNew.Instance.PictureUIManager.picTutor.CheckHasFixed();
            }
        }
    }
}
