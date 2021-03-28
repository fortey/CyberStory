using Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    public Text text;
    public Text cost;
    public GameObject costObj;



    public void Show(Chat.Answer answer, UnityAction onClick)
    {
        text.text = answer.text;
        var button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(onClick);

        if (answer.cost > 0)
        {
            costObj.SetActive(true);
            cost.text = $"-{answer.cost}";
            button.interactable = GlobalVariables.instance.Cristals >= answer.cost;
            button.onClick.AddListener(() => GlobalVariables.instance.Cristals -= answer.cost);
        }
        else
        {
            costObj.SetActive(false);
            button.interactable = true;
        }
    }
}
