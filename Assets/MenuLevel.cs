using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLevel : MonoBehaviour
{
    public Image menuImg;
    public Sprite completeImg;
    public Button[] playButtons;
    public GameObject[] itemPositions;
    public GameObject[] targetItemPositions;
	public List<GameObject> completeList = new List<GameObject>();	
	public int stageLeft;
	public bool hasDone= false;
	private void Start()
	{
		UIManager.instance.menuPanel.slider[0].maxValue = playButtons.Length;
		UIManager.instance.menuPanel.slider[1].maxValue = playButtons.Length;
		UIManager.instance.menuPanel.maxScore.text = playButtons.Length.ToString();
	}
	private void Update()
	{
		if (completeList.Count == playButtons.Length)
		{
			hasDone = true;
		}
		for (int i = 0; i < playButtons.Length; i++)
		{
			if (playButtons[i] != null)
			{
				if (playButtons[i].GetComponent<LevelButton>().hasDone)
				{
					PlayerPrefs.SetInt(i.ToString(), 1);
					
				}
			}
		}
		
	}
	private void OnEnable()
	{
		for (int i = 0; i < playButtons.Length; i++)
		{
			if (playButtons[i] != null)
			{
				if (PlayerPrefs.HasKey(i.ToString()))
				{
					if (PlayerPrefs.GetInt(i.ToString()) == 1){
						playButtons[i].gameObject.SetActive(false);
						if (playButtons[i].GetComponent<LevelButton>().fixedImg != null)
						{
							playButtons[i].GetComponent<LevelButton>().fixedImg.SetActive(true);
						}
					}
				}
			}
		}
		
	}
	public void NextLevel()
	{
		StartCoroutine(nextLevel());
	}
	IEnumerator nextLevel()
	{
		yield return new WaitForSeconds(2);
		UIManager.instance.menuPanel.CheckStrike();
	}
}
