using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class RePlayPanel : MonoBehaviour
{
	public RectTransform closeButton;
	public RectTransform panel;
	public rankpanel notEnoughpanel;
	public RectTransform watchAdButton;
	public int numOfUsed =1;
	public TextMeshProUGUI numOfUsedText;
	public CanvasGroup canvasGroup;


	private void Start()
	{
		numOfUsed = 1;
		canvasGroup = GetComponent<CanvasGroup>();
	}
	public void UseTicket()
	{
		if (SaveSystem.instance.powerTicket >= numOfUsed)
		{

			SaveSystem.instance.addTiket(-numOfUsed, 0);
			SaveSystem.instance.SaveData();
			numOfUsed++;
			this.CloseToReplay();
			
		}
		else
		{
			notEnoughpanel.ShowDialog();
		}
	}
	public void WatchAd()
	{
		// load ad 
		AdsManager.instance.ShowRewardVideo(() =>
		{
			UIManagerNew.Instance.UndoPanel.numOfUseByAds++;

			this.CloseToReplay();
			numOfUsed++;
		});
		
		
	}
	private void Update()
	{
		numOfUsedText.text = ("X" + (numOfUsed).ToString());
		if (numOfUsed == 1)
		{
			watchAdButton.GetComponent<Button>().interactable = true;
		}
		else
		{
			watchAdButton.GetComponent<Button>().interactable = false;
		}
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			canvasGroup.blocksRaycasts = false;
			AudioManager.instance.PlaySFX("OpenPopUp");
			//panel.localRotation = Quaternion.identity;
			canvasGroup.alpha = 0;
			//panel.localPosition = new Vector3(-351, 479, 0);
			//panel.localScale = new Vector3(.8f, .8f, 0);
			//closeButton.localPosition = new Vector3(364, 277.600006f, 0);
			canvasGroup.DOFade(1, 0.1f);
			panel.DOScale(new Vector3(1, 1, 1), 0.1f).OnComplete(() =>
			{
				ActiveCVGroup();
				GamePlayPanelUIManager.Instance.Close();
			});
			
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
            //		panel.DOAnchorPos(new Vector2(panel.transform.position.x, -1467f), 0.25f, false).OnComplete(() =>
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
				GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);

				ActiveCVGroup();
			});
        }
	}
	public void CloseToReplay()
	{
		if (this.gameObject.activeSelf)
		{
			canvasGroup.blocksRaycasts = false;
			closeButton.DOAnchorPos(new Vector2(552f, -105f), .1f, false).OnComplete(() =>
			{
				panel.DORotate(new Vector3(0, 0, -10f), 0.25f, RotateMode.Fast).OnComplete(() =>
				{
					panel.DOAnchorPos(new Vector2(panel.transform.position.x, -1467f), 0.25f, false).OnComplete(() =>
					{
						this.gameObject.SetActive(false);
						AudioManager.instance.PlaySFX("ClosePopUp");
						//UIManager.instance.ActiveTime();
						GameManagerNew.Instance.Replay();
						ActiveCVGroup();
					});
				});
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
}
