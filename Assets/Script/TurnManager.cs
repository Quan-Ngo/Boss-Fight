using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn {PLAYER, BOSS}

public class TurnManager : MonoBehaviour
{
	[SerializeField]
	private Turn currentActiveTurn;
	
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
	}
	
	void startBossTurn()
	{
		currentActiveTurn = Turn.BOSS;
	}
	
	public bool checkIsPlayerTurn()
	{
		return (currentActiveTurn == Turn.PLAYER);
	}
}
