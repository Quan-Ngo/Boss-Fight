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
    public Type Type;
    public Stats Stat;

    public int Duration;
    public BuffValue buffValue;

    public Buff(Type type, Stats stat, int value, int duration)
    {
        Type = type;
        Duration = duration;

        buffValue = new BuffValue(stat, value);
    }
}

[System.Serializable]
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
