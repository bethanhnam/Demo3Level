
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
    public float timeLimit = 45f;

    public float time;
    bool startTimer;
    bool hasAlarm;

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
            
            if(time <= 15)
            {
                UIManagerNew.Instance.MiniGamePlay.alertImage.ShowAlert();
                if (!hasAlarm)
                {
                    hasAlarm = true;
                    InvokeRepeating("AlarmBySound", 0, 5);
                }
            }
            if (time <= 5)
            {
                UIManagerNew.Instance.MiniGamePlay.closeBbutton.gameObject.SetActive(false);
            }
        }
        else
        {
            AudioManager.instance.sfxSource.volume = 0.7f;
            hasAlarm = false;
            CancelInvoke("AlarmBySound");
            UIManagerNew.Instance.MiniGamePlay.alertImage.DisableAlert();
            startTimer = false;
            time = 0;
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
            //lose minigame
            UIManagerNew.Instance.MiniGamePlay.closeBbutton.gameObject.SetActive(true);
            if (MiniGameStage.Instance.numOfIronPlates <= 0)
            {
                return;
            }
            else
            {
                MiniGameStage.Instance.canInteract = false;
                AudioManager.instance.PlaySFX("LosePop");
                UIManagerNew.Instance.MiniGamePlay.StopAllActionInMiniGame();
                UIManagerNew.Instance.MiniGamePlay.FailMiniGame.Appear(() =>
                {
                    UIManagerNew.Instance.StartMiniGamePanel.Appear();
                    UIManagerNew.Instance.StartMiniGamePanel.ShowRetry();
                    UIManagerNew.Instance.StartMiniGamePanel.AddReplay();
                });
            }
        }
    } 
    public void StopTimer()
    {
        if (startTimer) {
            startTimer = false;
        }
        CancelInvoke("AlarmBySound");
    }
    public void RestartTimer()
    {
        startTimer = false;
        time = timeLimit;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        clockSlider.fillAmount = time * multiplierFactor;
        CancelInvoke("AlarmBySound");
    }

    public void AlarmBySound()
    {
        AudioManager.instance.PlaySFX("Alarm2");
        AudioManager.instance.sfxSource.volume = 0.4f;
    }
}
