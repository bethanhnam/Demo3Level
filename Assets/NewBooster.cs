using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewBooster : MonoBehaviour
{
    //value
    public Sprite[] sprites;
    public String[] descriptions;
    public String[] itemNames;

    //properties
    public Image Image;
    public TextMeshProUGUI description;
    public TextMeshProUGUI itemName;

    public CanvasGroup canvasGroup;
    public Animator animator;
    public void Appear()
    {
        AudioManager.instance.PlaySFX("NewBooster");
        this.gameObject.SetActive(true);
        canvasGroup.enabled = true;
        canvasGroup.DOFade(1, 1).OnComplete(() =>
        {
            animator.enabled = true;
            if (Stage.Instance != null && Stage.Instance.gameObject.activeSelf)
            {
                Stage.Instance.canInteract = false;
            }
        });
    }
    public void Disappear()
    {
        canvasGroup.DOFade(0, 1).OnComplete(() =>
        {
            canvasGroup.enabled = false;
            animator.Play("NewBoosterDisappear");
            this.gameObject.SetActive(false);
        });
    }
    public void ShowThreshole()
    {
        if (UIManagerNew.Instance.GamePlayPanel.gameObject.activeSelf)
        {
            UIManagerNew.Instance.GamePlayPanel.DeactiveTime();
        }
        if(Stage.Instance !=null && Stage.Instance.gameObject.activeSelf)
        {
            Stage.Instance.canInteract = false;
        }
        if (LevelManagerNew.Instance.stage == 1)
        {
            UIManagerNew.Instance.ThresholeController.showThreshole("extrahole", Stage.Instance.holeToUnlock.transform.localScale, Stage.Instance.holeToUnlock.transform);
        }
        if (LevelManagerNew.Instance.stage == 3)
        {
            UIManagerNew.Instance.ThresholeController.showThreshole("unscrew", GamePlayPanelUIManager.Instance.boosterBar.deteleBT.transform.localScale, GamePlayPanelUIManager.Instance.boosterBar.deteleBT.transform);
        }

    }
    public void SetValue(int indexSprite)
    {
        Image.sprite = sprites[indexSprite];
        description.text = descriptions[indexSprite];
        itemName.text = itemNames[indexSprite];
    }
}
