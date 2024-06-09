using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
	public int numOfUseByAds = 0;
	public GameObject pointer;

	public bool isLock = false;
	private void Start()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		numOfUsed = 1;
        CheckNumOfUse();
    }
	public void UseTicket()
	{
		if (SaveSystem.instance.unscrewPoint >= numOfUsed)
		{
            ShowTutor();
            numOfUse++;
			FirebaseAnalyticsControl.Instance.Gameplay_Item_Unscrew_1(numOfUse,LevelManagerNew.Instance.stage);
            GamePlayPanelUIManager.Instance.showPointer(true);
            Stage.Instance.DeactiveTutor();
            SaveSystem.instance.AddBooster(-numOfUsed,0);
			SaveSystem.instance.SaveData();
			//hasUse = true;
			numOfUsed++;
            CheckNumOfUse();
            Stage.Instance.setDeteleting(true);
			//UIManager.instance.gamePlayPanel.ButtonOff();
			this.Close();
		}
		else
		{
			notEnoughpanel.ShowDialog();
		}
        
    }
	public void WatchAd()
	{
		AdsManager.instance.ShowRewardVideo(() =>
		{
            ShowTutor();
            //xem qu?ng cáo 
            numOfUseByAds++;
			FirebaseAnalyticsControl.Instance.Unscrew_RW_Change(numOfUseByAds);
			numOfUse++;
			FirebaseAnalyticsControl.Instance.Gameplay_Item_Unscrew_1(numOfUse, LevelManagerNew.Instance.stage);

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
		if (!isLock)
		{
			if (numOfUsed == 1)
			{
				Interactable();
			}
			else
			{
				Uniteractable();
			}
		}
    }
	public void LockOrUnlock(bool status)
	{
		isLock = status;
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
			OffPoiter();
            this.gameObject.SetActive(true);
			canvasGroup.blocksRaycasts = false;
			AudioManager.instance.PlaySFX("OpenPopUp");
			panel.localScale = new Vector3(.8f, .8f, 1);
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, 0.1f);
			panel.DOScale(new Vector3(1, 1, 1), 0.1f).OnComplete(() =>
			{
                GameManagerNew.Instance.CloseLevel(false);
                GamePlayPanelUIManager.Instance.Close();
				ActiveCVGroup();
                if (Stage.Instance.isTutor)
                {
                    ShowPointer(true);
                    Uniteractable();
                }
            });
			CheckNumOfUse();
        }
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			canvasGroup.blocksRaycasts = false;
			//closeButton.DOAnchorPos(new Vector2(552f, -105f), .1f, false).OnComplete(() =>
			//{
			//	panel.DORotate(new Vector3(0, 0, -10f), 0.25f, RotateMode.Fast).OnComplete(() =>
			//	{
			//		panel.DOAnchorPos(new Vector2(panel.transform.position.x, -1467f), .25f, false).OnComplete(() =>
			//		{
			//			this.gameObject.SetActive(false);
			//			AudioManager.instance.PlaySFX("ClosePopUp");
			//			GamePlayPanelUIManager.Instance.ActiveTime();
			//			GamePlayPanelUIManager.Instance.Appear();
			//			GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
						
			//			ActiveCVGroup();
			//		});
			//	});
			//});
            canvasGroup.DOFade(0, 0.1f);
            panel.DOScale(new Vector3(0.8f, 0.8f, 0), 0.1f).OnComplete(() =>
            {
                this.gameObject.SetActive(false);
                AudioManager.instance.PlaySFX("ClosePopUp");
                GamePlayPanelUIManager.Instance.ActiveTime();
                GamePlayPanelUIManager.Instance.Appear();
                GamePlayPanelUIManager.Instance.ShowPoiterAgain1();
                GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
                ActiveCVGroup();
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
	public void ShowTutor()
	{
		if (Stage.Instance.isTutor)
		{
			GamePlayPanelUIManager.Instance.boosterBar.ShowPointer(false);
			GamePlayPanelUIManager.Instance.ActiveBlackPic(false);
		}
	}
	public void OffPoiter()
	{
        if (Stage.Instance.isTutor)
        {
            GamePlayPanelUIManager.Instance.boosterBar.ShowPointer(false);
        }
    }
	public void ShowPointer(bool status)
	{
		pointer.gameObject.SetActive(status);
	}
}
