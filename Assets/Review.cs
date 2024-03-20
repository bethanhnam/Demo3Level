using Google.Play.Review;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Review : MonoBehaviour
{
	public static Review Instance { get; private set; }
	// Create instance of ReviewManager
	private ReviewManager _reviewManager;
	// ...
	PlayReviewInfo _playReviewInfo;



	private void Start()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		_reviewManager = new ReviewManager();
	}
	public void OpenReview(Action action)
	{
#if UNITY_ANDROID
		Application.OpenURL("market://details?id=com.wayfu.match.puzzle.crew");
#else
        Application.OpenURL("market://details?id=1644651720");
#endif
		action();
	}

	IEnumerator review()
	{
		yield return new WaitForSeconds(1f);
		var requestFlowOperation = _reviewManager.RequestReviewFlow();
		yield return requestFlowOperation;
		if (requestFlowOperation.Error != ReviewErrorCode.NoError)
		{
			// Log error. For example, using requestFlowOperation.Error.ToString().
			yield break;
		}
		_playReviewInfo = requestFlowOperation.GetResult();
		var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
		yield return launchFlowOperation;
		_playReviewInfo = null; // Reset the object
		if (launchFlowOperation.Error != ReviewErrorCode.NoError)
		{
			// Log error. For example, using requestFlowOperation.Error.ToString().
			yield break;
		}
		// The flow has finished. The API does not indicate whether the user
		// reviewed or not, or even whether the review dialog was shown. Thus, no
		// matter the result, we continue our app flow.
	}
}
