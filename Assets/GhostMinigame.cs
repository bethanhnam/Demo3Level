using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMinigame : MonoBehaviour
{
    public SkeletonGraphic skeletonGraphic;
    public Transform targetTransform;
    public Transform defaultTransform;
    public Transform reachTransform;

    public float distance;

    public float timeLimit = 60f;

    public float time;
    bool startTimer;

    bool isRunning = false;
    public bool hasReached = false;

    Tween runTween;
    private void Start()
    {
        time = timeLimit;
        skeletonGraphic.transform.position = defaultTransform.position;
        startTimer = false;
        hasReached = false;
    }

    [Button("startTimer")]
    public void StartTimer()
    {
        skeletonGraphic.gameObject.SetActive(true);
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
            CheckReachPoint();
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
        if(runTween != null)
        {
            runTween.Kill();
        }
        time = timeLimit;
        skeletonGraphic.transform.position = defaultTransform.position;
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
    public void CheckReachPoint()
    {
        if (time <= 35 && hasReached == false)
        {
            hasReached = true;
            skeletonGraphic.AnimationState.SetAnimation(0, "attack", false);
            DOVirtual.DelayedCall(1f, () =>
            {
                skeletonGraphic.AnimationState.SetAnimation(0, "move", true);
            });
        }
    }
    
}
