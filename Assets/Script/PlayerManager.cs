using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public int AP = 5;
    [SerializeField] private Text APText;

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
        if (AP >= 1) {
            updateAP(-1);
            Debug.Log("Stabby Stabby");
        }
        else
        {
            Debug.Log("Not Enough AP");
        }
    }

    public void defend()
    {
        if (AP >= 2)
        {
            updateAP(-2);
            Debug.Log("Blocky Blocky");
        }
        else
        {
            Debug.Log("Not Enough AP");
        }
    }

    public void buff()
    {
        if (AP >= 3)
        {
            updateAP(-3);
            Debug.Log("POWER OVERFLOWING");
        }
        else
        {
            Debug.Log("Not Enough AP");
        }
    }

    public void lifesteal()
    {
        if (AP >= 3)
        {
            updateAP(-3);
            Debug.Log("Your Soul is Mine!");
        }
        else
        {
            Debug.Log("Not Enough AP");
        }
    }

    private void updateAP(int amount)
    {
        AP += amount;
        APText.text = ("AP: " + AP.ToString());
    }

    public void refreshAP()
    {
        AP = 5;
    }

}
