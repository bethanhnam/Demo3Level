using EnhancedScrollerDemos.CellEvents;
using EnhancedScrollerDemos.GridSimulation;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollItem : MonoBehaviour {

    public GameObject smallPack;
    public GameObject bigPack;

    public void SetData(int startingIndex) {
        if (startingIndex % 2 == 0)
        {
            smallPack.gameObject.SetActive(false);
            bigPack.gameObject.SetActive(true);
        }
        else
        {
            smallPack.gameObject.SetActive(true);
            bigPack.gameObject.SetActive(false);
        }
    }
}

