using System;
using System.Collections.Generic;
using UnityEngine;

public class StatModifierContainer
{
    private readonly Dictionary<StatAttributeType, float> flatBuffs = new();
    private readonly Dictionary<StatAttributeType, float> flatDebuffs = new();
    private readonly Dictionary<StatAttributeType, float> percentBuffs = new();
    private readonly Dictionary<StatAttributeType, float> percentDebuffs = new();

    public event Action OnModified;

    public float GetTotalFlat(StatAttributeType type)
    {
        float buff = flatBuffs.GetValueOrDefault(type);
        float debuff = flatDebuffs.GetValueOrDefault(type);
        return buff - debuff;
    }

    public float GetTotalPercent(StatAttributeType type)
    {
        float buff = percentBuffs.GetValueOrDefault(type);
        float debuff = percentDebuffs.GetValueOrDefault(type);
        return Mathf.Max(buff - debuff, 0f);
    }

    public void SetModifier(StatAttributeType type, StatModifier modifier)
    {
        // FlatBonus 처리
        if (modifier.FlatBonus > 0f)
            flatBuffs[type] = modifier.FlatBonus;
        else if (modifier.FlatBonus < 0f)
            flatDebuffs[type] = -modifier.FlatBonus;

        // PercentBonus 처리
        if (modifier.PercentBonus > 0f)
            percentBuffs[type] = modifier.PercentBonus;
        else if (modifier.PercentBonus < 0f)
            percentDebuffs[type] = -modifier.PercentBonus;

        OnModified?.Invoke();
    }

    public void RemoveModifier(StatAttributeType type)
    {
        flatBuffs.Remove(type);
        percentBuffs.Remove(type);
        flatDebuffs.Remove(type);
        percentDebuffs.Remove(type);
        OnModified?.Invoke();
    }

    public StatModifier GetModifier(StatAttributeType type)
    {
        return new StatModifier
        {
            FlatBonus = GetTotalFlat(type),
            PercentBonus = GetTotalPercent(type)
        };
    }
}
