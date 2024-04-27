using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public int currentLevel = 0;
	public int checkLevel = 0;
	public bool endgame = false;
	public LevelManager levelManager;

	private int magicTiket;
	private int powerTicket;
	public bool deleting;
	public bool deletingIron;
	public bool hasUI;
	public bool hasMove;
	public bool hasDone;

	public LevelButton currentLevelButton;
	private void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
	private void Awake()
	{
		try
		{
			magicTiket = SaveSystem.instance.GetmagicTiket();
			powerTicket = SaveSystem.instance.GetpowerTicket();
		}
		catch
		{

		};
	}
	private void OnEnable()
	{
		
	}
	private void Update()
	{
	}
	public void Replay()
	{
		if (LevelManager.instance.transform.childCount > 0)
		{
			if (LevelManager.instance.transform.GetChild(0) != null)
			{
				Destroy(LevelManager.instance.transform.GetChild(0).gameObject);
				LevelManager.instance.levelInstances.Clear();
				StartCoroutine(LoadLevel());
				SaveSystem.instance.playHardTime = 0;
				SaveSystem.instance.playingHard = false;
				UIManager.instance.gamePlayPanel.Settimer();
			}
		}

	}
	public void Retry()
	{
		if (LevelManager.instance.transform.childCount > 0)
		{
			if (LevelManager.instance.transform.GetChild(0) != null)
			{
				Destroy(LevelManager.instance.transform.GetChild(0).gameObject);
				LevelManager.instance.levelInstances.Clear();
				UIManager.instance.gamePlayPanel.pausePanel.isdeleting = false;
				UIManager.instance.gamePlayPanel.pausePanel.isdeletingIron = false;
				StartCoroutine(LoadLevel());
				SaveSystem.instance.playingHard = false;
				UIManager.instance.gamePlayPanel.Settimer();
				UIManager.instance.gamePlayPanel.losePanel.watchAdButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UI Nut/export/win/bttn_green");
				UIManager.instance.gamePlayPanel.EnableBoosterButton();
			}
		}

	}
	public void LoadLevelFromUI()
	{
		StartCoroutine(LoadLevel());
	}
	public IEnumerator LoadLevel()
	{
			UIManager.instance.gamePlayPanel.ButtonOff();
			yield return new WaitForSeconds(0);
			UIManager.instance.gamePlayPanel.ButtonOn();
			try
			{
				InputManager.instance.hasSave = false;
			}
			catch
			{

			}
			levelManager.LoadLevel(currentLevel);
	}
	public void SetCurrentLevel(int level)
	{
		currentLevel = level;
	}

}
