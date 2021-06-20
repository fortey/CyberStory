using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationManager : MonoBehaviour
{
	public static LocationManager instance;

	public Image BG;
	public GameObject Blackout;

	private void Awake()
	{
		if (!instance)
			instance = this;
	}

	public static void ChangeLocation(Location location)
	{
		instance.StartCoroutine(instance.Change(location.sprite));
	}

	public IEnumerator Change(Sprite sprite)
	{
		Blackout.SetActive(true);
		var BOImage = Blackout.GetComponent<Image>();
		var time = 0.5f;
		var scale = 1 / time;
		while (time > 0f)
		{
			yield return new WaitForEndOfFrame();
			time -= Time.deltaTime;
			var color = BOImage.color;
			BOImage.color = new Color(color.r, color.g, color.b, 1 - Mathf.Clamp(time * scale, 0f, 1f));
		}

		BG.sprite = sprite;

		time = 0.5f;
		scale = 1 / time;
		while (time > 0f)
		{
			yield return new WaitForEndOfFrame();
			time -= Time.deltaTime;
			var color = BOImage.color;
			BOImage.color = new Color(color.r, color.g, color.b, Mathf.Clamp(time * scale, 0f, 1f));
		}

		Blackout.SetActive(false);
	}
}
