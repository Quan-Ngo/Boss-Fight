using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
	public TurnManager turnManager;
	
    bool isPlayerTurn()
	{
		return turnManager.checkIsPlayerTurn();
	}
	
	public void changeTurn()
	{
		if (isPlayerTurn())
		{
			turnManager.changeTurn();
		}
	}
}
