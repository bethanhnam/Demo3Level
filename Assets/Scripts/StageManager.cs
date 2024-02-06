using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	public static StageManager instance;
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
	public void LoadStage(int currentStage)
	{
		// Load prefab of the new level
		GameObject levelPrefab = levels[currentStage];
		if (levelPrefab != null)
		{
			// Instantiate prefab as a new instance in the scene
			GameObject stageInstance = Instantiate(levelPrefab, this.transform.position, Quaternion.identity);

			// Set the level instance as a child of a parent transform if needed
			stageInstance.transform.SetParent(transform);

			// Store the reference to the new level instance
			levelInstances.Add(stageInstance);
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
