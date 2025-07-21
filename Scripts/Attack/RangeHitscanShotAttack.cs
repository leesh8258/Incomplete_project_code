using System;
using System.Collections;
using UnityEngine;

public class RangeHitscanShotAttack : RangeAttackBase
{
    [SerializeField] private LineRenderer lineRenderer;

    protected override void PerformAttack()
    {
        for(int i = 0; i < bulletCount; i++)
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        int index = 0;

        if (character is Enemy enemy) index = enemy.GetSpriteManager.GetCardinal() - 1; 
        lineRenderer.SetPosition(0, bulletSpawnPoint[index].position);
        Vector3 shootDirection = ReturnAttackDirection();
        float angleOffset = SetBulletAngleToTarget();

        Vector3 finalDirection = Quaternion.AngleAxis(angleOffset, Vector3.up) * shootDirection;

        Ray ray = new Ray(bulletSpawnPoint[index].position, finalDirection);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, attackRange, targetLayerMask))
        {         
            if (hit.collider.TryGetComponent<HealthyEntity>(out HealthyEntity target))
            {
                lineRenderer.SetPosition(1, hit.point);
                StartCoroutine(ShowAttackGizmo(0.1f));
                target.TakeDamageAction?.Invoke(ReturnAtk(), this);
            }
        }

    }

    private IEnumerator ShowAttackGizmo(float duration)
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(duration);
        lineRenderer.enabled = false;
    }
}
