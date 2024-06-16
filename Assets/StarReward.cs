using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarReward : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve curveScale;
    [SerializeField]
    private AnimationCurve curveMove;
    [SerializeField]
    private ParticleSystem starParticle;
    [SerializeField]
    private ParticleSystem fireWorkParticle;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 stepPos;

    [SerializeField]
    private float startTime;
    [SerializeField]
    private float timeMove;
    [SerializeField]
    private float baseTimeMove;

    private bool isMove;
    private Action a;

    private void Update()
    {
        if (isMove)
        {
            this.transform.DOMove(SmoothPath(startPos, stepPos, endPos, (Time.time - startTime) / timeMove), 0.05f).SetEase(curveMove);
            var x = Vector2.Distance(this.transform.position, endPos);
            if (x < 0.3f)
            {
                if (Time.time >= startTime + timeMove)
                {
                    isMove = false;
                    if (a != null)
                    {
                        a();
                        a = null;
                    }
                }
            }
        }
    }

    public void MoveToFix(StarReward star, Vector3 pos, Vector3 targetAnchor, Vector3 scaleTarget, Action action)
    {
        Debug.Log("targetAnchor " + targetAnchor);

        if (!star.gameObject.activeSelf)
        {
            star.gameObject.SetActive(true);
        }
        star.transform.position = pos;

        Vector3 _direction = (pos - targetAnchor).normalized;

        float _distance = Vector2.Distance(targetAnchor, pos);
        if (_distance < 4)
        {
            _distance = 4;
        }

        //timeMove = baseTimeMove * _distance;
        timeMove = .7f;
        
        star.transform.DOScale(scaleTarget, timeMove).SetEase(curveScale);

        Vector3 p1 = pos + _direction * (_distance / 3f);
        Vector3 centrPos = (pos + targetAnchor) / 2f;
        Vector3 d2 = Vector3.Cross(_direction, Vector3.forward);
        //	Vector3 stepPos = pos + new Vector3(1, 1,pos.z);
        stepPos = p1 + d2 * (_distance / 3f) - new Vector3(1f, 1.5f, 0); ;
        startPos = pos ;
        endPos = targetAnchor;

        startTime = Time.time;
        a = action;
        isMove = true;
    }

    [SerializeField]
    private int SmoothingLength;
    private Vector3 SmoothPath(Vector3 v, Vector3 v1, Vector3 v2, float _deltaTime)
    {
        return v1 + Mathf.Pow((1 - _deltaTime), 2) * (v - v1) + (_deltaTime * _deltaTime) * (v2 - v1);
    }
    public void ActiveGlowParticle(bool status)
    {
        starParticle.gameObject.SetActive(status);
    }
    public void ActiveFireWorkParticle(bool status)
    {
        fireWorkParticle.gameObject.SetActive(status);
    }
}
