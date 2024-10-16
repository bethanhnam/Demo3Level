﻿using Spine.Unity;
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

	public void Appear()
	{
		Debug.Log("hiện win UI");
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		animButton.Play(appearButton, 0, 0);

		closeWindow();
        FirebaseAnalyticsControl.Instance.LogEventLevelStatus(LevelManagerNew.Instance.stage,LevelStatus.win);
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
	public void CloseBeforeWinUI()
	{
		UIManagerNew.Instance.GamePlayPanel.Close();
    }

	public void AppearWinGame() {
        UIManagerNew.Instance.WinUI.Appear();
		AudioManager.instance.musicSource.Stop();
		AudioManager.instance.PlaySFX("Winpop");
		
	}
	public void OpenWindow()
	{
		completeSkeleton.AnimationState.SetAnimation(0, completeSkeletonStage[1], false);
    }
	public void closeWindow()
	{
        completeSkeleton.startingAnimation = completeSkeletonStage[0];
    }

}
