using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float currentHP;
    public float CurrentHP => currentHP;

    public event Action<float> OnDamageTaken;
    public event Action OnDied;

    protected virtual void Awake() { }

    public virtual void TakeDamage(float damage)
    {
        currentHP = Mathf.Max(currentHP - damage, 0f);
        OnDamageTaken?.Invoke(damage);
        if (currentHP <= 0f) OnDied?.Invoke();
    }
}
