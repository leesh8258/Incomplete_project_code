using UnityEngine;
using System.Collections;

public class DashAttack : MeleeAttackBase_NEW
{
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashSpeed = 10f;

    private bool hasHit;

    public override void Execute()
    {
        enemy.StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        Vector3 target = enemy.transform.position + enemy.transform.forward * dashDistance;
        hasHit = false;

        while (Vector3.Distance(enemy.transform.position, target) > 0.1f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target, dashSpeed * Time.deltaTime);

            if (!hasHit)
            {
                int hitCount = Physics.OverlapSphereNonAlloc(enemy.transform.position, 0.5f, buffer, hitMask);
                for (int i = 0; i < hitCount; i++)
                {
                    if (buffer[i].TryGetComponent(out Health health))
                    {
                        health.TakeDamage(CalculateDamage());
                        hasHit = true;
                        break;
                    }
                }
            }

            yield return null;
        }
    }
}
