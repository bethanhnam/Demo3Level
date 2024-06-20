using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLevelStoryPic : MonoBehaviour
{
    public static DataLevelStoryPic instance;
    public int picIndex;
    public List<GameObject> listJson;
    private void Start()
    {
       if(instance == null)
        {
            instance = this;
        }
    }
}
