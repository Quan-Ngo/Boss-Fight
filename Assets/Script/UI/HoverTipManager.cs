using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HoverTipManager : MonoBehaviour
{
	public TextMeshProUGUI tipText;
	public RectTransform tipWindow;
	
	public static Action<string, Vector2> OnMouseOver;
	public static Action OnMouseAway;
	public int maxWidth;
	
	private void OnEnable()
	{
		OnMouseOver += ShowTip;
		OnMouseAway += HideTip;
	}
	
	private void OnDisable()
	{
		OnMouseOver -= ShowTip;
		OnMouseAway -= HideTip;
	}
	
    // Start is called before the first frame update
    void Start()
    {
        HideTip();
    }

    private void ShowTip(string tip, Vector2 mousePos)
	{
		tipText.text = tip;
		tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > maxWidth ? maxWidth : tipText.preferredWidth, tipText.preferredHeight + 20);
		
		tipWindow.gameObject.SetActive(true);
		tipWindow.transform.position = new Vector2(mousePos.x + tipWindow.sizeDelta.x, mousePos.y);
	}
	
	private void HideTip()
	{
		tipText.text = default;
		tipWindow.gameObject.SetActive(false);
	}
}
