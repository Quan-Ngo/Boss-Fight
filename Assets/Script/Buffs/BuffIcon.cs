using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffIcon : MonoBehaviour
{
    [SerializeField] private Text StackCount;
    [SerializeField] private Text DurationCount;
    [SerializeField] private Image BuffImage;

    public void CreateIcon(Sprite sprite, int Stacks, int Duration)
    {
        BuffImage.sprite = sprite;
        StackCount.text = "" + Stacks;
        if (Duration == -1)
        {
            DurationCount.enabled = false;
        }
        else
        {
            DurationCount.text = "" + Duration;
        }
    }
}
