using UnityEngine;

public abstract class RangeAttackBase : AttackBase
{
    [Header("Bullet Spawn Position")]
    [SerializeField] protected Transform[] bulletSpawnPoint;

    [Header("Bullet Prefab")]
    [SerializeField] protected GameObject bullet;
    
    [Header("Bullet Count")]
    [SerializeField] protected int bulletCount;

    [Header("Bullet Precision")]
    [SerializeField, Range(0.0f, 1.0f)] protected float precision;

    private readonly float MAXANGLE = 60f;

    protected float SetBulletAngleToTarget()
    {
        float deviation = (MAXANGLE - (MAXANGLE * precision));
        float angle = Random.Range(-deviation, deviation);
        return angle;
    }

    public override float ReturnAtk()
    {
        return character.CharacterStat.rangeAttackPower;
    }
}
