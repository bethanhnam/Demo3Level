using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
		UIManager.instance.gamePlayPanel.level.done.gameObject.SetActive(true);
		UIManager.instance.gamePlayPanel.level.notDone.gameObject.SetActive(false);
		UIManager.instance.gamePlayPanel.level.levelBar.gameObject.SetActive(false);
	}
	public void CheckLevel()
	{
		stageManager.RemoveLevel(stage);
		if (stage >= stageManager.levels.Count)
		{
			{
				SaveSystem.instance.playingHard = false;
				GameObject item = this.transform.GetChild(0).GetComponent<StageManager>().levels[0].GetComponent<Stage>().item;
				var item1 = Instantiate(itemObject, item.transform.position,Quaternion.identity,this.transform);
				item1.GetComponent<SpriteRenderer>().sprite = this.Item.itemImg;
				item1.GetComponent<SpriteRenderer>().enabled = true;
				//Vector3[] path = { new Vector3(item1.transform.position.x + 3, item1.transform.position.y + 5, 1), new Vector3(item1.transform.position.x + 2, item1.transform.position.y + 1, 1), new Vector3(item1.transform.position.x, item1.transform.position.y, 1) };
				UIManager.instance.gamePlayPanel.boosterButton.gameObject.SetActive(false);
				UIManager.instance.gamePlayPanel.blockImg.gameObject.SetActive(true);
				//GameObject gameObject1 = Instantiate(gameObject,);
				//itemAppearParticle.Play();
				item1.transform.DOScale(1.5f, 0.5f).OnComplete(() =>
				{
				//item1.transform.DOPath(path, 2f, PathType.CatmullRom)
					item1.transform.DOScale(1, 0.3f).OnComplete(() =>
					{
						Destroy(item1);
						UIManager.instance.gamePlayPanel.pausePanel.Home();
						hasDone = true;
						for (int i = 0; i < MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().playButtons.Length; i++)
						{
							if (GameManager.instance.currentLevel == MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().playButtons[i].GetComponent<LevelButton>().level)
							{
								MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().playButtons[i].GetComponent<LevelButton>().hasDone = true;
								if (!MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().completeList.Contains(MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().playButtons[i].GetComponent<LevelButton>().gameObject))
								{
									MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().completeList.Add(MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().playButtons[i].GetComponent<LevelButton>().gameObject);
								}
								UIManager.instance.menuPanel.CreateItem(item1.GetComponent<SpriteRenderer>().sprite);
								UIManager.instance.menuPanel.FixItem1();
							}
						}
						UIManager.instance.DeactiveTime();
						UIManager.instance.gamePlayPanel.level.done.gameObject.SetActive(true);
						UIManager.instance.gamePlayPanel.level.notDone.gameObject.SetActive(true);
						UIManager.instance.gamePlayPanel.level.levelBar.gameObject.SetActive(true);
						UIManager.instance.gamePlayPanel.blockImg.gameObject.SetActive(false);

					});
				});

			}
		}
		else
		{
			StartCoroutine(LoadHardLevel());
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
		UIManager.instance.gamePlayPanel.level.done.gameObject.SetActive(true);
		UIManager.instance.gamePlayPanel.level.notDone.gameObject.SetActive(true);
		UIManager.instance.gamePlayPanel.level.levelBar.gameObject.SetActive(true);
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