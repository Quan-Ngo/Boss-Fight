using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private int MaxHealth = 10;

    //Player Statistics
    [SerializeField] private int AP = 5;
    [SerializeField] private int Block = 0;
    [SerializeField] private int Damage = 5;
    [SerializeField] private int Health = 10;
    [SerializeField] private float Lifesteal = 0f;
	[SerializeField] private int blockGainOnAbility = 4;

    //Player Temp Statistics
    [SerializeField] private int TempDamage = 0;
    [SerializeField] private int TempBlock;
    private float TempLifesteal = 0f;

    //Player UI Elements
    [SerializeField] private Text APText;

    Dictionary<string, Buff> PlayerBuffs = new Dictionary<string, Buff>();
    private List<Buff> ExpiredBuffs = new List<Buff>();


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

    public void turnStart()
    {
        AP = 5;
        APText.text = ("AP: " + AP.ToString());

        updateBuffs();

        // Debug Statements
        Debug.Log("Health =" + Health.ToString());
        Debug.Log("Block = " + Block.ToString());

        foreach (KeyValuePair<string, Buff> buffs in PlayerBuffs)
        {
            Debug.Log("Current Buffs: " + buffs.Value.Name);
        }
    }

    public void attack()
    {
		int damageDealt;
		damageDealt = Damage + TempDamage;
		
		if (damageDealt < 0)
		{
			damageDealt = 0;
		}
		
        if (AP >= 1) {
            updateAP(-1);
            BossManager.Instance.takeDamage(damageDealt);
            Debug.Log("I attacked for " + (damageDealt).ToString() + " Damage");

            if(Lifesteal + TempLifesteal > 0)
            {
                int DmgToHP = (int) ( (damageDealt) * (Lifesteal + TempLifesteal) );
                heal(DmgToHP);
            }
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
            Block = blockGainOnAbility + TempBlock; //Temporary Value for Block
            Debug.Log("I gained " + Block.ToString() + " Block");
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
            Debug.Log("I Gained a Damage Buff of " + DamageBuff5.buffValue.Value);
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
            Debug.Log("I Gained a Lifesteal Buff of " + LifestealBuff50.buffValue.Value + "%");
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

    public void heal(int amount)
    {
        if (Health + amount >= MaxHealth)
        {
            Health = MaxHealth;
        }
        else
        {
            Health += amount;
        }
    }

    private void updateBuffs()
    {
        foreach (KeyValuePair<string, Buff> buff in PlayerBuffs)
        {
            if (buff.Value.Duration > 1)
            {
                buff.Value.Duration -= 1;
            }
            else if (buff.Value.Duration >= 0)
            {
                ExpiredBuffs.Add(buff.Value);
            }
        }

        removeBuff(ExpiredBuffs);
        foreach (Buff buff in ExpiredBuffs)
        {
            Debug.Log("Expired Buffs: " + buff.Name);
        }
    }

    public void addBuff(Buff buff)
    {
        if (PlayerBuffs.ContainsKey(buff.Name))
        {
            if (buff.Stacks >= 1)
            {
                PlayerBuffs[buff.Name].Stacks += buff.Stacks;
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
            case Stats.Block:
                TempBlock += buff.buffValue.Value * buff.Stacks;
                break;
            case Stats.Damage:
                TempDamage += buff.buffValue.Value * buff.Stacks;
                break;
            case Stats.Lifesteal:
                TempLifesteal += ((float) (buff.buffValue.Value)) / 100f;
                break;
        }
    }

    public void removeBuff(List<Buff> buffs)
    {
        foreach(Buff ExpiredBuff in buffs)
        {
            if (PlayerBuffs.ContainsKey(ExpiredBuff.Name))
            {
                if (PlayerBuffs[ExpiredBuff.Name].Stacks > 1)
                {
                    PlayerBuffs[ExpiredBuff.Name].Stacks -= 1;
                    removeBuffFromStat(ExpiredBuff);

                }
                else if (PlayerBuffs[ExpiredBuff.Name].Stacks <= 1)
                {
                    PlayerBuffs.Remove(ExpiredBuff.Name);
                    removeBuffFromStat(ExpiredBuff);
                }
            }
        }
    }

    private void removeBuffFromStat(Buff buff)
    {
        switch (buff.buffValue.Stat)
        {
            case Stats.Block:
                TempBlock -= buff.buffValue.Value;
                break;
            case Stats.Damage:
                TempDamage -= buff.buffValue.Value;
                break;
            case Stats.Lifesteal:
                TempLifesteal -= ((float)(buff.buffValue.Value)) / 100f;
                break;
        }
    }
}
