using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemPicture;

[CreateAssetMenu(menuName = "ScriptableObjects / Data Picture")]
public class DatatPictureScriptTableObject : ScriptableObject
{
	[SerializeField]
	private Stage1[] stage;
	[SerializeField]
	private PictureUIManager pictureUIManager;
	[SerializeField]
    private Rw[] presentA;

    public Stage1[] Stage { get => stage; }
	public PictureUIManager PictureUIManager { get => pictureUIManager; }
    public Rw[] PresentA { get => presentA; set => presentA = value; }
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
	private int star;
	[SerializeField]
	private Sprite sprItem;

	public int Id { get => id; }
	public int Star { get => star; }
	public Sprite SprItem { get => sprItem; }

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
	[Serializable]
	public class Rw
	{
		public string name;
		public int value;
		public type type;
		public Sprite rwSprite;
	}
	public enum type
	{
		gold,
		typea,
		typeb,
		typec
	}
}
