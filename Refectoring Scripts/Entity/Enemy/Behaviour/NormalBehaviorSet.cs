using UnityEngine;

public class NormalBehaviorSet : IEnemyBehaviorSet
{
    public IState<Enemy_NEW> IdleState { get; } = new Normal_Idle();

    public IState<Enemy_NEW> MoveState { get; } = null;

    public IState<Enemy_NEW> ChaseState { get; } = new Normal_Chase();

    public IState<Enemy_NEW> AttackState { get; } = null;

    public IState<Enemy_NEW> DeathState { get; } = null;
    public IState<Enemy_NEW> HitState { get; } = null;
}
