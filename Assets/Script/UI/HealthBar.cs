using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider Health;
    public Text healthText;

    [SerializeField] private Image FillColor;
    [SerializeField] private Sprite[] Fills;

    [SerializeField] private GameObject ShieldDisplay;
    [SerializeField] private Text BlockText;

    private void Start()
    {
        ShieldDisplay.SetActive(false);
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

	private void setHealthText()
	{
		healthText.text = Health.value + "/" + Health.maxValue;
    }

    public void SetBlock(int block)
    {
        if(block <= 0)
        {
            BlockText.text = "0";
            ShieldDisplay.SetActive(false);
            FillColor.sprite = Fills[0];
        }
        else
        {
			ShieldDisplay.SetActive(true);
			ShieldDisplay.GetComponent<Animator>().SetTrigger("Appear");
            BlockText.text = "" + block.ToString();
            FillColor.sprite = Fills[1];
        }
    }


}
