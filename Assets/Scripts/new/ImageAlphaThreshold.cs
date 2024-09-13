using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAlphaThreshold : MonoBehaviour
{
    [SerializeField] private Image theButton;
    [SerializeField][Range(0, 1)] private float threshold = 0.2f;
    // Use this for initialization
    void Start()
    {
        theButton.alphaHitTestMinimumThreshold = threshold;
    }
}
