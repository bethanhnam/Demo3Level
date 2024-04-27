//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using UnityEngine;
//using DG.Tweening;
//using UnityEngine.UI;
//using TMPro;
//using UnityEditor.Rendering;
//using System;
//using Unity.VisualScripting;
//using System.Linq.Expressions;
//using DG.Tweening;

//public class MenuPanel : MonoBehaviour
//{
//	[Header("Panel")]
//	public SettingPanel settingPanel;
//	public DailyPanel dailyPanel;
//	public NonAdsPanel nonAdsPanel;
//	public LevelManager levelManager;
//	public GameManager gameManager;
//	public bool isplaying;
//	public GameObject bgImg;
//	public GameObject blockImg;
//	public GameObject blockcanvas;
//	public GameObject buttons;
//	public GameObject canvas;

//	public GameObject menuFrame;
//	public Button settingButton;
//	public Button noAdsButton;
//	public GameObject magicBar;
//	public GameObject powerBar;

//	public CanvasGroup canvasGroup;
//	public RectTransform Blockpanel;

//	public int value = 0;
//	public Slider[] slider = new Slider[2];
//	public GameObject Present;
//	public TextMeshProUGUI strikeScore;
//	public TextMeshProUGUI maxScore;
//	public GameObject itemObject;

//	public bool hasComplete = false;
//	public bool Completing = false;

//	[SerializeField]
//	private Animator animButton;
//	[SerializeField]
//	private CanvasGroup cvButton;

//	private int appearButton = Animator.StringToHash("appear");
//	private int disappearButton = Animator.StringToHash("disappear");

//	private void Awake()
//	{
//		itemObject = Resources.Load<GameObject>("ObjjPrefab/item");
//	}

//	private void Start()
//	{
//		slider[0].value = SaveSystem.instance.strike;
//		strikeScore.text = (Mathf.Round(slider[0].value)).ToString();
//		AudioManager.instance.PlayMusic("MenuTheme");
//	}

//	public void ActiveUIStart()
//	{
//		cvButton.enabled = false;
//		animButton.Play(appearButton, 0, 0);
//	}

//	private void Update()
//	{
//		if (hasComplete)
//		{
//			FixItem(Level.instance.Item.itemImg, MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().currentPlayButtons, Level.instance.button);
//		}
//		//if (buttons.transform.childCount > 0)
//		//{
//		//	for (int i = 0; i < buttons.transform.childCount; i++)
//		//	{
//		//		buttons.transform.GetChild(i).position = new Vector3(buttons.transform.GetChild(i).position.x, buttons.transform.GetChild(i).position.y, 0);
//		//	}
//		//}
//	}
//	private void OnEnable()
//	{
//		Present.GetComponent<Animator>().SetBool("Shaking", false);
//		slider[0].value = SaveSystem.instance.strike;
//		strikeScore.text = (Mathf.Round(slider[0].value)).ToString();
//	}
//	public void PlayGame()
//	{
//		Close();
//		UIManager.instance.gamePlayPanel.GetComponent<CanvasGroup>().alpha = 1;
//		UIManager.instance.gamePlayPanel.Open();
//		isplaying = true;
//		AudioManager.instance.PlayMusic("GamePlayTheme");
//		UIManager.instance.gamePlayPanel.EnableBoosterButton();
//		GameManager.instance.hasDone = false;
//	}
//	public void OpenSettingPanel()
//	{
//		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.menu_setting, () =>
//		{
//			settingPanel.Open();
//		}, null);
//	}
//	public void OpenDailyPanel()
//	{
//		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.menu_daily, () =>
//		{
//			dailyPanel.Open();
//		}, null);

//	}
//	public void OpenNonAdsPanel()
//	{
//		nonAdsPanel.Open();

//	}
//	public void OpenShopPanel()
//	{
//		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.menu_shop, () =>
//		{
//			UIManager.instance.shopPanel.Open();
//		}, null);
//	}

//	public void Open()
//	{
//		AudioManager.instance.PlayMusic("MenuTheme");
//		if (!this.gameObject.activeSelf)
//		{
//			isplaying = false;
//			this.gameObject.SetActive(true);
//			Blockpanel.gameObject.SetActive(true);
//			canvasGroup.alpha = 0;
//			canvasGroup.DOFade(1, .5f).OnComplete(() =>
//			{
//				Blockpanel.gameObject.SetActive(false);
//			});
//		}
//	}
//	public void Close()
//	{
//		this.GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() =>
//		{
//			Deactive();
//		});

//		canvasGroup.blocksRaycasts = false;
//		animButton.Play(disappearButton);
//	}

//	public void Deactive()
//	{
//		if (gameObject.activeSelf)
//		{
//			gameObject.SetActive(false);
//		}
//	}



//	public void ChestSlider()
//	{

//		if (slider[0].gameObject.activeSelf == false)
//		{
//			return;
//		}
//		else
//		{
//			if ((Mathf.Round(slider[0].value) < slider[0].maxValue))
//			{
//				AudioManager.instance.PlaySFX("FillUpSlider");
//				strikeScore.text = (Mathf.Round(slider[0].value + 1)).ToString();
//				Present.GetComponent<Animator>().SetBool("Shaking", true);
//				slider[0].DOValue(slider[0].value + 1, .39f).OnComplete(() =>
//				{
//					slider[0].value = (Mathf.Round(slider[0].value));
//					strikeScore.text = (Mathf.Round(slider[0].value)).ToString();
//					UIManager.instance.menuPanel.blockImg.gameObject.SetActive(false);
//					Invoke("StopAnimation", 2f);
//				});
//			}
//		}
//		Completing = true;
//		ChangeImgDoneLevel();
//	}

//	public void ChangeImgDoneLevel()
//	{
//		if (PlayerPrefs.GetInt("HasDone") == 1)
//		{
//			MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().menuImg.gameObject.AddComponent<CanvasGroup>();
//			var completeImg = Instantiate(MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().menuImg.gameObject, MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().menuImg.transform.position, Quaternion.identity);
//			completeImg.AddComponent<CanvasGroup>();
//			completeImg.GetComponent<CanvasGroup>().alpha = 1.0f;
//			MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().menuImg.GetComponent<CanvasGroup>().alpha = 0;
//			MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().menuImg.sprite = MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().completeImg;
//			MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().menuImg.GetComponent<CanvasGroup>().DOFade(1, 0.55f);
//			completeImg.GetComponent<CanvasGroup>().DOFade(0.7f, 0.5f).OnComplete(() =>
//			{
//				for (int i = 0; i < MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters.Length; i++)
//				{
//					var gameobj = Instantiate(ParticlesManager.instance.StarParticleObject, MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters[i].transform.GetChild(0).transform.position, Quaternion.identity, this.transform);
//					ParticleSystem particleSystem = gameobj.transform.GetChild(0).GetComponent<ParticleSystem>();
//					var shape = particleSystem.shape;
//					shape.sprite = MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters[i].transform.GetChild(0).GetComponent<Image>().sprite;
//					Destroy(gameobj, 0.5f);
//					MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters[i].transform.GetChild(0).gameObject.SetActive(true);
//					MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters[i].transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0;
//					MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters[i].transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(1, 0.3f);
//					MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters[i].transform.GetChild(1).GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() =>
//					{
//						MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters[i].transform.GetChild(1).gameObject.SetActive(true);
//					});
//				}
//				MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().menuImg.DOFade(1, 0f).OnComplete(() =>
//				{
//					MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().NextLevel();
//				});
//				Destroy(completeImg, 0.2f);
//			});
//		}
//	}

//	public void StopAnimation()
//	{
//		Present.GetComponent<Animator>().SetBool("Shaking", false);
//	}
//	public void CheckStrike()
//	{
//		if ((Mathf.Round(SaveSystem.instance.strike) >= slider[0].maxValue))
//		{
//			PlayButton.instance.GetComponent<Button>().interactable = false;
//			PlayerPrefs.SetInt("HasComplete", 1);
//			this.gameObject.SetActive(true);
//			StartCoroutine(waitForSlider());
//		}
//	}
//	IEnumerator waitForSlider()
//	{
//		yield return new WaitForSeconds(1f);
//		this.gameObject.SetActive(false);
//		if (SaveSystem.instance.menuLevel == 0)
//		{
//			UIManager.instance.gamePlayPanel.OpenRatingPanel();
//		}

//		else
//		{
//			AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_chest, () =>
//			{
//				UIManager.instance.congratPanel.Open();
//			}, null);
//		}
//	}
//	public void MoveItem1(GameObject item, Button[] buttons, Transform transform)
//	{
//		StartCoroutine(MoveItem(item, transform));
//	}
//	IEnumerator MoveItem(GameObject item, Transform transform)
//	{
//		yield return new WaitForSeconds(1f);
//		UIManager.instance.menuPanel.blockImg.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() =>
//		{
//		});
//		float targetAspect = 9.0f / 16.0f;
//		float windowAspect = (float)Screen.width / (float)Screen.height;
//		item.transform.DOScale(1 * (targetAspect / windowAspect), 0.51f);
//		item.transform.DOMove(transform.position, 0.5f).OnComplete(() =>
//		{
//			Destroy(item, 1f);
//			hasComplete = true;
//			FixItem1();
//			//bgImg.transform.GetChild(0).transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
//		});

//	}
//	public void FixItem1()
//	{

//		StartCoroutine(FixItem(Level.instance.Item.itemImg, MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().currentPlayButtons, Level.instance.button));
//	}
//	IEnumerator FixItem(Sprite sprite, Button[] buttons, GameObject button)
//	{
//		yield return new WaitForSeconds(0.1f);
//		try
//		{
//			var x = 0;
//			button.GetComponent<LevelButton>().unfixedImg.SetActive(false);
//			button.GetComponent<LevelButton>().fixedImg.SetActive(true);
//			var gameobj = Instantiate(ParticlesManager.instance.StarParticleObject, button.GetComponent<LevelButton>().fixedImg.transform.position, Quaternion.identity, this.transform);
//			ParticleSystem particleSystem = gameobj.transform.GetChild(0).GetComponent<ParticleSystem>();
//			var shape = particleSystem.shape;
//			shape.sprite = sprite;
//			Destroy(gameobj, 1f);
//		}
//		catch { }
//		yield return new WaitForSeconds(0.4f);
//		LevelManager.instance.gameObject.SetActive(false);
//		foreach (var character in MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters)
//		{
//			character.gameObject.SetActive(true);
//			character.GetComponent<CanvasGroup>().alpha = 0;
//			character.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
//			UIManager.instance.menuPanel.settingButton.gameObject.SetActive(true);
//			UIManager.instance.menuPanel.menuFrame.gameObject.SetActive(true);
//			UIManager.instance.menuPanel.noAdsButton.gameObject.SetActive(true);
//			UIManager.instance.menuPanel.magicBar.gameObject.SetActive(true);
//			UIManager.instance.menuPanel.powerBar.gameObject.SetActive(true);

//			UIManager.instance.menuPanel.settingButton.GetComponent<CanvasGroup>().alpha = 0;
//			UIManager.instance.menuPanel.menuFrame.GetComponent<CanvasGroup>().alpha = 0;
//			UIManager.instance.menuPanel.noAdsButton.GetComponent<CanvasGroup>().alpha = 0;
//			UIManager.instance.menuPanel.magicBar.GetComponent<CanvasGroup>().alpha = 0;
//			UIManager.instance.menuPanel.powerBar.GetComponent<CanvasGroup>().alpha = 0;

//			UIManager.instance.menuPanel.settingButton.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
//			UIManager.instance.menuPanel.menuFrame.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
//			UIManager.instance.menuPanel.noAdsButton.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
//			UIManager.instance.menuPanel.magicBar.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
//			UIManager.instance.menuPanel.powerBar.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
//		}
//		yield return new WaitForSeconds(1f);
//		UIManager.instance.menuPanel.ChestSlider();

//		Blockpanel.gameObject.SetActive(false);
//		hasComplete = false;
//		UIManager.instance.gamePlayPanel.hasDoFade = false;
//		Level.instance.hasDone = false;
//	}
//}
