using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CompletePicImg : MonoBehaviour
{
    public Image completeImg;
    void Start()
    {

    }


    void Update()
    {

    }
    public void changeColor()
    {
        if(this.gameObject.activeSelf == false)
        {
            this.gameObject.SetActive(true);
        }
        completeImg.material.SetFloat("_Add", 0.5f);
        completeImg.material.SetFloat("_Mul", 0.5f);

        completeImg.material.DOFloat(1, "_Add", 2f);
        completeImg.material.DOFloat(1, "_Mul", 2f);
    }
    public void Disablepic()
    {
        this.gameObject.SetActive(false);
    }
}
