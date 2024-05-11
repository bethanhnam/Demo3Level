//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class ToolRebuildMap2 : MonoBehaviour
//{
//	[MenuItem("Component/addtoStage")]
//	public static void Button24()
//	{
//		Transform obj = Selection.activeTransform;
//		for (int j = 0; j < obj.childCount; j++)
//		{
//			Transform transform1 = obj.GetChild(j);
//			for (int i = 0; i < obj.GetChild(j).childCount; i++)
//			{
//				if (obj.GetChild(j).GetChild(i).gameObject.tag == "square")
//				{
//					Transform transform2 = obj.GetChild(j).GetChild(i).transform;
//					for (int k = 0; k < transform2.childCount; k++)
//					{
//						if (transform2.GetChild(k).gameObject.tag == "Hole")
//						{
//							Transform transform = transform2.GetChild(k);
//							RaycastHit2D[] HitHole = Physics2D.CircleCastAll(transform.position, 0.1f, Vector3.forward);
//							if (HitHole.Length > 0)
//							{
//								foreach (RaycastHit2D collider in HitHole)
//								{
//									if (collider.transform.tag == "Nail")
//									{
//										transform.GetComponent<Hole>().Nail = collider.transform.GetComponent<NailControl>();
//									}
//								}
//							}
//						}
//					}
//				}
//				if (obj.GetChild(j).GetChild(i).gameObject.tag == "Iron")
//				{
//					Transform transform2 = obj.GetChild(j).GetChild(i).transform;
//					for (int k = 0; k < transform2.childCount; k++)
//					{
//						Transform transform = transform2.GetChild(k);
//						RaycastHit2D[] HitHole = Physics2D.CircleCastAll(transform.position, 0.1f, Vector3.forward);
//						if (HitHole.Length > 0)
//						{
//							foreach (RaycastHit2D collider in HitHole)
//							{
//								if (collider.transform.tag == "Nail")
//								{
//									transform.GetComponent<NailDetector>().Nail = collider.transform.GetComponent<NailControl>();
//								}
//							}
//						}
//					}
//				}

//			}
//		}
//	}
//}
