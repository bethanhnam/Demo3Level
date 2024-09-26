using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float TimeLeft;
    public bool TimerOn = false;
	bool hasScale = false;
	bool hasscale = false;

	public TextMeshProUGUI TimerText;

	private void Start()
	{
		
	}
	public void SetTimer(float time)
	{
		TimerOn = false;
        TimeLeft = time;
		hasScale = false;
		hasscale =false;
        int minutes = Mathf.FloorToInt(TimeLeft / 60);
        int seconds = Mathf.FloorToInt(TimeLeft % 60);
        TimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
	private void Update()
	{
		if (TimerOn)
		{
			if(TimeLeft > 0)
			{
				TimeLeft -= Time.deltaTime;
				TimerText.color = Color.white;
				if(TimeLeft <= 20)
				{
					
					Vector3 minScale = new Vector3(1f, 1f, 1f);
					Vector3 maxScale = new Vector3(2.2f, 2.2f, 1f);
					if (hasScale == false)
					{
						TimerText.transform.DOScale(maxScale, 0.3f).OnComplete(() =>
						{
							TimerText.transform.DOScale(minScale, 0.3f);
							hasScale = true;
						});
					}
					TimerText.color = Color.red;
				
					if (hasscale == false)
					{
						TimerText.transform.DOScale(maxScale, 0.3f).OnComplete(() =>
						{
							TimerText.transform.DOScale(minScale, 0.3f);
							hasscale = true;
						});
					}
				}
			}
			else
			{
				TimerOn = false;
				Debug.Log("Time is Up !");
				// hien pop lose
				TimeLeft = 0;
				TimerText.color = Color.red;
				GamePlayPanelUIManager.Instance.OpenLosePanel();
			}
			int minutes = Mathf.FloorToInt(TimeLeft / 60);
			int seconds = Mathf.FloorToInt(TimeLeft % 60);
			TimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
		}
	}
}
