using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ChetPanel : MonoBehaviour
{
	// Start is called before the first frame update
	public int value = 0;
	public Slider[] slider = new Slider[2];
	public TextMeshProUGUI strikeScore;
	public RectTransform continueButton;
	private void Awake()
	{
		
	}
	void Start()
	{
		
	}
	private void OnEnable()
	{
		slider[0].value = SaveSystem.instance.strike;
		if (slider[0].value < slider[0].maxValue)
		{
			AudioManager.instance.PlaySFX("FillUpSlider");
			slider[1].value = slider[0].value;
			slider[0].DOValue(slider[0].value + 1, 1f);
			slider[1].DOValue(slider[1].value +1, .95f).OnComplete(() =>
			{
				slider[1].value = slider[0].value;
				strikeScore.text =	(Mathf.Round(slider[0].value)).ToString();
				SaveSystem.instance.strike = Mathf.RoundToInt((slider[0].value));
				SaveSystem.instance.SaveData();
			});
		} 
		else
		{

		}
	}
	// Update is called once per frame
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			UIManager.instance.gamePlayPanel.Close();
			this.gameObject.SetActive(true);
			GameManager.instance.hasUI = true;
			AudioManager.instance.PlaySFX("OpenPopUp");

		}
		if (slider[0].value == slider[0].maxValue)
		{
			continueButton.gameObject.SetActive(false);
			this.gameObject.SetActive(true);
			StartCoroutine(waitForSlider());
		}
		else
		{
			continueButton.gameObject.SetActive(true);
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{

			AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_chest, () =>
			{
				this.gameObject.SetActive(false);
				AudioManager.instance.PlaySFX("ClosePopUp");
				if (slider[0].value != slider[0].maxValue)
				{
					UIManager.instance.gamePlayPanel.backFromChestPanel = true;
					UIManager.instance.gamePlayPanel.backFromPause = false;
					UIManager.instance.gamePlayPanel.Open();
					GameManager.instance.hasUI = false;
				}
			});

		}
	}
	IEnumerator waitForSlider()
	{
		yield return new WaitForSeconds(1f);
		UIManager.instance.congratPanel.Open();
		slider[0].value = 0;
		slider[1].value = 0;
		SaveSystem.instance.strike = (int)slider[0].value;
		SaveSystem.instance.SaveData();
		this.gameObject.SetActive(false);
	}
}
