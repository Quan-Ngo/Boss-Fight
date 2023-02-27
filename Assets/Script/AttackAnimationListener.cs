using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationListener : MonoBehaviour
{
	public enum Actor {PLAYER, BOSS};
	public Actor actor; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void dealDamage(){
		if (actor == Actor.PLAYER)
		{
			PlayerManager.Instance.dealDamage();
		}
		else
		{
			BossManager.Instance.dealDamage();
		}
	}
}
