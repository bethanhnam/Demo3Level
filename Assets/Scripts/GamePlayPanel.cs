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

	public Button RelayButton;
	public Button HintButton;
	public Button DeteleNailButton;
	//public LosePanel winPanel;
	public TextMeshProUGUI levelText;

	public GameManager gameManager;
	public LevelManager levelManager;

	private void Start()
	{
		
	}
	private void Update()
	{
		levelText.text = GameManager.instance.currentLevel.ToString();
		if(GameManager.instance.deleting == true)
		{
			RelayButton.interactable = false;
			HintButton.interactable = false;
			DeteleNailButton.interactable =false;
		}
		else
		{
			RelayButton.interactable = true;
			HintButton.interactable = true;
			DeteleNailButton.interactable = true;
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
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			levelManager.gameObject.SetActive(true);
			gameManager.gameObject.SetActive(true) ;
			Settimer();
			timer.TimerOn = true;
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			gameManager.gameObject.SetActive(false);
			levelManager.gameObject.SetActive(false);
			this.gameObject.SetActive(false);
			GameManager.instance.deleting = false;
			timer.TimerOn = false;
			timer.SetTimer(0);
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
}
