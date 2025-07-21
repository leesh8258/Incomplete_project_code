using UnityEngine;

public class MeleeStraightAttack : MeleeAttackBase
{
    RaycastHit[] hitResult = new RaycastHit[10];

    protected override void PerformAttack()
    {
        Vector3 attackDirection = ReturnAttackDirection();
        Vector3 origin = transform.position + Quaternion.LookRotation(attackDirection) * attackPositionOffset;

        Ray ray = new Ray(origin, attackDirection);

        int hitCount = Physics.RaycastNonAlloc(ray, hitResult, attackRange, targetLayerMask);

        for(int i = 0; i < hitCount; i++)
        {
            RaycastHit hit = hitResult[i];
            if (hit.collider.gameObject == this.gameObject) continue;

            if(hit.collider.TryGetComponent<HealthyEntity>(out HealthyEntity target))
            {
                target.TakeDamageAction(ReturnAtk(), this);
            }
        }
    }
}
