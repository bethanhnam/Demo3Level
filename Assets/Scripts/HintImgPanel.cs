using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintImgPanel : MonoBehaviour
{

	public Image hintImg;
	public List<Sprite> hintImgs = new List<Sprite>();

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			GameManager.instance.hasUI = true;
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
			GameManager.instance.hasUI = false;
		}
	}
	public void LoadHint()
	{
			hintImg.sprite = LevelManager.instance.levelInstances[0].GetComponent<Level>().stageManager.levelInstances[0].GetComponent<Stage>().hintImg;
	}
}
