using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffIcon : MonoBehaviour
{
    public Text StackCount;
    public Text DurationCount;
    public Image BuffImage;

    public void CreateIcon(Sprite sprite, int Stacks, int Duration)
    {
        BuffImage.sprite = sprite;

        if (Stacks == -1)
        {
            StackCount.enabled = false;
        }
        else
        {
            StackCount.text = "" + Stacks;
        }

        if (Duration == -1)
        {
            DurationCount.enabled = false;
        }
        else
        {
            DurationCount.text = "" + Duration;
        }
    }

    public void updateIconStacks(int stacks)
    {
        StackCount.text = "" + stacks;
    }

    public void updateIconDuration(int duration)
    {
        DurationCount.text = "" + duration;
    }
}
