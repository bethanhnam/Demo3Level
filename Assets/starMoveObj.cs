using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class starMoveObj : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve curveScale;
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
            this.transform.position = SmoothPath(startPos, stepPos, endPos, (Time.time - startTime) / timeMove);
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

    public void MoveToFix(starMoveObj star,Vector3 pos, Vector3 targetAnchor, Action action)
    {

        if (!star.gameObject.activeSelf)
        {
            star.gameObject.SetActive(true);
        }
        star.transform.position = pos;
        star.transform.localScale = Vector3.one;

        Vector3 _direction = (pos - targetAnchor).normalized;
        float _distance = Vector2.Distance(targetAnchor, pos);
        if (_distance < 4)
        {
            _distance = 4;
        }

        timeMove = baseTimeMove * _distance;

        Vector3 targetScale = transform.localScale / 2f;
        star.transform.DOScale(targetScale, timeMove).SetEase(curveScale);

        Vector3 p1 = pos + _direction * (_distance / 3f);
        Vector3 centrPos = (pos + targetAnchor) / 2f;
        Vector3 d2 = Vector3.Cross(_direction, Vector3.forward);
        //	Vector3 stepPos = pos + new Vector3(1, 1,pos.z);
        stepPos = p1 + d2 * (_distance / 3f);
        startPos = pos;
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
}
