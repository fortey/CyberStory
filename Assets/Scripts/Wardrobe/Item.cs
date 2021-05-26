using System;
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
		for (int i = 0; i < features.Length - 1; i++)
		{
			desc += features[i].ToString() + Environment.NewLine;
		}
		if (features.Length > 0)
			desc += features[features.Length - 1].ToString();
		return desc;
	}
}

public enum ItemType { Head, Neck, Middle, Bottom }