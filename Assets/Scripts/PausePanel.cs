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
    public GameObject panel;
    public CanvasGroup canvasGroup;

    public GameObject soundOn;
    public GameObject soundOff;
    public GameObject musicOn;
    public GameObject musicOff;
    public GameObject alertOn;
    public GameObject alertOff;

    public CanvasGroup firstPanel;
    public CanvasGroup secondPanel;
    public CanvasGroup thirdPanel;

    public Animator animator;

    public void Home()
    {
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
        AdsManager.instance.ShowInterstial(AdsManager.PositionAds.ingame_pause, () =>
        {
            //this.gameObject.SetActive(false);
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
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.1f);
            panel.transform.DOScale(new Vector3(1, 1, 1), 0.1f).OnComplete(() =>
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
            panel.transform.DOScale(new Vector3(.8f, .8f, 1), 0.1f).OnComplete(() =>
            {
                canvasGroup.DOFade(0, .2f);
                this.gameObject.SetActive(false);
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
                Stage.Instance.AfterPanel();
                Stage.Instance.checked1 = false;
                
            });

        }
    }
    public void Retry()
    {
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
        AdsManager.instance.ShowInterstial(AdsManager.PositionAds.ingame_pause, () =>
        {
            FirebaseAnalyticsControl.Instance.LogEventLevelStatus(LevelManagerNew.Instance.stage, LevelStatus.retry);
            Close();
            Stage.Instance.ResetBooster();
            ConversationController.instance.Disappear();
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

    public void QuitFirstPanel()
    {
        animator.enabled = true;
        animator.Play("FirstQuit");
    }
    
    public void QuitSecondPanel()
    {
        animator.Play("SecondQuit");
    }

    public void ResetSetting()
    {
        firstPanel.GetComponent<CanvasGroup>().alpha = 1.0f;
        firstPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        firstPanel.transform.GetChild(0).localScale = Vector3.one;
        secondPanel.transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        secondPanel.GetComponent<CanvasGroup>().alpha = 0;
        secondPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        thirdPanel.GetComponent<CanvasGroup>().alpha = 0;
        thirdPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        animator.enabled = false;
    }

    private void OnDisable()
    {
        ResetSetting();
    }
    public void SoundOn()
    {
        soundOff.SetActive(false);
        soundOn.SetActive(true);
        AudioManager.instance.sfxSource.enabled = true;
    }
    public void SoundOff()
    {
        soundOn.SetActive(false);
        soundOff.SetActive(true);
        AudioManager.instance.sfxSource.enabled = false;
    }
    public void MusicOn()
    {
        musicOff.SetActive(false);
        musicOn.SetActive(true);
        AudioManager.instance.musicSource.enabled = true;
    }
    public void MusicOff()
    {
        musicOn.SetActive(false);
        musicOff.SetActive(true);
        AudioManager.instance.musicSource.enabled = false;
    }
    public void AlertOn()
    {
        GameManagerNew.Instance.hapticSouce.enabled = true;
        alertOff.SetActive(false);
        alertOn.SetActive(true);
    }
    public void AlertOff()
    {
        GameManagerNew.Instance.hapticSouce.enabled = false;
        alertOn.SetActive(false);
        alertOff.SetActive(true);
    }
}
