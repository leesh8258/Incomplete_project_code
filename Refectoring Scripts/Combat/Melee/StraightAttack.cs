using UnityEngine;

public class StraightAttack : MeleeAttackBase_NEW
{
    [SerializeField] private Vector3 boxSize = new Vector3(1f, 1f, 2f);

    public override void Execute()
    {
        Vector3 center = enemy.transform.position + enemy.transform.forward * boxSize.z * 0.5f;
        int hitCount = Physics.OverlapBoxNonAlloc(center, boxSize * 0.5f, buffer, enemy.transform.rotation, hitMask);

        for (int i = 0; i < hitCount; i++)
        {
            if (buffer[i].TryGetComponent(out Health health))
                health.TakeDamage(CalculateDamage());
        }
    }
}
