using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeleeFanAttack : MeleeAttackBase
{
    [Header("Attack Fan Property")]
    [SerializeField] private float attackAngle;
    private bool isAttacking = false; //임시
    protected override void PerformAttack()
    {
        StartCoroutine(ShowAttackGizmo(0.1f));

        Vector3 attackDirection = ReturnAttackDirection();
        Vector3 origin = transform.position + Quaternion.LookRotation(attackDirection) * attackPositionOffset;
        int count = Physics.OverlapSphereNonAlloc(origin, attackRange, hitResults, targetLayerMask);
        
        for(int i = 0; i < count; i++)
        {
            Collider hitCollider = hitResults[i];

            if (hitCollider.gameObject == this.gameObject) continue;

            if (hitCollider.TryGetComponent<HealthyEntity>(out HealthyEntity target))
            {
                Vector3 dirToTarget = (hitCollider.transform.position - origin).normalized;
                float angle = Vector3.Angle(attackDirection, dirToTarget);

                if (angle <= attackAngle / 2)
                {
                    target.TakeDamageAction?.Invoke(ReturnAtk(), this);
                }
            }
        }
    }

    private System.Collections.IEnumerator ShowAttackGizmo(float duration)
    {
        isAttacking = true;
        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (isAttacking)
        {
            Vector3 attackDirection = ReturnAttackDirection();
            Vector3 origin = transform.position + Quaternion.LookRotation(attackDirection) * attackPositionOffset;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(origin, attackDirection * attackRange);
        }
    }
#endif
}
