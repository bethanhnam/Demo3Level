using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class ExtralHole : MonoBehaviour
{
    public string layerName = "Hole";
    public ExtraHoleButton extraHoleButton;
    public RectTransform closeButton;
    public RectTransform panel;
    public rankpanel notEnoughpanel;
    public CanvasGroup canvasGroup;


    //text 
    public TextMeshProUGUI minusText;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Update()
    {
    }
    public void UseTicket()
    {
        if (SaveSystem.instance.extraHolePoint >= 1)
        {
            if (UIManagerNew.Instance.ThresholeController.gameObject.activeSelf)
            {
                UIManagerNew.Instance.ThresholeController.Disable();
            }
            FirebaseAnalyticsControl.Instance.LogEventLevelItem(LevelManagerNew.Instance.stage, LevelItem.drill);
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            SaveSystem.instance.AddBooster(0, 0, -1);
            SaveSystem.instance.SaveData();
            DOVirtual.DelayedCall(1f, () =>
            {
                UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
                this.Close();
                DOVirtual.DelayedCall(1.3f, () =>
                {
                    Stage.Instance.holeToUnlock.GetComponent<Hole>().extraHole = false;
                    Stage.Instance.holeToUnlock.GetComponent<ExtraHoleButton>().myButton.gameObject.SetActive(false);
                    UIManagerNew.Instance.GamePlayPanel.ShowDrillEffect(() =>
                    {
                        Stage.Instance.ChangeLayer();
                    });
                });
            });

        }
        else
        {
            notEnoughpanel.ShowDialog();
        }
    }
    public void WatchAd()
    {
        AdsManager.instance.ShowRewardVideo(AddType.Booster_Drill, null, () =>
        {
            // load ad 
            this.Close();
            DOVirtual.DelayedCall(1.3f, () =>
            {
                Stage.Instance.holeToUnlock.GetComponent<Hole>().extraHole = false;
                Stage.Instance.holeToUnlock.GetComponent<ExtraHoleButton>().myButton.gameObject.SetActive(false);
                UIManagerNew.Instance.GamePlayPanel.ShowDrillEffect(() =>
                {
                    Stage.Instance.ChangeLayer();
                });
            });
        });

    }
    public void Open()
    {
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            AudioManager.instance.PlaySFX("OpenPopUp");
            canvasGroup.blocksRaycasts = false;
            panel.localScale = new Vector3(.8f, .8f, 1f);
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.1f);
            panel.transform.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.2f).OnComplete(() =>
            {
                panel.transform.DOScale(Vector3.one, 0.15f).OnComplete(() =>
                {
                    ActiveCVGroup();
                    //GamePlayPanelUIManager.Instance.Close();
                });
            });
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
                if (LevelManagerNew.Instance.stage == 1)
                {
                    AudioManager.instance.PlaySFX("ClosePopUp");
                    //if (Stage.Instance.isWining)
                    //{
                    //    Stage.Instance.ScaleUpStage();
                    //}
                    //else
                    //{
                    //    GamePlayPanelUIManager.Instance.Appear();
                    //    GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
                    //}
                    GamePlayPanelUIManager.Instance.Appear();
                    Stage.Instance.checked1 = false;
                    ActiveCVGroup();
                    this.gameObject.SetActive(false);
                    Stage.Instance.AfterPanel();
                }
                else
                {
                    AudioManager.instance.PlaySFX("ClosePopUp");
                    GamePlayPanelUIManager.Instance.ActiveTime();
                    //if (Stage.Instance.isWining)
                    //{
                    //    Stage.Instance.ScaleUpStage();
                    //}
                    //else
                    //{
                    //    GamePlayPanelUIManager.Instance.Appear();
                    //    GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
                    //}
                    GamePlayPanelUIManager.Instance.Appear();
                    Stage.Instance.checked1 = false;
                    ActiveCVGroup();
                    this.gameObject.SetActive(false);
                    Stage.Instance.AfterPanel();
                }
            });
        }
    }

    public void SpendCoin()
    {
        if (SaveSystem.instance.coin >= 50)
        {

            if (UIManagerNew.Instance.ThresholeController.gameObject.activeSelf)
            {
                UIManagerNew.Instance.ThresholeController.Disable();
            }
            FirebaseAnalyticsControl.Instance.LogEventLevelItem(LevelManagerNew.Instance.stage, LevelItem.drill);
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);

            SaveSystem.instance.addCoin(-50);
            SaveSystem.instance.SaveData();
            DOVirtual.DelayedCall(1f, () =>
            {
                UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
                this.Close();
                //DOVirtual.DelayedCall(1.3f, () =>
                //{
                    Stage.Instance.holeToUnlock.GetComponent<Hole>().extraHole = false;
                    Stage.Instance.holeToUnlock.GetComponent<ExtraHoleButton>().myButton.gameObject.SetActive(false);
                    UIManagerNew.Instance.GamePlayPanel.ShowDrillEffect(() =>
                    {
                        Stage.Instance.ChangeLayer();
                    });
                //});
            });
        }
        else
        {
            UIManagerNew.Instance.ShopPanel.Open();
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
