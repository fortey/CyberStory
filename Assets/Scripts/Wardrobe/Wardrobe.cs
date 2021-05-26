using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Wardrobe : MonoBehaviour
{
	[Header("Шмотки на персе")]
	public Image Head;
	public Image Neck;
	public Image Middle;
	public Image Bottom;

	[Header("Кнопки категорий")]
	public Image HeadCategory;
	public Image NeckCategory;
	public Image MiddleCategory;
	public Image BottomCategory;

	[Space]
	public Image DescImage;
	public Text ItemName;
	public Text ItemDescription;

	public Button NextButton;
	public Button PreviousButton;

	private ItemType activeCategory;
	private Item[] items;
	private int itemIndex;
	private Dictionary<ItemType, Image> categoryImages;
	private Dictionary<ItemType, Image> categoryButtons;

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

		categoryButtons = new Dictionary<ItemType, Image>();
		categoryButtons.Add(ItemType.Head, HeadCategory);
		categoryButtons.Add(ItemType.Neck, NeckCategory);
		categoryButtons.Add(ItemType.Middle, MiddleCategory);
		categoryButtons.Add(ItemType.Bottom, BottomCategory);

		ActivateCategory(ItemType.Head);
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
		if (activeCategory != category || items == null)
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
		UpdateSwitchButtons();
		UpdateCategoryButtons();
	}

	private void ShowDescription(Item item)
	{
		ItemName.text = item.itemName;
		ItemDescription.text = item.Description();
		DescImage.sprite = item.sprite;
	}

	private void UpdateSwitchButtons()
	{
		NextButton.interactable = itemIndex + 1 < items.Length;
		PreviousButton.interactable = itemIndex > 0;
	}

	private void UpdateCategoryButtons()
	{
		foreach (var c in categoryButtons)
		{
			if (c.Key == activeCategory)
				c.Value.color = new Color(1f, 0.8f, 1f);
			else
				c.Value.color = Color.white;
		}
	}

	#region Button handlers
	public void NextItem()
	{
		if (itemIndex + 1 < items.Length)
		{
			itemIndex++;
			PutOn(items[itemIndex]);
		}
		UpdateSwitchButtons();
	}

	public void PreviousItem()
	{
		if (itemIndex > 0)
		{
			itemIndex--;
			PutOn(items[itemIndex]);
		}
		UpdateSwitchButtons();
	}

	public void ActivateHead()
	{
		ActivateCategory(ItemType.Head);
	}
	public void ActivateNeck()
	{
		ActivateCategory(ItemType.Neck);
	}
	public void ActivateMiddle()
	{
		ActivateCategory(ItemType.Middle);
	}
	public void ActivateBottom()
	{
		ActivateCategory(ItemType.Bottom);
	}
	#endregion
}
