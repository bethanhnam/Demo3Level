using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FixItemUI : MonoBehaviour
{
    [SerializeField]
	private Animator animButton;
	[SerializeField]
	private CanvasGroup cvButton;

	[SerializeField]
	private Animator animImage;
	[SerializeField]
	private Image imgPic;

	private int appearButton = Animator.StringToHash("appear");
	private int appear = Animator.StringToHash("appear");
	private int disappearButton = Animator.StringToHash("Disappear");

	private int appearImage = Animator.StringToHash("appear");
	private int idleImage = Animator.StringToHash("idle");
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
        AudioManager.instance.PlaySFX("ItemAppear");
        animImage.Play(appear);
		DisplayPicture();
	}

	public void Close()
	{
		cvButton.blocksRaycasts = false;
		//animImage.Play(idleImage);
		//animButton.Play(disappearButton);
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
	public void DisplayPicture()
	{
		GameManagerNew.Instance.PictureUIManager.HiddenButton();
		GameManagerNew.Instance.PictureUIManager.Open();
	}

	public void ContinueBT()
	{
		Deactive();
		GameManagerNew.Instance.ItemMoveControl.MoveToFix(imgPic.transform.position, GameManagerNew.Instance.PictureUIManager.GetCurrentPosItem(),imgPic.sprite, () =>
			{
                if (GameManagerNew.Instance.PictureUIManager.hasWindow)
                {
                    GameManagerNew.Instance.PictureUIManager.ChangeReaction(0, "tremble_happy", false, GameManagerNew.Instance.PictureUIManager.hasWindow);
                }
                else
                {
                    GameManagerNew.Instance.PictureUIManager.ChangeReaction(0, "sad-happy", false, GameManagerNew.Instance.PictureUIManager.hasWindow);
                }
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
                GameManagerNew.Instance.PictureUIManager.EnableCV();
            });
		Close();
	}
}
