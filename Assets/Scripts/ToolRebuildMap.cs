//using System.Collections.Generic;
//using UnityEditor;
//using UnityEditor.SceneManagement;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class ToolRebuildMap : Editor
//{
//	//[MenuItem("Services/1Clean")]
//	//public static void Button1()
//	//{
//	//	Transform obj = Selection.activeTransform;
//	//	for (int i = obj.childCount - 1; i >= 0; i--)
//	//	{
//	//		if (obj.GetChild(i).gameObject.name.Contains("Square"))
//	//		{
//	//			Transform transform = obj.GetChild(i).transform;
//	//			transform.gameObject.tag = "square";
//	//			transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Ingame 1/Popup");
//	//		}
//	//		if (obj.GetChild(i).gameObject.tag == "Iron")
//	//		{
//	//			Transform transform1 = obj.GetChild(i).transform;
//	//			transform1.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/New Shader Graph");
//	//			for (int k = transform1.childCount - 1; k >= 0; k--)
//	//			{
//	//				transform1.GetChild(k).gameObject.SetActive(true);
//	//				SpriteRenderer spriteRenderer = transform1.GetChild(k).GetComponent<SpriteRenderer>();
//	//				UnityEngine.Object.DestroyImmediate(spriteRenderer);
//	//				transform1.GetChild(k).GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Ui11/hole 2");
//	//			}
//	//			continue;
//	//		}
//	//		if (obj.GetChild(i).gameObject.tag == "NailsManager")
//	//		{
//	//			Transform transform2 = obj.GetChild(i).transform;
//	//			for (int j = 0; j < obj.GetChild(i).childCount; j++)
//	//			{
//	//				Transform transform3 = obj.GetChild(i).GetChild(j).transform;
//	//				transform3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Ingame 1/Screw 1");
//	//			}
//	//			continue;
//	//		}
//	//		if (obj.GetChild(i).gameObject.tag == "square")
//	//		{
//	//			Transform transform = obj.GetChild(i).transform;
//	//			for (int k = transform.childCount - 1; k >= 0; k--)
//	//			{
//	//				transform.GetChild(k).GetComponent<SpriteRenderer>().enabled = true;
//	//				transform.GetChild(k).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Ingame 1/Hole");
//	//			}
//	//			continue;
//	//		}
//	//	}
//	//}
//	//[MenuItem("Services/2caculateCollider")]
//	//public static void Button6()
//	//{
//	//	Transform obj = Selection.activeTransform;
//	//	for (int j = 0; j < obj.childCount; j++)
//	//	{
//	//		Transform child = obj.GetChild(j);
//	//		if (child.gameObject.CompareTag("Iron") && !child.gameObject.name.Contains("washer"))
//	//		{
//	//			try
//	//			{
//	//				Vector2[] points = child.GetComponent<PolygonCollider2D>().GetPath(0);
//	//				for (int i = 0; i < points.Length; i++)
//	//				{
//	//					points[i].x = child.localScale.x * points[i].x;
//	//					points[i].y = child.localScale.y * points[i].y;
//	//				}
//	//				child.GetComponent<PolygonCollider2D>().SetPath(0, points);
//	//			}
//	//			catch { }
//	//			continue;
//	//		}
//	//	}
//	//}
//	//[MenuItem("Services/3createmap")]
//	//public static void Button7()
//	//{
//	//	Transform obj = Selection.activeTransform;
//	//	for (int i = obj.childCount - 1; i >= 0; i--)
//	//	{
//	//		if (obj.GetChild(i).gameObject.tag == "Iron" && !obj.GetChild(i).gameObject.name.Contains("washer") && !obj.GetChild(i).gameObject.name.Contains("Solid"))
//	//		{
//	//			List<Transform> transforms = new List<Transform>();
//	//			for (int j = obj.GetChild(i).childCount - 1; j >= 0; j--)
//	//			{
//	//				transforms.Add(obj.GetChild(i).GetChild(j));
//	//				obj.GetChild(i).GetChild(j).transform.SetParent(obj);
//	//			}
//	//			obj.GetChild(i).GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
//	//			obj.GetChild(i).GetComponent<SpriteRenderer>().size = new Vector2(obj.GetChild(i).localScale.x * obj.GetChild(i).GetComponent<SpriteRenderer>().size.x, obj.GetChild(i).localScale.y * obj.GetChild(i).GetComponent<SpriteRenderer>().size.y);
//	//			obj.GetChild(i).localScale = Vector3.one;
//	//			foreach (Transform child in transforms)
//	//			{
//	//				child.SetParent(obj.GetChild(i));
//	//				child.SetParent(obj.GetChild(i));
//	//				child.localScale = new Vector3(0.3f, 0.3f, 1);
//	//				Transform transform = child.transform;
//	//				transform.GetComponentInChildren<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
//	//				transform.GetComponentInChildren<SpriteRenderer>().size = new Vector2(1f, 1f);
//	//				transform.GetChild(0).localScale = Vector2.one;

//	//			}
//	//			transforms.Clear();
//	//		}
//	//	}
//	//}
//	//[MenuItem("Services/4ResizeShadow")]
//	//public static void Button8()
//	//{
//	//	Transform obj = Selection.activeTransform;
//	//	for (int i = obj.childCount - 1; i >= 0; i--)
//	//	{
//	//		if (obj.GetChild(i).gameObject.tag == "Iron")
//	//		{
//	//			for (int j = 0; j < obj.GetChild(i).childCount; j++)
//	//			{
//	//				Transform transform = obj.GetChild(i).GetChild(j);
//	//				transform.GetChild(0).GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
//	//				transform.GetChild(0).GetComponent<SpriteRenderer>().size = new Vector2(1f, 1f);
//	//				transform.GetChild(0).localScale = new Vector2(1f, 1f);
//	//				transform.localScale = new Vector2(0.3f, 0.3f);
//	//			}
//	//		}
//	//	}
//	//}
//	//[MenuItem("Services/5CleanHinge")]
//	//public static void Button9()
//	//{
//	//	Transform obj = Selection.activeTransform;
//	//	for (int i = obj.childCount - 1; i >= 0; i--)
//	//	{
//	//		if (obj.GetChild(i).gameObject.tag == "Iron" && !obj.GetChild(i).gameObject.name.Contains("washer") && !obj.GetChild(i).gameObject.name.Contains("Solid"))
//	//		{
//	//			HingeJoint2D[] allComponents = obj.GetChild(i).GetComponents<HingeJoint2D>();
//	//			for (int j = 0; j < allComponents.Length; j++)
//	//			{
//	//				allComponents[j].connectedBody = null;
//	//				allComponents[j].autoConfigureConnectedAnchor = false;
//	//				allComponents[j].connectedAnchor = Vector2.zero;
//	//				allComponents[j].anchor = Vector2.zero;
//	//			}
//	//		}
//	//	}
//	//}
//	[MenuItem("Services/6CleanHinge")]
//	public static void Button10()
//	{
//		Transform obj = Selection.activeTransform;
//		for (int i = obj.childCount - 1; i >= 0; i--)
//		{
//			if (obj.GetChild(i).gameObject.tag == "Iron")
//			{
//				Transform transform = obj.GetChild(i).transform;
//				transform.GetComponent<Rigidbody2D>().sharedMaterial = Resources.Load<PhysicsMaterial2D>("PhysicMaterials/New Physics Material 2D");
//				var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
//				if (prefabStage != null)
//				{
//					EditorSceneManager.MarkSceneDirty(prefabStage.scene);
//					PrefabUtility.RecordPrefabInstancePropertyModifications(obj);
//				}

//			}
//		}
//	}
//}
