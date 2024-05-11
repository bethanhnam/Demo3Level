using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestSLider : MonoBehaviour
{
    public Slider mySlider;
	public GameObject present;
	public TextMeshProUGUI strikeScore;
	public TextMeshProUGUI maxValue;
	public Vector3 defauPos;

	private void Start()
	{
		defauPos = present.transform.position;
	}
	public void ChangeValue(Action action)
	{
		AudioManager.instance.PlaySFX("FillUpSlider");
		present.GetComponent<Animator>().SetBool("Shaking", true);
		mySlider.DOValue(mySlider.value + 1, 0.3f).OnComplete(() =>
		{
			present.GetComponent<Animator>().SetBool("Shaking", false);
			mySlider.value = (Mathf.Round(mySlider.value));
			strikeScore.text = (Mathf.Round(mySlider.value)).ToString();
			UIManagerNew.Instance.ButtonMennuManager.ActiveCVGroup();
			action();
		});
		
	}
	public void SetMaxValue(PictureUIManager pictureUIManager)
	{
		int max = 0;
		for(int i = 0; i< pictureUIManager.Stage.Length;i++)
		{
			max += pictureUIManager.Stage[i].ObjLock.Length;
		}
		mySlider.maxValue = max;
		maxValue.text = mySlider.maxValue.ToString();
	}
	public void SetCurrentValue(int value)
	{
		mySlider.value = value;
		strikeScore.text = (Mathf.Round(mySlider.value)).ToString();
	}
	public void returnPos()
	{
		present.transform.position = defauPos;
	}
}
