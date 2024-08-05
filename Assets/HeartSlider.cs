using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeartSlider : MonoBehaviour
{
    public float timeLimit = 60f;

    public float time;
    bool startTimer;

    float multiplierFactor;
    public Slider heartSlider;
    private void Start()
    {
        time = timeLimit;
        heartSlider.maxValue = time;
        heartSlider.value = time;
        startTimer = false;
    }

    [Button("startTimer")]
    public void StartTimer()
    {
        multiplierFactor = 1f / timeLimit;
        startTimer = true;
        heartSlider.value = time * multiplierFactor;
    }
    private void Update()
    {
        if (!startTimer) return;

        if (time > 0)
        {
            time -= Time.deltaTime;
            heartSlider.value = time;
        }
        else
        {
            startTimer = false;
            time = 0;
            heartSlider.value = 0;
            //show lose minigame popup
            Debug.Log("lose minigame");
        }
    }
    public void StopTimer()
    {
        if (startTimer)
        {
            startTimer = false;
        }
    }
    public void RestartTimer()
    {
        time = timeLimit;
        heartSlider.maxValue = time;
        heartSlider.value = time;
        startTimer = false;
    }
}
