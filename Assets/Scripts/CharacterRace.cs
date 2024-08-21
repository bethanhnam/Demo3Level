using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterRace : MonoBehaviour
{
    public int topIndex = 0;
    public TextMeshProUGUI topText;

    public void changeText(int topIndex)
    {
        topText.text = topIndex.ToString();
    }
}
