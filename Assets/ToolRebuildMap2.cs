//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.UI;

//public class ToolRebuildMap2 : MonoBehaviour
//{
//    [MenuItem("Tool/rebuild2/addtoStage")]
//    public static void Button24()
//    {
//        Transform obj = Selection.activeTransform;
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            Transform transform1 = obj.GetChild(j);
//            for (int i = 0; i < obj.GetChild(j).childCount; i++)
//            {
//                if (obj.GetChild(j).GetChild(i).gameObject.tag == "square")
//                {
//                    Transform transform2 = obj.GetChild(j).GetChild(i).transform;
//                    for (int k = 0; k < transform2.childCount; k++)
//                    {
//                        if (transform2.GetChild(k).gameObject.tag == "Hole")
//                        {
//                            Transform transform = transform2.GetChild(k);
//                            RaycastHit2D[] HitHole = Physics2D.CircleCastAll(transform.position, 0.1f, Vector3.forward);
//                            if (HitHole.Length > 0)
//                            {
//                                foreach (RaycastHit2D collider in HitHole)
//                                {
//                                    if (collider.transform.tag == "Nail")
//                                    {
//                                        transform.GetComponent<Hole>().Nail = collider.transform.GetComponent<NailControl>();
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//                if (obj.GetChild(j).GetChild(i).gameObject.tag == "Iron")
//                {
//                    Transform transform2 = obj.GetChild(j).GetChild(i).transform;
//                    for (int k = 0; k < transform2.childCount; k++)
//                    {
//                        Transform transform = transform2.GetChild(k);
//                        RaycastHit2D[] HitHole = Physics2D.CircleCastAll(transform.position, 0.1f, Vector3.forward);
//                        if (HitHole.Length > 0)
//                        {
//                            foreach (RaycastHit2D collider in HitHole)
//                            {
//                                if (collider.transform.tag == "Nail")
//                                {
//                                    transform.GetComponent<NailDetector>().Nail = collider.transform.GetComponent<NailControl>();
//                                }
//                            }
//                        }
//                    }
//                }

//            }
//        }
//    }
//    [MenuItem("Tool/rebuild2/testngu :)))")]
//    public static void button25()
//    {
//        Transform obj = Selection.activeTransform;
//        PictureUIManager pictureUIManager = obj.GetComponent<PictureUIManager>();
//        for (int i = 0; i < pictureUIManager.Stage.Length; i++)
//        {
//            for (int j = 0; j < pictureUIManager.Stage[i].ObjBtn.Length; j++)
//            {
//                GameObject originalObj = pictureUIManager.Stage[i].ObjBtn[j];
//                GameObject prefabObj = Resources.Load<GameObject>("ObjjPrefab/PicItemBT");

//                // Instantiate the prefab and copy the properties from the original object
//                GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefabObj, originalObj.transform.parent);

//                newObj.transform.SetParent(originalObj.transform.parent, false);
//                CopyComponentValues(originalObj, newObj);
//                CoppyPos(originalObj, newObj);
//                setStarText(originalObj, newObj);
//                CoppyChildImg(originalObj.transform.GetChild(0).transform.gameObject, newObj.transform.GetChild(0).transform.gameObject);

//                // Update the reference in PictureUIManager
//                pictureUIManager.Stage[i].ObjBtn[j] = newObj;

//                // Optionally destroy the original object
//                GameObject.DestroyImmediate(originalObj);
//            }
//        }
//    }

//    private static void CopyComponentValues(GameObject originalObj, GameObject newObj)
//    {
//        // Copy the values of components from originalObj to newObj
//        var originalComponents = originalObj.GetComponents<Component>();
//        foreach (var originalComponent in originalComponents)
//        {
//            var type = originalComponent.GetType();
//            var newComponent = newObj.GetComponent(type) ?? newObj.AddComponent(type);
//            foreach (var field in type.GetFields())
//            {
//                if (field.IsPublic && !field.IsStatic)
//                {
//                    field.SetValue(newComponent, field.GetValue(originalComponent));
//                }
//            }
//        }
//    }
//    private static void CoppyChildImg(GameObject originalObj, GameObject newObj)
//    {
//        newObj.GetComponent<Image>().sprite = originalObj.GetComponent<Image>().sprite;
//        newObj.GetComponent<Image>().SetNativeSize();
//    }
//    private static void CoppyPos(GameObject originalObj, GameObject newObj)
//    {
//        newObj.GetComponent<RectTransform>().position = originalObj.GetComponent<RectTransform>().position;
//        newObj.GetComponent<RectTransform>().rotation = originalObj.GetComponent<RectTransform>().rotation;
//        newObj.GetComponent<RectTransform>().localScale = originalObj.GetComponent<RectTransform>().localScale;
//    }
//    public static void setStarText(GameObject originalObj, GameObject newObj)
//    {
//        var text = newObj.transform.GetChild(1).GetChild(1);
//        newObj.GetComponent<LevelButton>().itemStar = text.GetComponent<TextMeshProUGUI>();
//    }
//}
