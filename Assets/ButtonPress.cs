using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button btn;
    private float scaleRate = 0.7f;

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
            transform.DOScale(minScale, .2f).SetEase(Ease.Linear).OnComplete(() =>
            {
            });
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