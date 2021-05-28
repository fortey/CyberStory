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
	private readonly ItemType[] categories = new ItemType[] { ItemType.Head, ItemType.Neck, ItemType.Middle, ItemType.Bottom };

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

		categoryImages = new Dictionary<ItemType, Image>
		{
			{ ItemType.Head, Head },
			{ ItemType.Neck, Neck },
			{ ItemType.Middle, Middle },
			{ ItemType.Bottom, Bottom }
		};

		categoryButtons = new Dictionary<ItemType, Image>
		{
			{ ItemType.Head, HeadCategory },
			{ ItemType.Neck, NeckCategory },
			{ ItemType.Middle, MiddleCategory },
			{ ItemType.Bottom, BottomCategory }
		};

		ActivateCategory(ItemType.Head);

		SwipeDetection.SwipeEvent += OnSwipe;
	}

	public void PutOn(Item item)
	{
		var oldItem = GlobalVariables.instance.GetItem(item.type);
		if (oldItem)
			PullOff(oldItem);
		GlobalVariables.instance.SetItem(item);
		categoryImages[item.type].sprite = item.sprite;
		GlobalVariables.instance.ApplyItem(item);
		ShowDescription(item);
	}

	public void PullOff(Item item)
	{
		GlobalVariables.instance.ApplyItem(item, false);
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

	public void OnSwipe(Vector2 direction)
	{
		if (direction == Vector2.right)
			NextItem();
		else if (direction == Vector2.left)
			PreviousItem();
		else
		{
			var catIndex = 0;
			for (int i = 0; i < categories.Length; i++)
			{
				if (activeCategory == categories[i])
				{
					catIndex = i;
					break;
				}
			}
			if (direction == Vector2.up && catIndex > 0)
				ActivateCategory(categories[catIndex - 1]);
			else if (direction == Vector2.down && catIndex < categories.Length - 1)
				ActivateCategory(categories[catIndex + 1]);
		}
	}

	private void OnDestroy()
	{
		SwipeDetection.SwipeEvent -= OnSwipe;
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
