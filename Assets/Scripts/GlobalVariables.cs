using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
	public static GlobalVariables instance;

	public event Action<int> OnChangeCristals;
	public event Action<int> OnChangeEnergy;

	[SerializeField]
	private int cristals = 100;
	public int Cristals
	{
		get => cristals;
		set
		{
			cristals = value;
			if (OnChangeCristals != null)
				OnChangeCristals.Invoke(cristals);
			PlayerPrefs.SetInt("Cristals", cristals);
		}
	}

	public int EnergyMax = 5;

	[SerializeField]
	private int energy = 5;
	public int Energy
	{
		get => energy;
		set
		{
			energy = Mathf.Clamp(value, 0, EnergyMax);
			if (OnChangeEnergy != null)
				OnChangeEnergy.Invoke(energy);
			PlayerPrefs.SetInt("Energy", energy);
		}
	}

	#region Items
	public Item Head, Neck, Middle, Bottom;
	public int Hacking, Disguise;
	public Item[] items;
	public Item[] allItems;
	#endregion

	void Awake()
	{
		if (!instance)
			instance = this;
		cristals = PlayerPrefs.GetInt("Cristals", cristals);
		energy = PlayerPrefs.GetInt("Energy", energy);
		LoadItems();
		PutOnItems();
	}

	public Item GetItem(ItemType type)
	{
		Item item;
		switch (type)
		{
			case ItemType.Head:
				item = Head;
				break;
			case ItemType.Neck:
				item = Neck;
				break;
			case ItemType.Middle:
				item = Middle;
				break;
			case ItemType.Bottom:
				item = Bottom;
				break;
			default:
				item = null;
				break;
		}
		return item;
	}

	public void SetItem(Item item)
	{
		switch (item.type)
		{
			case ItemType.Head:
				Head = item;
				PlayerPrefs.SetString("Head", Head.name);
				break;
			case ItemType.Neck:
				Neck = item;
				PlayerPrefs.SetString("Neck", Neck.name);
				break;
			case ItemType.Middle:
				Middle = item;
				PlayerPrefs.SetString("Middle", Middle.name);
				break;
			case ItemType.Bottom:
				Bottom = item;
				PlayerPrefs.SetString("Bottom", Bottom.name);
				break;
			default:
				break;
		}
	}

	private void LoadItems()
	{
		var itemDict = new Dictionary<string, Item>();
		foreach (var item in allItems)
		{
			itemDict.Add(item.name, item);
		}
		if (PlayerPrefs.HasKey("Items"))
		{

			var itemsJSON = PlayerPrefs.GetString("Items");
			var itemsData = JsonUtility.FromJson<ItemsData>(itemsJSON);

			items = new Item[itemsData.items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				if (itemDict.ContainsKey(itemsData.items[i]))
					items[i] = itemDict[itemsData.items[i]];
			}
		}
		if (PlayerPrefs.HasKey("Head"))
		{
			var itemName = PlayerPrefs.GetString("Head");
			if (itemDict.ContainsKey(itemName))
				Head = itemDict[itemName];
		}
		if (PlayerPrefs.HasKey("Neck"))
		{
			var itemName = PlayerPrefs.GetString("Neck");
			if (itemDict.ContainsKey(itemName))
				Neck = itemDict[itemName];
		}
		if (PlayerPrefs.HasKey("Middle"))
		{
			var itemName = PlayerPrefs.GetString("Middle");
			if (itemDict.ContainsKey(itemName))
				Middle = itemDict[itemName];
		}
		if (PlayerPrefs.HasKey("Bottom"))
		{
			var itemName = PlayerPrefs.GetString("Bottom");
			if (itemDict.ContainsKey(itemName))
				Bottom = itemDict[itemName];
		}
	}

	private void SaveItems()
	{
		var itemsData = new ItemsData();
		itemsData.items = new string[items.Length];
		for (int i = 0; i < items.Length; i++)
		{
			itemsData.items[i] = items[i].name;
		}
		var json = JsonUtility.ToJson(itemsData);
		PlayerPrefs.SetString("Items", json);
	}
	public void ApplyItem(Item item, bool isDonning = true)
	{
		foreach (var feature in item.features)
		{
			switch (feature.characteristic)
			{
				case Characteristic.Hacking:
					GlobalVariables.instance.Hacking += (isDonning ? 1 : -1) * feature.value;
					break;
				case Characteristic.Disguise:
					GlobalVariables.instance.Disguise += (isDonning ? 1 : -1) * feature.value;
					break;
				default:
					break;
			}
		}
	}

	private void PutOnItems()
	{
		if (Head)
			ApplyItem(Head);
		if (Neck)
			ApplyItem(Neck);
		if (Middle)
			ApplyItem(Middle);
		if (Bottom)
			ApplyItem(Bottom);
	}
}

public class ItemsData
{
	public string[] items;
}
