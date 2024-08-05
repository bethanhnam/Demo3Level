using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMinigame : MonoBehaviour
{
    public SkeletonGraphic skeletonGraphic;
    public Transform targetTransform;
    public Vector3 defaultTransform;

    public float distance;

    public float timeLimit = 60f;

    public float time;
    bool startTimer;

    bool isRunning = false;

    Tween runTween;
    private void Start()
    {
        time = timeLimit;
        defaultTransform = skeletonGraphic.transform.position;
        distance = Vector2.Distance(targetTransform.position,skeletonGraphic.transform.position);
        startTimer = false;
    }

    [Button("startTimer")]
    public void StartTimer()
    {
        startTimer = true;
    }
    private void Update()
    {
        if (!startTimer)
        {
            if(runTween != null)
            {
                MoveGhost(false);
            }
            return;
        }

        if (time > 0)
        {
            time -= Time.deltaTime;
            MoveGhost(true);
        }
        else
        {
            startTimer = false;
            time = 0;
            skeletonGraphic.transform.position = new Vector3 (targetTransform.position.x, skeletonGraphic.transform.position.y,1);  
            //show lose minigame popup
            Debug.Log("lose minigame");
        }
    }
    [Button("stop")]
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
        skeletonGraphic.transform.position = defaultTransform;
        distance = Vector2.Distance(new Vector2(targetTransform.position.x, 0), new Vector2(skeletonGraphic.transform.position.x, 0));
        startTimer = false;
    }
    public void MoveGhost(bool run)
    {
        if (run && isRunning == false)
        {
            isRunning = true;
            runTween = skeletonGraphic.transform.DOMoveX(targetTransform.position.x, time);
        }
        else
        {
            if(run == false)
            {
                isRunning = false;
                if (runTween != null)
                    runTween.Pause();
            }
        }
    }
}
