using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class draggingPiece : MonoBehaviour
{
	private bool isDragging = false;
	private Vector3 offset;
	[SerializeField] private GameObject target;
	[SerializeField] private Sprite targetSprite;
	[SerializeField] private Sprite defaultSprite;
	[SerializeField] private Vector3 defaultPosition;
	private bool complete = false;

	private void Start()
	{
		defaultPosition = this.transform.position;
	}
	private void OnMouseDown()
	{
		isDragging = true;
		offset = gameObject.transform.position - GetMouseWorldPosition();
	}
	private void Update()
	{
		
	}
	public void checkHitPoint(Vector2 piecePosition)
	{
		float radius = 0.7f;
		float reference = radius;
		float distance = Vector2.Distance(piecePosition, target.transform.position);
		if (distance < reference)
		{
			this.transform.position = target.transform.position;
			this.GetComponent<Collider2D>().enabled = false;
			complete = true;
			extrascreen.Instance.completePieces++;
			extrascreen.Instance.CheckLevel();
		}
		else
		{
			this.GetComponent<SpriteRenderer>().sprite = defaultSprite;
			//this.GetComponent<Image>().SetNativeSize();
			this.transform.position =defaultPosition;
		}
	}
	private void OnMouseUp()
	{
		isDragging = false;
		if (!complete)
			checkHitPoint(this.transform.position);
	}

	private void OnMouseDrag()
	{
		if (isDragging)
		{
			transform.position = GetMouseWorldPosition() + offset;
			this.GetComponent<SpriteRenderer>().sprite = targetSprite;
			//this.GetComponent<Image>().SetNativeSize();
			
		}
	}

	private Vector3 GetMouseWorldPosition()
	{
		Vector3 mousePoint = Input.mousePosition;
		mousePoint.z = Camera.main.transform.position.z;
		return Camera.main.ScreenToWorldPoint(mousePoint);
	}
}
