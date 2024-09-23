using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Analytics;
using DG.Tweening;
using GoogleMobileAds.Api;
using static FirebaseAnalyticsControl;

public class AdsManager : MonoBehaviour
{
	public static AdsManager instance = null;

	AdmobManager admobManager = null;

	public StructAdsConfig structAds = new StructAdsConfig();

	public StructAdsConfig StructAds { get => structAds; set => structAds = value; }

	public bool _appceptShowAds = true;

	public bool isRemoveAds;

	const string unityPlayerActivity = "UnityPlayerActivity";

	public float lastTime;

	public GameObject noAds;
	public GameObject noAdsPanel;
	public RectTransform popUpNoAdsRec;
	public CanvasGroup popUpNoAdsAlpha;

	public enum PositionAds
	{
		menu_setting,
		menu_daily,
		menu_shop,
		ingame_pause,
		ingame_money,
		endgame_lose,
		endgame_win,
		endgame_chest,
		endgame_bonus
	}

	public void InitDataShow(string value)
	{
		try
		{
			StructAds = JsonConvert.DeserializeObject<StructAdsConfig>(value);
			if (StructAds == null)
			{
				StructAds = new StructAdsConfig();
			}
			else
			{
				if (StructAds.StructAds == null)
				{
					StructAds.StructAds = new Dictionary<string, StructInfoAds>();
				}
			}
		}
		catch
		{
			StructAds = new StructAdsConfig();
		}

	}


	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	// Start is called before the first frame update
	void Start()
	{
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        admobManager = GetComponent<AdmobManager>();
		//StartCoroutine(WaitToLoadScene());
	}

	IEnumerator WaitToLoadScene()
	{
		//yield return new WaitForSecondsRealtime(6f);
		if (RemoteConfigController.instance.IsShowOpenAds == 1)
		{
			AdsControl.Instance.ShowOpenAds();
		}
		yield return new WaitForSecondsRealtime(0.25f);

	}

	public void StartInit(string data, string strucAds)
	{
		AdsControl.Instance.Init(data);
		LogEventScreen("Splash");
		InitDataShow(strucAds);
		StartCoroutine(ShowBannerIE());
	}
    private IEnumerator ShowBannerIE()
	{
		//Debug.Log("1111111" + AdsControl.Instance.isCanShowBanner);
		while (!AdsControl.Instance.isCanShowBanner)
		{
			yield return null;
		}
		//Debug.Log("22222222" + AdsControl.Instance.isCanShowBanner);
		if (PlayerPrefs.GetInt("NonADS") == 0)
		{
			ShowBanner();
		}
	}

	private Action closeAction;

	public void ShowInterstial(AdsManager.PositionAds p, Action actionDone, Action _actionClose)
	{
        if (isRemoveAds || PlayerPrefs.GetInt("NonADS") == 1)
        {
			Debug.Log("chạy vào khi remove ads");
			if (actionDone != null)
			{
				actionDone();
			}
			if (_actionClose != null)
			{
				_actionClose();
			}
            FirebaseAnalyticsControl.Instance.LogEvenAdsStatus(AdsType.force_ads, AdsStatus.success, p.ToString(), AdsRemoveStatus.yes, true);
            return;
		}

		closeAction = _actionClose;
		if(closeAction != null)
		{
			Debug.Log("co close");
		}
		Debug.Log("inter ads: " + p.ToString());
		if (StructAds.StructAds.ContainsKey(p.ToString()))
		{
			Debug.Log("inter ads: " + p.ToString());
			if (StructAds.StructAds[p.ToString()].IsShow  && AdsControl.Instance.CheckFA() && StructAds.ConfigBanner.LevelUnlock <= SaveSystem.instance.menuLevel + 1)
			{
				Debug.Log("inter ads: " + p.ToString());
 				if (Time.time - lastTime >= StructAds.StructAds[p.ToString()].TimeShow)
				{
					Debug.Log("inter ads: " + p.ToString());
					AdsControl.Instance.ShowFAAds(actionDone);
                    Debug.Log("Chạy vào action");
                    lastTime = Time.time;
                    FirebaseAnalyticsControl.Instance.LogEvenAdsStatus(AdsType.force_ads, AdsStatus.success, p.ToString(), AdsRemoveStatus.no, true);
                }
				else
				{
					if (actionDone != null)
					{
						actionDone();
						Debug.Log("chạy actiondone1");
                        FirebaseAnalyticsControl.Instance.LogEvenAdsStatus(AdsType.force_ads, AdsStatus.fail, p.ToString(), AdsRemoveStatus.no, true);
                    }
					if (closeAction != null)
					{
						closeAction();
					}
				}
			}
			else
			{
				if (actionDone != null)
				{
					actionDone();
                    Debug.Log("chạy actiondone2");
                    FirebaseAnalyticsControl.Instance.LogEvenAdsStatus(AdsType.force_ads, AdsStatus.fail, p.ToString(), AdsRemoveStatus.no, true);
                }
			}
		}
		else {
            if (actionDone != null)
            {
                actionDone();
                Debug.Log("chạy actiondone3");
                FirebaseAnalyticsControl.Instance.LogEvenAdsStatus(AdsType.force_ads, AdsStatus.fail, p.ToString(), AdsRemoveStatus.no, true);
            }
        }
	}

	public void CallCloseFA()
	{
		if (closeAction != null)
		{
			Debug.Log("co closeaction");
			closeAction();
			closeAction = null;
		}
	}

	public void ShowRewardVideo(AddType addType,string id,  Action actionDone)
	{
		if (AdsControl.Instance.CheckRW())
		{
			AdsControl.Instance.ShowReward(actionDone);
            FirebaseAnalyticsControl.Instance.LogEvenAdsStatus(AdsType.rewarded, AdsStatus.success, addType.ToString(),id, true);
        }
		else
		{
			OpenBlockAds();
            FirebaseAnalyticsControl.Instance.LogEvenAdsStatus(AdsType.rewarded, AdsStatus.fail, addType.ToString(),id, false);
        }
	}

	public void OpenBlockAds()
	{
		if (!noAds.activeSelf)
		{
			noAdsPanel.SetActive(true);
			noAds.SetActive(true);
		}
		popUpNoAdsAlpha.alpha = 0;
		popUpNoAdsAlpha.DOFade(1, 0.3f).SetEase(Ease.InOutBack);
		popUpNoAdsRec.localScale = Vector3.one * 3;
		popUpNoAdsRec.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutBack);
		Invoke("CloseBlockAds", 1f);
	}
	public void CloseBlockAds()
	{
		popUpNoAdsAlpha.DOFade(0, 0.3f).SetEase(Ease.InOutBack);
		popUpNoAdsRec.DOScale(Vector3.one * 3, 0.3f).SetEase(Ease.InOutBack).OnComplete(() =>
		{
			DisableBlockAds();
		});
	}
	public void DisableBlockAds()
	{
		noAdsPanel.SetActive(false);
		noAds.SetActive(false);
	}

	public void ShowBanner()
	{
		if (structAds.ConfigBanner.IsShow == true)
		{
			AdsControl.Instance.ShowBanner();
		}
	}

	public void HideBanner()
	{
		AdsControl.Instance.HiddenBanner();
	}


	private void OnApplicationPause(bool pauseStatus)
	{
		Debug.Log("Pause");
		if (!pauseStatus)
		{
			return;
		}
	}

	public void LogEventFirebase(string str)
	{
		FirebaseAnalytics.LogEvent(str);
		Debug.Log("Push Firebase Event: " + str);
	}

	public void LogEventParameter(string nameEvent, string nameParam, long valueEvent)
	{
		FirebaseAnalytics.LogEvent(nameEvent, new Firebase.Analytics.Parameter(nameParam, valueEvent));
	}

	public void LogEventScreen(string nameScreen)
	{
		FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventScreenView,
			new Parameter(FirebaseAnalytics.ParameterScreenName, nameScreen),
			new Parameter(FirebaseAnalytics.ParameterScreenClass, unityPlayerActivity));
	}


}


[Serializable]
public class StructInfoAds
{
	[SerializeField]
	private bool isShow;
	[SerializeField]
	private float timeShow;
	[SerializeField]
	private int levelUnlock;

	public bool IsShow { get => isShow; set => isShow = value; }
	public float TimeShow { get => timeShow; set => timeShow = value; }
	public int LevelUnlock { get => levelUnlock; set => levelUnlock = value; }

	public StructInfoAds()
	{
		isShow = false;
		timeShow = 0;
		levelUnlock = 0;
	}
}

[Serializable]
public class StructAdsConfig
{
	[SerializeField]
	private StructInfoAds configBanner;

	[SerializeField]
	private Dictionary<string, StructInfoAds> structAds;



	public StructInfoAds ConfigBanner { get => configBanner; set => configBanner = value; }
	public Dictionary<string, StructInfoAds> StructAds { get => structAds; set => structAds = value; }

	public StructAdsConfig()
	{
		configBanner = new StructInfoAds();
		structAds = new Dictionary<string, StructInfoAds>();
	}
}
