using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class weeklyReward : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    public Sprite[] rewardBackGrounds;
    public Sprite[] NumBackGrounds;

    public TMP_FontAsset[] FontAssets;

    public TextMeshProUGUI number;
    public Image rewardImg;
    public TextMeshProUGUI numOfReward;
    public Image claimImage;
    public Image rewardBackGround;
    public Image NumBackGround;

    public rewardType rewardType1;

    public int Index = 0;

    public RectTransform RectTransform { get => rectTransform; }

    private void Start()
    {
        //NotClaim();
    }

    public void Claimed()
    {
        rewardBackGround.sprite = rewardBackGrounds[2];
        NumBackGround.sprite = NumBackGrounds[2];

        numOfReward.font = FontAssets[2];
        numOfReward.ForceMeshUpdate();

        numOfReward.gameObject.SetActive(false);
        claimImage.gameObject.SetActive(true);
    }
    public void NotClaim()
    {
        rewardBackGround.sprite = rewardBackGrounds[0];
        NumBackGround.sprite = NumBackGrounds[0];
        numOfReward.font = FontAssets[0];
        numOfReward.ForceMeshUpdate();

        numOfReward.gameObject.SetActive(true);
        claimImage.gameObject.SetActive(false);
    }
    public void Selected()
    {
        rewardBackGround.sprite = rewardBackGrounds[1];
        NumBackGround.sprite = NumBackGrounds[1];
        numOfReward.font = FontAssets[1];
        numOfReward.ForceMeshUpdate();

        numOfReward.gameObject.SetActive(true);
        claimImage.gameObject.SetActive(false);
    }
    public void SetData(int i, Sprite sprite, int numOfReward1, rewardType rewardType)
    {
        Index = i;
        number.text = i.ToString();
        rewardImg.sprite = sprite;
        numOfReward.text = numOfReward1.ToString();
        rewardType1 = rewardType;
    }

    public enum rewardType
    {
        gold,
        unscrew,
        undo,
        drill
    }
}
