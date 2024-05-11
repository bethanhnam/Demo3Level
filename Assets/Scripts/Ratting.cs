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
	public TextMeshProUGUI rateLaterText;
	public CanvasGroup canvasGroup;

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		canvasGroup = GetComponent<CanvasGroup>();
		SetActiveStar(star.Count-1);
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
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, .3f).OnComplete(() =>
			{
			});

		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			StartCoroutine(DoAfterDelay());
		}
	}
	IEnumerator DoAfterDelay()
	{
		yield return new WaitForSeconds(0.05f); 
		this.gameObject.SetActive(false);
		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_chest, () =>
		{
			GameManagerNew.Instance.CompleteLevelAfterReward();
			//GameManagerNew.Instance.PictureUIManager.DisableCharacter();
			//Destroy(GameManagerNew.Instance.PictureUIManager.gameObject);
			//LevelManagerNew.Instance.NetxtLevel();
			//GameManagerNew.Instance.PictureUIManager = Instantiate(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PictureUIManager, GameManagerNew.Instance.parPic);
			//GameManagerNew.Instance.PictureUIManager.Init(LevelManagerNew.Instance.LevelBase.Level);
			//GameManagerNew.Instance.ScalePicForDevices(GameManagerNew.Instance.PictureUIManager.transform.gameObject);
			//UIManagerNew.Instance.ButtonMennuManager.Appear();
			//UIManagerNew.Instance.ChestSLider.SetMaxValue(GameManagerNew.Instance.PictureUIManager);
			//UIManagerNew.Instance.ChestSLider.SetCurrentValue(LevelManagerNew.Instance.LevelBase.CountLevelWin);
			//GameManagerNew.Instance.SetCompletImg();
			//GameManagerNew.Instance.SetCompleteStory();
			//Debug.Log(LevelManagerNew.Instance.LevelBase.CountLevelWin);
		}, null);

	}
	public void Rate()
	{
		isRated = true;
		if(numOfRate >= 5)
		{
			// mo ra store
			Review.Instance.OpenReview(() => {
			ratingBar.gameObject.SetActive(false);
			rateButton.gameObject.SetActive(false);
			thanksText.gameObject.SetActive(true);
			rateLaterText.gameObject.SetActive(false);
				Invoke("Close", 1f);
			//rateLaterText.text = "Tap To Continue!";
			});
		}
		else
		{
			ratingBar.gameObject.SetActive(false);
			rateButton.gameObject.SetActive(false);
			notThanksText.gameObject.SetActive(true);
			rateLaterText.gameObject.SetActive(false);
			Invoke("Close", 2f);
			//rateLaterText.text = "Tap To Continue!";
		}
	}
}
