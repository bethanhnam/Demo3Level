using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class RewardDaily : MonoBehaviour
{
	public Image panelImg;
	public TextMeshProUGUI dayText;
	public Vector3[] panelPosition;

	public Image[] rewardImg;
	public Image[] tickImg;

	public Sprite panelChangeSprite;
	public Sprite panelDefaultSprite;
	public Sprite panelClaimSprite;
	public RectTransform reciveRewardpoint;

	public bool isClaim = false;
	public bool isActive = false;
	public int magicTiket;
	public int powerTicket;
	public int extraHolePoint;
	public int gold;

	private void Start()
	{
		panelPosition = new Vector3[rewardImg.Length];
		for (int i = 0; i < rewardImg.Length; i++)
		{
			panelPosition[i] = rewardImg[i].transform.localPosition;
		}
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
			}
		}
		else
		{
			this.isActive = false;
			for (int i = 0; i < rewardImg.Length; i++)
			{
				rewardImg[i].transform.localPosition = panelPosition[i];
				rewardImg[i].sprite = Resources.Load<Sprite>("UI/UI Nut/export/Daily RW/Daily RW (2)/Daily RW/received stamp");
				rewardImg[i].GetComponent<Image>().SetNativeSize();
				rewardImg[i].enabled = true;
			}
			for (int i = 0; i < tickImg.Length; i++)
			{
				//tickImg[i].sprite = Resources.Load<Sprite>("UI/UI Nut/export/Daily RW/Daily RW (2)/Daily RW/received stamp");
    //            tickImg[i].GetComponent<Image>().SetNativeSize();
				//tickImg[i].GetComponent<Image>().enabled = true;
				tickImg[i].transform.gameObject.SetActive(false);
			}
			panelImg.sprite = panelClaimSprite;
            Color x =new Color(0xBB / 255f, 0x7B / 255f, 0x4C / 255f);
			dayText.color = x;
			//Deactive();
		}
	}
	public void Active()
	{
		panelImg.sprite = panelChangeSprite;
	}
	public void Deactive()
	{
		panelImg.sprite = panelDefaultSprite;

	}
}
