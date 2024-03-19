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

    public TextMeshProUGUI TimerText;

	private void Start()
	{
		
	}
	public void SetTimer(float time)
	{
		TimeLeft = time;	
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
					TimerText.color = Color.red;
					if (TimeLeft <= 10)
					{
						Vector3 minScale = new Vector3(1f, 1.1f, 1f);
						Vector3 maxScale = new Vector3(1.1f, 1.1f, 1f);
						TimerText.transform.DOScale(maxScale, 0.1f).OnComplete(() =>
						{
							TimerText.transform.DOScale(minScale, 0.1f);
						});
					}
				}
			}
			else
			{
				TimerOn = false;
				Debug.Log("Time is Up !");
				// hien pop lose
				UIManager.instance.gamePlayPanel.losePanel.Open();
				TimeLeft = 0;
				TimerText.color = Color.red;
			}
			int minutes = Mathf.FloorToInt(TimeLeft / 60);
			int seconds = Mathf.FloorToInt(TimeLeft % 60);
			TimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
		}
	}
}
