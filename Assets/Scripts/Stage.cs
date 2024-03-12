using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
	public Sprite hintImg;
    public GameObject holeToUnlock;
	public GameObject pointer1;
	public GameObject pointer2;
	public GameObject pointer;
	public GameObject[] pointerPositions;
	private bool hasSpawn = false;
	private bool hasSpawn1 = false;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (GameManager.instance.currentLevel == 0)
		{

			if (InputManager.instance.selectedNail == null)
			{
				if (!hasSpawn)
				{
					pointer1 = Instantiate(pointer, pointerPositions[0].transform.position, Quaternion.identity, this.transform);
					hasSpawn = true;
				}
			}
			else
			{
				if (!hasSpawn1)
				{
					Destroy(pointer1);
					Instantiate(pointer, pointerPositions[1].transform.position, Quaternion.identity, this.transform);
					hasSpawn1 = true;
				}
			}
		}
	}
}
