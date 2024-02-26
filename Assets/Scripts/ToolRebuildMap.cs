using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class ToolRebuildMap : Editor
{
	[MenuItem("Services/2Clean")]
	public static void Button()
	{
		Transform obj = Selection.activeTransform;
		for (int i = obj.childCount - 1; i >= 0; i--)
		{
			if (obj.GetChild(i).gameObject.name.Contains("Square"))
			{
				Transform transform = obj.GetChild(i).transform;
				transform.gameObject.tag = "square";
				transform.GetComponent <SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Ingame 1/Popup");
			}
			if(obj.GetChild(i).gameObject.tag == "Iron")
			{
				Transform transform1 = obj.GetChild(i).transform;
				transform1.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/New Shader Graph");
				for (int k = transform1.childCount - 1; k >= 0; k--)
				{
					transform1.GetChild(k).gameObject.SetActive(true);
					SpriteRenderer spriteRenderer = transform1.GetChild(k).GetComponent<SpriteRenderer>();
					Object.DestroyImmediate(spriteRenderer);
					transform1.GetChild(k).GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Ui11/hole 2");
				}
				continue;
			}
			if(obj.GetChild(i).gameObject.tag == "NailsManager")
			{
				Transform transform2 = obj.GetChild(i).transform;
				for(int j =0;j < obj.GetChild(i).childCount; j++)
				{
					Transform transform3 = obj.GetChild(i).GetChild(j).transform;
					transform3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Ingame 1/Screw 1");
				}
				continue;
			}
			if (obj.GetChild(i).gameObject.tag == "square")
			{
				Transform transform = obj.GetChild(i).transform;
				for(int k = transform.childCount - 1; k >= 0; k--)
				{
					transform.GetChild(k).GetComponent<SpriteRenderer>().enabled = true;
					transform.GetChild(k).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Ingame 1/Hole");
				}
				continue;
			}
		}
	}
}
