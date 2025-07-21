using System;
using UnityEngine;

public abstract class MeleeAttackBase_NEW : MonoBehaviour, IAttackHandler
{
    [SerializeField] protected AttackDataSO attackData;
    [SerializeField] protected Enemy_NEW enemy;
    [SerializeField] protected LayerMask hitMask;

    protected Collider[] buffer = new Collider[10];
    protected Vector3 directionNormalVector;

    public AttackDataSO AttackData => attackData;

    public void Initialize(Enemy_NEW enemy, AttackDataSO data)
    {
        this.enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        attackData = data;
    }

    public float CalculateDamage()
    {
        return enemy.GetStat(StatAttributeType.MeleeAttackPower) * attackData.attackCoefficient * UnityEngine.Random.Range(0.9f, 1.1f);
    }

    public abstract void Execute();

    public void SetAttackDirection(Vector3 target)
    {
        directionNormalVector = (target - enemy.transform.position).normalized;
    }
}
