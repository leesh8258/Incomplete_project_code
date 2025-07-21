using System.Collections.Generic;
using UnityEngine;

public class BuffReceiver : MonoBehaviour
{
    private Dictionary<BuffType_NEW, BuffStackQueue> buffQueues = new();
    private StatModifierContainer modifierContainer;
    private StatProcessor statProcessor;

    public void Initialize(StatProcessor processor)
    {
        this.statProcessor = processor;
        this.modifierContainer = processor.ModifierContainer;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        bool updated = false;

        foreach (var queue in buffQueues.Values)
        {
            updated |= queue.Tick(deltaTime);
        }

        if (updated)
            RecalculateStat();
    }

    // ✅ GroupType 추가 지원
    public void ApplyBuff(BuffType_NEW type, float duration, float flatBonus, float percentBonus, BuffGroupType groupType = BuffGroupType.None)
    {
        if (!buffQueues.TryGetValue(type, out var queue))
        {
            queue = new BuffStackQueue();
            buffQueues.Add(type, queue);
            queue.OnExpired += RecalculateStat;
        }

        queue.Add(new BuffStackEntry(flatBonus, percentBonus, duration, groupType));
        RecalculateStat();
    }

    private void RecalculateStat()
    {
        foreach (var type in buffQueues.Keys)
        {
            var queue = buffQueues[type];
            var current = queue.Current;

            if (current != null)
            {
                StatAttributeType statType = BuffTypeToStatAttributeConverter.ToStatAttribute(type);
                modifierContainer.SetModifier(statType, new StatModifier(current.FlatBonus, current.PercentBonus));
            }
            else
            {
                StatAttributeType statType = BuffTypeToStatAttributeConverter.ToStatAttribute(type);
                modifierContainer.RemoveModifier(statType);
            }
        }

        statProcessor.Recalculate();
    }


    public StatProcessor GetProcessor() => statProcessor;
}
