using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wardrobe/Item")]
public class Item : ScriptableObject
{
	public ItemType type;
	public string itemName;
	public Sprite sprite;
	public Feature[] features;

	public string Description()
	{
		var desc = "";
		foreach (var f in features)
		{
			desc += f.ToString() + "/n";
		}
		return desc;
	}
}

public enum ItemType { Head, Neck, Middle, Bottom }