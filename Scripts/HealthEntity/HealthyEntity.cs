using System;
using UnityEngine;

public abstract class HealthyEntity : MonoBehaviour
{
    [SerializeField] protected float currentHP;

    public float GetCurrentHP => currentHP;

    public Action<float, AttackBase> TakeDamageAction;
    public Action OnDeathAction;

    public abstract void TakeDamage(float damage, AttackBase attackType);
    protected abstract void OnDeath();

    protected bool IsDeath(float damage)
    {
        if(TryGetComponent<Enemy>(out Enemy enemy) && enemy.CharacterStat is EnemyStatBase enemyStat && enemyStat.IsUndying)
        {
            currentHP = enemyStat.HP;
        }

        else
        {
            currentHP = currentHP <= damage ? 0f : currentHP - damage;
        }
        return currentHP <= 0f ? true : false;
    }
}
