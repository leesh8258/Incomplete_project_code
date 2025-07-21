using System;
using System.Collections.Generic;
using UnityEngine;

public static class BuffTypeToStatAttributeConverter
{
    private static readonly Dictionary<BuffType_NEW, StatAttributeType> mapping = new()
    {
        { BuffType_NEW.MeleeAttackUp, StatAttributeType.MeleeAttackPower },
        { BuffType_NEW.MeleeAttackDown, StatAttributeType.MeleeAttackPower },
        { BuffType_NEW.RangedAttackUp, StatAttributeType.RangeAttackPower },
        { BuffType_NEW.RangedAttackDown, StatAttributeType.RangeAttackPower },
        { BuffType_NEW.DefenseUp, StatAttributeType.Defense },
        { BuffType_NEW.DefenseDown, StatAttributeType.Defense },
        // 필요한 매핑 추가
    };

    public static StatAttributeType ToStatAttribute(BuffType_NEW type)
    {
        if (!mapping.TryGetValue(type, out var statType))
        {
            Debug.LogError($"[Converter] No stat mapping defined for BuffType: {type}");
            return default;
        }

        return statType;
    }
}
