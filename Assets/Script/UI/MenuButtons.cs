using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
	public void startGame()
	{
		changeScene.instance.gameScene();
	}

	public void quitGame()
	{
		Application.Quit();
	}
}
