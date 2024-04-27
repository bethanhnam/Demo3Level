//using DG.Tweening;
//using DG.Tweening.Core.Easing;
//using JetBrains.Annotations;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[RequireComponent(typeof(CanvasGroup))]
//public class extrascreen : MonoBehaviour
//{
//	public static extrascreen Instance;
//	public GameObject[] targetPieces;
//	public int completePieces = 0;
//	public CanvasGroup canvasGroup;
//	public SpriteRenderer boardSprite;
//	public ParticleSystem[] particle;
//	public GameObject particles;
//	public GameObject pickUpEffect1;
//	Vector2 Ray;

//	private void Start()
//	{
//		Instance = this; 
//		canvasGroup = GetComponent<CanvasGroup>();
		
//		pickUpEffect1 = Resources.Load<GameObject>("UseablePartical/StarsYellow");
//	}
//	private void Update() 
//	{
//		Ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//		if (Input.GetMouseButtonDown(0))
//		{
//			var pickUpeffect1 = Instantiate(pickUpEffect1, Ray, Quaternion.identity,this.transform);
//			Destroy(pickUpeffect1, 2f);
//		}
//	}
//	public void CheckLevel()
//	{
//		if (completePieces == targetPieces.Length)
//		{
//			Color color;
//			if (ColorUtility.TryParseHtmlString("#58FF00", out color))
//			{
//				// Gán màu cho SpriteRenderer
//				boardSprite.color = color;
//			}
//			else
//			{
//				Debug.LogError("Invalid color format!"); // Xử lý trường hợp mã màu không hợp lệ
//			}
//			particle[0].Play();
//			particle[1].Play();
//			particle[2].Play();
//			AudioManager.instance.PlaySFX("Shining");
//			particles.transform.DOScale(new Vector3(1f, 1f, 1), 2f).OnComplete(() =>
//			{
//				Invoke("NextLevel", 3f);
//			});
//		}
//	}
//	private void NextLevel()
//	{
//		Level.instance.CheckLevel();
//	}
//}
