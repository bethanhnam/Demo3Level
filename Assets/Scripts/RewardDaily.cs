using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardDaily : MonoBehaviour
{
	public Image panelImg;
	public Vector3 panelPosition;
	public Image rewardImg;
	public Sprite panelChangeSprite;
	public Sprite panelDefaultSprite;
	public RectTransform reciveRewardpoint;

	public bool isClaim = false;
	public bool isActive = false;
	public int magicTiket;
	public int powerTicket;

	private void Start()
	{
		panelPosition = rewardImg.transform.localPosition;
	}
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

				panelImg.sprite = panelDefaultSprite;
				this.GetComponent<Button>().interactable = false;
			}
		}
		else
		{
			this.isActive = false;
			rewardImg.enabled = true;
			rewardImg.transform.localPosition = panelPosition;
			rewardImg.sprite = Resources.Load<Sprite>("UI/UI Nut/export/Daily RW/Daily RW/received stamp");
			rewardImg.GetComponent<Image>().SetNativeSize();
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
		panelImg.sprite = panelDefaultSprite;

	}
}
