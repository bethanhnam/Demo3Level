using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInit : MonoBehaviour
{
	public static DataInit instance;
	[SerializeField]
	public DataLevelManager dataLevelManager;
	[SerializeField]
	public LevelManagerNew levelManagerNew;

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		
		levelManagerNew.Init();
		dataLevelManager.Init();

		if(instance == null)
		{
			instance = this;
		}
	}
}
