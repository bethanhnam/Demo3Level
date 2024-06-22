using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryGamePlayLevel : MonoBehaviour
{
   public static StoryGamePlayLevel Instance;   
    public List<Stage> stageList = new List<Stage>();
    private void Start()
    {
         if (Instance == null)
        {
            Instance = this;
        }
    }
}