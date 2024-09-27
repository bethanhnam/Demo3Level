using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinReward : MonoBehaviour
{
    public BezierCurve bezierCurve;
    [SerializeField]
    private AnimationCurve curveMove;

    public Transform startPoint;
    public Transform endPoint;

    public bool hasComplete;

    void Start()
    {

    }
    public void MoveToDes(typeOfReward typeOfReward,Transform startPoint,Transform endPoint, float duration, float targetScale, Action action)
    {
        // Vẽ đường cong bằng LineRenderer
        bezierCurve = new BezierCurve();
        if (typeOfReward == typeOfReward.WinUI)
        {
            bezierCurve.SetPosForWinUI(startPoint.position, endPoint.position);
        }
        if (typeOfReward == typeOfReward.DailyRewardGold)
        {
            bezierCurve.SetPosForDailyRewardGold(startPoint.position, endPoint.position);
        }
        if (typeOfReward == typeOfReward.DailyRewardBooster)
        {
            bezierCurve.SetPosForDailyRewardBooster(startPoint.position, endPoint.position);
        }
        if (typeOfReward == typeOfReward.GiveAwayItem)
        {
            bezierCurve.SetPosForGiveAwayItem(startPoint.position, endPoint.position);
        }
        MoveObjectAlongCurve(targetScale, duration, action);
    }

    //public void SetFor
    public void MoveObjectAlongCurve(float targetScale, float duration, Action onCompleteAction)
    {
        Sequence moveSequence = DOTween.Sequence();
        this.transform.DOScale(targetScale, duration);
        // Lấy các điểm dọc theo đường cong
        Vector3[] points = bezierCurve.GetSegments(50);

        for (int i = 0; i < points.Length; i++)
        {
            moveSequence.Append(this.transform.DOMove(points[i], duration / points.Length).SetEase(Ease.Linear));
        }

        // Thực hiện hành động khi di chuyển hoàn thành
        moveSequence.OnComplete(() => onCompleteAction?.Invoke());

        moveSequence.Play();
    }

    public enum typeOfReward
    {
        WinUI,
        StarWinUI,
        DailyRewardGold,
        DailyRewardBooster,
        GiveAwayItem
    }
}
