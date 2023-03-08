using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimationListener : MonoBehaviour
{
	public enum Actor {PLAYER, BOSS};
	public Actor actor; 
	
	public void updateGameStateDeath()
	{
		if (actor == Actor.PLAYER)
		{
			GameStateManager.instance.playerDied();
		}
		else
		{
			GameStateManager.instance.bossDied();
		}
	}
}
