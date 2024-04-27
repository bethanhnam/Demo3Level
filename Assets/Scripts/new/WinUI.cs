using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
	[SerializeField]
	private Animator animButton;
	[SerializeField]
	private CanvasGroup cvButton;

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

}
