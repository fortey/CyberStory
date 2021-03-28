using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetScreen : MonoBehaviour
{
    private Button button;
    private Text buttonText;

    public double TimeForReturn = 600;
    private DateTime endTime;
    private TimeSpan rest;
    private bool isReady;

    void Start()
    {
        button = GetComponentInChildren<Button>();
        buttonText = button.GetComponentInChildren<Text>();

        var savedTime = PlayerPrefs.GetString("PetReturnTime", "");
        if (savedTime != "")
            endTime = DateTime.Parse(savedTime);
        else
        {
            endTime = DateTime.MinValue;
            button.onClick.AddListener(StartPet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (endTime != DateTime.MinValue)
        {
            if (!isReady)
            {
                rest = endTime - DateTime.UtcNow;
                if (rest.TotalSeconds > 0)
                {
                    buttonText.text = $"вернется с разведки через {rest:hh\\:mm\\:ss}";
                }
                else
                {
                    buttonText.text = "забрать добычу";
                    isReady = true;
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(PickUpReward);
                }
            }
        }
    }

    public void StartPet()
    {
        endTime = DateTime.UtcNow.AddSeconds(TimeForReturn);
        PlayerPrefs.SetString("PetReturnTime", endTime.ToString());
        button.onClick.RemoveAllListeners();
    }

    public void StopPet()
    {
        endTime = DateTime.MinValue;
        PlayerPrefs.SetString("PetReturnTime", "");

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(StartPet);
    }

    public void PickUpReward()
    {
        buttonText.text = "отправить на разведку";
        isReady = false;
        StopPet();
        GlobalVariables.instance.Energy++;
    }
}
