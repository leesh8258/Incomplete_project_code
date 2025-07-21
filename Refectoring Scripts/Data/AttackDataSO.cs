using UnityEngine;

public enum AttackType
{
    Melee,
    Ranged
}

[CreateAssetMenu(fileName = "NewAttackData", menuName = "Combat/Attack Data", order = 0)]
public class AttackDataSO : ScriptableObject
{
    public AttackType attackType;

    #region 공통 관련
    public float attackCoefficient;
    public float attackRange;
    public float hitTiming;
    public float totalDelay;
    #endregion

    #region 연속 공격 관련
    public bool isPersistent;
    public float persistDuration;
    public float persistInterval;
    #endregion

    #region 원거리 공격 관련
    public GameObject projectilePrefab;
    public Transform[] bulletSpawnPoint;
    public int bulletCount;
    [Range(0f, 1f)] public float precision;
    #endregion

    public AttackBuffEffectData[] effectsToApply;
    public AttackGroupBuffEffectData[] groupEffectsToApply;
}
