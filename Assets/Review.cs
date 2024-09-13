
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Review : MonoBehaviour
{
	public static Review Instance { get; private set; }
	// Create instance of ReviewManager
	bool isRun = false;



	private void Start()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}
	public void OpenReview(Action action)
	{
		if (isRun == true)
		{
			return;
		}
		else
		{
#if UNITY_ANDROID
			Application.OpenURL("market://details?id=com.wayfu.match.puzzle.crew");
#else
        Application.OpenURL("https://apps.apple.com/us/app/screw-story-pin-master/id6480027761");
#endif
            action();
		}
	}
}
