using Spine.Unity;
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

	public string[] completeSkeletonStage = {"idle_completed","completed"};
	public SkeletonGraphic completeSkeleton;

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
		UIManagerNew.Instance.WinUI.Appear();
		AudioManager.instance.musicSource.Stop();
		AudioManager.instance.PlaySFX("Winpop");
		GameManagerNew.Instance.PictureUIManager.Open();
	}
	public void OpenWindow()
	{
		completeSkeleton.AnimationState.SetAnimation(0, completeSkeletonStage[1], false);
    }
	IEnumerator closeWindow()
	{
		yield return new WaitForSeconds(1f);
        completeSkeleton.startingAnimation = completeSkeletonStage[0];
    }

}
