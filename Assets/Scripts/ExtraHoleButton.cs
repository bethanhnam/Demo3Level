using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraHoleButton : MonoBehaviour
{
	// Thêm một biến public để kéo và thả nút vào trong Inspector
	public GameObject extraHole;
	public Button myButton;

	void Start()
	{
		myButton = this.GetComponent<Button>();
		// Gắn sự kiện cho nút khi bắt đầu game
		myButton.onClick.AddListener(MyFunction);
	}

	public void MyFunction()
	{
		UIManager.instance.gamePlayPanel.extraHolePanel.Open();
	}
}
