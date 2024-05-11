using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteUI : MonoBehaviour
{
	[SerializeField]
	private Animator animButton;

	private int appearButton = Animator.StringToHash("appear");
	private int disappearButton = Animator.StringToHash("disappear");

	private Sprite spr;

	public void Appear(Sprite _spr)
	{
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		animButton.Play(appearButton, 0, 0);
		spr = _spr;
	}

	public void Close()
	{
		animButton.Play(disappearButton);
	}

	public void Deactive()
	{
		if (gameObject.activeSelf)
		{
			gameObject.SetActive(false);
		}
		
	}


	public void AppearWinGame() {
		UIManagerNew.Instance.GamePlayPanel.Close(true);
		UIManagerNew.Instance.WinUI.Appear(spr);
		AudioManager.instance.musicSource.Stop();
		AudioManager.instance.PlaySFX("Winpop");
		GameManagerNew.Instance.PictureUIManager.Open();
	}
}
