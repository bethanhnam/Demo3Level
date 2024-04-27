using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRWUI : MonoBehaviour
{
	[SerializeField]
	private Animator animButton;
	[SerializeField]
	private CanvasGroup cvButton;

	private int appearButton = Animator.StringToHash("appear");
	private int disappearButton = Animator.StringToHash("Disappear");


	public void Appear()
	{
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		cvButton.enabled = false;
		animButton.Play(appearButton, 0, 0);
	}

	public void Close()
	{
		cvButton.blocksRaycasts = false;
		animButton.Play(disappearButton,0,0);
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
		cvButton.blocksRaycasts = true;
		if (!cvButton.enabled)
		{
			cvButton.enabled = true;
		}
	}

}
