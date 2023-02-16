using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;
	public int phaseOneThreshholdPercent;
	public int phaseTwoThreshholdPercent;
	public int phaseThreeThreshholdPercent;
	public int maxBossHealth;
	
	
	[SerializeField]
	private int bossHealth;
	private int bossPhase;
	
	
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
				//phaseOneAI();
				break;
			case 2:
				//phaseTwoAI();
				break;
			case 3:
				//phaseThreeAI();
				break;
		}
    }

	void phaseZeroAI()
	{
		attackPlayer();
	}
	
	void attackPlayer()
	{
		PlayerManager.Instance.takeDamage(5);
		TurnManager.Instance.changeTurn();
	}
	
	void takeDamage(int amount)
	{
		bossHealth -= amount;
		if (bossHealth <= (int) (maxBossHealth * (phaseThreeThreshholdPercent/100)))
		{
			bossPhase = 3;
		}
		else if (bossHealth <= (int) (maxBossHealth * (phaseTwoThreshholdPercent/100)))
		{
			bossPhase = 2;
		}
		else if (bossHealth <= (int) (maxBossHealth * (phaseOneThreshholdPercent/100)))
		{
			bossPhase = 1;
		}
		
	}
}
