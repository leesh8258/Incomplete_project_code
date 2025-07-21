[System.Serializable]
public class AttackGroupBuffEffectData
{
    public BuffGroupType groupType;           // 미리 캐싱된 그룹 Enum
    public EffectApplyTargetType target;      // Self / Target
    public float flatBonus;                   // 그룹 전체 플랫 보너스
    public float percentBonus;                // 그룹 전체 퍼센트 보너스
    public float duration;                    // 그룹 전체 지속시간
}