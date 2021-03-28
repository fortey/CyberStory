using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables instance;

    public event Action<int> OnChangeCristals;
    public event Action<int> OnChangeEnergy;

    [SerializeField]
    private int cristals = 100;
    public int Cristals
    {
        get => cristals;
        set
        {
            cristals = value;
            if (OnChangeCristals != null)
                OnChangeCristals.Invoke(cristals);
            PlayerPrefs.SetInt("Cristals", cristals);
        }
    }

    public int EnergyMax = 5;

    [SerializeField]
    private int energy = 5;
    public int Energy
    {
        get => energy;
        set
        {
            energy = Mathf.Clamp(value,0, EnergyMax);
            if (OnChangeEnergy != null)
                OnChangeEnergy.Invoke(energy);
            PlayerPrefs.SetInt("Energy", energy);
        }
    }

    void Awake()
    {
        if (!instance)
            instance = this;
        cristals = PlayerPrefs.GetInt("Cristals", cristals);
        energy = PlayerPrefs.GetInt("Energy", energy);
    }


}
