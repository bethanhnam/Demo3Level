using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using System;
using Unity.Mathematics;

public class ToolMap : Editor
{
	public static void Clean(Transform obj)
	{
		GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj.gameObject);
		for (int i = 0; i < obj.childCount; i++)
		{
			Clean(obj.GetChild(i));
		}
	}

	[MenuItem("Component/2Clean")]
	public static void Button()
	{
		Transform obj = Selection.activeTransform;
		obj.transform.localScale = new Vector3(1.53f, 1.53f, 1f);
		Clean(obj);
		for (int i = obj.childCount - 1; i >= 0; i--)
		{
			if (obj.GetChild(i).gameObject.name.Contains("Square"))
			{
				for (int j = obj.GetChild(i).childCount - 1; j >= 0; j--)
				{
					if (obj.GetChild(i).GetChild(j).gameObject.name.Contains("stroke_board#1"))
					{
						UnityEngine.Object.DestroyImmediate(obj.GetChild(i).GetChild(j).gameObject);
					}
					if (obj.GetChild(i).GetChild(j).gameObject.name.Contains("stroke_board#2"))
					{
						UnityEngine.Object.DestroyImmediate(obj.GetChild(i).GetChild(j).gameObject);
					}
					else
					{
						Transform trs = obj.GetChild(i).GetChild(j);
						trs.SetParent(obj.transform);
					}
				}
				continue;
			}
		}
	}
	[MenuItem("Component/10CleanHoleInIron")]
	public static void Button6()
	{
		Transform obj = Selection.activeTransform;
		for (int i = obj.childCount - 1; i >= 0; i--)
		{
			if (obj.GetChild(i).gameObject.name.Contains("HoleInIronLeft"))
			{
				Transform tras = obj.GetChild(i);
				UnityEngine.Object.DestroyImmediate(tras.gameObject);
			}
		}
	}
	[MenuItem("Component/3ChangeMaterial")]
	public static void Button1()
	{
		Transform obj = Selection.activeTransform;
		for (int i = 0; i < obj.childCount; i++)
		{

			if (obj.GetChild(i).gameObject.tag == "Iron")
			{
				Material mat = Resources.Load<Material>("Materials/GrayWood");
				obj.GetChild(i).GetComponent<SpriteRenderer>().material = mat;
				continue;
			}
			if (obj.GetChild(i).gameObject.name.Contains("Square"))
			{
				Transform transform = obj.GetChild(i);
				Sprite sprite = Resources.Load<Sprite>("UI/In game/Popup wood");
				transform.GetComponent<SpriteRenderer>().sprite = sprite;
				transform.transform.localScale = new Vector3(0.65f, 0.65f, 1f);
				transform.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
				transform.transform.position = new Vector3(0, 0.79f, 0);
				continue;
			}
		}
	}
	[MenuItem("Component/1DeteleHole")]
	public static void Button5()
	{
		Transform obj = Selection.activeTransform;
		for (int i = obj.childCount - 1; i >= 0; i--)
		{
			if (obj.GetChild(i).gameObject.name.Contains("Hole"))
			{
				Transform transform = obj.GetChild(i);
				UnityEngine.Object.DestroyImmediate(transform.gameObject);
			}
		}
	}
	[MenuItem("Component/4CreateHole")]
	public static void Button2()
	{
		Transform obj = Selection.activeTransform;
		for (int i = 0; i < obj.childCount; i++)
		{
			if (obj.GetChild(i).gameObject.name.Contains("HoleInSquare"))
			{
				Transform trs = obj.GetChild(i);
				GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/HoleInSquare"), obj);
				a.transform.position = trs.position;
				a.transform.rotation = trs.rotation;
				a.transform.localScale = trs.localScale;
				a.transform.SetSiblingIndex(trs.GetSiblingIndex());
				GameObject b = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/HoleInIronLeft"), obj);
				b.transform.position = trs.position;
				b.transform.rotation = trs.rotation;
				b.transform.localScale = trs.localScale;
				b.transform.SetSiblingIndex(trs.GetSiblingIndex());
				UnityEngine.Object.DestroyImmediate(trs.gameObject);
				continue;
			}
		}
	}
	[MenuItem("Component/5HoleInSquareToSquare")]
	public static void Button3()
	{
		Transform obj = Selection.activeTransform;
		GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/NailsManager"), obj);
		Transform square = GameObject.FindGameObjectWithTag("square").transform;
		for (int j = 0; j < obj.childCount; j++)
		{
			if (obj.GetChild(j).gameObject.name.Contains("HoleInSquare"))
			{
				Transform transform = obj.GetChild(j);
				transform.SetParent(square.transform);

			}
		}
	}
	[MenuItem("Component/6CreateHoleInIronAndNail")]
	public static void Button7()
	{
		Transform obj = Selection.activeTransform;
		Transform nailsManager = GameObject.FindGameObjectWithTag("NailsManager").transform;
		for (int j = 0; j < obj.childCount; j++)
		{
			if (obj.GetChild(j).gameObject.name.Contains("HoleInIronLeft"))
			{
				Transform transform = obj.GetChild(j);
				RaycastHit2D[] HitHole = Physics2D.CircleCastAll(transform.position, 0.01f, Vector3.forward);
				if (HitHole.Length > 0)
				{
					foreach (RaycastHit2D collider in HitHole)
					{
						if (collider.transform.tag == "Iron")
						{
							quaternion oldRotat = collider.transform.rotation;
							if (collider.transform.eulerAngles.z != 0)
							{
								collider.transform.eulerAngles = new Vector3(0, 0, 90);
							}
							GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/HoleInIronLeft"), obj);
							a.transform.position = transform.position;
							a.transform.rotation = transform.rotation;
							a.transform.localScale = transform.localScale;
							//a.transform.SetSiblingIndex(transform.GetSiblingIndex());
							a.transform.SetParent(collider.transform);
							collider.transform.rotation = oldRotat;
							a.transform.position = transform.position;
							GameObject c = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/NailPrefab"), obj);
							c.transform.position = transform.position;
							c.transform.SetParent(nailsManager);
						}
					}
				}
			}
		}
	}
	[MenuItem("Component/7DeteleDupNail")]
	public static void Button10()
	{
		Transform obj = Selection.activeTransform;
		for (int j = obj.childCount - 1; j >= 1; j--)
		{
			if (obj.GetChild(j).gameObject.tag == "Nail")
			{
				if (obj.GetChild(j).gameObject.transform.position == obj.GetChild(j - 1).gameObject.transform.position)
				{
					UnityEngine.Object.DestroyImmediate(obj.GetChild(j).gameObject);
				}
			}
		}
	}
	[MenuItem("Component/9DeteleDupHoleInIron")]
	public static void Button14()
	{
		Transform obj = Selection.activeTransform;
		for (int j = 0; j < obj.childCount; j++)
		{
			if (obj.GetChild(j).gameObject.tag == "Iron")
			{
				Transform transform = obj.GetChild(j).transform;
				for (int i = obj.GetChild(j).childCount-1;i>=1; i--)
				{
					if (transform.GetChild(i).gameObject.transform.position == transform.GetChild(i - 1).gameObject.transform.position)
					{
						UnityEngine.Object.DestroyImmediate(transform.GetChild(i).gameObject);
					}
				}
			}
		}
	}
	[MenuItem("Component/8NailToHoleInSquare")]
	public static void Button11()
	{
		Transform obj = Selection.activeTransform;
		for (int j = 0; j < obj.childCount; j++)
		{
			if (obj.GetChild(j).gameObject.tag == "Hole")
			{
				Transform transform = obj.GetChild(j);
				RaycastHit2D[] HitHole = Physics2D.CircleCastAll(transform.position, 0.1f, Vector3.forward);
				if (HitHole.Length > 0)
				{
					foreach (RaycastHit2D collider in HitHole)
					{
						if (collider.transform.tag == "Nail")
						{
							transform.GetComponent<Hole>().Nail = collider.transform.gameObject;
						}
					}
				}
			}
		}
	}
	[MenuItem("Component/11CreateHingeJoints")]
	public static void Button12()
	{
		Transform obj = Selection.activeTransform;
		GameObject c = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/InputManager"), obj);
		for (int j = 0; j < obj.childCount; j++)
		{
			if (obj.GetChild(j).gameObject.name.Contains("Range"))
			{
				obj.GetChild(j).AddComponent<EndLine>();
			}
		}
		obj.AddComponent<Stage>();
		for (int j = 0; j < obj.childCount; j++)
		{
			if (obj.GetChild(j).gameObject.tag == "Iron")
			{
				obj.GetChild(j).GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
				obj.GetChild(j).GetComponent<Rigidbody2D>().angularDrag = 1f;
				obj.GetChild(j).AddComponent<IronPlate>();
				for (int i = 0; i < obj.GetChild(j).childCount - 1; i++)
				{
					obj.GetChild(j).AddComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
				}
			}
		}
	}
	[MenuItem("Component/12SetHingeJoints")]
	public static void Button13()
	{
		Transform obj = Selection.activeTransform;
		for (int j = 0; j < obj.childCount; j++)
		{
			if (obj.GetChild(j).gameObject.tag == "Iron")
			{
				HingeJoint2D[] allComponents = obj.GetChild(j).GetComponents<HingeJoint2D>();
				for (int i = 0; i < obj.GetChild(j).childCount; i++)
				{
					Transform transform = obj.GetChild(j).GetChild(i);
					RaycastHit2D[] HitHole = Physics2D.CircleCastAll(transform.position, 0.1f, Vector3.forward);
					if (HitHole.Length > 0)
					{
						foreach (RaycastHit2D collider in HitHole)
						{
							if (collider.transform.tag == "Nail")
							{
								if (allComponents[i].connectedBody == null)
								{
									allComponents[i].connectedBody = collider.transform.gameObject.GetComponent<Rigidbody2D>();
									allComponents[i].connectedAnchor = Vector2.zero;
								}
							}
						}
					}
				}
			}
		}
	}
	[MenuItem("Component/13caculate")]
	public static void Button15()
	{
		Transform obj = Selection.activeTransform;
		for (int j = 0; j < obj.childCount; j++)
		{
			if (obj.GetChild(j).gameObject.tag == "Iron")
			{
				HingeJoint2D[] allComponents = obj.GetChild(j).GetComponents<HingeJoint2D>();
				for (int i = 0; i < allComponents.Length; i++)
				{
					Transform nailTransform = allComponents[i].connectedBody.transform;
					allComponents[i].anchor = obj.GetChild(j).InverseTransformPoint(nailTransform.position);
				}
			}
		}
		for (int j = 0; j < obj.childCount; j++)
		{
			if (obj.GetChild(j).gameObject.tag == "NailsManager")
			{
				for (int i = 0; i < obj.GetChild(j).childCount; i++)
				{
					Transform transform = obj.GetChild(j).GetChild(i);
					transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/In game/screw");
				}
			}
		}

	}
}
//[MenuItem("Component/CreateHoleInIron")]
//public static void Button4()
//{
//	Transform obj = Selection.activeTransform;
//	for (int j = 0; j < obj.childCount; j++)
//	{
//		if (obj.GetChild(j).gameObject.name.Contains("HoleInSquare"))
//		{
//			Transform trs = obj.GetChild(i);
//			GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/HoleInSquare"), obj);
//			a.transform.position = trs.position;
//			a.transform.rotation = trs.rotation;
//			a.transform.localScale = trs.localScale;
//			a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//			Object.DestroyImmediate(trs.gameObject);
//			continue;
//		}
//	}
//}





//if (obj.GetChild(i).gameObject.name.Contains("NNew_Invaider"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/NNew_Invaider"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_Saw-Idle"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_Saw-Idle"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_tonnel_l"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_tonnel_l"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//continue;
//} 

//if (obj.GetChild(i).gameObject.name.Contains("Obctacle_Airscrew"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obctacle_Airscrew"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_balls_tube"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_balls_tube"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_buffer cone"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_buffer cone"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_tyres"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_tyres"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_Wooden_Barrels"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_Wooden_Barrels"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Fence"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Fence"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_spike"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_Spike"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_water_barrel"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_water_barrel"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_Wooden_Barriers_Line"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_Wooden_Barriers_Line"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obctacle_SpikeCylinder"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obctacle_SpikeCylinder"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());

//    a.transform.GetChild(1).SetLocalPositionAndRotation(trs.GetChild(1).localPosition, trs.GetChild(1).localRotation);

//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obctacle_Airscrew"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obctacle_Airscrew_1"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_SpikeBall_2"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_SpikeBall_2"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obctacle_STN_SpaceMill"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obctacle_STN_SpaceMill"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_STN_Asteroid"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_STN_Asteroid"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_STN_WreckingBox"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_STN_WreckingBox"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    a.transform.GetChild(1).SetLocalPositionAndRotation(trs.GetChild(1).localPosition, trs.GetChild(1).localRotation);
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obctacle_STN_SpaceShip_1"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obctacle_STN_SpaceShip_1"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_collumn"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_collumn"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_ColumnSpike_2"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_ColumnSpike_2"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_STN_CargoPress"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_STN_CargoPress"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obctacle_Sector"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obctacle_Sector"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    a.transform.GetChild(0).SetLocalPositionAndRotation(trs.GetChild(0).localPosition, trs.GetChild(0).localRotation);
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_STN_UFO "))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_STN_UFO_Idle"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_STN_UFO_Pendulum"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_STN_UFO_Pendulum"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    a.transform.GetChild(0).SetLocalPositionAndRotation(trs.GetChild(0).localPosition, trs.GetChild(0).localRotation);
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Wooden_Column"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Wooden_Column"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_Barrels"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_Barrels"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_Saw "))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_Saw"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_Saw_Move"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_Saw_Move"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("DTN_Obstacle_Cargo"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/DTN_Obstacle_Cargo"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obstacle_BarrerConcrete_2"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_BarrerConcrete_2"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("DTN_Obstacle_CargoGate"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/DTN_Obstacle_CargoGate"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("DTN_Obstacle_WreckingBal"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/DTN_Obstacle_WreckingBal"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("DTN_Obstacle_BarrelsAcross"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/DTN_Obstacle_BarrelsAcross"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("DTN_Obstacle_BarrelsAlong"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/DTN_Obstacle_BarrelsAlong"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("DTN_Obstacle_AirScrew"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/DTN_Obstacle_AirScrew"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("WreckingBall_obstacle"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/WreckingBall_obstacle"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    a.transform.GetChild(1).SetLocalPositionAndRotation(trs.GetChild(1).localPosition, trs.GetChild(1).localRotation);
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("Obctacle_Airscrew_double"))
//{
//    Transform trs = obj.GetChild(i);

//    GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obctacle_Airscrew_double"), obj);
//    a.transform.position = trs.position;
//    a.transform.rotation = trs.rotation;
//    a.transform.localScale = trs.localScale;
//    a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//    a.transform.GetChild(0).SetLocalPositionAndRotation(trs.GetChild(0).localPosition, trs.GetChild(0).localRotation);
//    Object.DestroyImmediate(trs.gameObject);
//    continue;
//}

//if (obj.GetChild(i).gameObject.name.Contains("pimple"))
//         {
//             Transform trs = obj.GetChild(i);

//             GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/pimple"), obj);
//             a.transform.position = trs.position;
//             a.transform.rotation = trs.rotation;
//             a.transform.localScale = trs.localScale;
//             a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//             Object.DestroyImmediate(trs.gameObject);
//             continue;
//         }

