using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanel : MonoBehaviour
{
	public PausePanel pausePanel;
	public RePlayPanel rePlayPanel;
	public HintPanel hintPanel;
	public DeteleNailPanel deteleNailPanel;
	public HintImgPanel hintImgPanel;
	public Timer timer;
	public LosePanel losePanel;
	public UndoPanel undoPanel;
	public HardLevel hardLevel;
	public welcomeLevel welcomeLevel;
	public ExtralHole extraHolePanel;
	public BuymagicTiket BuymagicTiketPanel;
	public BuypowerTicket BuypowerTicketPanel;

	public Button RelayButton;
	public Button HintButton;
	public Button DeteleNailButton;
	public Button UndoButton;
	public GameObject boosterButton;


	//public RectTransform Blockpanel;
	//public LosePanel winPanel;

	public DisplayLevel level;

	public TextMeshProUGUI levelText;

	public GameManager gameManager;
	public LevelManager levelManager;

	public bool isPause = false;
	public bool backFromPause = false;
	public bool backFromChestPanel = false;

	private void Start()
	{
		
	}
	private void Update()
	{
		if(GameManager.instance.currentLevel == 0)
		{
			if (!levelManager.levelInstances.IsNullOrEmpty()){
				if (levelManager.levelInstances[0].GetComponent<Level>().stage != 3)
					boosterButton.gameObject.SetActive(false);
				else
				{
					boosterButton.gameObject.SetActive(true);
				}
			}

		}
		else
		{

			boosterButton.gameObject.SetActive(true);

		}
		
		levelText.text = (GameManager.instance.currentLevel + 1 ).ToString();
			try
			{
				if (InputManager.instance.hasSave)
				{
					UndoButton.interactable = true;
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
		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.ingame_pause, () =>
		{
			pausePanel.Open();
			timer.TimerOn = false;
		});
	}
	public void ShowHint()
	{
		hintImgPanel.Open();
		try
		{
			hintImgPanel.LoadHint();
		}
		catch { }
		timer.TimerOn = false;
	}
	public void OpenReplayPanel()
	{
		rePlayPanel.Open();
		timer.TimerOn = false;
	}
	public void OpenHintPanel()
	{
		hintPanel.Open();
		timer.TimerOn = false;
	}
	public void OpenDeteleNailPanel()
	{
		deteleNailPanel.Open();
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
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
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
					gameManager.currentLevel++;
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
				UIManager.instance.gamePlayPanel.pausePanel.isdeleting = false;
			}
			timer.TimerOn = true;
		}
	}
	public void EnableBoosterButton()
	{
		UIManager.instance.gamePlayPanel.rePlayPanel.numOfUsed = 1;
		UIManager.instance.gamePlayPanel.hintPanel.numOfUsed = 1;
		UIManager.instance.gamePlayPanel.undoPanel.numOfUsed = 1;
		UIManager.instance.gamePlayPanel.deteleNailPanel.numOfUsed = 1;
		UIManager.instance.gamePlayPanel.losePanel.watchAdButton.GetComponent<Button>().interactable = true;
		UIManager.instance.gamePlayPanel.losePanel.hasUse = false;
		UIManager.instance.gamePlayPanel.hintPanel.hasUse = false;
		SaveSystem.instance.playHardTime = 0;
	} 
	public void ButtonOff()
	{
		RelayButton.interactable = false;
		HintButton.interactable = false;
		DeteleNailButton.interactable = false;
		UndoButton.interactable = false;
	}
	public void ButtonOn()
	{
		RelayButton.interactable = true;
		HintButton.interactable = true;
		DeteleNailButton.interactable = true;
	}

	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			isPause = true;
			//levelManager.gameObject.SetActive(false);
			//gameManager.gameObject.SetActive(false);
			this.gameObject.SetActive(false);
			GameManager.instance.deleting = false;
			timer.TimerOn = false;
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
	public void OpenmagicTiketShop()
	{
		BuymagicTiketPanel.Open();
		timer.TimerOn = false;
	}
	public void OpenpowerTicketShop()
	{
		BuypowerTicketPanel.Open();
		timer.TimerOn = false;
	}
}
