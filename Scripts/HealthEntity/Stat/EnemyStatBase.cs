using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStatBase : CharacterStatBase
{
    [field : SerializeField, Header("적 전용 스탯")] public string enemyName { get; set; }
    [field : SerializeField] public int ID { get; set; }
    [field: SerializeField] public EnemyTier enemyTier { get; set; }
    [field: SerializeField] public EnemyAffiliation enemyAffiliation { get; set; }
    [field: SerializeField] public float weight { get; set; }
    [field: SerializeField] public List<BuffType> permanentBuffs { get; set; }
    [field: SerializeField] public bool IsExpolosionResist { get; set; }
    [field: SerializeField] public bool IsUndying { get; set; }
    [field: SerializeField] public bool IsKnockbackResist { get; set; }
}
