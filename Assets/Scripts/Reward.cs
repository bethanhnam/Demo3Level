using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    public Image panelImg;
    public Image rewardImg;
	public Sprite panelChangeSprite;

    public bool isClaim = false;
    public bool isActive = false;
	public int purpleStar;
	public int GoldenStar;
    
	private void Update()
	{
		if (!isClaim)
		{
			if (isActive)
			{
				this.Active();
			}
			else
			{

				panelImg.sprite = Resources.Load<Sprite>("UI/UI Nut/export/Daily RW/Daily RW/popup_brown");
				this.GetComponent<Button>().interactable = false;
			}
		}
		else
		{
			this.isActive = false;
			rewardImg.enabled = true;
			rewardImg.sprite = Resources.Load<Sprite>("UI/UI Nut/export/Daily RW/Daily RW/received stamp");
			for (int i = 0; i < rewardImg.transform.childCount; i++)
			{
				rewardImg.transform.GetChild(i).gameObject.SetActive(false);
			}
			this.GetComponent<Button>().interactable = false;
			Deactive();
		}
	}
	public void Active()
	{
		panelImg.sprite = panelChangeSprite;
		this.GetComponent<Button>().interactable = true;
	}
	public void Deactive()
	{
		panelImg.sprite = Resources.Load<Sprite>("UI/UI Nut/export/Daily RW/Daily RW/popup_brown");
		
	}
}
