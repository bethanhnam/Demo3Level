using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public static LevelManager instance;
	public List<GameObject> levelInstances = new List<GameObject>(); // List to store the instances of loaded levels
	public List<GameObject> levels = new List<GameObject>(); // List to store the instances of loaded levels
	public int levelCount = 0;

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		levelCount = levels.Count;
	}
	public void LoadLevel(int currentLevel)
	{
		// Load prefab of the new level
		GameObject levelPrefab = levels[currentLevel];
		if (levelPrefab != null)
		{
			// Instantiate prefab as a new instance in the scene
			GameObject levelInstance = Instantiate(levelPrefab,levelPrefab.transform.position,Quaternion.identity);

			// Set the level instance as a child of a parent transform if needed
			levelInstance.transform.SetParent(transform);

			// Store the reference to the new level instance
			levelInstances.Add(levelInstance);
			GameManager.instance.currentLevel = currentLevel;
		}
		else
		{
			Debug.LogError("Level prefab not found!");
		}
		//InputManager.instance.setHinge();
	}

	public void RemoveLevel(int prelevel)
	{
		// Check if there is a level at the specified index
		if (prelevel >= 0)
		{
			GameObject levelToRemove = levelInstances[0];
			levelInstances.Remove(levelToRemove);
			Destroy(levelToRemove);
		}
	}
	public int getLevel()
	{
		return levelCount;
	}

}
