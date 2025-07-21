public class NonCombatBehaviorSet : IEnemyBehaviorSet
{
    public IState<Enemy_NEW> IdleState { get; } = new Normal_Idle();
    public IState<Enemy_NEW> HitState { get; }

    public IState<Enemy_NEW> MoveState => null;

    public IState<Enemy_NEW> ChaseState => null;

    public IState<Enemy_NEW> AttackState => null;

    public IState<Enemy_NEW> DeathState => null;
}
