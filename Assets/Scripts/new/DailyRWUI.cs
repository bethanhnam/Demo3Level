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
    private int idleButton = Animator.StringToHash("DailyPanel");
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
	public void PlayIdleState()
	{
        animButton.Play(idleButton, 0, 0);
    }

	public void Close()
	{
		cvButton.blocksRaycasts = false;
		animButton.Play(disappearButton,0,0);
        AudioManager.instance.PlaySFX("ClosePopUp");
        Debug.Log("chạy qua dailyRW");
    }

	public void Deactive()
	{
			gameObject.SetActive(false);
        Debug.Log("đã tắt dailyRW");
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
