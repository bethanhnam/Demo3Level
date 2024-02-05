using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class NailManager : MonoBehaviour
{
    public static NailManager instance;
    public List<GameObject> nails = new List<GameObject>();
    public GameObject nailPrefab;
    // Start is called before the first frame update
    void Start()
    {
        if(instance  == null)
        {
            instance = this;
        }
		nails = GetAllChildObjects(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	private List<GameObject> GetAllChildObjects(Transform parent)
	{
		List<GameObject> childObjects = new List<GameObject>(parent.childCount);

		for (int i = 0; i < parent.childCount; i++)
		{
			childObjects.Add(parent.GetChild(i).gameObject);
		}

		return childObjects;
	}
	public GameObject PoolNail(Vector2 spawnPosition)
	{
		if (nails.Count <= 0)
		{
			
			GameObject a = Instantiate(nailPrefab, new Vector2(InputManager.instance.selectedHole.transform.position.x, InputManager.instance.selectedHole.transform.position.y), quaternion.identity,transform);
			nails.Add(a);
			
			return a;
		}
		else
		{
			GameObject a = nails.Where(t => !t.activeSelf).FirstOrDefault();
			if (a == null)
			{
				a = Instantiate(nailPrefab, new Vector2(InputManager.instance.selectedHole.transform.position.x, InputManager.instance.selectedHole.transform.position.y), quaternion.identity,transform);
				nails.Add(a);
				
				return a;
			}
			else
			{
				a.transform.position = spawnPosition;
				a.SetActive(true);
				nails.Remove(a);
				
				return a;
			}
		}
	}
	public void DestroyNail(GameObject nail) {
        nail.SetActive(false);
        nails.Add(nail);
    }
    
}
