using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThresholeController : MonoBehaviour
{
    public NamedGameObject[] thresholes;
    public CanvasGroup CanvasGroup;

    private string thresholeName;

    private Dictionary<string, GameObject> thresholeDictionary;

    void Start()
    {
        // Khởi tạo Dictionary và thêm các phần tử
        thresholeDictionary = new Dictionary<string, GameObject>();
        foreach (var namedObject in thresholes)
        {
            if (!thresholeDictionary.ContainsKey(namedObject.objectName))
            {
                thresholeDictionary.Add(namedObject.objectName, namedObject.gameObject);
            }
        }
    }

    public void Appear(Action action = null)
    {
        this.gameObject.SetActive(true);
        CanvasGroup.enabled = true;
        CanvasGroup.DOFade(1, 1).OnComplete(() =>
        {
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
            action?.Invoke();
        });
    }
    public void Disable()
    {
        if (thresholeDictionary.TryGetValue(thresholeName, out GameObject go))
        {
            go.gameObject.SetActive(true);
        }
        CanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.enabled = false;
            this.gameObject.SetActive(false);
        });
    }
    [Button("Show threshole")]
    public void showThreshole(string thresholeName)
    {
        this.thresholeName = thresholeName;
        Appear();
        // Ví dụ: sử dụng Dictionary để lấy GameObject
        if (thresholeDictionary.TryGetValue(thresholeName, out GameObject go))
        {
            go.gameObject.SetActive(true);
        }
    }
}
[System.Serializable]
public class NamedGameObject
{
    public string objectName;
    public GameObject gameObject;
}
