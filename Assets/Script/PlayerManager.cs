using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void attack()
    {
        Debug.Log("Stabby Stabby");
    }

    public void defend()
    {
        Debug.Log("Blocky Blocky");
    }

    public void buff()
    {
        Debug.Log("POWER OVERFLOWING");
    }

    public void lifesteal()
    {
        Debug.Log("Your Soul is Mine!");
    }

}
