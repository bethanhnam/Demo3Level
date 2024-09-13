using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
	void Start()
	{
		float targetAspect = 9.0f / 16.0f;
		float windowAspect = (float)Screen.width / (float)Screen.height;
		if (windowAspect < targetAspect)
		{
			float scaleX = this.transform.localScale.x;
			float scaleY = this.transform.localScale.y;
			this.transform.localScale = new Vector3(scaleX, scaleY * targetAspect / windowAspect, 1f);
		}
		
	}

}
