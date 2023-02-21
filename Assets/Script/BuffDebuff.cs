using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Buff,
    Debuff,
    Other
}

public enum Stats
{
    Block,
    Damage,
    Lifesteal
}

public class Buff
{
    public string Name;
    public Type Type;

    public int Duration;
    public BuffValue buffValue;

    public int Stacks;

    public Buff(string name, Type type, Stats stat, int value, int duration, int stacks)
    {
        Name = name;
        Type = type;
        Duration = duration;
        Stacks = stacks;

        buffValue = new BuffValue(stat, value);
    }
}

public class BuffValue
{
    public Stats Stat;
    public int Value;

    public BuffValue(Stats stat, int value)
    {
        Stat = stat;
        Value = value;
    }
}
