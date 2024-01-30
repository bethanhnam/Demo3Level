using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIManager instance;
    public GameObject BlockRayCast;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Block()
    {
        BlockRayCast.SetActive(true);
    }
    public void UnBlock()
    {
        BlockRayCast.SetActive(false);
    }
}
