using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FixListPanel : MonoBehaviour
{

    public TextMeshProUGUI title;

    public Slider progressSlider;
    public TextMeshProUGUI valueText; 

    public ScrollRect ScrollRect;
    public GameObject fixObjectPrefab;

    public List<GameObject> fixList = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetDisplay()
    {
        //valueText.text = 
    }

    public void SpawnFixObj()
    {

    }

}
