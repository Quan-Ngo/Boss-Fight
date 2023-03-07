using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
	
    public Animator animator;
    [SerializeField] private BuffManager BuffManager;

    [SerializeField] private int MaxHealth = 10;
    private int Health = 10;

    //Player Statistics
    [SerializeField] private int AP = 5;
    [SerializeField] private int Block = 0;
    [SerializeField] private int Damage = 5;
    [SerializeField] private float Lifesteal = 0f;
	[SerializeField] private int Defense = 4;

    //Player Temp Statistics
    private int TempDamage = 0;
    private int TempDefense;
    private float TempLifesteal = 0f;

    //Player UI Elements
    [SerializeField] private Text APText;
    [SerializeField] private HealthBar HealthBar;
	
	private bool animationLock;

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

        HealthBar.SetMaxHealth(Health);
        Debug.Log("Block =" + Block.ToString());
		animationLock = false;
    }

    public void turnStart()
    {
        AP = 5;
        APText.text = ("AP: " + AP.ToString());

        BuffManager.updateBuffs();

        // Debug Statements
        Debug.Log("Block = " + Block.ToString());
    }

    public void attack()
    {
        if (AP >= 1 && !animationLock) {
            updateAP(-1);
			playAnimationClip("attack");
        }
        else
        {
            Debug.Log("Not Enough AP");
        }
    }
	
	public void dealDamage()
	{
		int damageDealt;
		damageDealt = Damage + TempDamage;
		
		if (damageDealt < 0)
		{
			damageDealt = 0;
		}
		
		BossManager.Instance.takeDamage(damageDealt);
		Debug.Log("I attacked for " + (damageDealt).ToString() + " Damage");

		if(Lifesteal + TempLifesteal > 0)
		{
			int DmgToHP = (int) ( (damageDealt) * (Lifesteal + TempLifesteal) );
			heal(DmgToHP);
		}
	}
		
    public void defend()
    {
        if (AP >= 2 && !animationLock)
        {
            updateAP(-2);
            Block += Defense + TempDefense; //Temporary Value for Block
            Debug.Log("I gained " + Block.ToString() + " Block");
        }
        else
        {
            Debug.Log("Not Enough AP");
        }
    }

    public void buff()
    {
        if (AP >= 3 && !animationLock)
        {
            updateAP(-3);
			playAnimationClip("buff");
            Buff DamageBuff5 = new Buff("DamageBuff5" ,Type.Buff, Stats.Damage, 5, -1, 1);
            BuffManager.addBuff(DamageBuff5);
            Debug.Log("I Gained a Damage Buff of " + DamageBuff5.buffValue.Value);
        }
        else
        {
            Debug.Log("Not Enough AP");
        }
    }

    public void lifesteal()
    {
        if (AP >= 3 && !animationLock)
        {
            updateAP(-3);
			playAnimationClip("buff");
            Buff LifestealBuff50 = new Buff("LifestealBuff50", Type.Buff, Stats.Lifesteal, 50, 3, -1);
            BuffManager.addBuff(LifestealBuff50);
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
		animator.SetTrigger("GetHit");
        if (Block >= Damage)
        {
            Block -= Damage;
        }
        else
        {
            Damage -= Block;
            Block = 0;
            Health -= Damage;
            HealthBar.SetHealth(Health);
        }
		
		if (Health <= 0)
		{
			GameStateManager.instance.playerDied();
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

        HealthBar.SetHealth(Health);
    }

    public void addBuff(Buff buff)
    {
        BuffManager.addBuff(buff);
    }

    public void removeBuff(Buff buff)
    {
        BuffManager.removeBuff(buff);
    }

    public void addStatBuff(Buff buff)
    {
        switch (buff.buffValue.Stat)
        {
            case Stats.Block:
                TempDefense += buff.buffValue.Value * buff.Stacks;
                break;
            case Stats.Damage:
                TempDamage += buff.buffValue.Value * buff.Stacks;
                break;
            case Stats.Lifesteal:
                TempLifesteal += ((float) (buff.buffValue.Value)) / 100f;
                break;
        }
    }

    public void removeStatBuff(Buff buff)
    {
        switch (buff.buffValue.Stat)
        {
            case Stats.Block:
                TempDefense -= buff.buffValue.Value * buff.Stacks;
                break;
            case Stats.Damage:
                TempDamage -= buff.buffValue.Value * buff.Stacks;
                break;
            case Stats.Lifesteal:
                TempLifesteal -= ((float)(buff.buffValue.Value)) / 100f;
                break;
        }
    }

    public void endAnimationLock()
	{
		animationLock = false;
	}
	
	private void playAnimationClip(string animationToPlay)
	{
		animationLock = true;
		
		switch (animationToPlay)
		{
			case "attack":
				animator.SetTrigger("Attack");
				break;
			case "buff":
				animator.SetTrigger("Buff");
				break;
		}
	}
}
