using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Analytics;
using DG.Tweening;

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
		InitDataShow(strucAds);
		LogEventScreen("Splash");
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
		ShowBanner();
	}

	private Action closeAction;

	public void ShowInterstial(AdsManager.PositionAds p, Action actionDone, Action _actionClose)
	{
		if (isRemoveAds)
		{
			if (actionDone != null)
			{
				actionDone();
			}
			if (_actionClose != null)
			{
				_actionClose();
			}

			return;
		}

		closeAction = _actionClose;
		if(closeAction != null)
		{
			Debug.Log("co close1");
		}
		Debug.Log("inter ads: " + p.ToString());

		if (actionDone != null)
		{
			actionDone();
		}


		if (StructAds.StructAds.ContainsKey(p.ToString()))
		{
			Debug.Log("inter ads: " + p.ToString());
			if (StructAds.StructAds[p.ToString()].IsShow && StructAds.ConfigBanner.LevelUnlock <= SaveSystem.instance.menuLevel + 1)
			{
				Debug.Log("inter ads: " + p.ToString());
				if (Time.time - lastTime > StructAds.StructAds[p.ToString()].TimeShow)
				{
					Debug.Log("inter ads: " + p.ToString());
					AdsControl.Instance.ShowFAAds();
					lastTime = Time.time;
				}
				else 
				{
					if (closeAction != null)
					{
						closeAction();
					}
				}
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

	public void ShowRewardVideo(Action actionDone)
	{
		if (AdsControl.Instance.CheckRW())
		{
			AdsControl.Instance.ShowReward(actionDone);
		}
		else
		{
			OpenBlockAds();
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
			if (RemoteConfigController.instance != null)
			{
				if (RemoteConfigController.instance.IsShowOpenAds == 1)
				{
					AdsControl.Instance.ShowOpenAds();
				}
			}
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
