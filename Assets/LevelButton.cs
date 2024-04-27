using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int level;
	private void Update()
	{
	}
	public void CreateLevel()
	{
		GameManagerNew.Instance.CreateLevel(level);
		UIManagerNew.Instance.ButtonMennuManager.Close();
	}
}
