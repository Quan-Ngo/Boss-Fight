using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: change player script to add buff based on stack instead of just 1

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;
	public float phaseOneThreshholdPercent;
	public float phaseTwoThreshholdPercent;
	public float phaseThreeThreshholdPercent;
	public int maxBossHealth;
	
	
	[SerializeField] private int bossHealth;
	[SerializeField] private int bossPhase;
	[SerializeField] private int baseDamage;
	
	Dictionary<string, Buff> BossBuffs = new Dictionary<string, Buff>();
	
	
	void Start()
	{
		bossPhase = 0;
		bossHealth = maxBossHealth;
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
				//phaseThreeAI();
				break;
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
				debuffPlayerDamage(2);
				break;
		}
	}
	
	void attackPlayer()
	{
		PlayerManager.Instance.takeDamage(baseDamage);
	}
	
	public void takeDamage(int amount)
	{
		bossHealth -= amount;
		if (bossHealth <= (maxBossHealth * (phaseThreeThreshholdPercent/100)))
		{
			bossPhase = 3;
		}
		else if (bossHealth <= (maxBossHealth * (phaseTwoThreshholdPercent/100)))
		{
			bossPhase = 2;
		}
		else if (bossHealth <= (maxBossHealth * (phaseOneThreshholdPercent/100)))
		{
			bossPhase = 1;
		}
	}
	
	void buffSelfDamage(int amount)
	{
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
	}
	
	void debuffPlayerDamage(int amount)
	{
		Debug.Log("Debuff Player");
		Buff damageDebuff = new Buff("damageDebuff", Type.Debuff, Stats.Damage, -1, -1, amount);
		PlayerManager.Instance.addBuff(damageDebuff);
	}
}
