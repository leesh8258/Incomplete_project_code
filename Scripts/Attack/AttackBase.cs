using System.Collections;
using UnityEngine;

[System.Serializable]
public abstract class AttackBase : MonoBehaviour
{
    protected enum AttackAttributes
    {
        BasicAttack,
        SkillAttack
    }

    protected enum AttackType
    {
        SingleAttack,
        MultipleAttack
    }

    protected Character character;

    [Header("Attack Property")]
    [SerializeField] protected AttackAttributes attackAttributes;
    [SerializeField] protected AttackType attackType;
    [SerializeField, Range(0f, 2f)] protected float attackCoefficient;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackDetectRange;
    [SerializeField] protected Vector3 attackPositionOffset;
    [SerializeField] protected LayerMask targetLayerMask;

    [Header("MultipleAttack Property")]
    [SerializeField] protected float hitIntervalCount;
    [SerializeField] protected int InProgressAttackAnimationFrame;

    [Header("Animation Frame")]
    [SerializeField] protected int BeforeAttackAnimationFrame;
    [SerializeField] protected int AfterAttackAnimationFrame;

    private float targetFrame = 0.0166f;

    public float GetAttackDetectRange { get { return attackDetectRange; } }
    public LayerMask GetTargetLayerMask { get { return targetLayerMask; } }
    public float GetBeforeAttackAnimationTime { get { return WaitForAnimationFrames(BeforeAttackAnimationFrame); } }
    public float GetAfterAttackAnimationTime { get { return WaitForAnimationFrames(AfterAttackAnimationFrame); } }
    public float GetInProgressAttackAnimationTime { get { return WaitForAnimationFrames(InProgressAttackAnimationFrame); } }

    protected abstract void PerformAttack();
    public abstract float ReturnAtk();

    private void Awake()
    {
        if (TryGetComponent(out Character character)) this.character = character;
    }

    protected float WaitForAnimationFrames(int animationFrame)
    {
        float waitTime = animationFrame * targetFrame;

        return waitTime;
    }

    public IEnumerator AttackSequence()
    {
        yield return new WaitForSeconds(GetBeforeAttackAnimationTime);

        switch (attackType)
        {
            case AttackType.SingleAttack:
                PerformAttack();
                break;

            case AttackType.MultipleAttack:
                float elapsedTime = 0f;
                float time = GetInProgressAttackAnimationTime / hitIntervalCount;
                while (elapsedTime < GetInProgressAttackAnimationTime)
                {
                    PerformAttack();
                    yield return new WaitForSeconds(time);
                    elapsedTime += time;
                }
                break;
        }

        yield return new WaitForSeconds(GetAfterAttackAnimationTime);
    }

    private IEnumerator WaitInterrupt(float duration, Enemy enemy)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            if (enemy != null && enemy.interruptAction) yield break;
            yield return null;
        }
    }

    protected bool IsCriticalAttack()
    {
        if (character.CharacterStat is PlayerStatBase stat)
        {
            return Random.Range(0f, 1f) <= stat.criticalChance ? true : false;
        }

        return false;
    }

    public float CalculateFinalDamage(float basicDamage)
    {
        if (attackAttributes is AttackAttributes.BasicAttack)
        {
            float criticalAttackCoefficient = IsCriticalAttack() ? 1f + (character.CharacterStat as PlayerStatBase)?.criticalDamage ?? 0f : 1f;

            return basicDamage * attackCoefficient * criticalAttackCoefficient;
        }

        else
        {
            return basicDamage * attackCoefficient;
        }

    }

    //임시 방향 조정
    protected Vector3 ReturnAttackDirection()
    {
        if (TryGetComponent<Player>(out var player))
        {
            return Vector3.zero;
        }

        if (TryGetComponent<Enemy>(out var enemy))
        {
            return enemy.GetMove.GetMoveDirection;
        }

        return Vector3.zero;
    }
}
