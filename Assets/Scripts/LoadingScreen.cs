using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.HDROutputUtils;

public class LoadingScreen : MonoBehaviour
{
	public static LoadingScreen instance;
	public GameObject loadingScreen;
	public Screen gamePlayScreen;
	public Slider[] sliders;

	[SerializeField]
	public CanvasGroup cv;

	//public void LoadingScene(int sceneId)
	//{
	//	StartCoroutine(LoadingSceneAsync());
	//	//StartCoroutine(changeSliderValue(sceneId));
	//}

	//IEnumerator LoadingSceneAsync()
	//{
	//	loadingScreen.SetActive(true);
	//	if (sliders[1].value <= 0.9f)
	//	{
	//		var x = 375.69f / 548.76f;
	//		sliders[0].DOValue(0.9f, 2f);
	//		sliders[1].DOValue(0.3f, 1.9f).OnComplete(() => {
	//			sliders[1].DOValue(0,1f).OnComplete(() => {
	//				AsyncOperation operation = SceneManager.LoadSceneAsync(1);
	//				operation.allowSceneActivation = true;
	//				RemoteConfigController.instance.Init();
	//			});
	//		});
	//	}
	//	yield return null;

	//}
	//private void Start()
	//{
	//	LoadingScene(1);
	//	AudioManager.instance.PlayMusic("Loading");
	//}
	
	public void LoadingScene(int sceneId)
	{
		StartCoroutine(LoadingSceneAsync(sceneId));
		//StartCoroutine(changeSliderValue(sceneId));
	}

	IEnumerator LoadingSceneAsync(int sceneId)
	{
		yield return null;
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
		operation.allowSceneActivation = false;
		loadingScreen.SetActive(true);
		while (sliders[0].value <= 0.9f && !operation.isDone)
		{
			if (sliders[0].value <= 0.9f)
			{
				sliders[0].value += 0.015f;
			}
			if (operation.progress >= 0.9f && sliders[0].value ==0.9f)
			{
				yield return new WaitForSecondsRealtime(0.3f);
				operation.allowSceneActivation = true;
				RemoteConfigController.instance.Init();
				DOVirtual.DelayedCall(0.1f, () =>
				{
					GameManagerNew.Instance.InitStartGame();
				});
				cv.DOFade(0, 0.3f).OnComplete(() =>
				{
					if (SaveSystem.instance.powerTicket > 0 || SaveSystem.instance.magicTiket > 0)
					{
						if (PlayerPrefs.GetInt("HasTransfer") == 0)
						{
							UIManagerNew.Instance.TransferPanel.Appear();
						}
					}
                    if (UIManagerNew.Instance.ChestSLider.currentValue != UIManagerNew.Instance.ChestSLider.maxValue1)
                    {
						UIManagerNew.Instance.ButtonMennuManager.OpenDailyRW();
						AudioManager.instance.PlayMusic("MenuTheme");
                    }
					if (RemoteConfigController.instance.IsShowOpenAds == 1)
					{
						AdsControl.Instance.ShowOpenAds();
					}
					Destroy(this.gameObject);
				});
			}
			yield return null;
		}
		yield return new WaitForSecondsRealtime(1f);
	}
	private void Start()
	{
        instance = this;

        DontDestroyOnLoad(this.gameObject);
		LoadingScene(1);
		//AudioManager.instance.PlayMusic("Loading");
		Application.targetFrameRate = 60;
	}
}
