
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Level : MonoBehaviour
{
	public static Level instance;
	public StageManager stageManager;
	public string layerName = "Hole";
	public bool hasDone = false;
	public Item Item;
	public int stage = 0;
	public GameObject itemObject;
	public GameObject itemAppearParticle;
	public Vector3 scale;
	public Transform transformFixImg;

	private void Awake()
	{
		itemObject = Resources.Load<GameObject>("ObjjPrefab/item");
		itemAppearParticle = Resources.Load<GameObject>("ObjjPrefab/item");
	}
	private void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		stage = 0;
		LoadStage(stage);
	}
	public void CheckLevel()
	{
		stageManager.RemoveLevel(stage);
		if (stage >= stageManager.levels.Count)
		{
			{
				SaveSystem.instance.playingHard = false;

			}
		}
		else
		{
			StartCoroutine(LoadHardLevel());
		}
	}
	public void CreateItemAndMove()
	{
		itemObject.transform.localScale = Level.instance.transform.GetChild(0).GetComponent<StageManager>().levels[0].GetComponent<Stage>().item.transform.localScale;
		GameObject item = this.transform.GetChild(0).GetComponent<StageManager>().levels[0].GetComponent<Stage>().item;
		this.transform.GetChild(0).GetChild(0).GetComponent<Stage>().item.gameObject.SetActive(false);
		var item1 = Instantiate(itemObject, Vector3.zero, Quaternion.identity, Level.instance.transform);
		foreach (var character in MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().characters)
		{
			character.SetActive(false);
		}
		UIManager.instance.menuPanel.menuFrame.gameObject.SetActive(false);
		UIManager.instance.menuPanel.settingButton.gameObject.SetActive(false);
		UIManager.instance.menuPanel.noAdsButton.gameObject.SetActive(false);
		UIManager.instance.menuPanel.magicBar.gameObject.SetActive(false);
		UIManager.instance.menuPanel.powerBar.gameObject.SetActive(false);
		//var gameobj = Instantiate(ParticlesManager.instance.ItemAppearlParticleObject, item1.transform.position, Quaternion.identity, this.transform);
		//ParticleSystem particleSystem = gameobj.transform.GetChild(0).GetComponent<ParticleSystem>();
		//var shape = particleSystem.shape;
		//shape.sprite = this.Item.itemImg;
		//Destroy(gameobj, 0.3f);
		Color color = new Color(1, 1, 1, 0.5f);
		item1.GetComponent<Image>().color = color;
		item1.GetComponent<SpriteRenderer>().sprite = this.Item.itemImg;
		item1.GetComponent<Image>().sprite = this.Item.itemImg;
		item1.GetComponent<Image>().enabled = true;
		item1.GetComponent<Image>().SetNativeSize();
		item1.transform.localScale = scale;
		Color color1 = new Color(1, 1, 1, 1);
		item1.GetComponent<Image>().DOColor(color1, 0.29f);
		UIManager.instance.gamePlayPanel.pausePanel.HomeFormComplete();
		UIManager.instance.menuPanel.blockImg.gameObject.SetActive(true);
		UIManager.instance.menuPanel.blockImg.GetComponent<CanvasGroup>().alpha = 0.6f;
		//SetHasdone(MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().currentStage);
		float targetAspect = 9.0f / 16.0f;
		float windowAspect = (float)Screen.width / (float)Screen.height;
		var s = DOTween.Sequence();
		s.Append(item1.transform.DOScale(new Vector3(1.5f * (targetAspect / windowAspect), 1.5f * (targetAspect / windowAspect), 1), 0.5f));
		s.AppendInterval(0.2f);
		s.OnComplete(() =>
		{
			Level.instance.SetHasdone(MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().currentStage);
			SaveSystem.instance.strike = Mathf.RoundToInt((UIManager.instance.menuPanel.slider[0].value + 1));
			SaveSystem.instance.SaveData();
			UIManager.instance.DeactiveTime();
			UIManager.instance.gamePlayPanel.blockImg.gameObject.SetActive(false);
			UIManager.instance.menuPanel.MoveItem1(item1, MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().currentStage, transformFixImg);
		});
	}
	public void SetHasdone(int stage)
	{
		int length = 0;
		length = MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().listPlayButtons[stage].Length;
		for (int i = 0; i < length; i++)
		{
			var buttons = MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().listPlayButtons[stage];
			var currentLevel = buttons[i].GetComponent<LevelButton>().level;
			if (GameManager.instance.currentLevel == currentLevel)
			{
				buttons[i].GetComponent<LevelButton>().hasDone = true;
				transformFixImg = buttons[i].GetComponent<LevelButton>().fixedImg.transform;

				buttons[i].GetComponent<LevelButton>().gameObject.SetActive(false);

				if (!MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().completeList.Contains(buttons[i].GetComponent<LevelButton>().gameObject))
				{
					MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().completeList.Add(buttons[i].GetComponent<LevelButton>().gameObject);
					MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().unCompleteList.Remove(buttons[i].GetComponent<LevelButton>().gameObject);
					
				}
			}
		}

	}
	public void LoadStage(int stage)
	{
		UIManager.instance.gamePlayPanel.ButtonOff();
		UIManager.instance.gamePlayPanel.ButtonOn();
		try
		{
			InputManager.instance.hasSave = false;
		}
		catch
		{
		}
		stageManager.LoadStage(stage);
		this.stage++;
		SaveSystem.instance.SaveData();
	}
	IEnumerator LoadHardLevel()
	{
		ClosePopUp();
		if (GameManager.instance.currentLevel == 0)
		{
			if (stage == 1)
			{
				UIManager.instance.gamePlayPanel.OpenWelcomePanel();
			}
			else
			{
				UIManager.instance.gamePlayPanel.OpenHardPanel();
				AudioManager.instance.PlaySFX("Warning");
			}
		}
		else
		{
			UIManager.instance.gamePlayPanel.OpenHardPanel();
		}
		yield return new WaitForSeconds(1.5f);
		if (GameManager.instance.currentLevel == 0)
		{
			if (stage == 1)
			{
				UIManager.instance.gamePlayPanel.welcomeLevel.Close();
			}
			else
			{
				UIManager.instance.gamePlayPanel.hardLevel.Close();
			}
		}
		else
		{
			UIManager.instance.gamePlayPanel.hardLevel.Close();
		}

		LoadStage(stage);
		SaveSystem.instance.playingHard = true;
		SaveSystem.instance.playHardTime = 0;

	}

	private static void ClosePopUp()
	{
		UIManager.instance.gamePlayPanel.extraHolePanel.gameObject.SetActive(false);
		UIManager.instance.gamePlayPanel.deteleNailPanel.gameObject.SetActive(false);
		UIManager.instance.gamePlayPanel.rePlayPanel.gameObject.SetActive(false);
		UIManager.instance.gamePlayPanel.undoPanel.gameObject.SetActive(false);
	}

	public void ChangeLayer()
	{
		int layer = LayerMask.NameToLayer(layerName); // Chuyển đổi tên layer thành ID layer
		if (layer != -1) // Kiểm tra xem layer có tồn tại không
		{
			if (stageManager.levelInstances[0].GetComponent<Stage>().holeToUnlock.gameObject != null)
			{
				stageManager.levelInstances[0].GetComponent<Stage>().holeToUnlock.gameObject.layer = layer; // Đặt layer cho GameObject

			}
		}
		else
		{
			Debug.LogError("Layer " + layerName + " does not exist!"); // In ra thông báo lỗi nếu layer không tồn tại
		}
	}
}