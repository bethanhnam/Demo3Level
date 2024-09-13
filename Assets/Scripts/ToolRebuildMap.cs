//using DG.Tweening;
//using Sirenix.Utilities;
//using System.Collections.Generic;
//using System.Linq;
//using Unity.VisualScripting;
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

//public class ToolRebuildMap : Editor
//{
//    [MenuItem("Tools/test")]
//    public void test()
//    {
//        Transform obj1 = Selection.activeTransform;
//        LevelManagerNew levelManagerNew = obj1.transform.GetComponent<LevelManagerNew>();

//        for (int i = 0; i < levelManagerNew.stageList.Count; i++)
//        {
//            LevelStage levelStage = new LevelStage();
//            levelStage.Level = i + 1;
//            levelStage.Stage1 = levelManagerNew.stageList[i];

//            if (!levelManagerNew.stageList1.Contains(levelStage))
//            {
//                levelManagerNew.stageList1.Add(levelStage);
//            }
//        }
//    }
//}
