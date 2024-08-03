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
        if (this.thresholeName != null)
        {
            if (thresholeDictionary.TryGetValue(thresholeName, out GameObject go))
            {
                go.gameObject.SetActive(false);
                this.thresholeName = null;
            }
        }
        CanvasGroup.DOFade(0, 1f).OnComplete(() =>
        {
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.enabled = false;
            this.gameObject.SetActive(false);
        });
    }
    [Button("TestThreshole")]
    public void showThreshole(string thresholeName, Vector3 scale, Transform transform = null)
    {
        if (this.thresholeName != null)
        {
            if (thresholeDictionary.TryGetValue(this.thresholeName, out GameObject go))
            {
                go.gameObject.SetActive(false);
            }
        }
        SetPos(thresholeName, transform, scale);
        this.thresholeName = thresholeName;
        Appear();
        // Ví dụ: sử dụng Dictionary để lấy GameObject
        if (thresholeDictionary.TryGetValue(thresholeName, out GameObject go1))
        {
            go1.gameObject.SetActive(true);
            UIManagerNew.Instance.BlockPicCanvas.SetActive(false);
        }
    }
    public void showNextThreshole(string thresholeName)
    {
        if (this.thresholeName != null)
        {
            if (thresholeDictionary.TryGetValue(this.thresholeName, out GameObject go))
            {
                go.gameObject.SetActive(false);
            }
        }
        SetPos(thresholeName, null, Vector3.zero);
        this.thresholeName = thresholeName;
        Appear();
        // Ví dụ: sử dụng Dictionary để lấy GameObject
        if (thresholeDictionary.TryGetValue(thresholeName, out GameObject go1))
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                go1.gameObject.SetActive(true);
                UIManagerNew.Instance.BlockPicCanvas.SetActive(false);
            });
        }
    }

    public void SetPos(string thresholeName,Transform transform,Vector3 scale)
    {
        if (transform != null)
        {
            if (thresholeDictionary.TryGetValue(thresholeName, out GameObject go))
            {
                go.transform.GetChild(1).transform.position = transform.position;
               
            }
            if (scale != null)
            {
                if (thresholeDictionary.TryGetValue(thresholeName, out GameObject go1))
                {
                    go1.transform.GetChild(1).localScale = scale;
                }
            }
        }
        
    }
    public void ActiveBlock()
    {
        UIManagerNew.Instance.BlockPicCanvas.SetActive(true);
    }
}
[System.Serializable]
public class NamedGameObject
{
    public string objectName;
    public GameObject gameObject;
}
