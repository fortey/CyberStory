using Dialogue;
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
        NameLabel.GetComponentInChildren<Text>().text = character.Name;
        AttitudeLabel.GetComponentInChildren<Text>().text = character.attitude.ToString();

        MessageLabel.GetComponentInChildren<Text>().text = graph.current.text;

        var answers = graph.current.answers;
        if (answers.Count > 0)
        {
            //Choice1.SetActive(true);
            //Choice1.GetComponentInChildren<Text>().text = answers[0].text;

            //var button = Choice1.GetComponent<Button>();
            //button.onClick.RemoveAllListeners();
            //button.onClick.AddListener(() =>
            //{
            //    graph.AnswerQuestion(0);
            //    RefreshChat();
            //});
            ShowChoice(answers[0], Choice1, 0);
        }
        else
        {
            Choice1.gameObject.SetActive(false);
        }

        if (answers.Count > 1)
        {
            //Choice2.SetActive(true);
            //Choice2.GetComponentInChildren<Text>().text = answers[1].text;

            //var button = Choice2.GetComponent<Button>();
            //button.onClick.RemoveAllListeners();
            //button.onClick.AddListener(() =>
            //{
            //    graph.AnswerQuestion(1);
            //    RefreshChat();
            //});
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

        //if (answer.cost > 0)
        //{

        //}

        //var button = choice.GetComponent<Button>();
        //button.onClick.RemoveAllListeners();
        //button.onClick.AddListener(() =>
        //{
        //    graph.AnswerQuestion(index);
        //    RefreshChat();
        //});
    }
}
