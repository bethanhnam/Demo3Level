using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Winpop : MonoBehaviour
{
	public TextMeshProUGUI PlayTimeText;
	public RectTransform PlayTimeRect;
	public RectTransform titleImage;
	public RectTransform continueButton;
	public RectTransform blockPanel;
	public RectTransform panel;
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);

			AudioManager.instance.PlaySFX("Winpop");
			blockPanel.gameObject.SetActive(true);
			SaveSystem.instance.playingHard = false;
			GameManager.instance.hasUI = true;
			int minutes = Mathf.FloorToInt(SaveSystem.instance.playHardTime / 60);
			int seconds = Mathf.FloorToInt(SaveSystem.instance.playHardTime % 60);
			PlayTimeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
			SaveSystem.instance.playHardTime = 0;
			panel.GetComponent<CanvasGroup>().alpha = 1;
			titleImage.transform.localPosition = new Vector3(6, 1805, 0);
			PlayTimeRect.GetComponent<CanvasGroup>().alpha = 0;
			continueButton.transform.localPosition = new Vector3(1.60000002f, -1744f, 0);
			titleImage.DOAnchorPos(new Vector3(6, -72f, 0), 1f, false).OnComplete(() =>
			{
				continueButton.DOAnchorPos(new Vector3(1.60000002f, 74f, 0), 0.5f, false);
				PlayTimeRect.GetComponent<CanvasGroup>().alpha = 1;
				blockPanel.gameObject.SetActive(false);
			});
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_win, () =>
			{
				blockPanel.gameObject.SetActive(true);
				panel.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() =>
				{

					blockPanel.gameObject.SetActive(false);
					this.gameObject.SetActive(false);
					AudioManager.instance.PlaySFX("ClosePopUp");
					UIManager.instance.chestPanel.Open();
					GameManager.instance.hasUI = false;
				});
			});
		}
	}
}
