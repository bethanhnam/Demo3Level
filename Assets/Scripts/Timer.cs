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
