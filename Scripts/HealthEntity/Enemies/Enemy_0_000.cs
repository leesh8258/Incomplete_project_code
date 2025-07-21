public class Enemy_0_000 : Enemy
{
    private IdleState idleState;
    private HitState hitState;
    
    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState(this);
        hitState = new HitState(this);
    }

    public override void TakeDamage(float damage, AttackBase attackType)
    {
        base.TakeDamage(damage, attackType);

        if (stateMachine.GetCurrentState is HitState) hitState.RegisterHit();
        else stateMachine.ChangeState(hitState);
    }

    public override void ChangeToIdleState() { stateMachine.ChangeState(idleState); }
    public override void ChangeToMoveState() { }
    public override void ChangeToChaseState() { }
    public override void ChangeToAttackPrepareState() { }
    public override void ChangeToAttackEndState() { }
    public override void ChangeToStunState() { }
    protected override void OnDeath() { }
    public override void ChooseAttackPattern() { }
    public override void InterruptAction() { }
}
