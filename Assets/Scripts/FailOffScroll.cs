using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class FailOffScroll : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public ScrollRect content;

    public Image[] Dots;
    public int selectedPack = 0;

    private Vector2 startDragPosition;

    public void OnDrag(PointerEventData eventData)
    {
        // Lấy vị trí bắt đầu của lần vuốt
        if (eventData.pressPosition != null)
        {
            startDragPosition = eventData.pressPosition;
        }
    }

    private void Start()
    {
        ActiveColor(Dots[0]);
    }

    public void ActiveColor(Image image)
    {
        image.color = Color.white;
    }
    public void DiactiveColor(Image image)
    {
        image.color = new Color(101, 101, 101, 100);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // Lấy vị trí kết thúc của lần vuốt
        Vector2 endDragPosition = eventData.position;

        // Tính toán hướng vuốt (trái hoặc phải)
        if (startDragPosition.x > endDragPosition.x)
        {
            if (selectedPack == 0)
            {
                // Vuốt sang phải -> Cuộn về cuối
                content.DOHorizontalNormalizedPos(1f, 0.5f).SetEase(Ease.OutCubic);
                selectedPack = 1;
                ActiveColor(Dots[1]);
                DiactiveColor(Dots[0]);
            }
            else
            {
                content.transform.GetChild(1).SetAsFirstSibling();
                content.horizontalNormalizedPosition = 0;
                content.DOHorizontalNormalizedPos(1f, 0.5f).SetEase(Ease.OutCubic);
                selectedPack = 0;
                ActiveColor(Dots[0]);
                DiactiveColor(Dots[1]);
            }
        }
        else if (startDragPosition.x < endDragPosition.x)
        {
            if (selectedPack == 1)
            {
                // Vuốt sang trái -> Cuộn về đầu
                content.DOHorizontalNormalizedPos(0f, 0.5f).SetEase(Ease.OutCubic);
                selectedPack = 0;
                ActiveColor(Dots[0]);
                DiactiveColor(Dots[1]);
            }
            else
            {
                content.transform.GetChild(0).SetAsFirstSibling();
                content.horizontalNormalizedPosition = 0;
                content.DOHorizontalNormalizedPos(0f, 0.5f).SetEase(Ease.OutCubic);
                selectedPack = 1;
                ActiveColor(Dots[1]);
                DiactiveColor(Dots[0]);
            }
        }
    }
}
