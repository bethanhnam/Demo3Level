using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackGroundFooter : MonoBehaviour
{

    public Animator Animator;
    
    string appear = "BackGroundFooter";
    string disappear = "BackGroundFooterDisappear";

    public GameObject footer;
    public GameObject upper;

    // Start is called before the first frame update
    [Button("Show")]
    public void ShowBackGroundFooter(bool hasUpper)
    {
        this.gameObject.SetActive(true);
        if (hasUpper) {
            upper.SetActive(true);
        }
        else
        {
            upper.SetActive(false);
        }
        FooterScaleForDevices();
        Animator.enabled = true;
        Animator.Play(appear,0,0);
    }
    [Button("Disapper")]
    public void DisappearBackGroundFooter()
    {
        
        Animator.Play(disappear,0,0);
    }
    public void deactive()
    {
        Animator.enabled = false;
        if (upper.gameObject.activeSelf)
        {
            upper.SetActive(false);
        }
    }
    public void FooterScaleForDevices()
    {
        float targetAspect = 9.0f / 17.0f;
        float windowAspect = (float)Screen.width / (float)Screen.height;

        if (windowAspect < targetAspect)
        {
            footer.transform.localScale = footer.transform.localScale * (targetAspect / windowAspect);
            upper.transform.localScale = upper.transform.localScale * (targetAspect / windowAspect);

            footer.transform.GetChild(0).transform.localScale = footer.transform.GetChild(0).transform.localScale / (targetAspect / windowAspect);

            Vector3 bottomCenter = new Vector3(Screen.width / 2, 0, 0);
            Vector3 worldBottomCenter = Camera.main.ScreenToWorldPoint(bottomCenter);

            footer.transform.position =  new Vector3(0,worldBottomCenter.y + footer.transform.localScale.y /2,1);
            upper.transform.position =  new Vector3(0,worldBottomCenter.y + footer.transform.localScale.y /2,1);
        }
    }
}
