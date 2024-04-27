using DG.Tweening;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanel : MonoBehaviour
{
	public PausePanel pausePanel;
	public RePlayPanel rePlayPanel;
	public DeteleNailPanel deteleNailPanel;
	public BoomNail boomNailPanel;
	public Timer timer;
	public LosePanel losePanel;
	public UndoPanel undoPanel;
	public HardLevel hardLevel;
	public welcomeLevel welcomeLevel;
	public ExtralHole extraHolePanel;
	public Ratting rattingPanel;
	public Notice noticePanel;

	public Button RelayButton;
	public Button DeteleNailButton;
	public Button UndoButton;
	public Button BoomButton;

	public GameObject boosterButton;
	public Image bgImg;
	public Image blockImg;

	public DisplayLevel level;

	public TextMeshProUGUI levelText;

	public GameManager gameManager;
	public LevelManager levelManager;

	public bool isPause = false;
	public bool backFromPause = false;
	public bool backFromChestPanel = false;
	public bool hasDoFade = false;

	private void Awake()
	{
	}
	private void OnEnable()
	{
		AudioManager.instance.PlayMusic("GamePlayTheme");
	}
	private void Update()
	{
		if (GameManager.instance.hasDone == false)
		{
			blockImg.gameObject.SetActive(false);
			boosterButton.gameObject.SetActive(true);
			//if (GameManager.instance.currentLevel == 0)
			//{
			//	if (!levelManager.levelInstances.IsNullOrEmpty())
			//	{
			//		if (levelManager.levelInstances[0].GetComponent<Level>().stage != 2)
			//			boosterButton.gameObject.SetActive(false);
			//		else
			//		{
			//			boosterButton.gameObject.SetActive(true);
			//		}
			//	}
			//}
			//else
			//{
			//	boosterButton.gameObject.SetActive(true);
			//}
		}
		else
		{
			blockImg.gameObject.SetActive(true);
			//if (Level.instance.hasDone == false)
			//{
				
			//	Level.instance.hasDone = true;
			//	//Level.instance.SetHasdone(MenuLevelManager.instance.levelInstances[0].GetComponent<MenuLevel>().currentStage);
			//	UIManager.instance.DeactiveTime();
			//	if (losePanel.gameObject.activeSelf)
			//	{
			//		losePanel.gameObject.SetActive(false);
			//	}
			//	//hasDoFade = true;
			//	UIManager.instance.gamePlayPanel.blockImg.GetComponent<CanvasGroup>().alpha = 0;
			//	//Level.instance.transform.GetChild(0).GetChild(0).GetComponent<Stage>().item.gameObject.SetActive(false);
			//	AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_chest,null, () =>
			//	{
			//		Level.instance.CreateItemAndMove();
			//		UIManager.instance.gamePlayPanel.blockImg.GetComponent<CanvasGroup>().DOFade(0.5f, 0.2f);
			//	});
			//}
			boosterButton.gameObject.SetActive(false);
		}

		levelText.text = (GameManager.instance.currentLevel + 1).ToString();
		try
		{
			if (InputManager.instance.hasSave)
			{
				if (GameManager.instance.deleting == false && GameManager.instance.deletingIron == false)
				{
					UndoButton.interactable = true;
				}
				else
				{
					UndoButton.interactable = false;
				}
			}
			else
			{
				UndoButton.interactable = false;
			}
		}
		catch
		{
		}

	}
	public void OpenPausePanel()
	{
			pausePanel.Open();
			timer.TimerOn = false;
	}
	public void OpenReplayPanel()
	{
		rePlayPanel.Open();
		timer.TimerOn = false;
	}
	public void OpenDeteleNailPanel()
	{
		deteleNailPanel.Open();
		timer.TimerOn = false;
	}
	public void OpenBoomNailPanel()
	{
		boomNailPanel.Open();
		timer.TimerOn = false;
	}
	public void OpenUndoPanel()
	{
		undoPanel.Open();
		timer.TimerOn = false;
	}
	public void OpenHardPanel()
	{
		hardLevel.Open();
		timer.TimerOn = false;
	}
	public void OpenWelcomePanel()
	{
		welcomeLevel.Open();
		timer.TimerOn = false;
	}
	public void OpenExtraHolePanel()
	{
		extraHolePanel.Open();
		timer.TimerOn = false;
	}
	public void OpenRatingPanel()
	{
		UIManager.instance.DeactiveTime();
		rattingPanel.Open();
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			DisplayPopUp();
			isPause = false;
			this.gameObject.SetActive(true);
			levelManager.gameObject.SetActive(true);
			gameManager.gameObject.SetActive(true);

			if (backFromPause == false)
			{
				if (levelManager.transform.childCount > 0)
				{
					if (levelManager.transform.GetChild(0) != null)
					{
						Destroy(levelManager.transform.GetChild(0).gameObject);
						levelManager.levelInstances.Clear();
					}
				}
				if (backFromChestPanel == true)
				{
					gameManager.LoadLevelFromUI();
					backFromChestPanel = false;
					EnableBoosterButton();
				}
				else
				{
					gameManager.LoadLevelFromUI();
					EnableBoosterButton();
				}
				backFromChestPanel = false;
				Settimer();
				backFromPause = false;
			}
			else
			{
				if (UIManager.instance.gamePlayPanel.pausePanel.isdeleting)
				{
					GameManager.instance.deleting = true;
				}
				if (UIManager.instance.gamePlayPanel.pausePanel.isdeletingIron)
				{
					GameManager.instance.deletingIron = true;
				}
				UIManager.instance.gamePlayPanel.pausePanel.isdeleting = false;
				UIManager.instance.gamePlayPanel.pausePanel.isdeletingIron = false;
			}
			timer.TimerOn = true;
		}
	}
	public void EnableBoosterButton()
	{
		UIManager.instance.gamePlayPanel.rePlayPanel.numOfUsed = 1;

		UIManager.instance.gamePlayPanel.undoPanel.numOfUsed = 1;
		UIManager.instance.gamePlayPanel.deteleNailPanel.numOfUsed = 1;
		UIManager.instance.gamePlayPanel.boomNailPanel.numOfUsed = 1;
		UIManager.instance.gamePlayPanel.losePanel.watchAdButton.GetComponent<Button>().interactable = true;
		UIManager.instance.gamePlayPanel.losePanel.watchAdButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UI Nut/export/win/bttn_green");
		UIManager.instance.gamePlayPanel.losePanel.hasUse = false;

		SaveSystem.instance.playHardTime = 0;
	}
	public void ButtonOff()
	{
		RelayButton.interactable = false;
		DeteleNailButton.interactable = false;
		UndoButton.interactable = false;
		BoomButton.interactable = false;
	}
	public void ButtonOn()
	{
		RelayButton.interactable = true;
		BoomButton.interactable = true;
		DeteleNailButton.interactable = true;
	}
	public void DisplayPopUp()
	{
		if (UIManager.instance.gamePlayPanel.pausePanel.gameObject.activeSelf)
		{
			UIManager.instance.gamePlayPanel.pausePanel.gameObject.SetActive(false);
		}
		if (UIManager.instance.gamePlayPanel.deteleNailPanel.gameObject.activeSelf)
		{
			UIManager.instance.gamePlayPanel.deteleNailPanel.gameObject.SetActive(false);
		}
		if (UIManager.instance.gamePlayPanel.rePlayPanel.gameObject.activeSelf)
		{
			UIManager.instance.gamePlayPanel.rePlayPanel.gameObject.SetActive(false);
		}
		if (UIManager.instance.gamePlayPanel.undoPanel.gameObject.activeSelf)
		{
			UIManager.instance.gamePlayPanel.undoPanel.gameObject.SetActive(false);
		}
		if (UIManager.instance.gamePlayPanel.extraHolePanel.gameObject.activeSelf)
		{
			UIManager.instance.gamePlayPanel.extraHolePanel.gameObject.SetActive(false);
		}
	}

	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			isPause = true;
			//levelManager.gameObject.SetActive(false);
			//gameManager.gameObject.SetActive(false);
			
			this.GetComponent<CanvasGroup>().alpha = 1;
			this.GetComponent<CanvasGroup>().DOFade(0, .4f).OnComplete(() =>
			{
			GameManager.instance.deleting = false;
			GameManager.instance.deletingIron = false;
			this.gameObject.SetActive(false);
			timer.TimerOn = false;
			});
		}
	}
	public void Settimer()
	{
		timer.SetTimer(181f);
	}
	public void OpenLosePanel()
	{
		losePanel.Open();
	}
}
