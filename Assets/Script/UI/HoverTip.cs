using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public string tipToShow;
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		HoverTipManager.OnMouseOver(tipToShow.Replace("<nl>", "\n"), Input.mousePosition);
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		HoverTipManager.OnMouseAway();
	}
}
