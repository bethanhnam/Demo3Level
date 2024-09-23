using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
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
    public bool isSwitching = false;
    private Coroutine autoScrollCoroutine;

    public Tween slide;

    private void Start()
    {
        ActiveDot(dots[0]);
        DeactiveDot(dots[1]);

        StartAutoScroll(); // Start the scrolling on initialization
    }

    private void OnEnable()
    {
        StartAutoScroll();
    }

    private void StartAutoScroll()
    {
        if (autoScrollCoroutine != null)
        {
            StopCoroutine(autoScrollCoroutine);
        }
        autoScrollCoroutine = StartCoroutine(AutoScrollRight());
    }

    private void StopAutoScroll()
    {
        if (autoScrollCoroutine != null)
        {
            StopCoroutine(autoScrollCoroutine);
            autoScrollCoroutine = null;
        }
    }

    private IEnumerator AutoScrollRight()
    {
        while (true)
        {
            yield return new WaitForSeconds(8f);

            if (!isSwitching)
            {
                isSwitching = true;
                AddItemToRight();
                RemoveItemFromLeft();

                slide = scrollRect.DOHorizontalNormalizedPos(1f, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    isSwitching = false;
                });
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 endDragPosition = eventData.position;

        if (!isSwitching)
        {
            StopAutoScroll(); // Stop the auto scroll when a manual swipe occurs

            if (slide != null)
            {
                slide.Kill();
                slide = null;
            }

            if (startDragPosition.x > endDragPosition.x)
            {
                isSwitching = true;
                AddItemToRight();
                RemoveItemFromLeft();
                isSwitching = false;
            }
            else if (startDragPosition.x < endDragPosition.x)
            {
                isSwitching = true;
                AddItemToLeft();
                RemoveItemFromRight();
                isSwitching = false;
            }

            StartAutoScroll(); // Restart auto-scroll after manual interaction
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        startDragPosition = eventData.pressPosition;
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
            newItem = Instantiate(itemPrefab[1], content);
            packIndex = 0;
        }

        newItem.transform.SetAsLastSibling();
        content.sizeDelta = new Vector2(content.sizeDelta.x + itemWidth, content.sizeDelta.y);
        SwitchDot(packIndex);

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
            newItem = Instantiate(itemPrefab[1], content);
            packIndex = 0;
        }

        newItem.transform.SetAsFirstSibling();
        content.anchoredPosition = new Vector2(content.anchoredPosition.x - itemWidth, content.anchoredPosition.y);
        content.sizeDelta = new Vector2(content.sizeDelta.x + itemWidth, content.sizeDelta.y);
        SwitchDot(packIndex);

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
        dot.transform.DOScale(0.4f, 0.3f);
    }

    public void DeactiveDot(Image dot)
    {
        dot.color = deactiveColor;
        dot.transform.DOScale(0.3f, 0.3f);
    }

    public void SwitchDot(int num)
    {
        if (num == 0)
        {
            ActiveDot(dots[0]);
            DeactiveDot(dots[1]);
        }
        else
        {
            ActiveDot(dots[1]);
            DeactiveDot(dots[0]);
        }
    }
}
