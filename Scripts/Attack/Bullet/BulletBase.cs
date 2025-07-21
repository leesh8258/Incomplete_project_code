using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float bulletDuration;
    [SerializeField] protected BuffType buffType;
    [SerializeField] protected float buffDuration;
    [SerializeField] protected bool IsPierce;
    [SerializeField] protected AttackBase attackBase;

    protected Vector3 bulletToTargetDirection;

    public void SetBulletDirection(Vector3 direction)
    {
        bulletToTargetDirection = direction.normalized;
    }

    public void SetAttackBase(AttackBase attack)
    {
        attackBase = attack;
    }

    protected abstract void BulletMove();
}
