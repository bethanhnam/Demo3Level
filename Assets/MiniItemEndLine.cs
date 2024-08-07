using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniItemEndLine : MonoBehaviour
{
    public static MiniItemEndLine instance;
    private void Start()
    {
        instance = this;
    }
}
