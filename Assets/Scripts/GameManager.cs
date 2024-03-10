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

	private int purpleStar;
	private int goldenStar;
	public bool deleting;
	public bool hasUI;
	public bool hasMove;

	public int GoldenStar { get => goldenStar; set => goldenStar = value; }
	public int PurpleStar { get => purpleStar; set => purpleStar = value; }

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		currentLevel = SaveSystem.instance.level;
	}
	private void Awake()
	{
		try
		{
			purpleStar = SaveSystem.instance.GetPurpleStar();
			goldenStar = SaveSystem.instance.GetGoldenStar();
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
				StartCoroutine(LoadLevel());
				UIManager.instance.gamePlayPanel.Settimer();
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
		yield return new WaitForSeconds(0.4f);
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

}
