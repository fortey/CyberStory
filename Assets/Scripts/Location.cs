using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Story/Location")]
public class Location : ScriptableObject
{
	public Sprite sprite;
	public void SwitchLocation()
	{
		LocationManager.ChangeLocation(this);
	}
}
