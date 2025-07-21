using UnityEngine;

public interface IAttackHandler
{
    void Initialize(Enemy_NEW enemy, AttackDataSO data);
    void Execute();
    void SetAttackDirection(Vector3 target);
}