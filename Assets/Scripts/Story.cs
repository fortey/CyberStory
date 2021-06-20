using Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XNode;

public class Story : SceneGraph<DialogueGraph>
{
	public static Story instance;

	public GameObject Characters;
	public GameObject ChatUI;
	public Color ShadowColor;

	public GameObject NameLabel;
	public GameObject AttitudeLabel;
	public GameObject MessageLabel;
	public ChoiceButton Choice1;
	public ChoiceButton Choice2;
	public GameObject ContinueButton;

	private List<Dialogue.CharacterInfo> characters;
	private Dialogue.CharacterInfo currentCharacter;

	private void Awake()
	{
		if (!instance)
		{
			instance = this;
		}
	}

	public void StartStory()
	{
		graph.Restart();
		RefreshChat();
		Characters.SetActive(true);
		ChatUI.SetActive(true);
	}

	public void RefreshChat()
	{

		//if (graph.current is Chat)
		//Chat chat = graph.current as Chat;
		var character = graph.current.character;
		if (currentCharacter != character)
		{
			if (character)
				TurnOnOffCharacter(character, true);
			if (currentCharacter)
				TurnOnOffCharacter(currentCharacter, false);
			currentCharacter = character;
		}
		if (character)
		{
			NameLabel.GetComponentInChildren<Text>().text = character.Name;
			NameLabel.SetActive(true);

			AttitudeLabel.GetComponentInChildren<Text>().text = character.attitude.ToString();
			AttitudeLabel.SetActive(!character.isHero);
		}
		else
		{
			NameLabel.SetActive(false);
			AttitudeLabel.SetActive(false);
		}

		MessageLabel.GetComponentInChildren<Text>().text = graph.current.text;

#pragma warning disable CS0612 // Type or member is obsolete
		var answers = graph.current.answers;
#pragma warning restore CS0612 // Type or member is obsolete
		if (answers.Count > 0)
		{
			ShowChoice(answers[0], Choice1, 0);
			ContinueButton.SetActive(false);
		}
		else
		{
			Choice1.gameObject.SetActive(false);
			ContinueButton.SetActive(true);
		}

		if (answers.Count > 1)
		{
			ShowChoice(answers[1], Choice2, 1);
		}
		else
		{
			Choice2.gameObject.SetActive(false);
		}
	}

	void ShowChoice(Chat.Answer answer, ChoiceButton choice, int index)
	{
		choice.gameObject.SetActive(true);
		choice.Show(answer, () =>
		{
			graph.AnswerQuestion(index);
			RefreshChat();
		});
	}

	public void Continue()
	{
		graph.AnswerQuestion(0);
		RefreshChat();
	}

	public static void ContinueWithDelay(Func<IEnumerator> func)
	{
		instance.StartCoroutine(func());
	}

	public void InitializeCharacters(Dialogue.CharacterInfo[] characters)
	{
		this.characters = characters.ToList();
		currentCharacter = null;

		for (int i = 0; i < characters.Length; i++)
		{
			if (i >= Characters.transform.childCount)
				break;

			var charUI = Characters.transform.GetChild(i);
			if (characters[i])
			{
				charUI.gameObject.SetActive(true);
				var charImage = charUI.GetComponent<Image>();
				charImage.sprite = characters[i].sprite;
				charImage.color = ShadowColor;
			}
			else
				charUI.gameObject.SetActive(false);
		}
	}

	public void TurnOnOffCharacter(Dialogue.CharacterInfo character, bool on)
	{
		var index = characters.IndexOf(character);
		if (index >= 0 && index < Characters.transform.childCount)
		{
			var charUI = Characters.transform.GetChild(index);
			var startColor = on ? ShadowColor : Color.white;
			var endColor = on ? Color.white : ShadowColor;
			StartCoroutine(ChangeColor(startColor, endColor, charUI.GetComponent<Image>()));
		}
	}

	public IEnumerator ChangeColor(Color startColor, Color endColor, Image charImage)
	{
		var time = 1f;
		var scale = 1 / time;
		while (time > 0f)
		{
			yield return new WaitForEndOfFrame();
			time -= Time.deltaTime;
			charImage.color = Color.Lerp(endColor, startColor, time * scale);
		}
	}
}
