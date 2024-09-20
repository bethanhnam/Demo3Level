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
    public rankpanel notEnoughpanel;
    public int numOfUsed = 1;
    public RectTransform watchAdButton;
    public TextMeshProUGUI numOfUsedText;
    public CanvasGroup canvasGroup;
    public int numOfUse = 0;
    public GameObject pointer;

    public bool hasUseTutor;

    //text 
    public TextMeshProUGUI minusText;

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

            numOfUsed++;

            hasUseTutor = true;

            CheckNumOfUse();

            Stage.Instance.setDeteleting(true);

            DOVirtual.DelayedCall(1f, () =>
            {
                ;
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
                numOfUse++;
                //FirebaseAnalyticsControl.Instance.Gameplay_Item_Unscrew(numOfUse, LevelManagerNew.Instance.stage);
                Stage.Instance.DeactiveTutor();
                SetMinusText('-', numOfUsed);
                SaveSystem.instance.AddBooster(-numOfUsed, 0, 0);
                SaveSystem.instance.SaveData();
                //hasUse = true;
                numOfUsed++;
                CheckNumOfUse();
                Stage.Instance.setDeteleting(true);
                //UIManager.instance.gamePlayPanel.ButtonOff();
                DOVirtual.DelayedCall(1f, () =>
                {
                    this.Close();
                    UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
                });
            }

            else
            {
                notEnoughpanel.ShowDialog();
            }
        }
    }
    public void WatchAd()
    {
        AdsManager.instance.ShowRewardVideo(() =>
        {

            //xem qu?ng cáo 
            numOfUse++;
            FirebaseAnalyticsControl.Instance.LogEventLevelItem(LevelManagerNew.Instance.stage, LevelItem.unscrew);

            //xoá nail(Đồng hồ đếm giờ dừng lại)
            Stage.Instance.setDeteleting(true);
            //UIManager.instance.gamePlayPanel.ButtonOff();
            numOfUsed++;
            CheckNumOfUse();

            this.Close();

        });

    }
    private void Update()
    {

    }
    public void CheckNumOfUse()
    {
        numOfUsedText.text = ("X" + (numOfUsed).ToString());
        if (numOfUsed == 1)
        {
            Interactable();
        }
        else
        {
            Uniteractable();
        }

    }
    public void Uniteractable()
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
            this.gameObject.SetActive(true);
            GameManagerNew.Instance.CloseLevel(false);
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
                        Uniteractable();
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
                if (LevelManagerNew.Instance.stage == 3 && hasUseTutor)
                {
                    AudioManager.instance.PlaySFX("ClosePopUp");

                    if (Stage.Instance.isWining)
                    {
                        Stage.Instance.ScaleUpStage();
                    }
                    else
                    {
                        GamePlayPanelUIManager.Instance.Appear();
                        GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
                    }
                    ActiveCVGroup();
                    Stage.Instance.checked1 = false;
                    this.gameObject.SetActive(false);
                    Stage.Instance.AfterPanel();
                }
                else
                {
                    AudioManager.instance.PlaySFX("ClosePopUp");
                    GamePlayPanelUIManager.Instance.ActiveTime();

                    if (Stage.Instance.isWining)
                    {
                        Stage.Instance.ScaleUpStage();
                    }
                    else
                    {
                        GamePlayPanelUIManager.Instance.Appear();
                        GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
                    }
                    ActiveCVGroup();
                    Stage.Instance.checked1 = false;
                    this.gameObject.SetActive(false);
                    Stage.Instance.AfterPanel();
                }

            });
        }
    }
    public void NewWayUse()
    {
        Stage.Instance.SetDefaultBeforeUnscrew();
        UIManagerNew.Instance.GamePlayPanel.animButton.Play(UIManagerNew.Instance.GamePlayPanel.disappearButtonForBooster);
        if (Stage.Instance.holeToUnlock != null)
        {
            Stage.Instance.holeToUnlock.myButton.gameObject.SetActive(false);
        }
        if ((LevelManagerNew.Instance.stage == 3))
        {
            Stage.Instance.DeactiveTutor();
            hasUseTutor = true;
            Stage.Instance.setDeteleting(true);
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
            SetMinusText('-', 1);
            SaveSystem.instance.AddBooster(-1, 0, 0);
            SaveSystem.instance.SaveData();
            Stage.Instance.setDeteleting(true);
        }
        else
        {
            AdsManager.instance.ShowRewardVideo(() =>
            {
                //xem qu?ng cáo 
                FirebaseAnalyticsControl.Instance.LogEventLevelItem(LevelManagerNew.Instance.stage, LevelItem.unscrew);
                Stage.Instance.setDeteleting(true);
            });
        }
    }

    public void ActiveCVGroup()
    {
        if (!canvasGroup.blocksRaycasts)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }
    public void SetMinusText(char t, int value)
    {
        minusText.gameObject.SetActive(true);
        minusText.text = t + value.ToString();
        StartCoroutine(DisableText());
    }
    IEnumerator DisableText()
    {
        yield return new WaitForSeconds(0.8f);
        minusText.gameObject.SetActive(false);
    }
}
