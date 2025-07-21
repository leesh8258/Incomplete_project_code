public class BuffStackEntry
{
    public float FlatBonus { get; }
    public float PercentBonus { get; }
    public float Duration { get; set; }

    public BuffGroupType GroupType { get; } = BuffGroupType.None; // ✅ 추가

    public BuffStackEntry(float flatBonus, float percentBonus, float duration, BuffGroupType groupType = BuffGroupType.None)
    {
        FlatBonus = flatBonus;
        PercentBonus = percentBonus;
        Duration = duration;
        GroupType = groupType;
    }
}
