using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn {PLAYER, BOSS}

public class TurnManager : MonoBehaviour
{
	public static TurnManager Instance;

	[SerializeField]
	private Turn currentActiveTurn;

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


	public void changeTurn()
	{
		switch (currentActiveTurn)
		{
			case Turn.PLAYER:
				startBossTurn();
				break;
			case Turn.BOSS:
				startPlayerTurn();
				break;
		}
	}
	
    void startPlayerTurn()
	{
		currentActiveTurn = Turn.PLAYER;
		PlayerManager.Instance.turnStart();
	}
	
	void startBossTurn()
	{
		currentActiveTurn = Turn.BOSS;
		BossManager.Instance.turnStart();
	}
	
	public bool checkIsPlayerTurn()
	{
		return (currentActiveTurn == Turn.PLAYER);
	}
}
