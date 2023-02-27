using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//TODO: change player script to add buff based on stack instead of just 1

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;
	public float phaseOneThreshholdPercent;
	public float phaseTwoThreshholdPercent;
	public float phaseThreeThreshholdPercent;
	public int maxBossHealth;
	public int enrageTurn;
	public TMP_Text HPDisplay;
	public TMP_Text dmgDisplay;
	public Animator animator;
	
	
	[SerializeField] private int bossHealth;
	[SerializeField] private int bossPhase;
	[SerializeField] private int baseDamage;
	[SerializeField] private bool phaseTransition;
	[SerializeField] private bool enraged;
	
	Dictionary<string, Buff> BossBuffs = new Dictionary<string, Buff>();
	
	
	void Start()
	{
		bossPhase = 0;
		bossHealth = maxBossHealth;
		phaseTransition = false;
		HPDisplay.text = "HP: " + bossHealth;
		dmgDisplay.text = "Damage: " + baseDamage;
	}
	
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

    public void turnStart()
    {
		if (!enraged && TurnManager.Instance.getTurnCount() >= enrageTurn)
		{
			enraged = true;
		}
		
		if (!enraged)
		{
			if (phaseTransition == true)
			{
				changePhase();
				phaseTransition = false;
			}
			
			switch (bossPhase)
			{
				case 0:
					phaseZeroAI();
					break;
				case 1:
					phaseOneAI();
					break;
				case 2:
					phaseTwoAI();
					break;
				case 3:
					phaseThreeAI();
					break;
			}
		}
		else
		{
			buffSelfDamage(5);
			attackPlayer();
		}
		TurnManager.Instance.changeTurn();
    }

	void phaseZeroAI()
	{
		attackPlayer();
	}
	
	void phaseOneAI()
	{
		switch (Random.Range(0, 2))
		{
			case 0:
				attackPlayer();
				break;
			case 1:
				buffSelfDamage(1);
				break;
		}
	}
	
	void phaseTwoAI()
	{
		switch (Random.Range(0, 3))
		{
			case 0:
				attackPlayer();
				break;
			case 1:
				buffSelfDamage(1);
				break;
			case 2:
				debuffPlayerDamage(1);
				break;
		}
	}
	
	void phaseThreeAI()
	{
		buffSelfDamage(1);
		attackPlayer();
	}
	
	
	void attackPlayer()
	{
		if (bossPhase == 3 || enraged)
		{
			animator.SetTrigger("AttackMagic");
		}
		else
		{
			animator.SetTrigger("Attack");
		}
	}
	
	public void dealDamage()
	{
		PlayerManager.Instance.takeDamage(baseDamage);
	}

	
	public void takeDamage(int amount)
	{
		bossHealth -= amount;
		HPDisplay.text = "HP: " + bossHealth;
		
		if (bossHealth <= (maxBossHealth * (phaseThreeThreshholdPercent/100)) && bossPhase < 3)
		{
			bossPhase = 3;
			phaseTransition = true;
		}
		else if (bossHealth <= (maxBossHealth * (phaseTwoThreshholdPercent/100)) && bossPhase < 2)
		{
			bossPhase = 2;
			phaseTransition = true;
		}
		else if (bossHealth <= (maxBossHealth * (phaseOneThreshholdPercent/100)) && bossPhase < 1)
		{
			bossPhase = 1;
			phaseTransition = true;
		}
	}
	
	void changePhase()
	{
		switch (bossPhase)
		{
			case 1:
				debuffPlayerDamage(10);
				break;
			case 2:
				debuffPlayerDamage(5);
				buffSelfDamage(5);
				debuffPlayerBlock(2);
				break;
		}
	}
	
	void buffSelfDamage(int amount)
	{
		animator.SetTrigger("Cast");
		Buff damageBuff = new Buff("damageBuff" ,Type.Buff, Stats.Damage, 1, -1, amount);
		if (BossBuffs.ContainsKey(damageBuff.Name))
		{
			BossBuffs[damageBuff.Name].Stacks += amount;
		}
		else 
		{
			BossBuffs.Add(damageBuff.Name, damageBuff);
		}
		baseDamage += amount;
		dmgDisplay.text = "Damage: " + baseDamage;
	}
	
	
	void debuffPlayerDamage(int amount)
	{
		Buff damageDebuff = new Buff("damageDebuff", Type.Debuff, Stats.Damage, -1, -1, amount);
		PlayerManager.Instance.addBuff(damageDebuff);
	}
	
	void debuffPlayerBlock(int amount)
	{
		Buff blockDebuff = new Buff("blockDebuff", Type.Debuff, Stats.Block, -1, -1, amount);
		PlayerManager.Instance.addBuff(blockDebuff);
	}
}
