using DG.Tweening.Core.Easing;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class extrascreen : MonoBehaviour
{
	public static extrascreen Instance;
	public GameObject[] targetPieces;
	public int completePieces = 0;
	public CanvasGroup canvasGroup;
	public SpriteRenderer boardSprite;
	
	private void Start()
	{
		Instance = this; 
		canvasGroup = GetComponent<CanvasGroup>();
	}
	public void CheckLevel()
	{
		if (completePieces == targetPieces.Length)
		{
			Color color;
			if (ColorUtility.TryParseHtmlString("#58FF00", out color))
			{
				// Gán màu cho SpriteRenderer
				boardSprite.color = color;
			}
			else
			{
				Debug.LogError("Invalid color format!"); // Xử lý trường hợp mã màu không hợp lệ
			}
			Invoke("NextLevel",1f);
		}
	}
	private void NextLevel()
	{
		Level.instance.CheckLevel();
	}
}
