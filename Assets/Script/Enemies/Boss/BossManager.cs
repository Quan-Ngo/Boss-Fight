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
	public TMP_Text dmgDisplay;
	public Animator animator;
	public Animator phaseChangeFX;
	
	
	[SerializeField] private int bossHealth;
	[SerializeField] private int bossPhase;
	[SerializeField] private int baseDamage;
	[SerializeField] private bool phaseTransition;
	[SerializeField] private bool enraged;

	[SerializeField] private HealthBar HealthBar;

	Dictionary<string, Buff> BossBuffs = new Dictionary<string, Buff>();
	
	
	void Start()
	{
		bossPhase = 0;
		bossHealth = maxBossHealth;
		HealthBar.SetMaxHealth(maxBossHealth);
		phaseTransition = false;
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
			StartCoroutine(normalAI());
		}
		else
		{
			StartCoroutine(enragedAI());
		}	
    }
	
	IEnumerator normalAI()
	{
		if (phaseTransition == true)
		{
			StartCoroutine(changePhase());
			phaseTransition = false;
			yield return new WaitForSeconds(1.7f);
		}
		
		switch (bossPhase)
		{
			case 0:
				StartCoroutine(phaseZeroAI());
				break;
			case 1:
				StartCoroutine(phaseOneAI());
				break;
			case 2:
				StartCoroutine(phaseTwoAI());
				break;
			case 3:
				StartCoroutine(phaseThreeAI());
				break;
		}
	}


	IEnumerator phaseZeroAI()
	{
		attackPlayer();
		yield return new WaitForSeconds(1.2f);
		TurnManager.Instance.changeTurn();
	}
	
	IEnumerator phaseOneAI()
	{
		switch (Random.Range(0, 2))
		{
			case 0:
				attackPlayer();
				yield return new WaitForSeconds(1.2f);
				break;
			case 1:
				StartCoroutine(buffSelfDamage(1));
				yield return new WaitForSeconds(1f);
				break;
		}
		TurnManager.Instance.changeTurn();
	}
	
	IEnumerator phaseTwoAI()
	{
		switch (Random.Range(0, 3))
		{
			case 0:
				attackPlayer();
				yield return new WaitForSeconds(1.2f);
				break;
			case 1:
				StartCoroutine(buffSelfDamage(1));
				yield return new WaitForSeconds(1f);
				break;
			case 2:
				StartCoroutine(debuffPlayerDamage(1));
				yield return new WaitForSeconds(1f);
				break;
		}
		TurnManager.Instance.changeTurn();
	}
	
	IEnumerator phaseThreeAI()
	{
		StartCoroutine(buffSelfDamage(1));
		yield return new WaitForSeconds(1f);
		attackPlayer();
		yield return new WaitForSeconds(1.2f);
		TurnManager.Instance.changeTurn();
	}
	
	IEnumerator enragedAI()
	{
		StartCoroutine(buffSelfDamage(5));
		yield return new WaitForSeconds(1f);
		attackPlayer();
		yield return new WaitForSeconds(1.2f);
		TurnManager.Instance.changeTurn();
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
		animator.SetTrigger("GetHit");
		bossHealth -= amount;
		HealthBar.SetHealth(bossHealth);

		if (bossHealth <= 0)
		{
			GameStateManager.instance.bossDied();
		}
		else if (bossHealth <= (maxBossHealth * (phaseThreeThreshholdPercent/100)) && bossPhase < 3)
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
	
	IEnumerator changePhase()
	{
		phaseChangeFX.SetTrigger("Start");
		switch (bossPhase)
		{
			case 1:
				StartCoroutine(debuffPlayerDamage(10));
				break;
			case 2:
				StartCoroutine(debuffPlayerBlock(2));
				yield return new WaitForSeconds(1f);
				StartCoroutine(buffSelfDamage(5));
				yield return new WaitForSeconds(1f);
				StartCoroutine(debuffPlayerDamage(5));
				yield return new WaitForSeconds(1f);
				break;
			case 3:
				StartCoroutine(debuffPlayerDamage(5));
				yield return new WaitForSeconds(1f);
				break;
		}
	}
	
	IEnumerator buffSelfDamage(int amount)
	{
		animator.SetTrigger("Cast");
		Buff damageBuff = new Buff("damageBuff" ,Type.Buff, Stats.Damage, 1, -1, amount);
		
		yield return new WaitForSeconds(0.5f);
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
	
	
	IEnumerator debuffPlayerDamage(int amount)
	{
		animator.SetTrigger("Cast");
		Buff damageDebuff = new Buff("damageDebuff", Type.Debuff, Stats.Damage, -1, -1, amount);
		yield return new WaitForSeconds(0.5f);
		Debug.Log("Debuff: " + damageDebuff);
		PlayerManager.Instance.addBuff(damageDebuff);
	}
	
	IEnumerator debuffPlayerBlock(int amount)
	{
		animator.SetTrigger("Cast");
		Buff blockDebuff = new Buff("blockDebuff", Type.Debuff, Stats.Block, -1, -1, amount);
		yield return new WaitForSeconds(0.5f);
		PlayerManager.Instance.addBuff(blockDebuff);
	}
}
