using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class draggingPiece : MonoBehaviour
{
	public bool isDragging = false;
	private Vector3 offset;
	public GameObject target;
	public Sprite targetSprite;
	public Sprite defaultSprite;
	public Vector3 defaultPosition;
	public bool complete = false;
	public bool hasTutol = false;
	public bool hasPointer = false;
	public bool canCreate = true;

	private void Start()
	{
		defaultPosition = this.transform.position;
		canCreate = true;
	}
	private void OnMouseDown()
	{
		isDragging = true;
		offset = gameObject.transform.position - GetMouseWorldPosition();
		if (hasTutol)
		{
			{
				canCreate = false;
				PointerTutorial.Instance.Pointer.SetActive(true);
				 PointerTutorial.Instance.Pointer.transform.DOMove(target.transform.position, 1f, false).OnComplete(() =>
				{
					canCreate = true;
				});
			}
		}
	}
	private void Update()
	{
		if (hasTutol)
		{
			if (!complete)
			{
				if (!hasPointer)
				{
					StartCoroutine(CreatePointer());
				}
			}
			else
			{
				PointerTutorial.Instance.Pointer.SetActive(false);
				hasTutol = false;
				
			}
		}
		if(complete)
		{
			PointerTutorial.Instance.Pointer.SetActive(false);
			this.GetComponent<SpriteRenderer>().sortingOrder = 1;
		}
	}
	IEnumerator CreatePointer()
	{
		yield return new WaitForSeconds(1f);
		if (canCreate)
		{
			PointerTutorial.Instance.Pointer.SetActive(true);
			PointerTutorial.Instance.Pointer.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1f, 1f);
			hasPointer = true;
		}
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
			if (hasTutol) {
				hasPointer = false;
				PointerTutorial.Instance.Pointer.transform.DOKill();
				PointerTutorial.Instance.Pointer.SetActive(false);
				PointerTutorial.Instance.Pointer.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y +1f,1f);
			}

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
			this.GetComponent<SpriteRenderer>().sortingOrder = 10;
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
