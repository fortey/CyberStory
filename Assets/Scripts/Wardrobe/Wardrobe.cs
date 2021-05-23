using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Wardrobe : MonoBehaviour
{
	public Image Head;
	public Image Neck;
	public Image Middle;
	public Image Bottom;

	public Image DescImage;
	public Text ItemName;
	public Text ItemDescription;

	private ItemType activeCategory;
	private Item[] items;
	private int itemIndex;
	private Dictionary<ItemType, Image> categoryImages;

	void Start()
	{
		if (GlobalVariables.instance.Head)
			Head.sprite = GlobalVariables.instance.Head.sprite;
		if (GlobalVariables.instance.Neck)
			Neck.sprite = GlobalVariables.instance.Neck.sprite;
		if (GlobalVariables.instance.Middle)
			Middle.sprite = GlobalVariables.instance.Middle.sprite;
		if (GlobalVariables.instance.Bottom)
			Bottom.sprite = GlobalVariables.instance.Bottom.sprite;

		categoryImages = new Dictionary<ItemType, Image>();
		categoryImages.Add(ItemType.Head, Head);
		categoryImages.Add(ItemType.Neck, Neck);
		categoryImages.Add(ItemType.Middle, Middle);
		categoryImages.Add(ItemType.Bottom, Bottom);
	}

	public void PutOn(Item item)
	{
		var oldItem = GlobalVariables.instance.GetItem(item.type);
		if (oldItem)
			PullOff(item);
		GlobalVariables.instance.SetItem(item);
		categoryImages[item.type].sprite = item.sprite;
		ApplyItem(item);
		ShowDescription(item);
	}

	public void PullOff(Item item)
	{
		ApplyItem(item, false);
	}

	public void ApplyItem(Item item, bool isDonnig = true)
	{
		foreach (var feature in item.features)
		{
			switch (feature.characteristic)
			{
				case Characteristic.Hacking:
					GlobalVariables.instance.Hacking += (isDonnig ? 1 : -1) * feature.value;
					break;
				case Characteristic.Disguise:
					GlobalVariables.instance.Disguise += (isDonnig ? 1 : -1) * feature.value;
					break;
				default:
					break;
			}
		}
	}

	public void ActivateCategory(ItemType category)
	{
		if (activeCategory != category)
		{
			activeCategory = category;
			items = GlobalVariables.instance.items.Where(i => i.type == category).ToArray();
			var item = GlobalVariables.instance.GetItem(category);
			for (int i = 0; i < items.Length; i++)
			{
				if (item == items[i])
				{
					itemIndex = i;
					break;
				}
			}
			ShowDescription(item);
		}
	}

	private void ShowDescription(Item item)
	{
		var desc = "";
		foreach (var feature in item.features)
		{
			desc += feature.ToString() + "/n";
		}

		ItemName.text = item.itemName;
		ItemDescription.text = desc;
		DescImage.sprite = item.sprite;
	}
}
