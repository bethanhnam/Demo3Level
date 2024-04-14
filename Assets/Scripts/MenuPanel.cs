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
using DG.Tweening;

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
	public GameObject blockImg;
	public GameObject blockcanvas;
	public GameObject buttons;
	public GameObject canvas;

	public GameObject menuFrame;
	public Button settingButton;
	public Button noAdsButton;
	public GameObject magicBar;
	public GameObject powerBar;

	public CanvasGroup canvasGroup;
	public RectTransform Blockpanel;

	public int value = 0;
	public Slider[] slider = new Slider[2];
	public GameObject Present;
	public TextMeshProUGUI strikeScore;
	public TextMeshProUGUI maxScore;
	public GameObject itemObject;

	public bool hasComplete = false;

	private void Awake()
	{
		itemObject = Resources.Load<GameObject>("ObjjPrefab/item");
	}

	private void Start()
	{
		slider[0].value = SaveSystem.instance.strike;
		slider[1].value = SaveSystem.instance.strike;
		slider[2].value = slider[0].maxValue;
		strikeScore.text = (Mathf.Round(slider[0].value)).ToString();
		AudioManager.instance.PlayMusic("MenuTheme");
	}
	private void Update()
	{
		if (hasComplete)
		{
			FixItem(Level.instance.Item.itemImg,MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().currentStage);
		}
		if (bgImg.transform.childCount > 0)
		{
			bgImg.transform.GetChild(0).transform.position = Vector3.zero;
		}
		if (buttons.transform.childCount > 0)
		{
			for (int i = 0; i < buttons.transform.childCount; i++)
			{
				buttons.transform.GetChild(i).position = new Vector3(buttons.transform.GetChild(i).position.x, buttons.transform.GetChild(i).position.y, 0);
			}
		}
	}
	private void OnEnable()
	{
		Present.GetComponent<Animator>().SetBool("Shaking", false);
		slider[0].value = SaveSystem.instance.strike;
		slider[1].value = SaveSystem.instance.strike;
		slider[2].value = slider[0].maxValue;
		strikeScore.text = (Mathf.Round(slider[0].value)).ToString();
	}
	public void PlayGame()
	{
		AudioManager.instance.PlaySFX("Button");
		Close();
		UIManager.instance.gamePlayPanel.GetComponent<CanvasGroup>().alpha = 1;
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
			canvasGroup.DOFade(1, .5f).OnComplete(() =>
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
				Present.GetComponent<Animator>().SetBool("Shaking", true);
				slider[0].DOValue(slider[0].value + 1, 0.4f);
				slider[1].DOValue(slider[1].value + 1, .39f).OnComplete(() =>
				{
					slider[1].value = (Mathf.Round(slider[0].value));
					strikeScore.text = (Mathf.Round(slider[0].value)).ToString();
					UIManager.instance.menuPanel.blockImg.gameObject.SetActive(false);
					Invoke("StopAnimation", 2f);
				});
			}
		}
		if (MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().hasDone == true)
		{
			MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().menuImg.gameObject.AddComponent<CanvasGroup>();
			MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().menuImg.DOFade(0.7f, 0.5f).OnComplete(() =>
			{
				MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().menuImg.sprite = MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().completeImg;
				for(int i =0;i < MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters.Length; i++)
				{
					var gameobj = Instantiate(ParticlesManager.instance.StarParticleObject, MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters[i].transform.GetChild(0).transform.position, Quaternion.identity, this.transform);
					ParticleSystem particleSystem = gameobj.transform.GetChild(0).GetComponent<ParticleSystem>();
					var shape = particleSystem.shape;
					shape.sprite = MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters[i].transform.GetChild(0).GetComponent<Image>().sprite;
					Destroy(gameobj, 0.5f);
					MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters[i].transform.GetChild(0).gameObject.SetActive(true);
					MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters[i].transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0;
					MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters[i].transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(1, 0.3f);
					MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters[i].transform.GetChild(1).GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() =>
					{
						MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters[i].transform.GetChild(1).gameObject.SetActive(true);
					});
				}
				MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().menuImg.DOFade(1, 0f).OnComplete(() =>
				{
					MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().NextLevel();
				});
			});
		}
	}
	public void StopAnimation()
	{
		Present.GetComponent<Animator>().SetBool("Shaking", false);
	}
	public void CheckStrike()
	{
		if ((Mathf.Round(SaveSystem.instance.strike) >= slider[0].maxValue))
		{
			PlayButton.instance.GetComponent<Button>().interactable = false;
			PlayerPrefs.SetInt("HasComplete", 1);
			this.gameObject.SetActive(true);
			StartCoroutine(waitForSlider());
		}
	}
	IEnumerator waitForSlider()
	{
		yield return new WaitForSeconds(1f);
		this.gameObject.SetActive(false);
		if (SaveSystem.instance.menuLevel == 1)
		{
			UIManager.instance.gamePlayPanel.OpenRatingPanel();
		}

		else
		{
			UIManager.instance.congratPanel.Open();
		}
	}
	public void MoveItem1(GameObject item,int i,Transform transform)
	{
		
		foreach (var character in MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters)
		{
			character.SetActive(false);
		}
		StartCoroutine(MoveItem(item, i,transform));
	}
	IEnumerator MoveItem(GameObject item, int i,Transform transform)
	{
		var buttons1 = MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().listPlayButtons[i];
		yield return new WaitForSeconds(1f);
		UIManager.instance.menuPanel.blockImg.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() => { 
		});
		float targetAspect = 9.0f / 16.0f;
		float windowAspect = (float)Screen.width / (float)Screen.height;
		item.transform.DOScale(1 * (targetAspect / windowAspect), 0.51f);
		item.transform.DOMove(transform.position, 0.5f).OnComplete(() =>
		{
			Destroy(item,1f);
			hasComplete = true;
			
			FixItem1();
			//bgImg.transform.GetChild(0).transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
		});
		
	}
	public void FixItem1()
	{
		
		StartCoroutine(FixItem(Level.instance.Item.itemImg, MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().currentStage));
	}
	IEnumerator FixItem(Sprite sprite,int stage)
	{
		yield return new WaitForSeconds(0.1f);
		try
		{
			for (int i = 0; i < MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().listPlayButtons[stage].Length; i++)
			{
				var buttons = MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().listPlayButtons[stage];
				if (buttons[i].GetComponent<LevelButton>().hasDone == true)
				{
					if (buttons[i].GetComponent<LevelButton>().fixedImg.activeSelf == false)
					{
						buttons[i].GetComponent<LevelButton>().unfixedImg.SetActive(false);
						buttons[i].GetComponent<LevelButton>().fixedImg.SetActive(true);
						var gameobj = Instantiate(ParticlesManager.instance.StarParticleObject, buttons[i].GetComponent<LevelButton>().fixedImg.transform.position, Quaternion.identity, this.transform);
						ParticleSystem particleSystem = gameobj.transform.GetChild(0).GetComponent<ParticleSystem>();
						var shape = particleSystem.shape;
						shape.sprite = sprite;
						Destroy(gameobj, 1f);

					}

				}
			}
		}
		catch { }
		yield return new WaitForSeconds(1f);
		LevelManager.instance.gameObject.SetActive(false);
		foreach (var character in MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters)
		{
			character.gameObject.SetActive(true);
			character.GetComponent<CanvasGroup>().alpha = 0;
			character.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
			UIManager.instance.menuPanel.settingButton.gameObject.SetActive(true);
			UIManager.instance.menuPanel.menuFrame.gameObject.SetActive(true);
			UIManager.instance.menuPanel.noAdsButton.gameObject.SetActive(true);
			UIManager.instance.menuPanel.magicBar.gameObject.SetActive(true);
			UIManager.instance.menuPanel.powerBar.gameObject.SetActive(true);

			UIManager.instance.menuPanel.settingButton.GetComponent<CanvasGroup>().alpha = 0;
			UIManager.instance.menuPanel.menuFrame.GetComponent<CanvasGroup>().alpha = 0;
			UIManager.instance.menuPanel.noAdsButton.GetComponent<CanvasGroup>().alpha = 0;
			UIManager.instance.menuPanel.magicBar.GetComponent<CanvasGroup>().alpha = 0;
			UIManager.instance.menuPanel.powerBar.GetComponent<CanvasGroup>().alpha = 0;

			UIManager.instance.menuPanel.settingButton.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
			UIManager.instance.menuPanel.menuFrame.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
			UIManager.instance.menuPanel.noAdsButton.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
			UIManager.instance.menuPanel.magicBar.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
			UIManager.instance.menuPanel.powerBar.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
		}
		yield return new WaitForSeconds(1f);
		UIManager.instance.menuPanel.ChestSlider();
		
		Blockpanel.gameObject.SetActive(false);
		hasComplete = false;
		UIManager.instance.gamePlayPanel.hasDoFade = false;
		Level.instance.hasDone = false;
	}
}
