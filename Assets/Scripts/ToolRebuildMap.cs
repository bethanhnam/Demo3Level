//using Sirenix.Utilities;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEditor;
//using UnityEditor.SceneManagement;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class ToolRebuildMap : Editor
//{
//	[MenuItem("Services/1Clean")]
//	public static void Button1()
//	{
//		Transform obj = Selection.activeTransform;
//		for (int i = obj.childCount - 1; i >= 0; i--)
//		{
//			if (obj.GetChild(i).gameObject.name.Contains("Square"))
//			{
//				Transform transform = obj.GetChild(i).transform;
//				transform.gameObject.tag = "square";
//				transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/upscale ingame/Untitled-1_0013_Layer-4");
//			}
//			if (obj.GetChild(i).gameObject.tag == "Iron")
//			{
//				Transform transform1 = obj.GetChild(i).transform;
//				transform1.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/New Shader Graph");
//				for (int k = transform1.childCount - 1; k >= 0; k--)
//				{
//					transform1.GetChild(k).gameObject.SetActive(true);
//					SpriteRenderer spriteRenderer = transform1.GetChild(k).GetComponent<SpriteRenderer>();
//					UnityEngine.Object.DestroyImmediate(spriteRenderer);
//					transform1.GetChild(k).GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Ui11/hole 2");
//				}
//				continue;
//			}
//			if (obj.GetChild(i).gameObject.tag == "NailsManager")
//			{
//				Transform transform2 = obj.GetChild(i).transform;
//				for (int j = 0; j < obj.GetChild(i).childCount; j++)
//				{
//					Transform transform3 = obj.GetChild(i).GetChild(j).transform;
//					transform3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/upscale ingame/Untitled-1_0000_Group-1-copy");
//				}
//				continue;
//			}
//			if (obj.GetChild(i).gameObject.tag == "square")
//			{
//				Transform transform = obj.GetChild(i).transform;
//				for (int k = transform.childCount - 1; k >= 0; k--)
//				{
//					transform.GetChild(k).GetComponent<SpriteRenderer>().enabled = true;
//					transform.GetChild(k).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/upscale ingame/Untitled-1_0002_Layer-1");
//				}
//				continue;
//			}
//		}
//	}
//	[MenuItem("Services/2caculateCollider")]
//	public static void Button6()
//	{
//		Transform obj = Selection.activeTransform;
//		for (int j = 0; j < obj.childCount; j++)
//		{
//			Transform child = obj.GetChild(j);
//			if (child.gameObject.CompareTag("Iron") && !child.gameObject.name.Contains("washer") && !child.gameObject.name.Contains("Solid"))
//			{
//				try
//				{
//					Vector2[] points = child.GetComponent<PolygonCollider2D>().GetPath(0);
//					for (int i = 0; i < points.Length; i++)
//					{
//						points[i].x = child.localScale.x * points[i].x;
//						points[i].y = child.localScale.y * points[i].y;
//					}
//					child.GetComponent<PolygonCollider2D>().SetPath(0, points);
//				}
//				catch { }
//				continue;
//			}
//		}
//	}
//	[MenuItem("Services/3createmap")]
//	public static void Button7()
//	{
//		Transform obj = Selection.activeTransform;
//		for (int i = obj.childCount - 1; i >= 0; i--)
//		{
//			if (obj.GetChild(i).gameObject.tag == "Iron" && !obj.GetChild(i).gameObject.name.Contains("washer") && !obj.GetChild(i).gameObject.name.Contains("Solid"))
//			{
//				List<Transform> transforms = new List<Transform>();
//				for (int j = obj.GetChild(i).childCount - 1; j >= 0; j--)
//				{
//					transforms.Add(obj.GetChild(i).GetChild(j));
//					obj.GetChild(i).GetChild(j).transform.SetParent(obj);
//				}
//				obj.GetChild(i).GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
//				obj.GetChild(i).GetComponent<SpriteRenderer>().size = new Vector2(obj.GetChild(i).localScale.x * obj.GetChild(i).GetComponent<SpriteRenderer>().size.x, obj.GetChild(i).localScale.y * obj.GetChild(i).GetComponent<SpriteRenderer>().size.y);
//				obj.GetChild(i).localScale = Vector3.one;
//				foreach (Transform child in transforms)
//				{
//					child.SetParent(obj.GetChild(i));
//					child.SetParent(obj.GetChild(i));
//					child.localScale = new Vector3(0.35f, 0.35f, 1);
//					Transform transform = child.transform;
//					transform.GetComponentInChildren<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
//					transform.GetComponentInChildren<SpriteRenderer>().size = new Vector2(1f, 1f);
//					transform.GetChild(0).localScale = Vector2.one;

//				}
//				transforms.Clear();
//			}
//		}
//	}
//	[MenuItem("Services/4ResizeShadow")]
//	public static void Button8()
//	{
//		Transform obj = Selection.activeTransform;
//		for (int i = obj.childCount - 1; i >= 0; i--)
//		{
//			if (obj.GetChild(i).gameObject.tag == "Iron")
//			{
//				for (int j = 0; j < obj.GetChild(i).childCount; j++)
//				{
//					Transform transform = obj.GetChild(i).GetChild(j);
//					transform.GetChild(0).GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
//					transform.GetChild(0).GetComponent<SpriteRenderer>().size = new Vector2(1f, 1f);
//					transform.GetChild(0).localScale = new Vector2(1f, 1f);
//					transform.localScale = new Vector2(0.35f, 0.35f);
//				}
//			}
//		}
//	}
//	[MenuItem("Services/5CleanHinge")]
//	public static void Button9()
//	{
//		Transform obj = Selection.activeTransform;
//		for (int i = obj.childCount - 1; i >= 0; i--)
//		{
//			if (obj.GetChild(i).gameObject.tag == "Iron" && !obj.GetChild(i).gameObject.name.Contains("washer") && !obj.GetChild(i).gameObject.name.Contains("Solid"))
//			{
//				HingeJoint2D[] allComponents = obj.GetChild(i).GetComponents<HingeJoint2D>();
//				for (int j = 0; j < allComponents.Length; j++)
//				{
//					allComponents[j].connectedBody = null;
//					allComponents[j].autoConfigureConnectedAnchor = false;
//					allComponents[j].connectedAnchor = Vector2.zero;
//					allComponents[j].anchor = Vector2.zero;
//				}
//			}
//		}
//	}
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
//	[MenuItem("Services/18eliminateExtraHinge")]
//	public static void Button21()
//	{
//		Transform obj = Selection.activeTransform;
//		Transform square = GameObject.FindGameObjectWithTag("square").transform;
//		for (int i = obj.childCount - 1; i >= 0; i--)
//		{
//			if (obj.GetChild(i).gameObject.tag == "Iron")
//			{
//				HingeJoint2D[] allComponents = obj.GetChild(i).GetComponents<HingeJoint2D>();
//				for (int j = 0; j < allComponents.Length; j++)
//				{
//					if (allComponents[j].connectedBody == null)
//					{
//						HingeJoint2D hingeJoint2D = allComponents[j];
//						UnityEngine.Object.DestroyImmediate(hingeJoint2D);
//					}
//				}
//			}
//		}
//	}
//	[MenuItem("Services/14CleanHoleInIron")]
//	public static void Button18()
//	{
//		Transform obj = Selection.activeTransform;
//		for (int i = obj.childCount - 1; i >= 0; i--)
//		{
//			if (obj.GetChild(i).gameObject.name.Contains("HoleInIronLeft"))
//			{
//				Transform tras = obj.GetChild(i);
//				UnityEngine.Object.DestroyImmediate(tras.gameObject);
//			}
//		}
//	}
//	[MenuItem("Services/15CreateHingeJoints")]
//	public static void Button11()
//	{
//		Transform obj = Selection.activeTransform;
//		GameObject c = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/InputManager"), obj);
//		for (int j = 0; j < obj.childCount; j++)
//		{
//			if (obj.GetChild(j).gameObject.name.Contains("Range"))
//			{
//				obj.GetChild(j).AddComponent<EndLine>();
//			}
//		}
//		obj.AddComponent<Stage>();
//		for (int j = 0; j < obj.childCount; j++)
//		{
//			if (obj.GetChild(j).gameObject.tag == "Iron")
//			{
//				obj.GetChild(j).GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
//				obj.GetChild(j).GetComponent<Rigidbody2D>().angularDrag = 1f;
//				obj.GetChild(j).AddComponent<IronPlate>();
//				for (int i = 0; i < obj.GetChild(j).childCount - 1; i++)
//				{
//					obj.GetChild(j).AddComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
//				}
//			}
//		}
//	}
//	[MenuItem("Services/16SetHingeJoints")]
//	public static void Button12()
//	{
//		Transform obj = Selection.activeTransform;
//		for (int j = 0; j < obj.childCount; j++)
//		{
//			if (obj.GetChild(j).gameObject.tag == "Iron")
//			{
//				HingeJoint2D[] allComponents = obj.GetChild(j).GetComponents<HingeJoint2D>();
//				for (int i = 0; i < obj.GetChild(j).childCount; i++)
//				{
//					Transform transform = obj.GetChild(j).GetChild(i);
//					RaycastHit2D[] HitHole = Physics2D.CircleCastAll(transform.position, 0.1f, Vector3.forward);
//					if (HitHole.Length > 0)
//					{
//						foreach (RaycastHit2D collider in HitHole)
//						{
//							if (collider.transform.tag == "Nail")
//							{
//								if (allComponents[i].connectedBody == null)
//								{
//									allComponents[i].connectedBody = collider.transform.gameObject.GetComponent<Rigidbody2D>();
//									allComponents[i].connectedAnchor = Vector2.zero;
//								}
//							}
//						}
//					}
//				}
//			}
//		}
//	}
//	[MenuItem("Services/17caculate")]
//	public static void Button13()
//	{
//		Transform obj = Selection.activeTransform;
//		for (int j = 0; j < obj.childCount; j++)
//		{
//			if (obj.GetChild(j).gameObject.tag == "Iron")
//			{
//				HingeJoint2D[] allComponents = obj.GetChild(j).GetComponents<HingeJoint2D>();
//				for (int i = 0; i < allComponents.Length; i++)
//				{
//					Transform nailTransform = allComponents[i].connectedBody.transform;
//					allComponents[i].anchor = obj.GetChild(j).InverseTransformPoint(nailTransform.position);
//				}
//			}
//		}
//		for (int j = 0; j < obj.childCount; j++)
//		{
//			if (obj.GetChild(j).gameObject.tag == "NailsManager")
//			{
//				for (int i = 0; i < obj.GetChild(j).childCount; i++)
//				{
//					Transform transform = obj.GetChild(j).GetChild(i);
//					transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/upscale ingame/Untitled-1_0000_Group-1-copy");
//				}
//			}
//		}
//	}
//	[MenuItem("Services/changeGravity")]
//	public static void Button14()
//	{
//		Transform obj = Selection.activeTransform;
//		for (int j = 0; j < obj.childCount; j++)
//		{
//			if (obj.GetChild(j).gameObject.tag == "Iron")
//			{
//				Transform transform = obj.GetChild(j);
//				transform.GetComponent<Rigidbody2D>().gravityScale = 2f;
//			}
//		}
//	}
//	[MenuItem("Services/changeNewImage")]
//	public static void Button19()
//	{
//		Transform obj = Selection.activeTransform;
//		for (int j = 0; j < obj.childCount; j++)
//		{
//			if (obj.GetChild(j).gameObject.tag == "square")
//			{
//				Transform transform = obj.GetChild(j);
//				transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/UI Nut/export/Ingame (2)/Ingame/bg_board");
//				transform.localPosition = new Vector3(0, -0.10810812f, 1f);
//				for (int i = transform.childCount - 1; i >= 0; i--)
//				{
//					Transform transform1 = transform.GetChild(i);
//					transform1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/UI Nut/export/Ingame (2)/Ingame/Hole");
//					if (transform1.childCount > 0)
//					{
//						transform1.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UI Nut/export/Ingame (2)/Ingame/+ blue");
//					}
//				}
//			}
//		}
//	}
//	[MenuItem("Services/changeSquareLayer")]
//	public static void Button22()
//	{
//		Transform obj = Selection.activeTransform;
//		string layerName = "Default";
//		int layerValue = LayerMask.NameToLayer(layerName);
//		for (int j = 0; j < obj.childCount; j++)
//		{
//			for (int i = 0; i < obj.GetChild(j).childCount; i++)
//			{
//				if (obj.GetChild(j).GetChild(i).gameObject.tag == "square")
//				{
//					Transform transform = obj.GetChild(j).GetChild(i);
//					transform.gameObject.layer = layerValue;

//				}
//			}
//		}
//	}
//	public static Transform findSquare(Transform transform)
//	{
//		for (int i = transform.childCount - 1; i >= 0; i--)
//		{
//			if (transform.GetChild(i).CompareTag("square"))
//			{
//				return transform.GetChild(i);
//			}
//		}
//		return null;
//	}
//	[MenuItem("Services/changeNailScale")]
//	public static void Button23()
//	{
//		Transform obj = Selection.activeTransform;
//		string layerName = "Default";
//		int layerValue = LayerMask.NameToLayer(layerName);
//		Transform transform1 = GameObject.FindGameObjectWithTag("square").transform;
//		for (int j = 0; j < obj.childCount; j++)
//		{
//			if (obj.GetChild(j).gameObject.CompareTag("NailsManager"))
//			{
//				Transform transform = obj.GetChild(j);
//				for (int k = 0; k < transform.childCount; k++)
//				{
//					transform.GetChild(k).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/upscale ingame/Untitled-1_0000_Group-1-copy");
//					transform.GetChild(k).localScale = new Vector3(transform1.GetChild(0).localScale.x + 0.01f, transform1.GetChild(0).localScale.y + 0.01f, 1f);
//				}
//			}
//			if (obj.GetChild(j).gameObject.CompareTag("Iron"))
//			{
//				Transform transform2 = obj.GetChild(j);
//				for (int k = 0; k < transform2.childCount; k++)
//				{
//					transform2.GetChild(k).localScale = new Vector3(transform1.GetChild(0).localScale.x / 2 + 0.01f, transform1.GetChild(0).localScale.y / 2 + 0.01f, 1f);
//				}
//			}
//		}
//	}

//	[MenuItem("Services/changeNewMaterial")]
//	public static void Button20()
//	{
//		Transform obj = Selection.activeTransform;
//		string IronLayer1 = "IronLayer1";
//		string IronLayer2 = "IronLayer2";
//		string IronLayer3 = "IronLayer3";
//		string IronLayer4 = "IronLayer4";
//		string IronLayer5 = "IronLayer5";
//		string IronLayer6 = "IronLayer6";
//		string IronLayer7 = "IronLayer7";
//		string IronLayer8 = "IronLayer8";
//		string layer1vs2 = "layer1vs2";
//		string layer1vs2vs3 = "layer1vs2vs3";
//		string layer1vs2vs3vs4 = "layer1vs2vs3vs4";
//		string layer1vs2vs3vs4vs5 = "layer1vs2vs3vs4vs5";
//		string BothLayer = "BothLayer";
//		List<int> list = new List<int>();
//		int layerValue1 = LayerMask.NameToLayer(IronLayer1);
//		int layerValue2 = LayerMask.NameToLayer(IronLayer2);
//		int layerValue3 = LayerMask.NameToLayer(IronLayer3);
//		int layerValue4 = LayerMask.NameToLayer(IronLayer4);
//		int layerValue5 = LayerMask.NameToLayer(IronLayer5);
//		int layerValue6 = LayerMask.NameToLayer(IronLayer6);
//		int layerValue7 = LayerMask.NameToLayer(IronLayer7);
//		int layerValue8 = LayerMask.NameToLayer(IronLayer8);
//		int layerValue9 = LayerMask.NameToLayer(layer1vs2);
//		int layerValue10 = LayerMask.NameToLayer(layer1vs2vs3);
//		int layerValue11 = LayerMask.NameToLayer(layer1vs2vs3vs4);
//		int layerValue12 = LayerMask.NameToLayer(layer1vs2vs3vs4vs5);
//		int BothLayerValue = LayerMask.NameToLayer(BothLayer);
//		list.Add(layerValue1);
//		list.Add(layerValue2);
//		list.Add(layerValue3);
//		list.Add(layerValue4);
//		list.Add(layerValue5);
//		list.Add(layerValue6);
//		list.Add(layerValue7);
//		list.Add(layerValue8);
//		list.Add(layerValue9);
//		list.Add(layerValue10);
//		list.Add(layerValue11);
//		list.Add(layerValue12);
//		list.Add(layerValue12);
//		list.Add(BothLayerValue);
//		for (int j = 0; j < obj.childCount; j++)
//		{
//			for (int i = 0; i < obj.GetChild(j).childCount; i++)
//			{
//				if (obj.GetChild(j).GetChild(i).gameObject.tag == "Iron")
//				{
//					Transform transform = obj.GetChild(j).GetChild(i);
//					for (int k = 0; k < list.Count; k++)
//						if (transform.gameObject.layer == list[k])
//						{
//							switch (k)
//							{
//								case 0:
//									transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial1 5");
//									break;
//								case 1:
//									transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial1");
//									break;
//								case 2:
//									transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial1 2");
//									break;
//								case 3:
//									transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial1 3");
//									break;
//								case 4:
//									transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial1 4");
//									break;
//								case 5:
//									transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial1 1");
//									break;
//								case 6:
//									transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial1");
//									break;
//								case 7:
//									transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial1 1");
//									break;
//								case 8:
//									transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial1");
//									break;
//								case 9:
//									transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial1 2");
//									break;
//								case 10:
//									transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial1 3");
//									break;
//								case 11:
//									transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial1 4");
//									break;
//								case 12:
//									transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial1 4");
//									break;
//								default:
//									transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial1 5");
//									break;
//							}
//						}
//				}
//			}
//		}
//	}
//}
