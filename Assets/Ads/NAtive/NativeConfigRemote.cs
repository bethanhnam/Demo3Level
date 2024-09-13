using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeConfigRemote 
{
    [SerializeField]
    private bool isEnable;
    [SerializeField]
    private float timeDestroy;
    [SerializeField]
    private List<string> listId;

    public bool IsEnable { get => isEnable; set => isEnable = value; }
    public float TimeDestroy { get => timeDestroy; set => timeDestroy = value; }
    public List<string> ListId { get => listId; set => listId = value; }
}
