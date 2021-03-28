using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text CristalLabel;

    public Text EnergyLabel;
    public Text EnergyTimer;

    public GameObject StartLevelPanel;
    public GameObject StoryScreen;

    public double TimeForEnergy = 300;
    private DateTime energyEnd = DateTime.MinValue;

    private Stack<GameObject> activeScreens = new Stack<GameObject>();

    void Start()
    {
        GlobalVariables.instance.OnChangeCristals += RefreshCristals;
        GlobalVariables.instance.OnChangeEnergy += RefreshEnergy;

        var savedTime = PlayerPrefs.GetString("EnergyTimer", "");
        if (savedTime != "")
        {
            var endTime = DateTime.Parse(savedTime);
            var rest = endTime - DateTime.UtcNow;
            var totalSeconds = rest.TotalSeconds;
            if (totalSeconds >= 0 || GlobalVariables.instance.Energy + 1 == GlobalVariables.instance.EnergyMax)
                StartCoroutine(StartEnergyTimer(rest.TotalSeconds));
            else
            {
                var energyCount = GlobalVariables.instance.Energy + 1;
                var newEnergy = (int)(-totalSeconds / TimeForEnergy);
                energyCount += newEnergy;
                if (energyCount < GlobalVariables.instance.EnergyMax)
                {
                    var restOdSeconds = -totalSeconds % TimeForEnergy;
                    StartCoroutine(StartEnergyTimer(restOdSeconds));
                }
                GlobalVariables.instance.Energy = energyCount;
            }
        }

        RefreshCristals(GlobalVariables.instance.Cristals);
        RefreshEnergy(GlobalVariables.instance.Energy);

        activeScreens.Push(StartLevelPanel);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshCristals(int amount)
    {
        CristalLabel.text = amount.ToString();
    }

    public void RefreshEnergy(int amount)
    {
        EnergyLabel.text = $"{amount}/{GlobalVariables.instance.EnergyMax}";
        if (GlobalVariables.instance.Energy != GlobalVariables.instance.EnergyMax && energyEnd == DateTime.MinValue)
        {

            StartCoroutine(StartEnergyTimer(TimeForEnergy));
            PlayerPrefs.SetString("EnergyTimer", energyEnd.ToString());
        }

        if (GlobalVariables.instance.Energy == GlobalVariables.instance.EnergyMax && energyEnd != DateTime.MinValue)
        {
            StopEnergyTimer();
            PlayerPrefs.SetString("EnergyTimer", "");
        }
    }

    private void OnDestroy()
    {
        GlobalVariables.instance.OnChangeCristals -= RefreshCristals;
        GlobalVariables.instance.OnChangeEnergy -= RefreshEnergy;
    }

    public void StartLevel()
    {
        if (GlobalVariables.instance.Energy > 0)
        {
            GlobalVariables.instance.Energy--;
            SwitchActiveScreen(StoryScreen);
            Story.instance.StartStory();
        }
    }

    public IEnumerator StartEnergyTimer(double seconds)
    {
        EnergyTimer.gameObject.SetActive(true);

        energyEnd = DateTime.UtcNow.AddSeconds(seconds);
        var rest = energyEnd - DateTime.UtcNow;
        while (rest.TotalSeconds > 0)
        {
            EnergyTimer.text = rest.ToString(@"mm\:ss");
            yield return new WaitForSeconds(0.2f);
            rest = energyEnd - DateTime.UtcNow;
        }

        StopEnergyTimer();
        GlobalVariables.instance.Energy++;
    }

    public void StopEnergyTimer()
    {
        energyEnd = DateTime.MinValue;
        EnergyTimer.gameObject.SetActive(false);
    }

    public void StartPuzzle()
    {
        //FindObjectOfType<Draw>().gameObject.SetActive(true);
    }

    public void SwitchActiveScreen(GameObject screen)
    {
        if (activeScreens.Peek() == screen)
        {
            screen.SetActive(false);
            activeScreens.Pop();
            var prevScreen = activeScreens.Peek();
            prevScreen.SetActive(true);
        }
        else
        {
            var prevScreen = activeScreens.Peek();
            prevScreen.SetActive(false);
            activeScreens.Push(screen);
            screen.SetActive(true);
        }
    }
}
