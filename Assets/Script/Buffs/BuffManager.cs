using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    [SerializeField] private GameObject Entity;
    private PlayerManager EntityManager = null;

    [SerializeField] private Sprite[] Sprite;
    [SerializeField] private GameObject BuffPrefab;

    private Dictionary<string, Buff> ActiveBuffs = new Dictionary<string, Buff>();
    private List<Buff> ExpiredBuffs = new List<Buff>();

    private List<GameObject> BuffIcons = new List<GameObject>();

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
            createIcon(buff);
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

    private void createIcon(Buff buff)
    {
        GameObject buffIcon = Instantiate(BuffPrefab, Vector3.zero, Quaternion.identity, this.gameObject.transform);

        int offset = BuffIcons.Count * 70;
        buffIcon.transform.localPosition = new Vector3(offset, 0, 0);

        int imageIndex = 0;
        switch (buff.buffValue.Stat)
        {
            case Stats.Block:
                imageIndex = 3;
                break;
            case Stats.Damage:
                if(buff.Type == Type.Buff)
                {
                    imageIndex = 0;
                }
                else if (buff.Type == Type.Debuff)
                {
                    imageIndex = 2;
                }
                break;
            case Stats.Lifesteal:
                imageIndex = 1;
                break;
        }
        
        buffIcon.GetComponent<BuffIcon>().CreateIcon(Sprite[imageIndex], buff.Stacks, buff.Duration);
        BuffIcons.Add(buffIcon);
    }
}
