﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraHoleButton : MonoBehaviour
{
	// Thêm một biến public để kéo và thả nút vào trong Inspector
	public GameObject extraHole;
	public Button myButton;
	public Animator myAnimator;
	public Vector3 myScale;
	public Image addImage;
	public ParticleSystem shinningParticle;
	void Start()
	{
		myScale = addImage.transform.localScale + new Vector3(0.1f,0.1f,0.1f);
	}

	public void MyFunction()
	{
		try { 
			GamePlayPanelUIManager.Instance.OpenExtraHolePanel();
		}
		catch { }
	}
	public void ScaleUp()
	{
        var scaleUp = myScale + new Vector3(0.1f, 0.1f, 0.1f);
        addImage.transform.DOScale(scaleUp, 0.5f);
    }
    public void ScaleDown()
    {
		var scaleDown = myScale - new Vector3(0.1f, 0.1f, 0.1f);
        addImage.transform.DOScale(scaleDown, 0.5f).OnComplete(() =>
		{
            //shinningParticle.Play();
        });
    }
    public void NomalScale()
    {
        addImage.transform.DOScale(myScale, 0.5f);
    }
}
