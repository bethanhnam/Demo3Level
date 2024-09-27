using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class ObjectMoverAlongCurve : MonoBehaviour
{
    public BezierCurve bezierCurve;
    [SerializeField]
    private AnimationCurve curveMove;

    public Transform startPoint;
    public Transform endPoint;
    public float duration;
    public float targetScale;

    public bool hasComplete;

    void Start()
    {
       
    }

    [Button("MoveToDes")]
    public void MoveToDes()
    {
        // Vẽ đường cong bằng LineRenderer
        bezierCurve = new BezierCurve();
        Vector3[] points = bezierCurve.GetSegments(50);

        MoveObjectAlongCurve(targetScale,duration,null);
    }
    public void MoveObjectAlongCurve(float targetScale,float duration, Action onCompleteAction)
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
}