using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasGroup))]

public class DeteleNailPanel : MonoBehaviour
{
    public RectTransform closeButton;
    public RectTransform panel;

    public TextMeshProUGUI priceText;

    public int numOfUsed = 1;
    public bool hasWatchAd = false;
    public RectTransform watchAdButton;

    public CanvasGroup canvasGroup;
    public int numOfUse = 0;
    public GameObject pointer;

    public bool hasUseTutor;


    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        numOfUsed = 1;
        CheckNumOfUse();
    }
    public void UseTicket()
    {
        if ((LevelManagerNew.Instance.stage == 3))
        {
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);

            Stage.Instance.DeactiveTutor();

            hasUseTutor = true;

            CheckNumOfUse();

            Stage.Instance.isDeteleting = true;
            Stage.Instance.DisplayUnscrew(true);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                if (!Stage.Instance.isWining)
                {
                    Stage.Instance.setDeteleting(true);
                }
                else
                {
                    Stage.Instance.isDeteleting = false;
                    Stage.Instance.DisplayUnscrew(false);
                }
                Stage.Instance.canInteract = true;
            });

            DOVirtual.DelayedCall(1f, () =>
            {
                UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
            });
        }
        else
        {
            if (SaveSystem.instance.unscrewPoint >= numOfUsed)
            {
                if (UIManagerNew.Instance.ThresholeController.gameObject.activeSelf)
                {
                    UIManagerNew.Instance.ThresholeController.Disable();
                }

                if (LevelManagerNew.Instance.stage == 3 && numOfUsed == 1)
                {
                    hasUseTutor = true;
                }
                UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
                //FirebaseAnalyticsControl.Instance.Gameplay_Item_Unscrew(numOfUse, LevelManagerNew.Instance.stage);
                Stage.Instance.DeactiveTutor();
                SaveSystem.instance.AddBooster(-numOfUsed, 0, 0);
                SaveSystem.instance.SaveData();

                CheckNumOfUse();
                Stage.Instance.isDeteleting = true;
                Stage.Instance.DisplayUnscrew(true);
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    if (!Stage.Instance.isWining)
                    {
                        Stage.Instance.setDeteleting(true);
                    }
                    else
                    {
                        Stage.Instance.isDeteleting = false;
                        Stage.Instance.DisplayUnscrew(false);
                    }
                    Stage.Instance.canInteract = true;
                });
                //UIManager.instance.gamePlayPanel.ButtonOff();
                DOVirtual.DelayedCall(1f, () =>
                {
                    this.Close();
                    UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
                });
            }

            else
            {

            }
        }
    }
    public void WatchAd()
    {
        AdsManager.instance.ShowRewardVideo(AddType.Booster_Unscrew, null, () =>
        {
            
            //xem qu?ng cáo 
            FirebaseAnalyticsControl.Instance.LogEventLevelItem(LevelManagerNew.Instance.stage, LevelItem.unscrew);

            //xoá nail(Đồng hồ đếm giờ dừng lại)
            //UIManager.instance.gamePlayPanel.ButtonOff();
            hasWatchAd = true;

            CheckNumOfUse();
            Stage.Instance.isDeteleting = true;
            Stage.Instance.DisplayUnscrew(true);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                if (!Stage.Instance.isWining)
                {
                    Stage.Instance.setDeteleting(true);
                }
                else
                {
                    Stage.Instance.isDeteleting = false;
                    Stage.Instance.DisplayUnscrew(false);
                }
                Stage.Instance.canInteract = true;
            });
            this.Close();

        });
    }
    public void SpendCoin()
    {
        if (SaveSystem.instance.coin >= 50 * numOfUsed)
        {
            if (UIManagerNew.Instance.ThresholeController.gameObject.activeSelf)
            {
                UIManagerNew.Instance.ThresholeController.Disable();
            }
            if (LevelManagerNew.Instance.stage == 3 && numOfUsed == 1)
            {
                hasUseTutor = true;
            }
            //FirebaseAnalyticsControl.Instance.Gameplay_Item_Unscrew(numOfUse, LevelManagerNew.Instance.stage);
            Stage.Instance.DeactiveTutor();
            SaveSystem.instance.addCoin(-(50 * numOfUsed));
            numOfUsed += 1;
            SaveSystem.instance.SaveData();
            Stage.Instance.isDeteleting = true;
            Stage.Instance.DisplayUnscrew(true);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                if (!Stage.Instance.isWining)
                {
                    Stage.Instance.setDeteleting(true);
                }
                else
                {
                    Stage.Instance.isDeteleting = false;
                    Stage.Instance.DisplayUnscrew(false);
                }
                Stage.Instance.canInteract = true;
            });
            this.Close();
        }
        else
        {
            UIManagerNew.Instance.ShopPanel.Open();
        }
    }
    private void Update()
    {

    }
    public void CheckNumOfUse()
    {

    }
    public void Uninteractable()
    {
        watchAdButton.GetComponent<Button>().interactable = false;
    }
    public void Interactable()
    {
        watchAdButton.GetComponent<Button>().interactable = true;
    }
    public void Open()
    {
        if (!this.gameObject.activeSelf)
        {
            SetActiveWatchButton();

            priceText.text = (50 * numOfUsed).ToString();

            this.gameObject.SetActive(true);
            //GameManagerNew.Instance.CloseLevel(false);
            canvasGroup.blocksRaycasts = false;
            AudioManager.instance.PlaySFX("OpenPopUp");
            panel.localScale = new Vector3(.8f, .8f, 1f);
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.1f);
            panel.transform.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.2f).OnComplete(() =>
            {
                panel.transform.DOScale(Vector3.one, 0.15f).OnComplete(() =>
                {

                    GamePlayPanelUIManager.Instance.Close();
                    ActiveCVGroup();
                    if (Stage.Instance.isTutor && LevelManagerNew.Instance.stage == 3)
                    {
                        Uninteractable();
                    }
                });
            });
            CheckNumOfUse();
        }
    }
    public void Close()
    {
        if (this.gameObject.activeSelf)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOFade(0, 0.1f);
            panel.DOScale(new Vector3(0.8f, 0.8f, 0), 0.1f).OnComplete(() =>
            {
                UIManagerNew.Instance.hasUI = false;
                if (LevelManagerNew.Instance.stage == 3 && hasUseTutor)
                {
                    AudioManager.instance.PlaySFX("ClosePopUp");
                    if (Stage.Instance.holeToUnlock.GetComponent<Hole>().extraHole == true)
                    {
                        Stage.Instance.holeToUnlock.myButton.gameObject.SetActive(true);
                    }
                    GamePlayPanelUIManager.Instance.Appear();
                    ActiveCVGroup();
                    Stage.Instance.checked1 = false;
                    this.gameObject.SetActive(false);
                    Stage.Instance.AfterPanel();
                }
                else
                {
                    AudioManager.instance.PlaySFX("ClosePopUp");
                    GamePlayPanelUIManager.Instance.ActiveTime();

                    if (Stage.Instance.holeToUnlock.GetComponent<Hole>().extraHole == true)
                    {
                        Stage.Instance.holeToUnlock.myButton.gameObject.SetActive(true);
                    }
                    GamePlayPanelUIManager.Instance.Appear();
                    ActiveCVGroup();
                    Stage.Instance.checked1 = false;
                    this.gameObject.SetActive(false);
                    Stage.Instance.AfterPanel();
                }

            });
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Stage.Instance.canInteract = true;
            });
        }
    }
    public void NewWayUse()
    {
        Stage.Instance.SetDefaultBeforeUnscrew();
        Stage.Instance.canInteract = false;
        UIManagerNew.Instance.GamePlayPanel.animButton.Play(UIManagerNew.Instance.GamePlayPanel.disappearButtonForBooster);
        if (Stage.Instance.holeToUnlock != null)
        {
            Stage.Instance.holeToUnlock.myButton.gameObject.SetActive(false);
        }
        if ((LevelManagerNew.Instance.stage == 3))
        {

            Stage.Instance.DeactiveTutor();
            hasUseTutor = true;
            GamePlayPanelUIManager.Instance.Appear();
            Stage.Instance.isDeteleting = true;
            Stage.Instance.DisplayUnscrew(true);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                if (!Stage.Instance.isWining)
                {
                    Stage.Instance.setDeteleting(true);
                }
                else
                {
                    Stage.Instance.isDeteleting = false;
                    Stage.Instance.DisplayUnscrew(false);
                }
                Stage.Instance.canInteract = true;
            });
        }
        else
        if (SaveSystem.instance.unscrewPoint >= 1)
        {
            if (UIManagerNew.Instance.ThresholeController.gameObject.activeSelf)
            {
                UIManagerNew.Instance.ThresholeController.Disable();
            }
            if (LevelManagerNew.Instance.stage == 3 && numOfUsed == 1)
            {
                hasUseTutor = true;
            }
            //FirebaseAnalyticsControl.Instance.Gameplay_Item_Unscrew(numOfUse, LevelManagerNew.Instance.stage);
            Stage.Instance.DeactiveTutor();

            SaveSystem.instance.AddBooster(-1, 0, 0);
            GamePlayPanelUIManager.Instance.Appear();
            Stage.Instance.isDeteleting = true;
            Stage.Instance.DisplayUnscrew(true);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                if (!Stage.Instance.isWining)
                {
                    Stage.Instance.setDeteleting(true);
                }
                else
                {
                    Stage.Instance.isDeteleting = false;
                    Stage.Instance.DisplayUnscrew(false);
                }
                Stage.Instance.canInteract = true;
            });
        }
        else
        {
            UIManagerNew.Instance.GamePlayPanel.OpenDetelePanel();
            
        }
    }


    public void SetActiveWatchButton()
    {
        if(!hasWatchAd)
        {
            Interactable();
        }
        else
        {
            Uninteractable();
        }
    }
    public void ActiveCVGroup()
    {
        if (!canvasGroup.blocksRaycasts)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }
}
