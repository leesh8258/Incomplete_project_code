using UnityEngine;

[System.Serializable]
public class CharacterStatBase : CommonStatBase
{
    [field: SerializeField, Header("캐릭터 공통 스탯")] public float meleeAttackPower { get; set; }
    [field: SerializeField] public float meleeAttackSpeed { get; set; }
    [field: SerializeField] public float rangeAttackPower { get; set; }
    [field: SerializeField] public float rangeAttackSpeed { get; set; }
    [field: SerializeField] public float evasion { get; set; }
    [field: SerializeField] public float resistance { get; set; }
}
