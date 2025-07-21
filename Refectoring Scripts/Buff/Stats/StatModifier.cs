public struct StatModifier
{
    public float FlatBonus;   // 고정 보정치 (+30 등)
    public float PercentBonus; // % 보정치 (0.1f == 10%)

    public StatModifier(float flat, float percent)
    {
        FlatBonus = flat;
        PercentBonus = percent;
    }

    public static StatModifier operator +(StatModifier a, StatModifier b)
        => new StatModifier(a.FlatBonus + b.FlatBonus, a.PercentBonus + b.PercentBonus);
}
