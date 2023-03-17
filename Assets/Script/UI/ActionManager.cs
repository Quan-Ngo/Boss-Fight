using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
	public static ActionManager Instance;

	[SerializeField] private HoverTip Action1Tip;
	[SerializeField] private HoverTip Action2Tip;
	[SerializeField] private HoverTip Action3Tip;
	[SerializeField] private HoverTip Action4Tip;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}
	}

	private void Start()
    {
		Action1Tip.tipToShow = "Slash\nCost: 1AP\n\nAttack for " + 
			(PlayerManager.Instance.Damage + PlayerManager.Instance.TempDamage) + " damage.";

		Action2Tip.tipToShow = "Shield\nCost: 2AP\n\nGain " +
			(PlayerManager.Instance.Defense + PlayerManager.Instance.TempDefense) + " block.";

		Action3Tip.tipToShow = "Power Up\nCost: 2AP\n\nPermanently increases damage dealt by 1.";

		Action4Tip.tipToShow = "Life Steal\nCost: 3AP\n\nFor 3 turns, attacks restore HP equal to 1/3 of damage dealt.";
	}

	public void updateTips()
	{
		Action1Tip.tipToShow = "Slash\nCost: 1AP\n\nAttack for " +
			(PlayerManager.Instance.Damage + PlayerManager.Instance.TempDamage) + " damage.";

		Action2Tip.tipToShow = "Shield\nCost: 2AP\n\nGain " +
			(PlayerManager.Instance.Defense + PlayerManager.Instance.TempDefense) + " block.";
	}

	private bool isPlayerTurn()
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
    }

	public void defend()
	{
		if (isPlayerTurn())
		{
			PlayerManager.Instance.defend();
		}
	}

	public void buff()
	{
		if (isPlayerTurn())
		{
			PlayerManager.Instance.buff();
		}
	}

	public void lifesteal()
	{
		if (isPlayerTurn())
		{
			PlayerManager.Instance.lifesteal();
		}
	}
}
