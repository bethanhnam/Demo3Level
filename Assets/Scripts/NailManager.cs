using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class NailManager : MonoBehaviour
{
    public static NailManager instance;
    public List<NailControl> nails = new List<NailControl>();
    public NailControl nailPrefab;
    // Start is called before the first frame update
    void Start()
    {
        if(instance  == null)
        {
            instance = this;
        }
		//nails = GetAllChildObjects(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	//private List<NailControl> GetAllChildObjects(Transform parent)
	//{
	//	List<NailControl> childObjects = new List<NailControl>(parent.childCount);

	//	for (int i = 0; i < parent.childCount; i++)
	//	{
	//		childObjects.Add(parent.GetChild(i));
	//	}

	//	return childObjects;
	//}
	public NailControl PoolNail(Vector2 spawnPosition)
	{
		if (nails.Count <= 0)
		{
			
			NailControl a = Instantiate(nailPrefab, spawnPosition, quaternion.identity,transform);
			nails.Add(a);
			
			return a;
		}
		else
		{
			NailControl a = nails.Where(t => !t.gameObject.activeSelf).FirstOrDefault();
			if (a == null)
			{
				a = Instantiate(nailPrefab, spawnPosition, quaternion.identity,transform);
				nails.Add(a);
				a.GetComponent<SpriteRenderer>().sprite = null;
				return a;
			}
			else
			{
				a.transform.position = spawnPosition;
				a.gameObject.SetActive(true);
				nails.Remove(a);
				a.GetComponent<SpriteRenderer>().sprite = null;
				return a;
			}
		}
		
	}
	public void DestroyNail(NailControl nail) {
		nail.gameObject.SetActive(false);
        nails.Add(nail);
    }
    
}
