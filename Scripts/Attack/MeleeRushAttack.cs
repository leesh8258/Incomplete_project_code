using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MeleeRushAttack : MeleeAttackBase
{
    [Header("RushAttack Settings")]
    [SerializeField] private bool isRushMaintain;
    [SerializeField] private float rushSpeedPerSecond;
    [SerializeField] private float rushDuration;

    private bool isAttacking = false; //임시

    protected override void PerformAttack()
    {
        StartCoroutine(RushAttack());
    }

    private IEnumerator RushAttack()
    {
        Vector3 rushDirection = ReturnAttackDirection();
        float timer = 0f;
        bool stopByCantDestroyBlock = false;

        while (timer < rushDuration)
        {
            if (character.interruptAction) yield break;

            float moveDistance = rushSpeedPerSecond * Time.deltaTime;
            Vector3 origin =  transform.position + Quaternion.LookRotation(rushDirection) * attackPositionOffset;
            stopByCantDestroyBlock = false;

            int hitCount = Physics.OverlapSphereNonAlloc(origin, attackRange, hitResults, targetLayerMask);

            StartCoroutine(ShowAttackGizmo(0.1f));

            for (int i = 0; i < hitCount; i++)
            {
                Collider hitCollider = hitResults[i];

                if (hitCollider.gameObject == this.gameObject) continue;

                if(hitCollider.TryGetComponent<HealthyEntity>(out HealthyEntity target))
                {
                    target.TakeDamageAction?.Invoke(ReturnAtk(), this);

                    if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Block")
                        || hitCollider.gameObject.layer == LayerMask.NameToLayer("Structure")
                        || hitCollider.gameObject.layer == LayerMask.NameToLayer("Center")
                        || hitCollider.gameObject.layer == LayerMask.NameToLayer("Unbreakable"))
                    {
                        if (!isRushMaintain) character.interruptAction = true;
                        stopByCantDestroyBlock = true;
                    }
                }
            }

            if (!stopByCantDestroyBlock)
            {
                transform.position += rushDirection * rushSpeedPerSecond * Time.deltaTime;
            }
            timer += Time.deltaTime;
            yield return null;

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
            Gizmos.DrawWireSphere(origin, attackRange);
        }
    }
#endif
}
