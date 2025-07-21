using UnityEngine;

public static class AttackEffectTargetUtility
{
    public static void Apply(GameObject attacker, GameObject target, AttackDataSO data)
    {
        if (data == null)
            return;

        ApplyEffects(attacker, target, data);
        ApplyGroupEffects(attacker, target, data);
    }

    private static void ApplyEffects(GameObject attacker, GameObject target, AttackDataSO data)
    {
        foreach (var effect in data.effectsToApply)
        {
            GameObject applyTarget = (effect.target == EffectApplyTargetType.Self) ? attacker : target;
            if (applyTarget == null)
                continue;

            if (!applyTarget.TryGetComponent(out BuffReceiver receiver))
                continue;

            receiver.ApplyBuff(
                effect.buffType,
                effect.duration,
                effect.flatBonus,
                effect.percentBonus
            );
        }
    }

    private static void ApplyGroupEffects(GameObject attacker, GameObject target, AttackDataSO data)
    {
        foreach (var group in data.groupEffectsToApply)
        {
            GameObject applyTarget = (group.target == EffectApplyTargetType.Self) ? attacker : target;
            if (applyTarget == null)
                continue;

            if (!applyTarget.TryGetComponent(out BuffReceiver receiver))
                continue;

            // 그룹 버프를 구성하는 BuffType 리스트를 가져온다
            var groupBuffList = BuffGroupFactory.GetEffects(group.groupType);

            if (groupBuffList == null)
                continue;

            foreach (var subBuff in groupBuffList)
            {
                receiver.ApplyBuff(
                    subBuff,
                    group.duration,        // ✅ 그룹 전체 duration을 사용
                    group.flatBonus,        // ✅ 그룹 전체 flat 보너스
                    group.percentBonus,     // ✅ 그룹 전체 percent 보너스
                    group.groupType         // ✅ GroupType 정보를 넘긴다
                );
            }
        }
    }
}
