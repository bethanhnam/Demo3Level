using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeartSlider : MonoBehaviour
{
    public float timeLimit = 45f;

    public float time;
    public float counttime;
    bool startTimer;

    float multiplierFactor;
    public Slider heartSlider;
    private void Start()
    {
        counttime = 0;
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
            counttime += Time.deltaTime;
            heartSlider.value = time;
            MiniGamePlay.instance.MiniGameMaps[MiniGamePlay.instance.selectedMinimap].frozenImg.color = new Color(1, 1, 1, counttime/45);
        }
        else
        {
            startTimer = false;
            counttime = 0;
            time = 0;
            heartSlider.value = 0;
            MiniGamePlay.instance.MiniGameMaps[MiniGamePlay.instance.selectedMinimap].frozenImg.color = new Color(1, 1, 1, 1);
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
        counttime = 0;
        time = timeLimit;
        heartSlider.maxValue = time;
        heartSlider.value = time;
        startTimer = false;
    }
}
