using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

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

    public GameObject eventCollectItem;
    public GameObject starImage;
    public GameObject coinImage;
    public TextMeshProUGUI eventCollectItemNum;

    public int numOfItemCollection = 0;

    public CanvasGroup firstPanel;
    public CanvasGroup secondPanel;
    public CanvasGroup thirdPanel;

    public Animator animator;

    public void Home()
    {
        //UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
        AdsManager.instance.ShowInterstial(AdsManager.PositionAds.ingame_pause, () =>
        {
            //this.gameObject.SetActive(false);
            UIManagerNew.Instance.ButtonMennuManager.isShowingFixing = false;
            UIManagerNew.Instance.GamePlayLoading.appear();
            DOVirtual.DelayedCall(.7f, () =>
            {
                GameManagerNew.Instance.PictureUIManager.Open();
                this.gameObject.SetActive(false);
                Stage.Instance.ResetBooster();
                UIManagerNew.Instance.ButtonMennuManager.Appear();
                canvasGroup.DOFade(0, 0.8f).OnComplete(() =>
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
            CheckSetSound();
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
                UIManagerNew.Instance.hasUI = false;;
                Stage.Instance.checked1 = false;

            });

        }
    }
    public void CloseForWin()
    {
        if (this.gameObject.activeSelf)
        {
            this.GetComponent<CanvasGroup>().DOFade(0, 0.1f);
            panel.transform.DOScale(new Vector3(.8f, .8f, 1), 0.1f).OnComplete(() =>
            {
                canvasGroup.DOFade(0, .2f);
                this.gameObject.SetActive(false);
                if (Stage.Instance.isWining)
                {
                    if (!UIManagerNew.Instance.WinUI.gameObject.activeSelf)
                    {
                        Stage.Instance.ScaleUpStage();
                    }
                }
                else
                {
                    if (!UIManagerNew.Instance.WinUI.gameObject.activeSelf)
                    {
                        GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
                    }
                }
                UIManagerNew.Instance.hasUI = false;
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
        if (EventController.instance.weeklyEvent != null)
        {
            eventCollectItem.gameObject.SetActive(true);
            eventCollectItemNum.text = numOfItemCollection.ToString();
        }
        else
        {
            eventCollectItem.gameObject.SetActive(false);
        }

        animator.Play("FirstQuit");
    }

    public void QuitSecondPanel()
    {

        animator.Play("SecondQuit");
    }

    public void SetImage()
    {
        if (EventController.instance.weeklyEvent != null)
        {
            starImage.transform.DOMove(new Vector3(0.00f, starImage.transform.position.y, 89.99f),0.2f);
            eventCollectItem.transform.DOMove(new Vector3(-1.88f, eventCollectItem.transform.position.y, 89.99f),0.2f);
            coinImage.transform.DOMove(new Vector3(1.84f, coinImage.transform.position.y, 89.99f), 0.2f);
        }
        else
        {
            eventCollectItem.gameObject.SetActive(false);
            starImage.transform.DOMove(new Vector3(-1.26f, starImage.transform.position.y, 89.99f), 0.2f);
            eventCollectItem.transform.DOMove(new Vector3(-1.88f, eventCollectItem.transform.position.y, 89.99f), 0.2f);
            coinImage.transform.DOMove(new Vector3(1.19f, eventCollectItem.transform.position.y, 89.99f), 0.2f);
        }
    }
    [Button("TestAnim")]
    public void MoveItem()
    {
        eventCollectItem.GetComponent<Animator>().Play("EventCollectItem");
        DOVirtual.DelayedCall(0.2f, () =>
        {
            starImage.GetComponent<Animator>().Play("EventCollectItem");
            DOVirtual.DelayedCall(0.2f, () =>
            {
                coinImage.GetComponent<Animator>().Play("EventCollectItem");
            });
        });
    }
    [Button("showPos")]
    public void PrintPos()
    {
        Debug.Log(starImage.transform.position);
        Debug.Log(eventCollectItem.transform.position);
        Debug.Log(coinImage.transform.position);
    }

    public void ResetSetting()
    {
        firstPanel.GetComponent<CanvasGroup>().alpha = 1.0f;
        firstPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        firstPanel.transform.GetChild(0).localScale = new Vector3(1.2f,1.2f,1);
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

    public void CheckSetSound()
    {
        if (AudioManager.instance.sfxSource.enabled == true)
        {
            SoundOn();
        }
        else
        {
            SoundOff();
        }
        if(GameManagerNew.Instance.hapticSouce.enabled == true)
        {
            AlertOn();
        }
        else
        {
            AlertOff();
        }
        if(AudioManager.instance.musicSource.enabled == true)
        {
            MusicOn();
        }
        else
        {
            MusicOff();
        }
    }

    public void SetNumOfCollectItem()
    {
        numOfItemCollection = 0;
        for (int i = 0;i< Stage.Instance.ironPlates.Length; i++)
        {
            if (Stage.Instance.ironPlates[i].isEventItem)
            {
                numOfItemCollection++;
            }
        }
    }
    private void OnEnable()
    {
        try
        {
            eventCollectItem.transform.GetChild(0).GetComponent<Image>().sprite = EventController.instance.weeklyEventItemSprite;
        }
        catch
        {

        }
        //SetImage();
    }
}
