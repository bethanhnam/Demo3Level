//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using static UnityEngine.GraphicsBuffer;

//[CustomEditor(typeof(ProductData))]
//public class ProductDataEditor : Editor
//{
//	public override void OnInspectorGUI()
//	{
//		base.OnInspectorGUI();

//		ProductData productData = (ProductData)target;

//		EditorGUILayout.Space();

//		productData.packName = (PackName)EditorGUILayout.EnumPopup("Pack Name", productData.packName);

//		if (GUI.changed)
//		{
//			EditorUtility.SetDirty(productData);
//		}
//	}
//}
