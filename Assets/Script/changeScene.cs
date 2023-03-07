using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour
{
	public static changeScene instance;
	public Animator sceneFade;
	
	void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}
	
    public void gameScene()
	{
		StartCoroutine(loadScene(1));
	}
	
	public void victoryScene()
	{
		StartCoroutine(loadScene(3));
	}
	
	public void defeatScene()
	{
		StartCoroutine(loadScene(2));
	}
	
	IEnumerator loadScene(int sceneIndexToLoad)
	{
		sceneFade.SetTrigger("FadeOut");
		yield return new WaitForSeconds(0.21f);
		SceneManager.LoadScene(sceneIndexToLoad);
	}
}
