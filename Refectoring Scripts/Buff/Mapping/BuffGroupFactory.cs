using System;
using System.Collections.Generic;
using UnityEngine;

public static class BuffGroupFactory
{
    private static readonly Dictionary<BuffGroupType, BuffType_NEW[]> groupMappings = new()
    {
        { BuffGroupType.AllAttackUp, new BuffType_NEW[]
            {
                BuffType_NEW.MeleeAttackUp,
                BuffType_NEW.RangedAttackUp
            }
        }

        // 필요한 그룹 추가
    };

    public static BuffType_NEW[] GetEffects(BuffGroupType groupType)
    {
        if (!groupMappings.TryGetValue(groupType, out var effects))
        {
            Debug.LogError($"[BuffGroupFactory] No effects mapped for group: {groupType}");
            return Array.Empty<BuffType_NEW>();
        }

        return effects;
    }
}
