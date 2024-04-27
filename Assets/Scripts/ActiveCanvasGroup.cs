using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCanvasGroup : MonoBehaviour
{
	[SerializeField]
	private CanvasGroup cv;

	public void Active() {
		if (!cv.enabled)
		{
			cv.enabled = true;
		}
	}
}
