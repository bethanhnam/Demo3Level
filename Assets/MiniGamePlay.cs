using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MiniGamePlay : MonoBehaviour
{
    public static MiniGamePlay instance;

    public MiniItem[] miniItem;

    public Slider collectSlider;
    public Slider heartSlider;

    public float collectValue;
    public float heartValue;

    public TextMeshProUGUI maxCollectText;
    public TextMeshProUGUI collectText;

    public clockFill clockFill;
    public Sprite[] sprites;

    public CanvasGroup canvasGroup;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    public void Appear(Action action)
    {
        this.gameObject.SetActive(true);
        canvasGroup.DOFade(1, 0.9f).OnComplete(() =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            action();
        });
    }
    public void Disappear(Action action)
    {
        canvasGroup.DOFade(0, 0.35f).OnComplete(() =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            this.gameObject.SetActive(false);
            action();
        });
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetItem(int indexSprite,int Maxcollectvalue)
    {
        collectValue = 0;
        collectSlider.value = collectValue;
        SetMaxValue(Maxcollectvalue);
        for (int i = 0; i < miniItem.Length; i++)
        {
            miniItem[i].SetImage(sprites[indexSprite]);
            miniItem[i].itemImg.SetNativeSize();
        }
        DOVirtual.DelayedCall(0.6f, () => { 
        clockFill.StartTimer();
        });
    }
    public void ChangeCollectSliderValue()
    {
        AudioManager.instance.PlaySFX("FillUpSlider");
        collectSlider.DOValue(collectSlider.value + 1, 0.7f).OnComplete(() =>
        {
            collectValue += 1;
            collectText.text = collectValue.ToString();
            if (collectSlider.value >= collectSlider.maxValue)
            {
                //win minigame
                Debug.Log("win minigames");
            }
        });

    }
    public void ChangeHeartSlider()
    {
        collectSlider.DOValue(collectSlider.value - 1, 0.7f).OnComplete(() =>
        {
            collectValue += 1;
            collectText.text = collectValue.ToString();
            if (collectSlider.value >= collectSlider.maxValue)
            {
                //win minigame
                Debug.Log("win minigames");
            }
        });
    }
    public void SetMaxValue(int numOfItem)
    {
        collectSlider.maxValue = numOfItem;
        maxCollectText.text = numOfItem.ToString(); 
    }
}
