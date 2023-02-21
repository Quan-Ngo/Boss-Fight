using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn {PLAYER, BOSS}

public class TurnManager : MonoBehaviour
{
	public static TurnManager Instance;
	
	[SerializeField] private int turnCount;

	[SerializeField]
	private Turn currentActiveTurn;

	void Awake()
	{
		turnCount = 1;
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
				turnCount++;
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
	
	public int getTurnCount()
	{
		return turnCount;
	}
}
