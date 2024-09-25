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
    public CoinReward ImageMove;
    public TextMeshProUGUI itemName;

    public Transform defaultPos;

    public CanvasGroup canvasGroup;
    public Animator animator;
    private void Start()
    {
    }
    public void Appear()
    {
        Image.transform.position = defaultPos.position;
        AudioManager.instance.PlaySFX("ItemAppear"); 
        this.gameObject.SetActive(true);
        canvasGroup.enabled = true;
        animator.enabled = true;
        if (Stage.Instance != null && Stage.Instance.gameObject.activeSelf)
        {
            Stage.Instance.canInteract = false;
        }
    }
    public void Disappear()
    {
        canvasGroup.enabled = false;
        animator.Play("NewBoosterDisappear");
    }
    public void Deactive()
    {
        this.gameObject.SetActive(false);
    }
    public void ShowThreshole()
    {
        if (UIManagerNew.Instance.GamePlayPanel.gameObject.activeSelf)
        {
            UIManagerNew.Instance.GamePlayPanel.DeactiveTime();
        }
        if (Stage.Instance != null && Stage.Instance.gameObject.activeSelf)
        {
            Stage.Instance.canInteract = false;
        }
        if (LevelManagerNew.Instance.stage == 3)
        {
            FirebaseAnalyticsControl.Instance.LogEventTutorialStatus(LevelManagerNew.Instance.stage, TutorialStatus.tut_unscrew_start);
            GameManagerNew.Instance.conversationController.SetInteractable(false);
            DOVirtual.DelayedCall(0.6f, () =>
            {
                UIManagerNew.Instance.ThresholeController.showThreshole("unscrew", GamePlayPanelUIManager.Instance.boosterBar.deteleBT.transform.localScale, GamePlayPanelUIManager.Instance.boosterBar.deteleBT.transform);
            });
            GameManagerNew.Instance.conversationController.StartConversation(1, 12, "UnscrewTutor", () =>
            {

            }, false);
        }
    }

    public void ShowGiveAwayItem()
    {
        animator.enabled = false;
        if (LevelManagerNew.Instance.stage == 3)
        {
            UIManagerNew.Instance.BlockPicCanvas.SetActive(true);
            ImageMove.MoveToDes(CoinReward.typeOfReward.GiveAwayItem, Image.transform, UIManagerNew.Instance.GamePlayPanel.boosterBar.deteleBT.transform.position,0.7f,.7f,() =>
            {
                SaveSystem.instance.AddBooster(2, 0, 0);
                AudioManager.instance.PlaySFX("Coins");
                UIManagerNew.Instance.GamePlayPanel.boosterBar.freeUnscrewImg.gameObject.SetActive(true);
                Deactive();
                var x = UIManagerNew.Instance.GamePlayPanel.boosterBar.deteleBT.transform.localScale;
                UIManagerNew.Instance.GamePlayPanel.boosterBar.deteleBT.transform.DOScale(x + new Vector3(0.1f, 0.1f, 1), 0.5f).OnComplete(() =>
                {
                    UIManagerNew.Instance.GamePlayPanel.boosterBar.deteleBT.transform.DOScale(x, 0.3f).OnComplete(() =>
                    {
                        if (PlayerPrefs.GetInt("GiveAwayUnscrew") == 0)
                        {
                            PlayerPrefs.SetInt("GiveAwayUnscrew", 1);
                            SaveSystem.instance.SaveData();
                        }
                        ShowThreshole();
                    });
                });
            });
        }
        if (LevelManagerNew.Instance.stage == 4)
        {
            ImageMove.MoveToDes(CoinReward.typeOfReward.GiveAwayItem,Image.transform, UIManagerNew.Instance.GamePlayPanel.boosterBar.UndoBT.transform.position,0.7f, .7f, () =>
            {
                SaveSystem.instance.AddBooster(0, 2, 0);
                Stage.Instance.canInteract = true;
                AudioManager.instance.PlaySFX("Coins");
                UIManagerNew.Instance.GamePlayPanel.boosterBar.blockUndoImage.gameObject.SetActive(false);
                UIManagerNew.Instance.GamePlayPanel.boosterBar.undoNumImg.gameObject.SetActive(true);
                UIManagerNew.Instance.GamePlayPanel.boosterBar.undoNumText.text = "2";
                Deactive();
                var x = UIManagerNew.Instance.GamePlayPanel.boosterBar.UndoBT.transform.localScale;
                UIManagerNew.Instance.GamePlayPanel.boosterBar.UndoBT.transform.DOScale(x + new Vector3(0.1f, 0.1f, 1), 0.5f).OnComplete(() =>
                {
                    UIManagerNew.Instance.GamePlayPanel.boosterBar.UndoBT.transform.DOScale(x, 0.3f).OnComplete(() =>
                    {
                        if (PlayerPrefs.GetInt("GiveAwayUndo") == 0)
                        {
                            PlayerPrefs.SetInt("GiveAwayUndo", 1);
                            SaveSystem.instance.SaveData();
                        }
                    });
                });
            });
        }
    }
    public void SetValue(int indexSprite)
    {
        Image.sprite = sprites[indexSprite];
        itemName.text = itemNames[indexSprite];
    }
}
