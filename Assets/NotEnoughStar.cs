using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotEnoughStar : MonoBehaviour
{
    public RectTransform closeButton;
    public RectTransform panel;
    public CanvasGroup canvasGroup;
    public GameObject panelBoard;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Open()
    {
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            panelBoard.gameObject.SetActive(false);
            panelBoard.transform.localScale = new Vector3(.9f, .9f, 1f);
            AudioManager.instance.PlaySFX("OpenPopUp");
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, .3f);
            panelBoard.gameObject.SetActive(true);
            panelBoard.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.2f).OnComplete(() =>
            {
                panelBoard.transform.DOScale(Vector3.one, 0.15f).OnComplete(() =>
                {

                });
            });
        }
    }
    public void Close()
    {
        if (this.gameObject.activeSelf)
        {
            canvasGroup.alpha = 1;
            canvasGroup.DOFade(0, .3f).OnComplete(() =>
            {
                UIManagerNew.Instance.ButtonMennuManager.Appear();
                this.gameObject.SetActive(false);
                AudioManager.instance.PlaySFX("ClosePopUp");
            });
            panelBoard.transform.DOScale(new Vector3(.9f, .9f, 1f), 0.3f);
        }
    }
    public void CloseForPlay()
    {
        if (this.gameObject.activeSelf)
        {
            canvasGroup.alpha = 1;
            canvasGroup.DOFade(0, .3f).OnComplete(() =>
            {
                this.gameObject.SetActive(false);
                AudioManager.instance.PlaySFX("ClosePopUp");
            });
            panelBoard.transform.DOScale(new Vector3(.9f, .9f, 1f), 0.3f);
        }
    }
    public void PlayGameViaButton()
    {
        int level = LevelManagerNew.Instance.GetStage();
        //GameManagerNew.Instance.CreateLevel(level);
        if (GameManagerNew.Instance.CheckLevelStage())
        {
            UIManagerNew.Instance.ButtonMennuManager.OpenCompletePanel();
        }
        else
        {
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            UIManagerNew.Instance.GamePlayLoading.appear();
			DOVirtual.DelayedCall(0.7f, () =>
            {
                UIManagerNew.Instance.GamePlayPanel.AppearForCreateLevel();
                if (PlayerPrefs.GetInt("HasCompleteLastLevel") == 1)
                {
                    int replayLevel = UnityEngine.Random.Range(5, 29);
                    GameManagerNew.Instance.CreateLevel(replayLevel);
                }
                else
                {
                    GameManagerNew.Instance.CreateLevel(level);
                }
            });
            if (LevelManagerNew.Instance.stage == 1)
            {
                GamePlayPanelUIManager.Instance.boosterBar.gameObject.SetActive(false);
                //tuto undo 
                if (SaveSystem.instance.extraHolePoint == 0)
                {
                    SaveSystem.instance.extraHolePoint = 1;
                    UIManagerNew.Instance.LoadData(SaveSystem.instance.unscrewPoint, SaveSystem.instance.undoPoint, SaveSystem.instance.extraHolePoint, SaveSystem.instance.coin, SaveSystem.instance.star);
                }
                GameManagerNew.Instance.conversationController.StartConversation(1, 4, () =>
                {
                    UIManagerNew.Instance.NewBooster.ShowThreshole();
                });
                CloseForPlay();
            }
        }
    }
}
