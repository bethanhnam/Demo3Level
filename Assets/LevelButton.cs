using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int level;
    public bool hasDone;
	public GameObject fixedImg;
	public GameObject unfixedImg;
	private void Update()
	{
		if (hasDone)
		{
			this.gameObject.SetActive(false);
		}
	}
}
