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
	public Animator animator;
	public Animator phaseChangeFX;
	public AudioClip[] audioClips;
	public Animator[] enrageFX;
	public int phaseOneTurnLimit;
	public int phaseTwoTurnLimit;
	public int phaseThreeTurnLimit;
	
	
	
	[SerializeField] private int bossHealth;
	[SerializeField] private int bossPhase;
	[SerializeField] private int baseDamage;
	[SerializeField] private bool phaseTransition;
	[SerializeField] private bool enraged;

	[SerializeField] private HealthBar healthBar;
	[SerializeField] private GameObject buffIcon;
	[SerializeField] private int prevBossPhase;

	private AudioSource audioSource;
	Dictionary<string, Buff> BossBuffs = new Dictionary<string, Buff>();
	
	
	void Start()
	{
		bossPhase = 0;
		bossHealth = maxBossHealth;
		healthBar.SetMaxHealth(maxBossHealth);
		buffIcon.SetActive(false);
		phaseTransition = false;
		audioSource = GetComponent<AudioSource>();
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
		if (bossPhase < 1 && TurnManager.Instance.getTurnCount() == phaseOneTurnLimit)
		{
			bossPhase = 1;
			phaseTransition = true;
		}
		else if (bossPhase < 2 && TurnManager.Instance.getTurnCount() == phaseTwoTurnLimit)
		{
			bossPhase = 2;
			phaseTransition = true;
		}
		else if (bossPhase < 3 && TurnManager.Instance.getTurnCount() == phaseThreeTurnLimit)
		{
			bossPhase = 3;
			phaseTransition = true;
		}
		else if (!enraged && TurnManager.Instance.getTurnCount() == enrageTurn)
		{
			StartCoroutine(beginEnrage());
		}
		
		if (!enraged && bossHealth > 0)
		{
			StartCoroutine(normalAI());
		}
		else if (bossHealth > 0)
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
		
		switch (Random.Range(0, 3))
		{
			case 0:
				attackPlayer();
				yield return new WaitForSeconds(1.2f);
				break;
			case 1:
				attackPlayer();
				yield return new WaitForSeconds(1.2f);
				break;
			case 2:
				StartCoroutine(buffSelfDamage(1));
				yield return new WaitForSeconds(1f);
				break;
		}
		TurnManager.Instance.changeTurn();
	}
	
	IEnumerator phaseTwoAI()
	{
		switch (Random.Range(0, 4))
		{
			case 0:
				attackPlayer();
				yield return new WaitForSeconds(1.2f);
				break;
			case 1:
				attackPlayer();
				yield return new WaitForSeconds(1.2f);
				break;
			case 2:
				StartCoroutine(buffSelfDamage(1));
				yield return new WaitForSeconds(1f);
				break;
			case 3:
				StartCoroutine(debuffPlayerDamage(1));
				yield return new WaitForSeconds(1f);
				break;
		}
		TurnManager.Instance.changeTurn();
	}
	
	IEnumerator phaseThreeAI()
	{
		StartCoroutine(buffSelfDamage(2));
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
			audioSource.clip = audioClips[1];
			animator.SetTrigger("AttackMagic");
		}
		else
		{
			audioSource.clip = audioClips[0];
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
		healthBar.SetHealth(bossHealth);

		if (bossHealth <= 0)
		{
			audioSource.clip = audioClips[3];
			animator.SetTrigger("Death");
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
		while (prevBossPhase < bossPhase)
		{
			switch (prevBossPhase)
			{
				case 0:
					StartCoroutine(debuffPlayerDamage(10));
					yield return new WaitForSeconds(1f);
					StartCoroutine(buffSelfDamage(2));
					yield return new WaitForSeconds(1f);
					break;
				case 1:
					StartCoroutine(debuffPlayerBlock(2));
					yield return new WaitForSeconds(1f);
					StartCoroutine(buffSelfDamage(4));
					yield return new WaitForSeconds(1f);
					StartCoroutine(debuffPlayerDamage(4));
					yield return new WaitForSeconds(1f);
					break;
				case 2:
					StartCoroutine(debuffPlayerDamage(5));
					yield return new WaitForSeconds(1f);
					break;
			}
			prevBossPhase++;
		}
	}
	
	IEnumerator beginEnrage()
	{
		animator.SetTrigger("Cast");
		enraged = true;
		foreach(Animator effect in enrageFX)
		{
			effect.SetTrigger("Start");
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	IEnumerator buffSelfDamage(int amount)
	{
		audioSource.clip = audioClips[2];
		animator.SetTrigger("Cast");
		Buff damageBuff = new Buff("damageBuff1" ,Type.Buff, Stats.Damage, 1, -1, "Damage increased by 1 per stack.", amount);
		
		yield return new WaitForSeconds(0.5f);
		if (BossBuffs.ContainsKey(damageBuff.Name))
		{
			BossBuffs[damageBuff.Name].Stacks += amount;
		}
		else 
		{
			BossBuffs.Add(damageBuff.Name, damageBuff);
			buffIcon.SetActive(true);
		}
		buffIcon.GetComponent<BuffIcon>().updateIconStacks(BossBuffs[damageBuff.Name].Stacks);
		buffIcon.GetComponent<HoverTip>().tipToShow = damageBuff.Tooltip;
		baseDamage += amount;
	}
	
	IEnumerator debuffPlayerDamage(int amount)
	{
		audioSource.clip = audioClips[2];
		animator.SetTrigger("Cast");
		Buff damageDebuff = new Buff("damageDebuff1", Type.Debuff, Stats.Damage, -1, -1, "Damage reduced by 1 per stack.", amount);
		yield return new WaitForSeconds(0.5f);
		PlayerManager.Instance.addBuff(damageDebuff);
	}
	
	IEnumerator debuffPlayerBlock(int amount)
	{
		audioSource.clip = audioClips[2];
		animator.SetTrigger("Cast");
		Buff blockDebuff = new Buff("blockDebuff1", Type.Debuff, Stats.Block, -1, -1, "Decreases shields gained by 1 per stack.", amount);
		yield return new WaitForSeconds(0.5f);
		PlayerManager.Instance.addBuff(blockDebuff);
	}
}
