using System.Collections;
using System.Collections.Generic;
using TMPro;
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
	public ExtralHole extraHolePanel;
	public BuyPurpleStar BuyPurpleStarPanel;
	public BuyGoldenStar BuyGoldenStarPanel;

	public Button RelayButton;
	public Button HintButton;
	public Button DeteleNailButton;
	public Button UndoButton;
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
		
		levelText.text = (GameManager.instance.currentLevel + 1 ).ToString();
		if (undoPanel.hasUse)
		{
			UndoButton.interactable = false;
		}
		else
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
		if (deteleNailPanel.hasUse)
		{
			DeteleNailButton.interactable = false;
		}
	}
	public void OpenPausePanel()
	{
		pausePanel.Open();
		timer.TimerOn = false;
	}
	public void ShowHint()
	{
		hintImgPanel.Open();
		hintImgPanel.LoadHint();
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
	}
	public void OpenExtraHolePanel()
	{
		extraHolePanel.Open();
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			isPause = false;
			this.gameObject.SetActive(true);
			levelManager.gameObject.SetActive(true);
			gameManager.gameObject.SetActive(true) ;
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
					UIManager.instance.gamePlayPanel.deteleNailPanel.hasUse = false;
					UIManager.instance.gamePlayPanel.undoPanel.hasUse = false;
					gameManager.LoadLevelFromUI();
					backFromChestPanel = false;
				}
				else
				{
					gameManager.LoadLevelFromUI();
				}
				backFromChestPanel = false;
				Settimer();
				backFromPause = false;
			}
			timer.TimerOn = true;
		}
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
			levelManager.gameObject.SetActive(false);
			gameManager.gameObject.SetActive(false);
			this.gameObject.SetActive(false);
			GameManager.instance.deleting = false;
			timer.TimerOn = false;
		}
	}
	public void Settimer()
	{
		timer.SetTimer(121f);
	}
	public void OpenLosePanel()
	{
		losePanel.Open();
	}
	public void OpenPurpleStarShop()
	{
		BuyPurpleStarPanel.Open();
	}
	public void OpenGoldenStarShop()
	{
		BuyGoldenStarPanel.Open();
	}
}
