using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRWUI : MonoBehaviour
{
	[SerializeField]
	private Animator animButton;
	[SerializeField]
	private CanvasGroup cvButton;
	[SerializeField]
	private GameObject panelBoard;
    [SerializeField]
    private Image backGroundImg;


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
        AudioManager.instance.PlaySFX("OpenPopUp");
    }

	public void Close()
	{
		cvButton.blocksRaycasts = false;
		animButton.Play(disappearButton,0,0);
        AudioManager.instance.PlaySFX("ClosePopUp");
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
