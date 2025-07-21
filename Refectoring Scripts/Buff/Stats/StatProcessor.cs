using System;

public class StatProcessor
{
    private readonly CharacterStatBase baseStat;
    private readonly StatModifierContainer modifierContainer;
    private readonly CharacterStatResult final = new();

    public StatModifierContainer ModifierContainer => modifierContainer;
    public CharacterStatResult Final => final;

    public StatProcessor(CharacterStatBase baseStat, StatModifierContainer modifierContainer)
    {
        this.baseStat = baseStat;
        this.modifierContainer = modifierContainer;

        modifierContainer.OnModified += Recalculate;
        Recalculate();
    }

    public void Recalculate()
    {
        foreach (StatAttributeType type in Enum.GetValues(typeof(StatAttributeType)))
        {
            float baseValue = baseStat.GetBaseStat(type);
            float flat = modifierContainer.GetTotalFlat(type);
            float percent = modifierContainer.GetTotalPercent(type);

            float finalValue = (baseValue + flat) * (1f + percent);
            final.SetStat(type, finalValue);
        }
    }

    public float GetFinalStat(StatAttributeType type)
    {
        return final.GetStat(type);
    }
}
