using DG.Tweening;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStoryImage : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve curveScale;
    [SerializeField]
    private AnimationCurve curveMove;

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
            if (Time.time >= startTime + timeMove + .3f)
            {
                isMove = false;
                if (a != null)
                {
                    a();
                    a = null;
                }
            }
            else
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

    public void MoveToFix(float time, Vector3 pos, Vector3 targetAnchor, Vector3 scaleTarget, Action action)
    {
        Debug.Log("targetAnchor " + targetAnchor);

        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
        }
        this.transform.position = pos;

        Vector3 _direction = -(pos - targetAnchor).normalized;

        float _distance = Vector2.Distance(targetAnchor, pos);
        if (_distance < 4)
        {
            _distance = 4;
        }

        //timeMove = baseTimeMove * _distance;
        timeMove = time;

        this.transform.DOScale(scaleTarget, timeMove).SetEase(curveScale);

        Vector3 p1 = pos + _direction * (_distance / 3f);
        Vector3 centrPos = (pos + targetAnchor) / 2f;
        Vector3 d2 = Vector3.Cross(_direction, Vector3.forward);
        //	Vector3 stepPos = pos + new Vector3(1, 1,pos.z);
        stepPos = p1 + d2 * (_distance / 3f) + SetPos(targetAnchor) * new Vector3(SetPos(targetAnchor) *5f, -6f, 0); ;
        startPos = pos;
        endPos = targetAnchor;

        startTime = Time.time;
        a = action;
        isMove = true;
    }
     public int SetPos(Vector3 targetAnchor)
    {
        int x = 1;

        Vector3 transform = targetAnchor;
        if (this.transform.position.x < targetAnchor.x) {
            x = -1;
        }
        return x;
    }

    [SerializeField]
    private int SmoothingLength;
    private Vector3 SmoothPath(Vector3 v, Vector3 v1, Vector3 v2, float _deltaTime)
    {
        return v1 + Mathf.Pow((1 - _deltaTime), 2) * (v - v1) + (_deltaTime * _deltaTime) * (v2 - v1);
    }
}
