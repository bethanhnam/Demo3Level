//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using System.Linq;
//using Unity.VisualScripting;
//using System;
//using Unity.Mathematics;
//using DG.Tweening;
//using UnityEditor.SceneManagement;
//using UnityEngine.UI;
//using TMPro;
//using TMPro.EditorUtilities;

//public class ToolMap : Editor
//{
//    [MenuItem("Component/FirstPhase")]
//    public static void FirstPhase()
//    {
//        Transform obj = Selection.activeTransform;
//        if (obj == null)
//        {
//            Debug.LogWarning("No object selected.");
//            return;
//        }

//        ExecuteSequence(obj);
//    }
//    private static void ExecuteSequence(Transform obj)
//    {
//        DeteleHole(obj);
//        EditorApplication.delayCall += () =>
//        {
//            Clean(obj);
//            EditorApplication.delayCall += () =>
//            {
//                ChangeMaterial(obj);
//            };
//        };
//    }


//    public static void Clean(Transform obj)
//    {
//        obj.transform.localScale = new Vector3(1f, 1f, 1f);
//        clean(obj);
//        for (int i = obj.childCount - 1; i >= 0; i--)
//        {
//            if (obj.GetChild(i).gameObject.name.Contains("Square"))
//            {
//                obj.GetChild(i).gameObject.tag = "square";
//                for (int j = obj.GetChild(i).childCount - 1; j >= 0; j--)
//                {
//                    if (obj.GetChild(i).GetChild(j).gameObject.name.Contains("stroke_board#1"))
//                    {
//                        UnityEngine.Object.DestroyImmediate(obj.GetChild(i).GetChild(j).gameObject);
//                        continue;
//                    }
//                    if (obj.GetChild(i).GetChild(j).gameObject.name.Contains("stroke_board#2"))
//                    {
//                        UnityEngine.Object.DestroyImmediate(obj.GetChild(i).GetChild(j).gameObject);
//                        continue;
//                    }
//                    else
//                    {
//                        Transform trs = obj.GetChild(i).GetChild(j);
//                        trs.SetParent(obj.transform);
//                        continue;
//                    }
//                }
//                continue;
//            }
//        }
//    }
//    public static void clean(Transform obj)
//    {
//        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj.gameObject);
//        for (int i = 0; i < obj.childCount; i++)
//        {
//            clean(obj.GetChild(i));
//        }
//    }
//    public static void DeteleHole(Transform obj)
//    {
//        for (int i = obj.childCount - 1; i >= 0; i--)
//        {
//            if (obj.GetChild(i).gameObject.name.Contains("Hole"))
//            {
//                Transform transform = obj.GetChild(i);
//                UnityEngine.Object.DestroyImmediate(transform.gameObject);
//            }
//        }
//    }
//    public static void ChangeMaterial(Transform obj)
//    {
//        obj.position = new Vector3(0, 0.2f, 0f);
//        obj.localScale = new Vector3(1.85f, 1.85f, 1f);
//        for (int i = 0; i < obj.childCount; i++)
//        {

//            if (obj.GetChild(i).gameObject.tag == "Iron")
//            {
//                obj.GetChild(i).GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/New Shader Graph");
//                continue;
//            }
//            if (obj.GetChild(i).gameObject.tag == "square")
//            {
//                Transform transform = obj.GetChild(i);
//                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/UI Nut/export/Ingame (2)/Ingame/bg_board");
//                transform.transform.localScale = new Vector3(.54f, .54f, 1f);
//                transform.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
//                transform.transform.position = new Vector3(0, 0, 0);
//                continue;
//            }
//        }
//    }


//    [MenuItem("Component/SecondPhase")]
//    public static void SecondPhase()
//    {
//        Transform obj = Selection.activeTransform;
//        if (obj == null)
//        {
//            Debug.LogWarning("No object selected.");
//            return;
//        }

//        ExecuteSequence1(obj);
//    }
//    private static void ExecuteSequence1(Transform obj)
//    {
//        HoleInSquareToIron(obj);
//        EditorApplication.delayCall += () =>
//        {
//            HoleInSquareIronToWorld(obj);
//            EditorApplication.delayCall += () =>
//            {
//                DeteleDupHoleInSquare(obj);
//            };
//        };
//    }

//    public static void HoleInSquareToIron(Transform obj)
//    {
//        List<Transform> transforms = new List<Transform>();
//        for (int i = obj.childCount - 1; i >= 0; i--)
//        {
//            if (obj.GetChild(i).gameObject.name.Contains("HoleInSquare"))
//            {
//                Transform transform = obj.GetChild(i);
//                RaycastHit2D[] HitHole = Physics2D.CircleCastAll(transform.position, 0.01f, Vector3.forward);
//                if (HitHole.Length > 0)
//                {
//                    foreach (RaycastHit2D collider in HitHole)
//                    {
//                        if (collider.transform.tag == "Iron")
//                        {
//                            if (!transforms.Contains(transform))
//                            {
//                                transforms.Add(transform);
//                            }
//                            GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/HoleInSquare"), obj);
//                            a.transform.position = transform.position;
//                            a.transform.rotation = transform.rotation;
//                            a.transform.localScale = transform.localScale;
//                            a.transform.SetSiblingIndex(transform.GetSiblingIndex());
//                            a.transform.SetParent(collider.transform);

//                            continue;
//                        }
//                    }
//                }
//                continue;
//            }
//        }
//        for (int k = transforms.Count - 1; k >= 0; k--)
//        {
//            UnityEngine.Object.DestroyImmediate(transforms[k].gameObject);
//        }
//    }
//    public static void HoleInSquareIronToWorld(Transform obj)
//    {
//        for (int i = obj.childCount - 1; i >= 0; i--)
//        {
//            if (obj.GetChild(i).gameObject.tag == "Iron")
//            {
//                Transform transform = obj.GetChild(i);
//                for (int j = transform.childCount - 1; j >= 0; j--)
//                {
//                    transform.GetChild(j).SetParent(obj.transform);
//                }
//            }
//        }
//    }
//    public static void DeteleDupHoleInSquare(Transform obj)
//    {
//        List<Transform> transforms = new List<Transform>();
//        for (int i = obj.childCount - 1; i >= 0; i--)
//        {
//            if (obj.GetChild(i).gameObject.name.Contains("HoleInSquare"))
//            {
//                Transform transform = obj.GetChild(i);
//                for (int j = i - 1; j >= 0; j--)
//                {
//                    if (transform.position == obj.GetChild(j).position)
//                    {
//                        if (obj.GetChild(j).gameObject.name.Contains("HoleInSquare"))
//                        {
//                            transforms.Add(obj.GetChild(i));
//                            continue;
//                        }
//                    }
//                }
//                continue;
//            }
//        }
//        for (int k = 0; k < transforms.Count; k++)
//        {
//            if (transforms[k] != null)
//            {
//                UnityEngine.Object.DestroyImmediate(transforms[k].gameObject);
//                continue;
//            }
//        }
//    }


//    [MenuItem("Component/7changePostionhole")]
//    public static void Button18()
//    {
//        Transform obj = Selection.activeTransform;
//        for (int i = obj.childCount - 1; i >= 0; i--)
//        {
//            if (obj.GetChild(i).gameObject.name.Contains("HoleInSquare"))
//            {
//                Transform transform = obj.GetChild(i);
//                RaycastHit2D[] HitHole = Physics2D.CircleCastAll(transform.position, 0.01f, Vector3.forward);
//                if (HitHole.Length > 0)
//                {
//                    foreach (RaycastHit2D collider in HitHole)
//                    {
//                        if (collider.transform.tag == "Iron")
//                        {
//                            transform.SetParent(collider.transform);
//                            transform.localPosition = new Vector3(transform.localPosition.x, 0f, 1f);
//                            transform.SetParent(obj.transform);
//                            continue;
//                        }
//                    }
//                }
//                continue;
//            }
//        }
//    }



//    [MenuItem("Component/ThirdPhase")]
//    public static void ThirdPhase()
//    {
//        Transform obj = Selection.activeTransform;
//        if (obj == null)
//        {
//            Debug.LogWarning("No object selected.");
//            return;
//        }

//        ExecuteSequence2(obj);
//    }
//    private static void ExecuteSequence2(Transform obj)
//    {
//        CreateHole(obj);
//        EditorApplication.delayCall += () =>
//        {
//            HoleInSquareToSquare(obj);
//            EditorApplication.delayCall += () =>
//            {
//                CreateHoleInIronAndNail(obj);
//                EditorApplication.delayCall += () =>
//                {
//                    DeteleDupNail(obj);
//                    EditorApplication.delayCall += () =>
//                    {
//                        NailToHoleInSquare(obj);
//                        EditorApplication.delayCall += () =>
//                        {
//                            DeteleDupHoleInIron(obj);
//                        };
//                    };
//                };
//            };
//        };
//    }


//    public static void CreateHole(Transform obj)
//    {
//        for (int i = 0; i < obj.childCount; i++)
//        {
//            if (obj.GetChild(i).gameObject.name.Contains("HoleInSquare"))
//            {
//                Transform trs = obj.GetChild(i);
//                GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/HoleInSquare"), obj);
//                a.transform.position = trs.position;
//                a.transform.rotation = trs.rotation;
//                a.transform.localScale = trs.localScale;
//                a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//                GameObject b = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/HoleInIronLeft"), obj);
//                b.transform.position = trs.position;
//                b.transform.rotation = trs.rotation;
//                b.transform.localScale = trs.localScale;
//                b.transform.SetSiblingIndex(trs.GetSiblingIndex());
//                UnityEngine.Object.DestroyImmediate(trs.gameObject);
//                continue;
//            }
//        }
//    }
//    public static void HoleInSquareToSquare(Transform obj)
//    {
//        GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/NailsManager"), obj);
//        Transform square = GameObject.FindGameObjectWithTag("square").transform;
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            if (obj.GetChild(j).gameObject.name.Contains("HoleInSquare"))
//            {
//                Transform transform = obj.GetChild(j);
//                transform.SetParent(square.transform);

//            }
//        }
//    }
//    public static void CreateHoleInIronAndNail(Transform obj)
//    {
//        Transform nailsManager = GameObject.FindGameObjectWithTag("NailsManager").transform;
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            if (obj.GetChild(j).gameObject.name.Contains("HoleInIronLeft"))
//            {
//                Transform transform = obj.GetChild(j);
//                RaycastHit2D[] HitHole = Physics2D.CircleCastAll(transform.position, 0.01f, Vector3.forward);
//                if (HitHole.Length > 0)
//                {
//                    foreach (RaycastHit2D collider in HitHole)
//                    {
//                        if (collider.transform.tag == "Iron")
//                        {
//                            quaternion oldRotat = collider.transform.rotation;
//                            if (collider.transform.eulerAngles.z != 0)
//                            {
//                                collider.transform.eulerAngles = new Vector3(0, 0, 90);
//                            }
//                            GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/HoleInIronLeft"), obj);
//                            a.transform.position = transform.position;
//                            a.transform.rotation = transform.rotation;
//                            a.transform.localScale = transform.localScale;
//                            a.transform.SetSiblingIndex(transform.GetSiblingIndex());
//                            a.transform.SetParent(collider.transform);
//                            collider.transform.rotation = oldRotat;
//                            a.transform.position = transform.position;
//                            GameObject c = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/NailPrefab"), obj);
//                            c.transform.position = transform.position;
//                            c.transform.SetParent(nailsManager);
//                        }
//                    }
//                }
//            }
//        }
//    }
//    public static void DeteleDupNail(Transform obj)
//    {
//        for (int i = 0; i < obj.childCount; i++)
//        {
//            if (obj.GetChild(i).gameObject.tag == "NailsManager")
//            {
//                Transform transform1 = obj.GetChild(i).transform;
//                for (int j = transform1.childCount - 1; j >= 1; j--)
//                {
//                    if (transform1.GetChild(j).gameObject.tag == "Nail")
//                    {
//                        if (transform1.GetChild(j).gameObject.transform.position == transform1.GetChild(j - 1).gameObject.transform.position)
//                        {
//                            UnityEngine.Object.DestroyImmediate(transform1.GetChild(j).gameObject);
//                        }
//                    }
//                }
//            }
//        }
//    }
//    public static void NailToHoleInSquare(Transform obj)
//    {
//        for (int i = 0; i < obj.childCount; i++)
//        {
//            if (obj.GetChild(i).gameObject.tag == "square")
//            {
//                Transform transform1 = obj.GetChild(i).transform;
//                for (int j = 0; j < transform1.childCount; j++)
//                {
//                    if (transform1.GetChild(j).gameObject.tag == "Hole")
//                    {
//                        Transform transform = transform1.GetChild(j);
//                        RaycastHit2D[] HitHole = Physics2D.CircleCastAll(transform.position, 0.1f, Vector3.forward);
//                        if (HitHole.Length > 0)
//                        {
//                            foreach (RaycastHit2D collider in HitHole)
//                            {
//                                if (collider.transform.tag == "Nail")
//                                {
//                                    transform.GetComponent<Hole>().Nail = collider.transform.GetComponent<NailControl>();
//                                }
//                            }
//                        }
//                    }
//                }
//            }

//        }

//    }
//    public static void DeteleDupHoleInIron(Transform obj)
//    {
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            if (obj.GetChild(j).gameObject.tag == "Iron")
//            {
//                Transform transform = obj.GetChild(j).transform;
//                for (int i = obj.GetChild(j).childCount - 1; i >= 1; i--)
//                {
//                    if (transform.GetChild(i).gameObject.transform.position == transform.GetChild(i - 1).gameObject.transform.position)
//                    {
//                        UnityEngine.Object.DestroyImmediate(transform.GetChild(i).gameObject);
//                    }
//                }
//            }
//        }
//    }

//    /// Tool Rebuild map 2 
//    [MenuItem("Component/FourPhase")]
//    public static void FourPhase()
//    {
//        Transform obj = Selection.activeTransform;
//        if (obj == null)
//        {
//            Debug.LogWarning("No object selected.");
//            return;
//        }

//        ExecuteSequence3(obj);
//    }
//    private static void ExecuteSequence3(Transform obj)
//    {
//        CleanHoleInIron(obj);
//        EditorApplication.delayCall += () =>
//        {
//            CreateHingeJoints(obj);
//            EditorApplication.delayCall += () =>
//            {
//                SetHingeJoints(obj);
//                EditorApplication.delayCall += () =>
//                {
//                    caculate(obj);
//                    EditorApplication.delayCall += () =>
//                    {
//                        CleanRebuild(obj);
//                        EditorApplication.delayCall += () =>
//                        {
//                            caculateCollider(obj);
//                            EditorApplication.delayCall += () =>
//                            {
//                                createmap(obj);
//                                EditorApplication.delayCall += () =>
//                                {
//                                    ResizeShadow(obj);
//                                    EditorApplication.delayCall += () =>
//                                    {
//                                        CleanHinge(obj);
//                                        EditorApplication.delayCall += () =>
//                                        {
//                                            CleanHinge1(obj);
//                                            EditorApplication.delayCall += () =>
//                                            {
//                                                SetHingeJoints(obj);
//                                                EditorApplication.delayCall += () =>
//                                                {
//                                                    caculate(obj);
//                                                    EditorApplication.delayCall += () =>
//                                                    {
//                                                        change(obj);
//                                                        EditorApplication.delayCall += () =>
//                                                        {
//                                                            changeEndlinePosition(obj);
//                                                            EditorApplication.delayCall += () =>
//                                                            {
//                                                                changeGravity(obj);
//                                                                EditorApplication.delayCall += () =>
//                                                                {
//                                                                    changeNailScale(obj);
//                                                                    EditorApplication.delayCall += () =>
//                                                                    {
//                                                                        changeNewImage(obj);
//                                                                    };
//                                                                };
//                                                            };
//                                                        };
//                                                    };
//                                                };
//                                            };
//                                        };
//                                    };
//                                };
//                            };
//                        };
//                    };
//                };
//            };
//        };
//    }


//    //14
//    public static void CleanHoleInIron(Transform obj)
//    {
//        for (int i = obj.childCount - 1; i >= 0; i--)
//        {
//            if (obj.GetChild(i).gameObject.name.Contains("HoleInIronLeft"))
//            {
//                Transform tras = obj.GetChild(i);
//                UnityEngine.Object.DestroyImmediate(tras.gameObject);
//            }
//        }
//    }
//    //15
//    public static void CreateHingeJoints(Transform obj)
//    {
//        GameObject c = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/InputManager"), obj);
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            if (obj.GetChild(j).gameObject.name.Contains("Range"))
//            {
//                obj.GetChild(j).AddComponent<EndLine>();
//            }
//        }
//        obj.AddComponent<Stage>();
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            if (obj.GetChild(j).gameObject.tag == "Iron")
//            {
//                obj.GetChild(j).GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
//                obj.GetChild(j).GetComponent<Rigidbody2D>().angularDrag = 1f;
//                obj.GetChild(j).AddComponent<IronPlate>();
//                for (int i = 0; i < obj.GetChild(j).childCount - 1; i++)
//                {
//                    obj.GetChild(j).AddComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
//                }
//            }
//        }
//    }
//    //16
//    public static void SetHingeJoints(Transform obj)
//    {
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            if (obj.GetChild(j).gameObject.tag == "Iron")
//            {
//                HingeJoint2D[] allComponents = obj.GetChild(j).GetComponents<HingeJoint2D>();
//                for (int i = 0; i < obj.GetChild(j).childCount; i++)
//                {
//                    Transform transform = obj.GetChild(j).GetChild(i);
//                    RaycastHit2D[] HitHole = Physics2D.CircleCastAll(transform.position, 0.1f, Vector3.forward);
//                    if (HitHole.Length > 0)
//                    {
//                        foreach (RaycastHit2D collider in HitHole)
//                        {
//                            if (collider.transform.tag == "Nail")
//                            {
//                                if (allComponents[i].connectedBody == null)
//                                {
//                                    allComponents[i].connectedBody = collider.transform.gameObject.GetComponent<Rigidbody2D>();
//                                    allComponents[i].connectedAnchor = Vector2.zero;
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//    //17
//    public static void caculate(Transform obj)
//    {
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            if (obj.GetChild(j).gameObject.tag == "Iron")
//            {
//                HingeJoint2D[] allComponents = obj.GetChild(j).GetComponents<HingeJoint2D>();
//                for (int i = 0; i < allComponents.Length; i++)
//                {
//                    Transform nailTransform = allComponents[i].connectedBody.transform;
//                    allComponents[i].anchor = obj.GetChild(j).InverseTransformPoint(nailTransform.position);
//                }
//            }
//        }
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            if (obj.GetChild(j).gameObject.tag == "NailsManager")
//            {
//                for (int i = 0; i < obj.GetChild(j).childCount; i++)
//                {
//                    Transform transform = obj.GetChild(j).GetChild(i);
//                    transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/upscale ingame/Untitled-1_0000_Group-1-copy");
//                }
//            }
//        }
//    }
//    //18
//    public static void CleanRebuild(Transform obj)
//    {
//        for (int i = obj.childCount - 1; i >= 0; i--)
//        {
//            if (obj.GetChild(i).gameObject.name.Contains("Square"))
//            {
//                Transform transform = obj.GetChild(i).transform;
//                transform.gameObject.tag = "square";
//                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/UI Nut/export/Ingame (2)/Ingame/bg_board");
//            }
//            if (obj.GetChild(i).gameObject.tag == "Iron")
//            {
//                Transform transform1 = obj.GetChild(i).transform;
//                transform1.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/New Shader Graph");
//                for (int k = transform1.childCount - 1; k >= 0; k--)
//                {
//                    transform1.GetChild(k).gameObject.SetActive(true);
//                    SpriteRenderer spriteRenderer = transform1.GetChild(k).GetComponent<SpriteRenderer>();
//                    UnityEngine.Object.DestroyImmediate(spriteRenderer);
//                    transform1.GetChild(k).GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Ui11/hole 2");
//                }
//                continue;
//            }
//            if (obj.GetChild(i).gameObject.tag == "NailsManager")
//            {
//                Transform transform2 = obj.GetChild(i).transform;
//                for (int j = 0; j < obj.GetChild(i).childCount; j++)
//                {
//                    Transform transform3 = obj.GetChild(i).GetChild(j).transform;
//                    transform3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/upscale ingame/Untitled-1_0000_Group-1-copy");
//                }
//                continue;
//            }
//            if (obj.GetChild(i).gameObject.tag == "square")
//            {
//                Transform transform = obj.GetChild(i).transform;
//                for (int k = transform.childCount - 1; k >= 0; k--)
//                {
//                    transform.GetChild(k).GetComponent<SpriteRenderer>().enabled = true;
//                    transform.GetChild(k).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/upscale ingame/Untitled-1_0002_Layer-1");
//                }
//                continue;
//            }
//        }
//    }
//    //19
//    public static void caculateCollider(Transform obj)
//    {
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            Transform child = obj.GetChild(j);
//            if (child.gameObject.CompareTag("Iron") && !child.gameObject.name.Contains("washer") && !child.gameObject.name.Contains("Solid") && !child.gameObject.name.Contains("Solid_Circle_1") && !child.gameObject.name.Contains("Circle"))
//            {
//                try
//                {
//                    Vector2[] points = child.GetComponent<PolygonCollider2D>().GetPath(0);
//                    for (int i = 0; i < points.Length; i++)
//                    {
//                        points[i].x = child.localScale.x * points[i].x;
//                        points[i].y = child.localScale.y * points[i].y;
//                    }
//                    child.GetComponent<PolygonCollider2D>().SetPath(0, points);
//                }
//                catch { }
//                continue;
//            }
//        }
//    }
//    //20
//    public static void createmap(Transform obj)
//    {
//        for (int i = obj.childCount - 1; i >= 0; i--)
//        {
//            if (obj.GetChild(i).gameObject.tag == "Iron" && !obj.GetChild(i).gameObject.name.Contains("washer") && !obj.GetChild(i).gameObject.name.Contains("Solid") && !obj.GetChild(i).gameObject.name.Contains("Solid_Circle_1") && !obj.GetChild(i).gameObject.name.Contains("Circle"))
//            {
//                List<Transform> transforms = new List<Transform>();
//                for (int j = obj.GetChild(i).childCount - 1; j >= 0; j--)
//                {
//                    transforms.Add(obj.GetChild(i).GetChild(j));
//                    obj.GetChild(i).GetChild(j).transform.SetParent(obj);
//                }
//                obj.GetChild(i).GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
//                obj.GetChild(i).GetComponent<SpriteRenderer>().size = new Vector2(obj.GetChild(i).localScale.x * obj.GetChild(i).GetComponent<SpriteRenderer>().size.x, obj.GetChild(i).localScale.y * obj.GetChild(i).GetComponent<SpriteRenderer>().size.y);
//                obj.GetChild(i).localScale = Vector3.one;
//                foreach (Transform child in transforms)
//                {
//                    child.SetParent(obj.GetChild(i));
//                    child.SetParent(obj.GetChild(i));
//                    child.localScale = new Vector3(0.35f, 0.35f, 1);
//                    Transform transform = child.transform;
//                    transform.GetComponentInChildren<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
//                    transform.GetComponentInChildren<SpriteRenderer>().size = new Vector2(1f, 1f);
//                    transform.GetChild(0).localScale = Vector2.one;

//                }
//                transforms.Clear();
//            }
//        }
//    }
//    //21
//    public static void ResizeShadow(Transform obj)
//    {
//        for (int i = obj.childCount - 1; i >= 0; i--)
//        {
//            if (obj.GetChild(i).gameObject.tag == "Iron")
//            {
//                for (int j = 0; j < obj.GetChild(i).childCount; j++)
//                {
//                    Transform transform = obj.GetChild(i).GetChild(j);
//                    transform.GetChild(0).GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
//                    transform.GetChild(0).GetComponent<SpriteRenderer>().size = new Vector2(1f, 1f);
//                    transform.GetChild(0).localScale = new Vector2(1f, 1f);
//                    transform.localScale = new Vector2(0.35f, 0.35f);
//                }
//            }
//        }
//    }
//    [MenuItem("Component/ResizeCollider")]
//    public static void ResizeCollider()
//    {
//        Transform obj = Selection.activeTransform;
//        for (int k = 0; k < obj.childCount; k++)
//        {
//            Transform objChild = obj.GetChild(k);
//            for (int i = objChild.childCount - 1; i >= 0; i--)
//            {
//                if (objChild.GetChild(i).gameObject.tag == "Iron")
//                {
//                    ResetIronPlate(objChild.GetChild(i));
//                }
//                EditorApplication.delayCall += () =>
//                {
//                    RunCoroutine(caculate1(objChild));
//                };
//            }
//        }
//    }

//    public static void ResetIronPlate(Transform gameObject)
//    {
//        List<GameObject> li = new List<GameObject>();

//        for (int j = gameObject.childCount - 1; j >= 0; j--)
//        {
//            li.Add(gameObject.GetChild(j).gameObject);
//            gameObject.GetChild(j).transform.parent = gameObject.transform.parent;
//        }
//        try
//        {
//            PolygonCollider2D polygon = gameObject.GetComponent<PolygonCollider2D>();
//            if (polygon != null)
//            {
//                DestroyImmediate(polygon);
//                gameObject.AddComponent<PolygonCollider2D>();
//            }
//            //CircleCollider2D circle = objChild.GetChild(i).GetComponent<CircleCollider2D>();
//            //if(circle != null)
//            //{
//            //    DestroyImmediate(circle);
//            //    objChild.GetChild(i).AddComponent<CircleCollider2D>();
//            //}

//        }
//        catch { }
//        EditorApplication.delayCall += () =>
//        {
//            gameObject.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Simple;
//            if (gameObject.GetComponent<SpriteRenderer>().sprite.name == "Solid_Circle_1" || gameObject.GetComponent<SpriteRenderer>().sprite.name == "Layer-10" || gameObject.GetComponent<SpriteRenderer>().sprite.name == "Holed_Circle")
//            {
//                gameObject.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
//            }
//            EditorApplication.delayCall += () =>
//            {
//                for (int i = li.Count - 1; i >= 0; i--)
//                {
//                    li[i].transform.SetParent(gameObject.transform);
//                }
//            };
//        };
//    }

//    private static void RunCoroutine(IEnumerator coroutine)
//    {
//        void Update()
//        {
//            if (!coroutine.MoveNext())
//            {
//                EditorApplication.update -= Update;
//            }
//        }
//        EditorApplication.update += Update;
//    }
//    private static IEnumerator WaitForSeconds(float seconds)
//    {
//        float start = (float)EditorApplication.timeSinceStartup;
//        while ((float)EditorApplication.timeSinceStartup < start + seconds)
//        {
//            yield return null;
//        }
//    }
//    private static IEnumerator caculate1(Transform obj)
//    {
//        yield return new WaitForSeconds(2);
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            if (obj.GetChild(j).gameObject.tag == "Iron")
//            {
//                HingeJoint2D[] allComponents = obj.GetChild(j).GetComponents<HingeJoint2D>();
//                for (int i = 0; i < allComponents.Length; i++)
//                {
//                    Transform nailTransform = allComponents[i].connectedBody.transform;
//                    allComponents[i].anchor = obj.GetChild(j).InverseTransformPoint(nailTransform.position);
//                }
//            }
//        }
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            if (obj.GetChild(j).gameObject.tag == "NailsManager")
//            {
//                for (int i = 0; i < obj.GetChild(j).childCount; i++)
//                {
//                    Transform transform = obj.GetChild(j).GetChild(i);
//                    transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/upscale ingame/Untitled-1_0000_Group-1-copy");
//                }
//            }
//        }
//    }
//    //22
//    public static void CleanHinge(Transform obj)
//    {

//        for (int i = obj.childCount - 1; i >= 0; i--)
//        {
//            if (obj.GetChild(i).gameObject.tag == "Iron" && !obj.GetChild(i).gameObject.name.Contains("washer") && !obj.GetChild(i).gameObject.name.Contains("Solid"))
//            {
//                HingeJoint2D[] allComponents = obj.GetChild(i).GetComponents<HingeJoint2D>();
//                for (int j = 0; j < allComponents.Length; j++)
//                {
//                    allComponents[j].connectedBody = null;
//                    allComponents[j].autoConfigureConnectedAnchor = false;
//                    allComponents[j].connectedAnchor = Vector2.zero;
//                    allComponents[j].anchor = Vector2.zero;
//                }
//            }
//        }
//    }
//    //23
//    public static void CleanHinge1(Transform obj)
//    {
//        for (int i = obj.childCount - 1; i >= 0; i--)
//        {
//            if (obj.GetChild(i).gameObject.tag == "Iron")
//            {
//                Transform transform = obj.GetChild(i).transform;
//                transform.GetComponent<Rigidbody2D>().sharedMaterial = Resources.Load<PhysicsMaterial2D>("PhysicMaterials/New Physics Material 2D");
//                var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
//                if (prefabStage != null)
//                {
//                    EditorSceneManager.MarkSceneDirty(prefabStage.scene);
//                    PrefabUtility.RecordPrefabInstancePropertyModifications(obj);
//                }

//            }
//        }
//    }
//    //24
//    public static void change(Transform obj)
//    {
//        string layerName = "Default";
//        int layerValue = LayerMask.NameToLayer(layerName);
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            for (int i = 0; i < obj.GetChild(j).childCount; i++)
//            {
//                if (obj.GetChild(j).GetChild(i).gameObject.tag == "Iron")
//                {
//                    Transform transform = obj.GetChild(j).GetChild(i);
//                    transform.GetComponent<Rigidbody2D>().angularDrag = 0.05f;
//                    transform.GetComponent<Rigidbody2D>().useAutoMass = false;
//                    transform.GetComponent<Rigidbody2D>().mass = 1;
//                    transform.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Discrete;
//                }
//            }
//        }
//    }
//    //25
//    public static void changeEndlinePosition(Transform obj)
//    {
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            for (int i = 0; i < obj.GetChild(j).childCount; i++)
//            {
//                if (obj.GetChild(j).GetChild(i).gameObject.name.Contains("Range"))
//                {
//                    Transform transform = obj.GetChild(j).GetChild(i);
//                    transform.position = new Vector3(transform.position.x, transform.position.y - 4, 1f);
//                    transform.localScale = new Vector3(20, 2, 1f);

//                }
//            }
//        }
//    }
//    //26
//    public static void changeGravity(Transform obj)
//    {
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            if (obj.GetChild(j).gameObject.tag == "Iron")
//            {
//                Transform transform = obj.GetChild(j);
//                transform.GetComponent<Rigidbody2D>().gravityScale = 2f;
//            }
//        }
//    }
//    //27
//    public static void changeNailScale(Transform obj)
//    {
//        string layerName = "Default";
//        int layerValue = LayerMask.NameToLayer(layerName);
//        Transform transform1 = GameObject.FindGameObjectWithTag("square").transform;
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            if (obj.GetChild(j).gameObject.CompareTag("NailsManager"))
//            {
//                Transform transform = obj.GetChild(j);
//                for (int k = 0; k < transform.childCount; k++)
//                {
//                    transform.GetChild(k).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/upscale ingame/Untitled-1_0000_Group-1-copy");
//                    transform.GetChild(k).localScale = new Vector3(transform1.GetChild(0).localScale.x + 0.015f, transform1.GetChild(0).localScale.y + 0.015f, 1f);
//                }
//            }
//            if (obj.GetChild(j).gameObject.CompareTag("Iron"))
//            {
//                Transform transform2 = obj.GetChild(j);
//                for (int k = 0; k < transform2.childCount; k++)
//                {
//                    transform2.GetChild(k).localScale = new Vector3(transform1.GetChild(0).localScale.x / 2 + 0.01f, transform1.GetChild(0).localScale.y / 2 + 0.01f, 1f);
//                }
//            }
//        }
//    }
//    //28
//    public static void changeNewImage(Transform obj)
//    {
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            if (obj.GetChild(j).gameObject.tag == "square")
//            {
//                Transform transform = obj.GetChild(j);
//                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Story 1 - 2/bar/bar (2)");
//                transform.localPosition = new Vector3(0, -0.10810812f, 1f);
//                for (int i = transform.childCount - 1; i >= 0; i--)
//                {
//                    Transform transform1 = transform.GetChild(i);
//                    transform1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/UI Nut/export/Ingame (2)/Ingame/Hole");
//                    if (transform1.childCount > 0)
//                    {
//                        transform1.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UI Nut/export/Ingame (2)/Ingame/+ blue");
//                    }
//                }
//            }
//        }
//    }



//    [MenuItem("Services/18eliminateExtraHinge")]
//    public static void eliminateExtraHinge()
//    {
//        Transform obj = Selection.activeTransform;
//        Transform square = GameObject.FindGameObjectWithTag("square").transform;
//        for (int i = obj.childCount - 1; i >= 0; i--)
//        {
//            if (obj.GetChild(i).gameObject.tag == "Iron")
//            {
//                HingeJoint2D[] allComponents = obj.GetChild(i).GetComponents<HingeJoint2D>();
//                for (int j = 0; j < allComponents.Length; j++)
//                {
//                    if (allComponents[j].connectedBody == null)
//                    {
//                        HingeJoint2D hingeJoint2D = allComponents[j];
//                        UnityEngine.Object.DestroyImmediate(hingeJoint2D);
//                    }
//                }
//            }
//        }
//    }


//    [MenuItem("Component/fifthphase")]
//    public static void fifthphase()
//    {
//        Transform obj = Selection.activeTransform.parent;
//        if (obj == null)
//        {
//            Debug.LogWarning("No object selected.");
//            return;
//        }

//        ExecuteSequence4(obj);
//    }
//    private static void ExecuteSequence4(Transform obj)
//    {
//        //changeNewMaterialRebuild(obj);
//        EditorApplication.delayCall += () =>
//        {
//            changeSquareCollider(obj);
//            EditorApplication.delayCall += () =>
//            {
//                changeSquareLayer(obj);
//                EditorApplication.delayCall += () =>
//                {
//                    addtoStage(obj);
//                    EditorApplication.delayCall += () =>
//                    {
//                        //SetTextureCircle(obj);
//                        EditorApplication.delayCall += () =>
//                        {
//                            SetRadiusCollider(obj);
//                        };
//                    };
//                };
//            };
//        };
//    }
//    //29
//    [MenuItem("Component/changeNewMaterialRebuild")]
//    public static void changeNewMaterialRebuild()
//    {
//        Transform obj = Selection.activeTransform;
//        string IronLayer1 = "IronLayer1";
//        string IronLayer2 = "IronLayer2";
//        string IronLayer3 = "IronLayer3";
//        string IronLayer4 = "IronLayer4";
//        string IronLayer5 = "IronLayer5";
//        string IronLayer6 = "IronLayer6";
//        string IronLayer7 = "IronLayer7";
//        string IronLayer8 = "IronLayer8";
//        string IronLayer9 = "IronLayer9";
//        string layer1vs2vs3 = "layer10";
//        string layer1vs2vs3vs4 = "layer11";
//        string layer1vs2vs3vs4vs5 = "layer12";
//        string BothLayer = "BothLayer";
//        List<int> list = new List<int>();
//        int layerValue1 = LayerMask.NameToLayer(IronLayer1);
//        int layerValue2 = LayerMask.NameToLayer(IronLayer2);
//        int layerValue3 = LayerMask.NameToLayer(IronLayer3);
//        int layerValue4 = LayerMask.NameToLayer(IronLayer4);
//        int layerValue5 = LayerMask.NameToLayer(IronLayer5);
//        int layerValue6 = LayerMask.NameToLayer(IronLayer6);
//        int layerValue7 = LayerMask.NameToLayer(IronLayer7);
//        int layerValue8 = LayerMask.NameToLayer(IronLayer8);
//        int layerValue9 = LayerMask.NameToLayer(IronLayer9);
//        int layerValue10 = LayerMask.NameToLayer(layer1vs2vs3);
//        int layerValue11 = LayerMask.NameToLayer(layer1vs2vs3vs4);
//        int layerValue12 = LayerMask.NameToLayer(layer1vs2vs3vs4vs5);
//        int BothLayerValue = LayerMask.NameToLayer(BothLayer);
//        list.Add(layerValue1);
//        list.Add(layerValue2);
//        list.Add(layerValue3);
//        list.Add(layerValue4);
//        list.Add(layerValue5);
//        list.Add(layerValue6);
//        list.Add(layerValue7);
//        list.Add(layerValue8);
//        list.Add(layerValue9);
//        list.Add(layerValue10);
//        list.Add(layerValue11);
//        list.Add(layerValue12);
//        list.Add(BothLayerValue);
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            for (int i = 0; i < obj.GetChild(j).childCount; i++)
//            {
//                if (obj.GetChild(j).GetChild(i).gameObject.tag == "Iron")
//                {
//                    Transform transform = obj.GetChild(j).GetChild(i);
//                    for (int k = 0; k < list.Count; k++)
//                        if (transform.gameObject.layer == list[k])
//                        {
//                            switch (k)
//                            {
//                                case 0:
//                                    transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial/NewMaterial1 5");
//                                    break;
//                                case 1:
//                                    transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial/NewMaterial1");
//                                    break;
//                                case 2:
//                                    transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial/NewMaterial1 2");
//                                    break;
//                                case 3:
//                                    transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial/NewMaterial1 3");
//                                    break;
//                                case 4:
//                                    transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial/NewMaterial1 4");
//                                    break;
//                                case 5:
//                                    transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial/NewMaterial1 1");
//                                    break;
//                                case 6:
//                                    transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial/NewMaterial1 6");
//                                    break;
//                                case 7:
//                                    transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial/NewMaterial1 7");
//                                    break;
//                                case 8:
//                                    transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial/NewMaterial1 8");
//                                    break;
//                                case 9:
//                                    transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial/NewMaterial1 2");
//                                    break;
//                                case 10:
//                                    transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial/NewMaterial1 9");
//                                    break;
//                                case 11:
//                                    transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial/NewMaterial1 1");
//                                    break;
//                                case 12:
//                                    transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial/NewMaterial1 5");
//                                    break;
//                                default:
//                                    transform.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/NewMaterial/NewMaterial1 6");
//                                    break;
//                            }
//                        }
//                }
//            }
//        }
//    }
//    //30 
//    public static void changeSquareCollider(Transform obj)
//    {
//        string layerName = "Default";
//        int layerValue = LayerMask.NameToLayer(layerName);
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            for (int i = 0; i < obj.GetChild(j).childCount; i++)
//            {
//                if (obj.GetChild(j).GetChild(i).gameObject.tag == "square")
//                {
//                    Transform transform = obj.GetChild(j).GetChild(i);
//                    BoxCollider2D boxCollider2D = transform.GetComponent<BoxCollider2D>();
//                    //Object.DestroyImmediate(boxCollider2D);
//                    transform.AddComponent<BoxCollider2D>();
//                    transform.GetComponent<BoxCollider2D>().isTrigger = true;

//                    transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Story 1 - 2/bar/bar (2)");
//                }
//            }
//        }
//    }
//    //31
//    public static void changeSquareLayer(Transform obj)
//    {
//        string layerName = "Default";
//        int layerValue = LayerMask.NameToLayer(layerName);
//        for (int j = 0; j < obj.childCount; j++)
//        {

//            for (int i = 0; i < obj.GetChild(j).childCount; i++)
//            {
//                if (obj.GetChild(j).GetChild(i).gameObject.tag == "square")
//                {
//                    Transform transform = obj.GetChild(j).GetChild(i);
//                    transform.gameObject.layer = layerValue;

//                }
//            }


//        }
//    }
//    public static Transform findSquare(Transform transform)
//    {
//        for (int i = transform.childCount - 1; i >= 0; i--)
//        {
//            if (transform.GetChild(i).CompareTag("square"))
//            {
//                return transform.GetChild(i);
//            }
//        }
//        return null;
//    }


//    // Tool Rebuild map 2 
//    //32
//    public static void addtoStage(Transform obj)
//    {
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
//    //33
//    [MenuItem("Component/SetTextureCircle")]
//    public static void SetTextureCircle1()
//    {
//        Transform obj = Selection.activeTransform;
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            for (int i = 0; i < obj.GetChild(j).childCount; i++)
//            {
//                if (obj.GetChild(j).GetChild(i).gameObject.tag == "Iron")
//                {
//                    if (obj.GetChild(j).GetChild(i).transform.GetComponent<SpriteRenderer>().sprite.name == "Solid_Circle_1" || obj.GetChild(j).GetChild(i).transform.GetComponent<SpriteRenderer>().sprite.name == "Layer-10" || obj.GetChild(j).GetChild(i).transform.GetComponent<SpriteRenderer>().sprite.name == "Holed_Circle")
//                    {
//                        Collider2D polygonCollider2D = obj.GetChild(j).GetChild(i).transform.GetComponent<Collider2D>();
//                        DestroyImmediate(polygonCollider2D);
//                        obj.GetChild(j).GetChild(i).transform.AddComponent<CircleCollider2D>();
//                        CircleCollider2D circleCollider2D = obj.GetChild(j).GetChild(i).transform.GetComponent<CircleCollider2D>();
//                        circleCollider2D.sharedMaterial = Resources.Load<PhysicsMaterial2D>("PhysicMaterials/New Physics Material 2D");
//                    }
//                }

//            }
//        }
//    }
//    //34
//    public static void SetRadiusCollider(Transform obj)
//    {
//        for (int j = 0; j < obj.childCount; j++)
//        {
//            Transform transform1 = obj.GetChild(j);
//            for (int i = 0; i < obj.GetChild(j).childCount; i++)
//            {
//                if (obj.GetChild(j).GetChild(i).gameObject.tag == "square")
//                {
//                    Transform transform2 = obj.GetChild(j).GetChild(i).transform;
//                    var min = transform2.GetChild(0).GetComponent<CircleCollider2D>().radius;
//                    for (int k = 0; k < transform2.childCount; k++)
//                    {
//                        if (transform2.GetChild(k).GetComponent<CircleCollider2D>().radius < min)
//                        {
//                            float minRadius = transform2.GetChild(k).GetComponent<CircleCollider2D>().radius;
//                            ChangeRadiusCollider(transform2, minRadius);
//                        }
//                    }
//                }

//            }
//        }
//    }
//    public static void ChangeRadiusCollider(Transform transform, float minRadius)
//    {
//        for (int k = 0; k < transform.childCount; k++)
//        {
//            transform.GetChild(k).GetComponent<CircleCollider2D>().radius = minRadius;
//        }
//    }
//    //[MenuItem("Tool/rebuild2/testngu :)))")]
//    //public static void button25()
//    //{
//    //    Transform obj = Selection.activeTransform;
//    //    PictureUIManager pictureUIManager = obj.GetComponent<PictureUIManager>();
//    //    for (int i = 0; i < pictureUIManager.Stage.Length; i++)
//    //    {
//    //        for (int j = 0; j < pictureUIManager.Stage[i].ObjBtn.Length; j++)
//    //        {
//    //            GameObject originalObj = pictureUIManager.Stage[i].ObjBtn[j];
//    //            GameObject prefabObj = Resources.Load<GameObject>("ObjjPrefab/PicItemBT");

//    //            // Instantiate the prefab and copy the properties from the original object
//    //            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefabObj, originalObj.transform.parent);

//    //            newObj.transform.SetParent(originalObj.transform.parent, false);
//    //            CopyComponentValues(originalObj, newObj);
//    //            CoppyPos(originalObj, newObj);
//    //            setStarText(originalObj, newObj);
//    //            CoppyChildImg(originalObj.transform.GetChild(0).transform.gameObject, newObj.transform.GetChild(0).transform.gameObject);

//    //            // Update the reference in PictureUIManager
//    //            pictureUIManager.Stage[i].ObjBtn[j] = newObj;

//    //            // Optionally destroy the original object
//    //            GameObject.DestroyImmediate(originalObj);
//    //        }
//    //    }
//    //}    //[MenuItem("Tool/rebuild2/testngu :)))")]
//    //public static void button25()
//    //{
//    //    Transform obj = Selection.activeTransform;
//    //    PictureUIManager pictureUIManager = obj.GetComponent<PictureUIManager>();
//    //    for (int i = 0; i < pictureUIManager.Stage.Length; i++)
//    //    {
//    //        for (int j = 0; j < pictureUIManager.Stage[i].ObjBtn.Length; j++)
//    //        {
//    //            GameObject originalObj = pictureUIManager.Stage[i].ObjBtn[j];
//    //            GameObject prefabObj = Resources.Load<GameObject>("ObjjPrefab/PicItemBT");

//    //            // Instantiate the prefab and copy the properties from the original object
//    //            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefabObj, originalObj.transform.parent);

//    //            newObj.transform.SetParent(originalObj.transform.parent, false);
//    //            CopyComponentValues(originalObj, newObj);
//    //            CoppyPos(originalObj, newObj);
//    //            setStarText(originalObj, newObj);
//    //            CoppyChildImg(originalObj.transform.GetChild(0).transform.gameObject, newObj.transform.GetChild(0).transform.gameObject);

//    //            // Update the reference in PictureUIManager
//    //            pictureUIManager.Stage[i].ObjBtn[j] = newObj;

//    //            // Optionally destroy the original object
//    //            GameObject.DestroyImmediate(originalObj);
//    //        }
//    //    }
//    //}

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
////[MenuItem("Component/CreateHoleInIron")]
////public static void Button4()
////{
////	Transform obj = Selection.activeTransform;
////	for (int j = 0; j < obj.childCount; j++)
////	{
////		if (obj.GetChild(j).gameObject.name.Contains("HoleInSquare"))
////		{
////			Transform trs = obj.GetChild(i);
////			GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("ObjjPrefab/HoleInSquare"), obj);
////			a.transform.position = trs.position;
////			a.transform.rotation = trs.rotation;
////			a.transform.localScale = trs.localScale;
////			a.transform.SetSiblingIndex(trs.GetSiblingIndex());
////			Object.DestroyImmediate(trs.gameObject);
////			continue;
////		}
////	}
////}





//////if (obj.GetChild(i).gameObject.name.Contains("NNew_Invaider"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/NNew_Invaider"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_Saw-Idle"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_Saw-Idle"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_tonnel_l"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_tonnel_l"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obctacle_Airscrew"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obctacle_Airscrew"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_balls_tube"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_balls_tube"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_buffer cone"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_buffer cone"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_tyres"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_tyres"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_Wooden_Barrels"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_Wooden_Barrels"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Fence"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Fence"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_spike"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_Spike"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_water_barrel"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_water_barrel"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_Wooden_Barriers_Line"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_Wooden_Barriers_Line"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obctacle_SpikeCylinder"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obctacle_SpikeCylinder"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());

//////	a.transform.GetChild(1).SetLocalPositionAndRotation(trs.GetChild(1).localPosition, trs.GetChild(1).localRotation);

//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obctacle_Airscrew"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obctacle_Airscrew_1"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_SpikeBall_2"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_SpikeBall_2"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obctacle_STN_SpaceMill"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obctacle_STN_SpaceMill"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_STN_Asteroid"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_STN_Asteroid"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_STN_WreckingBox"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_STN_WreckingBox"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	a.transform.GetChild(1).SetLocalPositionAndRotation(trs.GetChild(1).localPosition, trs.GetChild(1).localRotation);
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obctacle_STN_SpaceShip_1"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obctacle_STN_SpaceShip_1"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_collumn"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_collumn"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_ColumnSpike_2"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_ColumnSpike_2"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_STN_CargoPress"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_STN_CargoPress"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obctacle_Sector"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obctacle_Sector"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	a.transform.GetChild(0).SetLocalPositionAndRotation(trs.GetChild(0).localPosition, trs.GetChild(0).localRotation);
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_STN_UFO "))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_STN_UFO_Idle"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_STN_UFO_Pendulum"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_STN_UFO_Pendulum"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	a.transform.GetChild(0).SetLocalPositionAndRotation(trs.GetChild(0).localPosition, trs.GetChild(0).localRotation);
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Wooden_Column"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Wooden_Column"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_Barrels"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_Barrels"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_Saw "))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_Saw"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_Saw_Move"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_Saw_Move"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("DTN_Obstacle_Cargo"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/DTN_Obstacle_Cargo"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obstacle_BarrerConcrete_2"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obstacle_BarrerConcrete_2"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("DTN_Obstacle_CargoGate"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/DTN_Obstacle_CargoGate"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("DTN_Obstacle_WreckingBal"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/DTN_Obstacle_WreckingBal"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("DTN_Obstacle_BarrelsAcross"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/DTN_Obstacle_BarrelsAcross"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("DTN_Obstacle_BarrelsAlong"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/DTN_Obstacle_BarrelsAlong"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("DTN_Obstacle_AirScrew"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/DTN_Obstacle_AirScrew"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("WreckingBall_obstacle"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/WreckingBall_obstacle"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	a.transform.GetChild(1).SetLocalPositionAndRotation(trs.GetChild(1).localPosition, trs.GetChild(1).localRotation);
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("Obctacle_Airscrew_double"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/Obctacle_Airscrew_double"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	a.transform.GetChild(0).SetLocalPositionAndRotation(trs.GetChild(0).localPosition, trs.GetChild(0).localRotation);
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

//////if (obj.GetChild(i).gameObject.name.Contains("pimple"))
//////{
//////	Transform trs = obj.GetChild(i);

//////	GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Obstacles/pimple"), obj);
//////	a.transform.position = trs.position;
//////	a.transform.rotation = trs.rotation;
//////	a.transform.localScale = trs.localScale;
//////	a.transform.SetSiblingIndex(trs.GetSiblingIndex());
//////	Object.DestroyImmediate(trs.gameObject);
//////	continue;
//////}

