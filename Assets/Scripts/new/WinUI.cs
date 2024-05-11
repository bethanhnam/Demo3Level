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

	[SerializeField]
	private Animator animImage;
	[SerializeField]
	private Image imgPic;

	private int appearButton = Animator.StringToHash("appear");
	private int appear = Animator.StringToHash("appear");
	private int disappearButton = Animator.StringToHash("Disappear");

	private int appearImage = Animator.StringToHash("appear");
	private int idleImage = Animator.StringToHash("idle");

	[SerializeField]
	private RectTransform posImage;

	public RectTransform PosImage { get => posImage; }

	public void Appear(Sprite spr)
	{
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		cvButton.blocksRaycasts = false;
		animButton.Play(appearButton, 0, 0);
		//imgPic.transform.localScale = Vector3.zero;
		imgPic.sprite = spr;
		animImage.Play(appear);
		displayTime();
		DisplayPicture();
	}

	public void Close()
	{
		cvButton.blocksRaycasts = false;
		animImage.Play(idleImage);
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

	public void ContinueBT()
	{
		UIManagerNew.Instance.WinUI.Deactive();
		GameManagerNew.Instance.ItemMoveControl.MoveToFix(UIManagerNew.Instance.WinUI.imgPic.transform.position, GameManagerNew.Instance.PictureUIManager.GetCurrentPosItem(),imgPic.sprite, () =>
			{
				GameManagerNew.Instance.PictureUIManager.ChangeReaction(0, 1, false);
				UIManagerNew.Instance.ButtonMennuManager.DiactiveCVGroup();
				GameManagerNew.Instance.CreateParticleEF();
				GameManagerNew.Instance.ItemMoveControl.gameObject.SetActive(false);
				GameManagerNew.Instance.PictureUIManager.ChangeItemOnly(LevelManagerNew.Instance.LevelBase.Level);
				GameManagerNew.Instance.PictureUIManager.ChangeItem(GameManagerNew.Instance.PictureUIManager.Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].ObjunLock[GameManagerNew.Instance.Level]);
				AudioManager.instance.PlayMusic("MenuTheme");
				UIManagerNew.Instance.ChestSLider.ChangeValue(() =>
				{
					GameManagerNew.Instance.SetCompleteStory();
				});
			});
		UIManagerNew.Instance.WinUI.Close();
	}
}
