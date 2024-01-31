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
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
		}
	}
	public void LoadHint()
	{
		hintImg.sprite = hintImgs[GameManager.instance.currentLevel];
	}
}
