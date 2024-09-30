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
    public GameObject[] itemPrefab;
    private Vector2 startDragPosition;

    public Image[] dots;

    public Color deactiveColor;

    public int packIndex = 0;

    public CanvasGroup canvasGroup;

    private void Start()
    {
        ActiveDot(dots[0]);
        DeactiveDot(dots[1]);
    }

    public void OnDrag(PointerEventData eventData)
    {
        startDragPosition = eventData.pressPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 endDragPosition = eventData.position;

        if (startDragPosition.x > endDragPosition.x)
        {
            AddItemToRight();
            RemoveItemFromLeft();
        }
        else if (startDragPosition.x < endDragPosition.x)
        {
            AddItemToLeft();
            RemoveItemFromRight();
        }
    }

    private void AddItemToRight()
    {
        canvasGroup.blocksRaycasts = false;
        float itemWidth = 1080f;
        GameObject newItem;
        if (packIndex == 0)
        {
            newItem = Instantiate(itemPrefab[0], content);
            packIndex = 1;
        }
        else
        {
            packIndex = 0;
             newItem = Instantiate(itemPrefab[1], content);
        }
        newItem.transform.SetAsLastSibling();
        content.sizeDelta = new Vector2(content.sizeDelta.x + itemWidth, content.sizeDelta.y);

        ActiveDot(dots[1]);
        DeactiveDot(dots[0]);

        scrollRect.DOHorizontalNormalizedPos(1f, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            canvasGroup.blocksRaycasts = true;
        });
    }

    private void AddItemToLeft()
    {
        canvasGroup.blocksRaycasts = false;
        float itemWidth = 1080f;
        GameObject newItem;
        if (packIndex == 0)
        {
            newItem = Instantiate(itemPrefab[0], content);
            packIndex = 1;
        }
        else
        {
            packIndex = 0;
            newItem = Instantiate(itemPrefab[1], content);
        }
        newItem.transform.SetAsFirstSibling();
        content.anchoredPosition = new Vector2(content.anchoredPosition.x - itemWidth, content.anchoredPosition.y);
        content.sizeDelta = new Vector2(content.sizeDelta.x + itemWidth, content.sizeDelta.y);

        ActiveDot(dots[0]);
        DeactiveDot(dots[1]);

        scrollRect.DOHorizontalNormalizedPos(0f, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            canvasGroup.blocksRaycasts = true;
        });
    }

    private void RemoveItemFromLeft()
    {
        if (content.childCount > 1)
        {
            GameObject firstItem = content.GetChild(0).gameObject;
            float itemWidth = 1080f;
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Destroy(firstItem);
                content.sizeDelta = new Vector2(content.sizeDelta.x - itemWidth, content.sizeDelta.y);
            });
        }
    }

    private void RemoveItemFromRight()
    {
        if (content.childCount > 1)
        {
            GameObject lastItem = content.GetChild(content.childCount - 1).gameObject;
            float itemWidth = 1080f;
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Destroy(lastItem);
                content.sizeDelta = new Vector2(content.sizeDelta.x - itemWidth, content.sizeDelta.y);
            });
        }
    }

    public void ActiveDot(Image dot)
    {
        dot.color = Color.white;
        dot.transform.DOScale(0.6f, 0.3f);
    }
    public void DeactiveDot(Image dot)
    {
        dot.color = deactiveColor;
        dot.transform.DOScale(0.5f, 0.3f);
    }
}
