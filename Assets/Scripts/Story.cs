using Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;

public class Story : SceneGraph<DialogueGraph>
{
	public static Story instance;

	public GameObject Characters;
	public GameObject ChatUI;

	public GameObject NameLabel;
	public GameObject AttitudeLabel;
	public GameObject MessageLabel;
	public ChoiceButton Choice1;
	public ChoiceButton Choice2;
	public GameObject ContinueButton;

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
}
