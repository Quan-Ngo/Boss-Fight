using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    //Player Statistics
    private int AP = 5;
    private int Block = 0;
    private int Damage = 5;
    private int Health = 10;
    private int Lifesteal = 0;

    //Player Temp Statistics
    private int TempAP = 0;  
    private int TempDamage = 0;
    private int TempLifesteal = 0;

    //Player UI Elements
    [SerializeField] private Text APText;

    Dictionary<string, Buff> PlayerBuffs = new Dictionary<string, Buff>();


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

    public void attack()
    {
        if (AP >= 1) {
            updateAP(-1);
            Debug.Log("I attacked for " + (Damage + TempDamage).ToString() + " Damage");
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
            Buff DamageBuff5 = new Buff("DamageBuff5" ,Type.Buff, Stats.Damage, 5, -1, 1);
            addBuff(DamageBuff5);
            Debug.Log("" + TempDamage.ToString());
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
            Buff LifestealBuff50 = new Buff("LifestealBuff50", Type.Buff, Stats.Lifesteal, 50, 3, -1);
            addBuff(LifestealBuff50);
            Debug.Log("" + TempLifesteal.ToString());
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
        APText.text = ("AP: " + AP.ToString());

        
        
        // Debug Statements
        Debug.Log("Health =" + Health.ToString());
        Debug.Log("Block = " + Block.ToString());

        foreach (KeyValuePair<string, Buff> buff in PlayerBuffs)
        {
            Debug.Log(buff.Key);
        }
    }

    public void takeDamage(int Damage)
    {
        if (Block >= Damage)
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

    public void addBuff(Buff buff)
    {
        if (PlayerBuffs.ContainsKey(buff.Name))
        {
            if (buff.Stacks >= 1)
            {
                PlayerBuffs[buff.Name].Stacks += 1;
                addBuffToStat(buff);
            }
            else
            {
                PlayerBuffs[buff.Name].Duration = buff.Duration;
            }
        }
        else
        {
            PlayerBuffs.Add(buff.Name, buff);
            addBuffToStat(buff);
        }
    }

    private void addBuffToStat(Buff buff)
    {
        switch (buff.buffValue.Stat)
        {
            case Stats.Damage:
                TempDamage += buff.buffValue.Value;
                break;
            case Stats.Lifesteal:
                TempLifesteal += buff.buffValue.Value;
                break;
        }
    }
}
