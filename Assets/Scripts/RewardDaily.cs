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


	public Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
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
                button.enabled = true;
                panelImg.sprite = panelChangeSprite;
            }
			else
			{
				panelImg.sprite = panelDefaultSprite;
                button.enabled = false;
            }
		}
		else
		{
            button.enabled = false;
            this.isActive = false;
			for (int i = 0; i < rewardImg.Length; i++)
			{
                button.interactable = false;
                rewardImg[i].transform.localPosition = panelPosition[i];
				rewardImg[i].sprite = Resources.Load<Sprite>("UI/UI Nut/received stamp");
				rewardImg[i].GetComponent<Image>().SetNativeSize();
				rewardImg[i].enabled = true;
			}
			for (int i = 0; i < tickImg.Length; i++)
			{

                tickImg[i].transform.gameObject.SetActive(false);
			}
			panelImg.sprite = panelClaimSprite;
            Color x =new Color(0xBB / 255f, 0x7B / 255f, 0x4C / 255f);
			dayText.color = x;
			//Deactive();
		}
	}

}
