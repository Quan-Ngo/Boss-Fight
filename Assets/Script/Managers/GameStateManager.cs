using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
	public static GameStateManager instance;
    // Start is called before the first frame update
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
		StartCoroutine(changeSceneDefeat());
	}
	
	IEnumerator changeSceneDefeat()
	{
		yield return new WaitForSeconds(0.21f);
		changeScene.instance.defeatScene();
	}
	
	public void bossDied()
	{
		StartCoroutine(changeSceneVictory());
	}
	
	IEnumerator changeSceneVictory()
	{
		yield return new WaitForSeconds(0.21f);
		changeScene.instance.victoryScene();
	}
}
