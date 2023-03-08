using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
	public static GameStateManager instance;
    // Start is called before the first frame update
	
	private enum State {VICTORY, DEFEAT};
	private State state;
	
    void Start()
    {
        if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
    }

    public void playerDied()
	{
		state = State.DEFEAT;
		StartCoroutine(changeGameScene(state));
	}
	
	/*IEnumerator changeSceneDefeat()
	{
		yield return new WaitForSeconds(0.5f);
		changeScene.instance.defeatScene();
	}*/
	
	public void bossDied()
	{
		state = State.VICTORY;
		StartCoroutine(changeGameScene(state));
	}
	
	IEnumerator changeGameScene(State currentState)
	{
		yield return new WaitForSeconds(0.5f);
		
		if (currentState == State.VICTORY)
		{
			changeScene.instance.victoryScene();
		}
		else if (currentState == State.DEFEAT)
		{
			changeScene.instance.defeatScene();
		}
	}
}
