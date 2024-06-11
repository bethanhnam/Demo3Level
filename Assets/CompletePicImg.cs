using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CompletePicImg : MonoBehaviour
{
    public Image completeImg;
    public GameObject continueBT;
    public GameObject congratText;
    void Start()
    {

    }


    void Update()
    {

    }
    public void changeSize()
    {
        float targetAspect = 9.0f / 16.0f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        if (LevelManagerNew.Instance.LevelBase.Level != 0)
        {
            continueBT.transform.localScale = Vector3.one - new Vector3(0.216998f, 0.216998f, 0);
            congratText.transform.localScale = Vector3.one - new Vector3(0.216998f, 0.216998f, 0);
        }
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
