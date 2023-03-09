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
			PlayerManager.Instance.lifesteal();
		}
		else
		{
			Debug.Log("Not your turn");
		}
	}
	
	public void startGame()
	{
		changeScene.instance.gameScene();
	}
	
	public void quitGame()
	{
		Application.Quit();
	}
}
