using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostMinigame : MonoBehaviour
{
    public SkeletonGraphic skeletonGraphic;
    public Transform targetTransform;
    public Vector3 defaultTransform;
    public Vector3 darkdDefaultTransform;
    public Transform reachTransform;

    public SpriteRenderer dark;

    public float distance;

    public float timeLimit = 45f;

    public float time;
    bool startTimer;

    bool isRunning = false;
    public bool hasReached = false;

    Tween runTween;
    Tween runTween1;
    private void Start()
    {
        time = timeLimit;
        dark.transform.position = new Vector3(defaultTransform.x+2, this.transform.position.y, 1);
        skeletonGraphic.transform.position = new Vector3(defaultTransform.x, this.transform.position.y, 1);

        startTimer = false;
        hasReached = false;
    }

    [Button("startTimer")]
    public void StartTimer()
    {
        dark.gameObject.SetActive(true);
        dark.transform.position = new Vector3(defaultTransform.x + 2, this.transform.position.y, 1);
        skeletonGraphic.gameObject.SetActive(true);
        startTimer = true;
        AudioManager.instance.PlaySFX("GhostAppear");
        skeletonGraphic.AnimationState.SetAnimation(0, "attack", false);
        DOVirtual.DelayedCall(1f, () =>
        {
            skeletonGraphic.AnimationState.SetAnimation(0, "move", true);
        });
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
        dark.transform.position = new Vector3(defaultTransform.x + 2, this.transform.position.y, 1);
        skeletonGraphic.transform.position =  new Vector3(defaultTransform.x,this.transform.position.y,1);
        distance = Vector2.Distance(new Vector2(targetTransform.position.x, 0), new Vector2(skeletonGraphic.transform.position.x, 0));
        startTimer = false;
    }
    public void MoveGhost(bool run)
    {
        if (run && isRunning == false)
        {
            isRunning = true;
            runTween = skeletonGraphic.transform.DOMoveX(targetTransform.position.x, time);
            runTween1 = dark.transform.DOMoveX(targetTransform.position.x, time);
        }
        else
        {
            if(run == false)
            {
                isRunning = false;
                if (runTween != null)
                    runTween.Pause();
                if (runTween1 != null)
                    runTween1.Pause();
            }
        }
    }
    public void CheckReachPoint()
    {
        if (time <= 25 && hasReached == false)
        {
            AudioManager.instance.PlaySFX("GhostAppear");
            hasReached = true;
            UIManagerNew.Instance.MiniGamePlay.MiniGameMaps[UIManagerNew.Instance.MiniGamePlay.selectedMinimap].characterStepBack.skeletonGraphic.AnimationState.SetAnimation(0, "evade", false);
            skeletonGraphic.AnimationState.SetAnimation(0, "attack", false);
            DOVirtual.DelayedCall(1f, () =>
            {
                UIManagerNew.Instance.MiniGamePlay.MiniGameMaps[UIManagerNew.Instance.MiniGamePlay.selectedMinimap].characterStepBack.skeletonGraphic.AnimationState.SetAnimation(0, "idle", true);
                skeletonGraphic.AnimationState.SetAnimation(0, "move", true);
            });
        }
    }
    
}
