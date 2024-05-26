using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    public Image greenMarker;
    public Image completePoint;
    public TextMeshProUGUI levelText;

    public void SetText(int level)
    {
        levelText.text = level.ToString();
    }
    public void TurnOnGreenMarker()
    {
        greenMarker.gameObject.SetActive(true);
    }
    public void TurnOffGreenMarker(int level)
    {
        greenMarker.gameObject.SetActive(false);
        SetText(level);
    }
}
