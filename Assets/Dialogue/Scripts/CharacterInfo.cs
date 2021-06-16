using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
	[CreateAssetMenu(menuName = "Dialogue/CharacterInfo")]
	public class CharacterInfo : ScriptableObject
	{
		public Color color;
		public string Name;
		public int attitude = 50;
		public bool isHero;

		public void ChangeAttitude(int amount)
		{
			attitude = Mathf.Clamp(attitude + amount, 0, 100);
		}
	}
}