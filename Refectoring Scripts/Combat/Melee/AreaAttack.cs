using UnityEngine;

public class AreaAttack : MeleeAttackBase_NEW
{
    [SerializeField] private float radius = 3f;
    [SerializeField] private float angle = 90f;

    public override void Execute()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(enemy.transform.position, radius, buffer, hitMask);
        Vector3 forward = enemy.transform.forward;

        for (int i = 0; i < hitCount; i++)
        {
            Vector3 dir = (buffer[i].transform.position - enemy.transform.position).normalized;
            if (Vector3.Angle(forward, dir) <= angle * 0.5f)
            {
                if (buffer[i].TryGetComponent(out Health health))
                {
                    health.TakeDamage(CalculateDamage());
                    AttackEffectTargetUtility.Apply(enemy.gameObject, buffer[i].gameObject, attackData);
                }
            }
        }
    }
}
