using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider Health;
    public Text healthText;
	
    //public Slider Block;

    private void Start()
    {
        //Block.value = 0;
    }

    public void SetMaxHealth(int health)
    {
        Health.maxValue = health;
        Health.value = health;
		setHealthText();
    }

    public void SetHealth(int health)
    {
        Health.value = health;
		setHealthText();
    }

	void setHealthText()
	{
		healthText.text = Health.value + "/" + Health.maxValue;
    }


}
