using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy_NEW))]
public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] private List<IAttackHandler> attackHandlers = new();
    [SerializeField] private List<AttackDataSO> attackDataSO = new();

    private Animator animator;
    private Enemy_NEW enemy;
    private GameObject currentTarget;
    private int currentAttackDataIndex;

    private void Awake()
    {
        if (attackHandlers.Count != attackDataSO.Count)
        {
            Debug.LogError("Attack handler와 SO 개수가 일치하지 않습니다!");
            return;
        }

        enemy = GetComponent<Enemy_NEW>();
        animator = enemy.GetComponent<Animator>();
        for(int i = 0; i < attackHandlers.Count; i++)
        {
            attackHandlers[i].Initialize(enemy, attackDataSO[i]);
        }
    }

    public void UseAttackSkill(int index, GameObject target)
    {
        currentAttackDataIndex = index;
        currentTarget = target;

        animator.SetTrigger("Attack");
        StartCoroutine(DelayedHitAndEnd());
    }

    private IEnumerator DelayedHitAndEnd()
    {
        float hitTiming = attackDataSO[currentAttackDataIndex].hitTiming;
        
        yield return new WaitForSeconds(hitTiming);
        attackHandlers[currentAttackDataIndex].SetAttackDirection(currentTarget.transform.position);
        attackHandlers[currentAttackDataIndex].Execute();

        float delay = attackDataSO[currentAttackDataIndex].totalDelay - hitTiming;
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        enemy.ToChaseState();
    }

}
