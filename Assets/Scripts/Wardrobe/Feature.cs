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
		return $"{characteristic} +{value}";
	}
}

public enum Characteristic { Hacking, Disguise }
