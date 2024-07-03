using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

[RequireComponent(typeof(CanvasGroup))]
public class PausePanel : MonoBehaviour
{
    public bool isdeleting;
    public bool isdeletingIron;
    public CanvasGroup canvasGroup;
    public void Home()
    {
        AdsManager.instance.ShowInterstial(AdsManager.PositionAds.ingame_pause, () =>
        {
            //this.gameObject.SetActive(false);
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            UIManagerNew.Instance.GamePlayLoading.appear();
            DOVirtual.DelayedCall(.7f, () =>
            {
                GameManagerNew.Instance.PictureUIManager.Open();
                this.gameObject.SetActive(false);
                Stage.Instance.ResetBooster();
                UIManagerNew.Instance.ButtonMennuManager.Appear();
                canvasGroup.DOFade(0, 0.5f).OnComplete(() =>
                {
                    GameManagerNew.Instance.Bg.gameObject.SetActive(false);
                    Destroy(GameManagerNew.Instance.CurrentLevel.gameObject);
                });
            });

        }, null);
    }
    public void Open()
    {
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = false;
            if (Stage.Instance.isDeteleting)
            {
                isdeleting = true;
            }
            //if (Stage.Instance.deletingIron)
            //{
            //	isdeletingIron = true;
            //}
            //UIManager.instance.DeactiveTime();
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.1f);
            this.GetComponent<RectTransform>().DOScale(new Vector3(1, 1, 1), 0.1f).OnComplete(() =>
            {
                ActiveCVGroup();
            });

        }
    }
    public void Close()
    {
        if (this.gameObject.activeSelf)
        {
            this.GetComponent<CanvasGroup>().DOFade(0, 0.1f);
            GamePlayPanelUIManager.Instance.AppearForReOpen();
            this.GetComponent<RectTransform>().DOScale(new Vector3(.8f, .8f, 1), 0.1f).OnComplete(() =>
            {
                canvasGroup.DOFade(0, .2f);
                this.gameObject.SetActive(false);
                GamePlayPanelUIManager.Instance.ActiveTime();
                //UIManager.instance.ActiveTime();
                //UIManager.instance.gamePlayPanel.backFrom
                //= true;
                if (Stage.Instance.isWining)
                {
                    Stage.Instance.ScaleUpStage();
                }
                else
                {
                    GamePlayPanelUIManager.Instance.Appear();
                    GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
                }
                Stage.Instance.AfterPanel();
                Stage.Instance.checked1 = false;
                
            });

        }
    }
    public void Retry()
    {
        AdsManager.instance.ShowInterstial(AdsManager.PositionAds.ingame_pause, () =>
        {
            Close();
            Stage.Instance.ResetBooster();
            GameManagerNew.Instance.Retry();
            //UIManager.instance.gamePlayPanel.backFromPause = false;
        }, null);

    }
    public void ActiveCVGroup()
    {
        if (!canvasGroup.blocksRaycasts)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }
}
