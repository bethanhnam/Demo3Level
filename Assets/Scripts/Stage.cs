using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public GameObject holeToUnlock;
	public GameObject pointer1;
	public GameObject pointer2;
	public GameObject pointer;
	public GameObject[] pointerPositions;
	private bool hasSpawn = false;
	private bool hasSpawn1 = false;
	public Sprite itemImg;
	public string itemName;
	public GameObject item;
	public GameObject Square;
	// Start is called before the first frame update
	void Start()
	{
		
		item.transform.position = Vector3.zero;
	}
	public void ChangeSize(Sprite sprite)
	{
		float height = 0;
		Vector2 size = sprite.rect.size;
		if (size.x > 500 || size.y > 500)
		{
			if (size.x > size.y)
			{
				height = (size.x * 2 / 3) / size.x;
				item.transform.localScale = new Vector3(item.transform.localScale.x * height, item.transform.localScale.y, 1);
			}
			else
			{
				height = (size.y * 2 / 3) / size.y;
				item.transform.localScale = new Vector3(item.transform.localScale.x, item.transform.localScale.y * height, 1);
			}
		}
		else
		{
			if (size.x > 300 || size.y > 300)
			{
				if (size.x > size.y)
				{
					height = size.y / 500;
					item.transform.localScale = new Vector3(item.transform.localScale.x * height, item.transform.localScale.y * height, 1);
				}
				else
				{
					height = size.x / 500;
					item.transform.localScale = new Vector3(item.transform.localScale.x * height, item.transform.localScale.y * height, 1);
				}
			}
			else
			{
				if (size.x > size.y)
				{
					height = 350 / size.x;
					item.transform.localScale = new Vector3(item.transform.localScale.x * height, item.transform.localScale.y * height, 1);
				}
				else
				{
					height = 350 / size.y;
					item.transform.localScale = new Vector3(item.transform.localScale.x * height, item.transform.localScale.y * height, 1);
				}
			}
		}
		// Lấy sprite renderer từ game object
		SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();

		// Lấy màu hiện tại của sprite
		Color color = spriteRenderer.color;

		// Thiết lập độ trong suốt mới cho màu (ở đây giả sử là 50% độ trong suốt)
		color.a = 0.5f;

		// Gán màu mới cho sprite renderer
		spriteRenderer.color = color;

		
	}

	private void Awake()
	{
		item.GetComponent<SpriteRenderer>().sprite = Level.instance.Item.itemImg;
		itemImg = Level.instance.Item.itemImg;
		itemName = Level.instance.Item.itemName;
	}

	// Update is called once per frame
	void Update()
    {
		if (GameManager.instance.currentLevel == 0)
		{

			if (InputManager.instance.selectedNail == null)
			{
				if (this.pointer !=null)
				{
					if (!hasSpawn)
					{
						pointer1 = Instantiate(pointer, pointerPositions[0].transform.position, Quaternion.identity, this.transform);
						hasSpawn = true;
					}
				}
			}
			else
			{
				if (this.pointer != null)
					if (!hasSpawn1)
				{
					Destroy(pointer1);
					Instantiate(pointer, pointerPositions[1].transform.position, Quaternion.identity, this.transform);
					hasSpawn1 = true;
				}
			}
		}
	}
	public Vector3 returnScale()
	{
		float targetAspect = 9.0f / 16.0f;
		float windowAspect = (float)Screen.width / (float)Screen.height;
		return (item.transform.localScale * (targetAspect/windowAspect));
	}
}
