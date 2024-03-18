using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CanvasGroup))]
public class Ratting : MonoBehaviour
{
	public static Ratting instance;
	public List<Star> star = new List<Star>();
	int numOfRate = 0;
	public bool isRated = false;
	public GameObject ratingBar;
	public GameObject rateButton;
	public GameObject panelBoard;
	public TextMeshProUGUI thanksText;
	public TextMeshProUGUI notThanksText;
	public RectTransform Blockpanel;
	public CanvasGroup canvasGroup;

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		canvasGroup = GetComponent<CanvasGroup>();
	}
	private void Update() {
		for (int i = 0; i < star.Count; i++) {
			if (star[i].GetComponent<Star>().isActive)
			{
				SetActiveStar(i);
				SetDeactiveStar(i);
			}
		}

	}
	public void SetActiveStar(int starIndex)
	{
		numOfRate = 0;
		for (int i = 0; i <= starIndex; i++)
		{
			star[i].GetComponent<Image>().color = new Color(
			star[i].GetComponent<Image>().color.r,
			star[i].GetComponent<Image>().color.g,
			star[i].GetComponent<Image>().color.b,
			1f
			);
			numOfRate++;
		}
	}
	public void SetDeactiveStar(int starIndex)
	{
		for (int i = starIndex + 1; i < star.Count; i++)
		{
			star[i].GetComponent<Image>().color = new Color(
			star[i].GetComponent<Image>().color.r,
			star[i].GetComponent<Image>().color.g,
			star[i].GetComponent<Image>().color.b,
			0f
			);
		}
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			AudioManager.instance.PlaySFX("LosePop");
			Blockpanel.gameObject.SetActive(true);
			GameManager.instance.hasUI = true;
			UIManager.instance.DeactiveTime();
			UIManager.instance.gamePlayPanel.Close();
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, .3f).OnComplete(() =>
			{
				Blockpanel.gameObject.SetActive(false);
			});

		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			Blockpanel.gameObject.SetActive(true);
			panelBoard.GetComponent<RectTransform>().DOScale(new Vector3(1f, .1f, 1), 5f).OnComplete(() =>
			{
				Blockpanel.gameObject.SetActive(false);
				this.gameObject.SetActive(false);
				UIManager.instance.winPanel.Open();
			});
		}
	}
	public void Rate()
	{
		isRated = true;
		if(numOfRate >= 5)
		{
			// mo ra store
			Review.Instance.OpenReview();
			
		}
		else
		{
			ratingBar.gameObject.SetActive(false);
			rateButton.gameObject.SetActive(false);
			notThanksText.gameObject.SetActive(true);
			Close();
		}
	}
}
