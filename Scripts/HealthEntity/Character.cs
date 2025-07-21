using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BuffSystem))]
[RequireComponent(typeof(SpriteManager))]
[RequireComponent(typeof(DamageDisplay))]
public abstract class Character : HealthyEntity
{
    protected DamageDisplay damageDisplay;
    protected BuffSystem buffSystem;
    protected SpriteManager spriteManager;
    protected Collider characterCollider;
    protected Coroutine IsInveulnerable;

    //protected Enemy_UI Enemy_UI;

    public abstract CharacterStatBase CharacterStat { get; }
    public bool interruptAction;
    public SpriteManager GetSpriteManager => spriteManager;
    public Collider GetCharacterCollider => characterCollider;

    protected virtual void Awake()
    {
        buffSystem = GetComponent<BuffSystem>();
        spriteManager = GetComponent<SpriteManager>();
        damageDisplay = GetComponent<DamageDisplay>();

        characterCollider = GetComponent<Collider>();
        interruptAction = false;

        TakeDamageAction += TakeDamage;

        OnDeathAction += OnDeath;
    }

    public void ApplyTemporaryBuff(BuffType type, float duration)
    {
        Buff tempBuff = BuffFactory.CreateBuff(type, duration);
        buffSystem.AddBuff(tempBuff);
    }

    //protected float HpValue()
    //{
    //    return currentHP / CharacterStat.HP;
    //}

    public override void TakeDamage(float damage, AttackBase attack)
    {
        if (IsInveulnerable == null) IsInveulnerable = StartCoroutine(ResetInvulnerability(0.2f));

        float basicDamage = Mathf.Max((damage - CharacterStat.defense) * UnityEngine.Random.Range(0.9f, 1.1f), 0f);
        float finalDamage;

        if (attack != null) finalDamage = attack.CalculateFinalDamage(basicDamage);
        else finalDamage = basicDamage;

        float dmg = finalDamage * (1 - CharacterStat.defense / (CharacterStat.defense + 100));
        damageDisplay.InstantiateDamageGUI(dmg, attack);
        if (IsDeath(dmg)) OnDeathAction?.Invoke();

        //if(TryGetComponent<Enemy>(out _)) Enemy_UI.SetHealth(HpValue());
    }

    protected IEnumerator ResetInvulnerability(float duration)
    {
        spriteManager.FlashDamageEffect();

        characterCollider.enabled = false;
        yield return new WaitForSeconds(duration);
        characterCollider.enabled = true;

        IsInveulnerable = null;
    }

    public void StopAllCharacterCoroutines()
    {
        MonoBehaviour[] components = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour comp in components)
        {
            comp.StopAllCoroutines();
        }
    }
        
    public abstract void ApplyStatBoost(Action<Character> action);
}
