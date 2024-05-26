using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button btn;
    [SerializeField][Range(0, 1)] private float scaleRate = 0.8f;

    private Vector3 _scaleDefault;

    private void OnValidate()
    {
        if (btn == null)
            btn = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _scaleDefault = btn.gameObject.transform.localScale;
    }

    private void OnDisable()
    {

        gameObject.transform.localScale = _scaleDefault;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (btn != null && btn.interactable)
        {
            var minScale = _scaleDefault * scaleRate;
            transform.DOScale(minScale, .2f).SetEase(Ease.Linear);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (btn != null && btn.interactable)
        {
            transform.DOScale(_scaleDefault, .3f).SetEase(Ease.InOutBack);
        }
    }
}