using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchWidth : MonoBehaviour
{

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private float defaultOrhto;

	private void Start()
	{
		float targetAspect = 9.0f / 16.0f;
		float windowAspect = (float)Screen.width / (float)Screen.height;

		if (windowAspect < targetAspect)
		{
			cam.orthographicSize = defaultOrhto * (targetAspect / windowAspect);
		}
	}
}
