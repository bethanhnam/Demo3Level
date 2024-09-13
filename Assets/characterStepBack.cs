using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterStepBack : MonoBehaviour
{
    public SkeletonGraphic skeletonGraphic;
    public Transform targetTransform;
    public Transform defaultTransform;
    public Vector3 winTargetTransform;

    public float distance;

    public float timeLimit = 5f;

    public float time;
    bool startTimer;

    bool isRunning = false;

    Tween runTween2;
    private void Start()
    {
        time = timeLimit;
        skeletonGraphic.transform.position = defaultTransform.position;
        skeletonGraphic.AnimationState.SetAnimation(0, "move", true);
        distance = Vector2.Distance(targetTransform.position, skeletonGraphic.transform.position);
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
            if (runTween2 != null)
            {
                MoveCharacter(false);
            }
            return;
        }

        if (time > 0)
        {
            time -= Time.deltaTime;
            MoveCharacter(true);
        }
        else
        {
            startTimer = false;
            time = 0;
            skeletonGraphic.transform.position = new Vector3(targetTransform.position.x, skeletonGraphic.transform.position.y, 1);
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
        skeletonGraphic.AnimationState.SetAnimation(0, "move", true);
        skeletonGraphic.transform.position = defaultTransform.position;
        distance = Vector2.Distance(new Vector2(targetTransform.position.x, 0), new Vector2(skeletonGraphic.transform.position.x, 0));
        startTimer = false;
    }
    public void MoveCharacter(bool run)
    {
        if (run && isRunning == false)
        {
            isRunning = true;
            runTween2 = skeletonGraphic.transform.DOMoveX(targetTransform.position.x, time).OnComplete(() =>
            {
                skeletonGraphic.AnimationState.SetAnimation(0, "idle", true);
                if (runTween2 != null)
                {
                    runTween2.Kill();
                }
            });
        }
        else
        {
            if (run == false)
            {
                isRunning = false;
                if (runTween2 != null)
                    runTween2.Pause();
            }
        }
    }
    public void changeEmo()
    {
        if (Vector2.Distance(skeletonGraphic.transform.position,targetTransform.position) < 0.5f){
            skeletonGraphic.AnimationState.SetAnimation(0, "Idle", true);
            if(runTween2 != null)
            {
                runTween2.Kill();
            }
        }
    }
    public void GoToWin(Action action)
    {
        skeletonGraphic.AnimationState.SetAnimation(0, "move", true);
        skeletonGraphic.transform.DOMoveX(winTargetTransform.x, 3f).OnComplete(() =>
        {
            action();
        });
    }
    public void GotCatched()
    {
        skeletonGraphic.AnimationState.SetAnimation(0, "lose2", false);
    }
}
