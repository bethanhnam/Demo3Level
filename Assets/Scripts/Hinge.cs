using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hinge : MonoBehaviour
{
    public HingeJoint2D hinge;


    public void OffHinge()
    {

        if(hinge.enabled == true)
        {
            hinge.enabled = false;
        }
        this.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
