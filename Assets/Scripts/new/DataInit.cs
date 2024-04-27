using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInit : MonoBehaviour
{
	[SerializeField]
	private DataLevelManager dataLevelManager;
	[SerializeField]
	private LevelManagerNew levelManagerNew;

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);

		levelManagerNew.Init();
		dataLevelManager.Init();
	}
}
