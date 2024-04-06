using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Rendering;
using System;
using Unity.VisualScripting;
using System.Linq.Expressions;

public class MenuPanel : MonoBehaviour
{
	[Header("Panel")]
	public SettingPanel settingPanel;
	public DailyPanel dailyPanel;
	public NonAdsPanel nonAdsPanel;
	public LevelManager levelManager;
	public GameManager gameManager;
	public bool isplaying;
	public GameObject bgImg;
	public GameObject buttons;

	public CanvasGroup canvasGroup;
	public RectTransform Blockpanel;

	public int value = 0;
	public Slider[] slider = new Slider[2];
	public TextMeshProUGUI strikeScore;
	public TextMeshProUGUI maxScore;
	public GameObject itemObject;
	public GameObject completeParticle;

	public bool hasComplete = false;
	private void Awake()
	{
		itemObject = Resources.Load<GameObject>("ObjjPrefab/item");
		completeParticle = Resources.Load<GameObject>("UseablePartical/particle");
	}
	private void Start()
	{
		slider[0].value = SaveSystem.instance.strike;
		AudioManager.instance.PlayMusic("MenuTheme");
	}
	private void Update()
	{
		if (hasComplete)
		{
			FixItem();
		}
	}
	private void OnEnable()
	{
		slider[0].value = SaveSystem.instance.strike;
		slider[1].value = SaveSystem.instance.strike;
		strikeScore.text = (Mathf.Round(slider[0].value)).ToString();
		CheckStrike();

	}
	public void PlayGame()
	{
		AudioManager.instance.PlaySFX("Button");
		Close();
		UIManager.instance.gamePlayPanel.Open();
		isplaying = true;
		AudioManager.instance.PlayMusic("GamePlayTheme");
		UIManager.instance.gamePlayPanel.EnableBoosterButton();
		GameManager.instance.hasDone = false;
	}
	public void OpenSettingPanel()
	{
		AudioManager.instance.PlaySFX("Button");
		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.menu_setting, () =>
		{
			settingPanel.Open();
		});
	}
	public void OpenDailyPanel()
	{
		AudioManager.instance.PlaySFX("Button");
		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.menu_daily, () =>
		{
			dailyPanel.Open();
		});

	}
	public void OpenNonAdsPanel()
	{
		AudioManager.instance.PlaySFX("Button");
		nonAdsPanel.Open();

	}
	public void OpenShopPanel()
	{
		AudioManager.instance.PlaySFX("Button");
		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.menu_shop, () =>
		{
			UIManager.instance.shopPanel.Open();
		});
	}

	public void Open()
	{
		AudioManager.instance.PlayMusic("MenuTheme");
		if (!this.gameObject.activeSelf)
		{
			isplaying = false;
			this.gameObject.SetActive(true);
			Blockpanel.gameObject.SetActive(true);
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, .7f).OnComplete(() =>
			{
				Blockpanel.gameObject.SetActive(false);
			});
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{

			this.gameObject.SetActive(true);
			Blockpanel.gameObject.SetActive(true);
			canvasGroup.alpha = 1;
			canvasGroup.DOFade(0, .7f).OnComplete(() =>
			{
				Blockpanel.gameObject.SetActive(false);
				this.gameObject.SetActive(false);
			});
		}
	}
	public void ChestSlider()
	{
		if (slider[0].gameObject.activeSelf == false)
		{
			return;
		}
		else
		{
			
			if ((Mathf.Round(slider[0].value) < slider[0].maxValue))
			{
				AudioManager.instance.PlaySFX("FillUpSlider");
				slider[1].value = (Mathf.Round(slider[0].value));
				strikeScore.text = (Mathf.Round(slider[0].value + 1)).ToString();
				slider[0].DOValue(slider[0].value + 1, 0.4f);
				slider[1].DOValue(slider[1].value + 1, .35f).OnComplete(() =>
				{
					slider[1].value = (Mathf.Round(slider[0].value));
					strikeScore.text = (Mathf.Round(slider[0].value)).ToString();
				});
			}
		}
		if (MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().hasDone == true)
		{
				MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().menuImg.sprite = MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().completeImg;
				MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().NextLevel();
		}
	}
	public void CheckStrike()
	{
		if ((Mathf.Round(slider[0].value) == slider[0].maxValue))
		{
			this.gameObject.SetActive(true);
			StartCoroutine(waitForSlider());
		}
	}
	IEnumerator waitForSlider()
	{
		yield return new WaitForSeconds(1f);
		slider[0].value = 0;
		slider[1].value = 0;
		SaveSystem.instance.strike = (int)slider[0].value;
		SaveSystem.instance.SaveData();
		this.gameObject.SetActive(false);
		this.gameObject.SetActive(false);
		if (SaveSystem.instance.level == 1)
		{
			UIManager.instance.gamePlayPanel.OpenRatingPanel();
			MenuLevelManager.instance.RemoveLevel(SaveSystem.instance.level);
		}

		else
		{
			UIManager.instance.congratPanel.Open();
		}
	}
	public void CreateItem(Sprite sprite)
	{
		SaveSystem.instance.strike = Mathf.RoundToInt((slider[1].value + 1));
		SaveSystem.instance.SaveData();
		for (int i = 0; i < MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().playButtons.Length; i++)
		{
			if (MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().playButtons[i].GetComponent<LevelButton>().hasDone == true)
			{
				if (MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().playButtons[i].GetComponent<LevelButton>().fixedImg.activeSelf == false)
				{
					var item = Instantiate(itemObject, Vector3.zero, Quaternion.identity, this.transform);
					item.GetComponent<SpriteRenderer>().enabled = false;
					item.AddComponent<Image>().sprite = sprite;
					item.GetComponent<Image>().SetNativeSize();
					item.transform.localScale = new Vector3(1, 1, 1);
					item.transform.DOMove(MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().playButtons[i].GetComponent<LevelButton>().transform.GetChild(0).transform.position, 1f).OnComplete(() =>
					{
						Destroy(item);
						bgImg.transform.GetChild(0).transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
					});
					
				}
			}
		}
		hasComplete = true;
	}
	public void FixItem1()
	{
		StartCoroutine(FixItem());
	}
	IEnumerator FixItem()
	{
		yield return new WaitForSeconds(1);
		try
		{
			for (int i = 0; i < MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().playButtons.Length; i++)
			{
				if (MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().playButtons[i].GetComponent<LevelButton>().hasDone == true)
				{
					MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().playButtons[i].GetComponent<LevelButton>().fixedImg.SetActive(true);
					//var gameobj = Instantiate(completeParticle, MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().playButtons[i].GetComponent<LevelButton>().fixedImg.transform.position, Quaternion.identity, this.transform);
					//Destroy(gameobj, 2f);
				}
			}
			if (MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().hasDone == true)
			{
				for (int i = 0; i < MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().menuImg.transform.childCount; i++)
				{
					Destroy(MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().menuImg.transform.GetChild(i).gameObject);
				}
			}
		}
		
		catch { }
		yield return new WaitForSeconds(2);
		ChestSlider();
		hasComplete = false;
	}
}
