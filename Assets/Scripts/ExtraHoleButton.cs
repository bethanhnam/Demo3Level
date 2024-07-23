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

	void Start()
	{
	}

	public void MyFunction()
	{
		GamePlayPanelUIManager.Instance.OpenExtraHolePanel();
	}
}
