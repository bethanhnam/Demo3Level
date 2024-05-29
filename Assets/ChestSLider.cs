using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class ChestSLider : MonoBehaviour
{
    public Slider mySlider;
    public GameObject present;
    //public TextMeshProUGUI strikeScore;
    //public TextMeshProUGUI maxValue;
    public GameObject defauPos;
    public Marker markerPrefab;
    public GameObject markerCoverPrefab;
    public GameObject markerCoverPoint;
    public List<Marker> markers = new List<Marker>();
    public List<GameObject> markerCovers = new List<GameObject>();
    public Transform startPos;
    public Transform endPos;
    public Marker starMarker;
    public int maxValue1;
    public int currentValue;

    private void Start()
    {
    }
    public void ChangeValue(Action action)
    {
        AudioManager.instance.PlaySFX("FillUpSlider");
        present.GetComponent<Animator>().SetBool("Shaking", true);
        mySlider.DOValue(mySlider.value + 1, 0.7f).OnComplete(() =>
        {
            currentValue += 1;
            present.GetComponent<Animator>().SetBool("Shaking", false);
            //strikeScore.text = (Mathf.Round(mySlider.value)).ToString();
            UIManagerNew.Instance.ButtonMennuManager.ActiveCVGroup();
            action();
            if (mySlider.value >= mySlider.maxValue)
            {
                SetSliderValue();
            }
            //ChangeSlider();
            changeMarkerImgAtPoint((int)mySlider.value);
        });


    }
    public void SetMaxValue(PictureUIManager pictureUIManager)
    {
        int max = 0;
        for (int i = 0; i < pictureUIManager.Stage.Length; i++)
        {
            max += pictureUIManager.Stage[i].ObjLock.Length;
        }
        mySlider.maxValue = 4;
        maxValue1 = max;

        //maxValue.text = mySlider.maxValue.ToString();
    }
    public void SetCurrentValue(int value)
    {
        currentValue = value;
        //strikeScore.text = (Mathf.Round(mySlider.value)).ToString();

    }
    public void returnPos()
    {
        present.transform.position = defauPos.transform.position;
    }
    public void CreateMarker()
    {
        //for(int i = 0; i < markers.Count; i++)
        //{
        //	markers[i].SetText(currentValue + i);
        //}
        var distance = endPos.position.x - startPos.position.x;
        Debug.Log(distance);
        for (int i = 1; i < 4; i++)
        {
            var marPos = distance / 4 * i;
            Debug.Log(marPos);
            var markerCover = Instantiate(markerCoverPrefab, new Vector2(startPos.position.x + marPos, startPos.position.y), Quaternion.identity, markerCoverPoint.transform);
            markerCovers.Add(markerCover);
        }
        SetSliderValue();
        //ChangeSlider();
        changeMarkerImg((int)mySlider.value);

    }
    public void changeMarkerImg(int max)
    {
        if ((mySlider.value ==4))
        {
            for (int i = 0; i < max; i++)
            {
                markers[i].greenMarker.gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i <= max; i++)
            {
                markers[i].greenMarker.gameObject.SetActive(true);
            }
        }
       
    }
    public void changeMarkerImgAtPoint(int max)
    {
        markers[max].greenMarker.gameObject.SetActive(true);
    }
    //public void ChangeSlider()
    //{
    //    if (maxValue1 > 4)
    //    {
    //        if (mySlider.value >= mySlider.maxValue)
    //        {
    //            if (currentValue + 4 > maxValue1)
    //            {
    //                markers[0].SetText(maxValue1 - 3);
    //                markers[1].TurnOffGreenMarker(maxValue1 - 2);
    //                markers[2].TurnOffGreenMarker(maxValue1 - 1);
    //                markers[3].TurnOffGreenMarker(maxValue1 - 0);
    //            }
    //            else
    //            {
    //                mySlider.value = 0;
    //                markers[0].SetText(currentValue);
    //                for (int i = 1; i < markers.Count; i++)
    //                {
    //                    markers[i].TurnOffGreenMarker(currentValue + i);
    //                }
    //            }
    //        }
    //    }
    //}
    public void SetSliderValue()
    {
        var x = currentValue % 4;
        mySlider.value = x;
        if (currentValue + 4 > maxValue1)
        {
            markers[0].SetText(maxValue1 - 3);
            markers[1].TurnOffGreenMarker(maxValue1 - 2);
            markers[2].TurnOffGreenMarker(maxValue1 - 1);
            markers[3].TurnOffGreenMarker(maxValue1 - 0);
            mySlider.value = 4 - (maxValue1 - currentValue);
            changeMarkerImg(4 - (maxValue1 - currentValue));
        }
        else
        {
            for (int i = 0; i < markers.Count; i++)
            {
                markers[i].TurnOffGreenMarker(currentValue - x + i);
            }
        }
    }
}