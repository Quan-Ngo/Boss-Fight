using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    [SerializeField] private GameObject Entity;
    [SerializeField] private PlayerManager EntityManager = null;

    private Dictionary<string, Buff> ActiveBuffs = new Dictionary<string, Buff>();
    private List<Buff> ExpiredBuffs = new List<Buff>();

    public void Start()
    {
        EntityManager = Entity.GetComponent<PlayerManager>();
    }

    public void updateBuffs()
    {
        foreach (KeyValuePair<string, Buff> buff in ActiveBuffs)
        {
            if (buff.Value.Duration > 1)
            {
                buff.Value.Duration -= 1;
            }
            else if (buff.Value.Duration >= 0)
            {
                ExpiredBuffs.Add(buff.Value);
            }
        }

        foreach (Buff buff in ExpiredBuffs)
        {
            removeBuff(buff);
        }
    }

    public void addBuff(Buff buff)
    {
        if (ActiveBuffs.ContainsKey(buff.Name))
        {
            if (buff.Stacks >= 1)
            {
                ActiveBuffs[buff.Name].Stacks += buff.Stacks;
                applyBuff(buff);
            }
            else
            {
                ActiveBuffs[buff.Name].Duration = buff.Duration;
            }
        }
        else
        {
            ActiveBuffs.Add(buff.Name, buff);
            applyBuff(buff);
        }
    }

    public void removeBuff(Buff buff)
    {
        if (ActiveBuffs.ContainsKey(buff.Name))
        {
            if (ActiveBuffs[buff.Name].Stacks > 1)
            {
                ActiveBuffs[buff.Name].Stacks -= 1;
                killBuff(buff);
            }
            else if (ActiveBuffs[buff.Name].Stacks <= 1)
            {
                ActiveBuffs.Remove(buff.Name);
                killBuff(buff);
            }
        }
    }

    private void applyBuff(Buff buff)
    {
        EntityManager.addStatBuff(buff);
    }

    private void killBuff(Buff buff)
    {
        EntityManager.removeStatBuff(buff);
    }

}
