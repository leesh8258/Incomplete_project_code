public interface IEnemyBehaviorSet
{
    IState<Enemy_NEW> IdleState { get; }
    IState<Enemy_NEW> MoveState { get; }
    IState<Enemy_NEW> ChaseState { get; }
    IState<Enemy_NEW> AttackState { get; }
    IState<Enemy_NEW> DeathState { get; }
    IState<Enemy_NEW> HitState { get; }

}