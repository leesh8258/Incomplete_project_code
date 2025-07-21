using System;
using UnityEngine;

public abstract class ProjectileHandler : MonoBehaviour, IAttackHandler
{
    [SerializeField] private AttackDataSO attackData;
    private Enemy_NEW enemy;
    protected Vector3 targetPosition;

    public AttackDataSO AttackData => attackData;

    public void Initialize(Enemy_NEW enemy, AttackDataSO data)
    {
        this.enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        attackData = data;
    }

    protected void InitializeProjectile(GameObject projectile, Vector3 direction)
    {
        if(projectile.TryGetComponent<ProjectileBase>(out ProjectileBase component))
        {
            component.Initialize(attackData, enemy.GetStat(StatAttributeType.RangeAttackPower), direction);
        }
    }
    
    public abstract void Execute();
    protected abstract Vector3 SetProjectilesDirection(Vector3 start, Vector3 target);

    public void SetAttackDirection(Vector3 target)
    {
        targetPosition = target;
    }

}
