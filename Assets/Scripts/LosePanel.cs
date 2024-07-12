using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Timers;
using System.Runtime.InteropServices.WindowsRuntime;

[RequireComponent(typeof(CanvasGroup))]
public class LosePanel : MonoBehaviour
{
	public CanvasGroup canvasGroup;
	public bool hasUse = false;
	public Button watchAdButton;
	// Start is called before the first frame update
	void Start()
    {
		watchAdButton.interactable = true;
	}

    // Update is called once per frame
    void Update()
    {
		if (hasUse)
		{
			watchAdButton.interactable = false;
		}
		else
		{
			watchAdButton.interactable = true;
			watchAdButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UI Nut/export/win/bttn_green");
		}
    }
    public void WatchAd()
    {
		// load ad 
		if (!hasUse)
		{
			AdsManager.instance.ShowRewardVideo(() =>
			{
				Close();
				GamePlayPanelUIManager.Instance.timer.SetTimer(61f);
				GamePlayPanelUIManager.Instance.ActiveTime();
				GamePlayPanelUIManager.Instance.Appear();
				GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
				hasUse = true;
				watchAdButton.interactable = false;
				watchAdButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UI Nut/export/win/bttn_grey");
                Stage.Instance.CheckDoneLevel();
                FirebaseAnalyticsControl.Instance.LogEventLevelStatus(LevelManagerNew.Instance.stage,LevelStatus.revive);
            });
			
		}
		
	}
	public void Replay()
    {
		// load ad 
		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_lose, () =>
		{
			Close();
			GameManagerNew.Instance.Retry();
			hasUse = false;
		}, null);
	}
	public void Open()
	{
		//neu da win thi khong mo lose
		if(Stage.Instance.numOfIronPlates <= 0)
		{
			return;
		}
		else
		{
            FirebaseAnalyticsControl.Instance.LogEventLevelStatus(LevelManagerNew.Instance.stage,LevelStatus.fail);
            if (!this.gameObject.activeSelf)
            {
                this.gameObject.SetActive(true);
                AudioManager.instance.PlaySFX("LosePop");
                canvasGroup.alpha = 0;
                canvasGroup.DOFade(1, .3f).OnComplete(() =>
                {

                });
            }
        }
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			canvasGroup.alpha = 1;
			canvasGroup.DOFade(0, .3f).OnComplete(() =>
			{
				this.gameObject.SetActive(false);
				AudioManager.instance.PlaySFX("ClosePopUp");
                Stage.Instance.checked1 = false;
            });
		}
	}
}
