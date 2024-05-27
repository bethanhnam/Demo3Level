using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class ExtralHole : MonoBehaviour
{
	public string layerName = "Hole";
	public ExtraHoleButton extraHoleButton;
	public RectTransform closeButton;
	public RectTransform panel;
	public rankpanel notEnoughpanel;
	public CanvasGroup canvasGroup;

	private void Start()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}
	private void Update()
	{
	}
	public void UseTicket()
	{
		if (SaveSystem.instance.coin >= 20)
		{
			this.Close();
			SaveSystem.instance.addCoin(-20);
			SaveSystem.instance.SaveData();
			Stage.Instance.ChangeLayer();
			Stage.Instance.holeToUnlock.GetComponent<Hole>().extraHole = false;
			Stage.Instance.holeToUnlock.GetComponent<ExtraHoleButton>().myButton.gameObject.SetActive(false);
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
			// load ad 
			this.Close();
			Stage.Instance.ChangeLayer();
			Stage.Instance.holeToUnlock.GetComponent<Hole>().extraHole = false;
			Stage.Instance.holeToUnlock.GetComponent<ExtraHoleButton>().myButton.gameObject.SetActive(false);
		});
		
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			AudioManager.instance.PlaySFX("OpenPopUp");
			canvasGroup.blocksRaycasts = false;
			//UIManager.instance.DeactiveTime();
			//panel.localRotation = Quaternion.identity;
			canvasGroup.alpha = 0;
			//panel.localPosition = new Vector3(-351, 479, 0);
			//panel.localScale = new Vector3(.8f, .8f, 0);
			//closeButton.localPosition = new Vector3(359.100006f, 275.600006f, 0);
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
			//		panel.DOAnchorPos(new Vector2(panel.transform.position.x, -1467f), 0.25f, true).OnComplete(() =>
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

	public void ActiveCVGroup()
	{
		if (!canvasGroup.blocksRaycasts)
		{
			canvasGroup.blocksRaycasts = true;
		}
	}
}
