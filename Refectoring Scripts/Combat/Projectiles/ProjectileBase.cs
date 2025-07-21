using UnityEngine;
public abstract class ProjectileBase : MonoBehaviour
{
    private readonly float MAXANGLE = 60f;

    protected AttackDataSO attackData;
    protected Collider[] buffer = new Collider[10];
    protected float enemyRangeAttackPower;
    protected bool IsFire = false;
    protected Vector3 directionNormalVector;
    protected Vector3 shootAngle;

    protected abstract void FireReady();

    public void Initialize(AttackDataSO data, float power, Vector3 normal)
    {
        attackData = data;
        enemyRangeAttackPower = power;
        SetDirectionNormalVector(normal);
        FireReady();
    }

    protected float CalculateDamage()
    {
        return enemyRangeAttackPower * attackData.attackCoefficient * Random.Range(0.9f, 1.1f);
    }

    public void SetShootAngle()
    {
        float deviation = (MAXANGLE - (MAXANGLE * attackData.precision));
        float angle = Random.Range(-deviation, deviation);
        
        Vector3 finalDirection = Quaternion.AngleAxis(angle, Vector3.up) * directionNormalVector;

        shootAngle = finalDirection;
    }

    public void SetDirectionNormalVector(Vector3 normal)
    {
        directionNormalVector = normal;

        if (normal != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(normal);
    }
}
