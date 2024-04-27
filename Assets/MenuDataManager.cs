using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRWManager : MonoBehaviour
{
	public static DailyRWManager instance;
	
	public int days = 0;
	public int nonAds = 0;

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		if (instance == null)
		{
			instance = this;
		}
		LoadData();
		CreateData();
		SaveData();
	}
	private void Start()
	{
		if (nonAds > 0)
		{
			AdsManager.instance.isRemoveAds = true;
		}
	}
	private void Update()
	{
	}
	public void SaveData()
	{
		PlayerPrefs.SetInt("Days", days);
		PlayerPrefs.SetInt("NonADS", nonAds);
	}
	public void CreateData()
	{
	}
	public void LoadData()
	{
		days = PlayerPrefs.GetInt("Days");
		nonAds = PlayerPrefs.GetInt("NonADS");
	}


}
