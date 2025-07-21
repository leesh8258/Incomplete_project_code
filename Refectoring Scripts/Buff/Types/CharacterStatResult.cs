[System.Serializable]
public class CharacterStatResult
{
    public float HP;
    public float meleeAttackPower;
    public float meleeAttackSpeed;
    public float rangeAttackPower;
    public float rangeAttackSpeed;
    public float defense;
    public float evasion;
    public float resistance;

    public float GetStat(StatAttributeType type)
    {
        return type switch
        {
            StatAttributeType.HP => HP,
            StatAttributeType.MeleeAttackPower => meleeAttackPower,
            StatAttributeType.MeleeAttackSpeed => meleeAttackSpeed,
            StatAttributeType.RangeAttackPower => rangeAttackPower,
            StatAttributeType.RangeAttackSpeed => rangeAttackSpeed,
            StatAttributeType.Defense => defense,
            StatAttributeType.Evasion => evasion,
            StatAttributeType.Resistance => resistance,
            _ => 0f
        };
    }

    public void SetStat(StatAttributeType type, float value)
    {
        switch (type)
        {
            case StatAttributeType.HP: HP = value; break;
            case StatAttributeType.MeleeAttackPower: meleeAttackPower = value; break;
            case StatAttributeType.MeleeAttackSpeed: meleeAttackSpeed = value; break;
            case StatAttributeType.RangeAttackPower: rangeAttackPower = value; break;
            case StatAttributeType.RangeAttackSpeed: rangeAttackSpeed = value; break;
            case StatAttributeType.Defense: defense = value; break;
            case StatAttributeType.Evasion: evasion = value; break;
            case StatAttributeType.Resistance: resistance = value; break;
            default: break;
        }
    }
}
