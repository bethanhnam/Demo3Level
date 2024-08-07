
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class clockFill : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public Image clockSlider;
    public float timeLimit = 60f;

    public float time;
    bool startTimer;

    float multiplierFactor;

    private void Start()
    {
        timeText.text = timeLimit.ToString();
        time = timeLimit;
        startTimer = false;
        //clockSlider.fillAmount = time * multiplierFactor;
    }

    [Button("startTimer")]
    public void StartTimer()
    {
        multiplierFactor = 1f/ timeLimit;
        startTimer = true;
        clockSlider.fillAmount = time * multiplierFactor;
    }
    private void Update()
    {
        if(!startTimer) return;

        if (time > 0) {
            time -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);

            clockSlider.fillAmount = time * multiplierFactor;
        }
        else
        {
            startTimer = false;
            time = 0;
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
            UIManagerNew.Instance.StartMiniGamePanel.Appear();
            UIManagerNew.Instance.StartMiniGamePanel.ChangeText("RETRY");
            UIManagerNew.Instance.StartMiniGamePanel.playButton.onClick.AddListener(UIManagerNew.Instance.MiniGamePlay.ReplayMinigame);
        }
    } 
    public void StopTimer()
    {
        if (startTimer) {
            startTimer = false;
        }
    }
    public void RestartTimer()
    {
        time = timeLimit;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        clockSlider.fillAmount = time * multiplierFactor;
    }
}
