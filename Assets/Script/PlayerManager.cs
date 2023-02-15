using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private int AP = 5;
    [SerializeField] private Text APText;

    private int Health = 10;

    private int Block = 0;

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

        Debug.Log("Health =" + Health.ToString());
        Debug.Log("Block =" + Block.ToString());
    }

    public void takeDamage(int Damage)
    {
        if (Block >=  Damage)
        {
            Block -= Damage;
        }
        else
        {
            Damage -= Block;
            Block = 0;
            Health -= Damage;
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
            Block += 10; //Temporary Value for Block
            Debug.Log("Block =" + Block.ToString());
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
            BuffDebuffManager.instance.applyBuffDebuffToPlayer(BuffAndDebuff.DAMAGEUP);
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
            BuffDebuffManager.instance.applyBuffDebuffToPlayer(BuffAndDebuff.LIFESTEAL);
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

    public void turnStart()
    {
        AP = 5;
        Block = 0;

        Debug.Log("Health =" + Health.ToString());
        Debug.Log("Block =" + Block.ToString());
    }

}
