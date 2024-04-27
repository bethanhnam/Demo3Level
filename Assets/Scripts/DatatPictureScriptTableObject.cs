using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects / Data Picture")]
public class DatatPictureScriptTableObject : ScriptableObject
{
	[SerializeField]
	private Stage1[] stage;
	[SerializeField]
	private PictureUIManager pictureUIManager;

	public Stage1[] Stage { get => stage; }
	public PictureUIManager PictureUIManager { get => pictureUIManager; }
}

[Serializable]
public class Stage1 {
	[SerializeField]
	private ItemPicture[] item;
	[SerializeField]
	private Sprite completeimg;
	public ItemPicture[] Item { get => item;  }
	public Sprite Completeimg { get => completeimg; set => completeimg = value; }
}

[Serializable]
public class ItemPicture : IComparable<ItemPicture>
{
	[SerializeField]
	private int id;
	[SerializeField]
	private Stage level;
	[SerializeField]
	private Sprite sprItem;

	public int Id { get => id; }
	public Stage Level { get => level; }
	public Sprite SprItem { get => sprItem;  }

	public int CompareTo(ItemPicture other)
	{
		if (other.id > id)
		{
			return -1;
		}
		else
		{
			if (other.id < id)
			{
				return 1;
			}
			else
				return 0;
		}
	}
}
