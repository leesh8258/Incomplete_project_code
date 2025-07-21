using System;
using UnityEngine;

public class RangeSingleShotAttack : RangeAttackBase
{

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

        if(bullet == null || bulletSpawnPoint == null)
        {
            Debug.LogWarning("총알 Prefab 또는 총알 생성 위치가 설정되지 않았습니다.");
        }
        if (character is Enemy enemy) index = enemy.GetSpriteManager.GetCardinal() - 1;
        Vector3 shootDirection = ReturnAttackDirection();
        float angleOffset = SetBulletAngleToTarget();

        //spawnPoint가 아니라 공격방향 기준 z+1 로 변경예정
        Vector3 finalDirection = Quaternion.AngleAxis(angleOffset, Vector3.up) * shootDirection;

        GameObject newBullet = Instantiate(bullet, bulletSpawnPoint[index].position, Quaternion.identity);
        
        BulletBase bulletComponent = newBullet.GetComponent<BulletBase>();
        if (bulletComponent != null)
        {
            bulletComponent.SetBulletDirection(finalDirection);
            bulletComponent.SetAttackBase(this);
        }
    }
}
