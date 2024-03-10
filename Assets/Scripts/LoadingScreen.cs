using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	public GameObject loadingScreen;
	public Slider[] sliders;
	public float progressDelta = 0.01f; // Điều chỉnh delta tại đây

	public void LoadingScene(int sceneId)
	{
		StartCoroutine(LoadingSceneAsync(sceneId));
	}

	IEnumerator LoadingSceneAsync(int sceneId)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
		loadingScreen.SetActive(true);
		while (!operation.isDone)
		{
			float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

			// Điều chỉnh tốc độ tăng của giá trị
			float targetValue = sliders[0].value + progressDelta;
			sliders[0].value = Mathf.Clamp01(targetValue);

			targetValue = sliders[1].value + progressDelta;
			sliders[1].value = Mathf.Clamp01(targetValue);

			yield return null;
		}
	}

	private void Start()
	{
		LoadingScene(1);
	}
}
