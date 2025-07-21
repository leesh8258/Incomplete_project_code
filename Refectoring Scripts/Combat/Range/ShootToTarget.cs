using UnityEngine;

public class ShootToTarget : ProjectileHandler
{
    public override void Execute()
    {
        for(int i = 0; i < AttackData.bulletCount; i++)
        {
            foreach(var position in AttackData.bulletSpawnPoint)
            {
                GameObject projectile = Instantiate(AttackData.projectilePrefab, position.position, Quaternion.identity);
                Vector3 direction = SetProjectilesDirection(position.position, targetPosition);
                InitializeProjectile(projectile, direction);
                
            }
        }
    }

    protected override Vector3 SetProjectilesDirection(Vector3 start, Vector3 target)
    {
        return (target - start).normalized;
    }
}
