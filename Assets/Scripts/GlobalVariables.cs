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
	#endregion

	void Awake()
	{
		if (!instance)
			instance = this;
		cristals = PlayerPrefs.GetInt("Cristals", cristals);
		energy = PlayerPrefs.GetInt("Energy", energy);
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
				break;
			case ItemType.Neck:
				Neck = item;
				break;
			case ItemType.Middle:
				Middle = item;
				break;
			case ItemType.Bottom:
				Bottom = item;
				break;
			default:
				break;
		}
	}
}
