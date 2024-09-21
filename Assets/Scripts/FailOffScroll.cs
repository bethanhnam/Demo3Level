using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class FailOffScroll : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public List<GameObject> items;

    public Image[] dots;

    public Color grayColor;

    public GridLayoutGroup layoutGroup;

    private Vector2 startDragPosition;

    private void Start()
    {
        SetActivate(dots[0]);
        SetDeactivate(dots[1]);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Lấy vị trí bắt đầu khi vuốt
        startDragPosition = eventData.pressPosition;
    }

    public void SetActivate(Image dot)
    {
        dot.color = Color.white;
        dot.transform.DOScale(.6f, 0.4f);
    }
    public void SetDeactivate(Image dot)
    {
        dot.color = grayColor;
        dot.transform.DOScale(.5f, 0.4f);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // Lấy vị trí kết thúc khi vuốt
        Vector2 endDragPosition = eventData.position;

        // Vuốt sang phải -> Cuộn về cuối
        if (startDragPosition.x > endDragPosition.x)
        {
            SetActivate(dots[1]);
            SetDeactivate(dots[0]);
            // Cuộn mượt sang vị trí cuối
            scrollRect.DOHorizontalNormalizedPos(1f, 0.5f).OnComplete(() =>
            {
                // Chuyển item đầu tiên xuống cuối để tạo hiệu ứng lặp
                layoutGroup.childAlignment = TextAnchor.MiddleRight;
                
            });
        }
        // Vuốt sang trái -> Cuộn về đầu
        else if (startDragPosition.x < endDragPosition.x)
        {
            SetActivate(dots[0]);
            SetDeactivate(dots[1]);
            // Cuộn mượt về đầu
            scrollRect.DOHorizontalNormalizedPos(0f, 0.5f).OnComplete(() =>
            {
                // Chuyển item cuối lên đầu để tạo hiệu ứng lặp
                layoutGroup.childAlignment = TextAnchor.MiddleLeft;
                
            });
        }
    }
}
