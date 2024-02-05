using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    public Image Off;
    public Image Active;
    public Image Check;
    public bool isClaim = false;
	public int purpleStar;
	public int GoldenStar;
    
	private void Update()
	{
        if (!isClaim)
        {
			Active.gameObject.SetActive(false);
			Off.gameObject.SetActive(true);
			Check.gameObject.SetActive(false);
		}
        else
        {
			Active.gameObject.SetActive(true);
			Off.gameObject.SetActive(false);
			Check.gameObject.SetActive(true);
		}
	}
}
