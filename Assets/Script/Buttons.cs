using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
	
    bool isPlayerTurn()
	{
		return TurnManager.Instance.checkIsPlayerTurn();
	}
	
	public void changeTurn()
	{
		if (isPlayerTurn())
		{
			TurnManager.Instance.changeTurn();
		}
	}

	public void attack()
    {
        if (isPlayerTurn())
        {
			//CHECK FOR AP ONCE THAT EXISTS

			PlayerManager.Instance.attack();
        }
        else
        {
			Debug.Log("Not your turn");
        }
    }

	public void defend()
	{
		if (isPlayerTurn())
		{
			//CHECK FOR AP ONCE THAT EXISTS

			PlayerManager.Instance.defend();
		}
		else
		{
			Debug.Log("Not your turn");
		}
	}

	public void buff()
	{
		if (isPlayerTurn())
		{
			//CHECK FOR AP ONCE THAT EXISTS

			PlayerManager.Instance.buff();
		}
		else
		{
			Debug.Log("Not your turn");
		}
	}

	public void lifesteal()
	{
		if (isPlayerTurn())
		{
			//CHECK FOR AP ONCE THAT EXISTS

			PlayerManager.Instance.lifesteal();
		}
		else
		{
			Debug.Log("Not your turn");
		}
	}
}
