using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class reciveRewardPanel : MonoBehaviour
{
	public RewardDaily[] dayRewards;
	public DailyPanel dailyReward;
	public RectTransform singleReward;
	public RectTransform pairReward;
	public TextMeshProUGUI singleRewardText;
	public TextMeshProUGUI pairReward1Text;
	public TextMeshProUGUI pairReward2Text;
	//public ParticleSystem particle;
	private void OnEnable()
	{
		singleReward.localPosition = new Vector3(27, -13, 0);
		pairReward.localPosition = new Vector3(0, 0, 0);
	}
	public void Show(int lastDate,int numOfReward,Action action)
	{
		//particle.gameObject.SetActive(true);
		if (dailyReward.isClaim)
		{
			
			if (numOfReward > 1)
			{
				singleReward.gameObject.SetActive(false);
				pairReward.gameObject.SetActive(true);
				pairReward.transform.DOScale(1.3f, 0.5f).OnComplete(() =>
				{
					pairReward.DOScale(1, 0.3f);
				});
				pairReward1Text.text = ( "X" +dayRewards[lastDate].powerTicket).ToString();
				pairReward2Text.text = ("X"+ dayRewards[lastDate].magicTiket).ToString();

			}
			else
			{
				pairReward.gameObject.SetActive(false);
				singleReward.GetComponent<Image>().sprite = dayRewards[lastDate].rewardImg[0].sprite;
				singleReward.gameObject.SetActive(true);
				singleReward.transform.DOScale(1.3f, 0.5f).OnComplete(() =>
				{
					singleReward.DOScale(1, 0.3f);
				});
				if (dayRewards[lastDate].powerTicket > 0)
				{
					singleRewardText.text = ("X" + dayRewards[lastDate].powerTicket).ToString();
				}
				else
				{
					singleRewardText.text = ("X" + dayRewards[lastDate].magicTiket).ToString();
				}
			}
		}
		if (dailyReward.isClaimX2)
		{
			if (numOfReward > 1)
			{
				singleReward.gameObject.SetActive(false);
				pairReward.gameObject.SetActive(true);
				pairReward.transform.DOScale(1.3f, 0.5f).OnComplete(() =>
				{
					pairReward.DOScale(1, 0.3f);
				});
				pairReward1Text.text = ("X" + (dayRewards[lastDate].powerTicket * 2)).ToString();
				pairReward2Text.text = ("X" + (dayRewards[lastDate].magicTiket * 2)).ToString();

			}
			else
			{
				pairReward.gameObject.SetActive(false);
				singleReward.gameObject.SetActive(true);
				singleReward.GetComponent<Image>().sprite = dayRewards[lastDate].rewardImg[0].sprite;
				singleReward.transform.DOScale(1.3f, 0.5f).OnComplete(() =>
				{
					singleReward.DOScale(1, 0.3f);
				});
				if (dayRewards[lastDate].powerTicket > 0)
				{
					singleRewardText.text = ("X" + (dayRewards[lastDate].powerTicket *2)).ToString();
				}
				else
				{
					singleRewardText.text = ("X" + (dayRewards[lastDate].magicTiket * 2)).ToString();
				}
			}
		}
		this.gameObject.SetActive(true);
		
		action();
	}
	public void Claim(int lastDate,Action action)
    {
		//dayRewards[lastDate].Active.gameObject.SetActive(true) ;
		PlayerPrefs.SetString("LastClaimTime", DateTime.Today.ToString());
		if (dayRewards[lastDate].rewardImg.Length <= 1)
		{
			var trail = Instantiate(ParticlesManager.instance.StarTrailParticleObject, Vector3.zero, Quaternion.identity,singleReward.transform);
			singleReward.DOAnchorPos(new Vector3(693, 1130, 0), 0.5f).OnComplete(() =>
			{
				AudioManager.instance.PlaySFX("GetReward");
				dayRewards[lastDate].isClaim = true;
				SaveSystem.instance.days = lastDate + 1;
				SaveSystem.instance.addTiket(dayRewards[lastDate].powerTicket, dayRewards[lastDate].magicTiket);
				SaveSystem.instance.SaveData();
				singleReward.gameObject.SetActive(false);
				Destroy(trail);
				Close();
			});
		}
		else
		{
			pairReward.DOAnchorPos(new Vector3(693, 1130, 0), 0.5f).OnComplete(() =>
			{
				AudioManager.instance.PlaySFX("GetReward");
				dayRewards[lastDate].isClaim = true;
				SaveSystem.instance.days = lastDate + 1;
				SaveSystem.instance.addTiket(dayRewards[lastDate].powerTicket, dayRewards[lastDate].magicTiket);
				SaveSystem.instance.SaveData();
				pairReward.gameObject.SetActive(false);
				Close();
			});
		}
	
	}
	public void ClaimX2(int lastDate,Action action)
	{
		//dayRewards[lastDate].Active.gameObject.SetActive(true) ;
		PlayerPrefs.SetString("LastClaimTime", DateTime.Today.ToString());
		if (dayRewards[lastDate].rewardImg.Length <= 1)
		{
			var trail = Instantiate(ParticlesManager.instance.StarTrailParticleObject, Vector3.zero, Quaternion.identity, singleReward.transform);
			singleReward.DOAnchorPos(new Vector3(693, 1130, 0), 0.5f).OnComplete(() =>
			{
				
				dayRewards[lastDate].isClaim = true;
				SaveSystem.instance.days = lastDate + 1;
				SaveSystem.instance.addTiket(dayRewards[lastDate].powerTicket * 2, dayRewards[lastDate].magicTiket * 2);
				SaveSystem.instance.SaveData();
				singleReward.gameObject.SetActive(false);
				Destroy(trail);
				Close();
			});
		}
		else
		{
			pairReward.DOAnchorPos(new Vector3(693, 1130, 0), 0.5f).OnComplete(() =>
			{
				
				dayRewards[lastDate].isClaim = true;
				SaveSystem.instance.days = lastDate + 1;
				SaveSystem.instance.addTiket(dayRewards[lastDate].powerTicket * 2, dayRewards[lastDate].magicTiket * 2);
				SaveSystem.instance.SaveData();
				pairReward.gameObject.SetActive(false);
				Close();
			});
		}
		
	}
	public void Close()
	{
		dailyReward.Close();
	}
}
