using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Feature
{
	public Characteristic characteristic;
	public int value;

	public override string ToString()
	{
		return $"{characteristicsNames[characteristic]} +{value}";
	}

	public static Dictionary<Characteristic, string> characteristicsNames = new Dictionary<Characteristic, string> {
		{ Characteristic.Disguise, "����������" },
		{ Characteristic.Hacking, "�����" }
	};
}

public enum Characteristic { Hacking, Disguise }
