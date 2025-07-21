
public class Enemy_0_005 : Enemy
{
    private IdleState idleState;
    private MoveState moveState;
    private ChaseState chaseState;
    private AttackStartState attackStartState;
    private AttackEndState attackEndState;
    private DeathState deathState;

    
    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState(this);
        chaseState = new ChaseState(this);
        moveState = new MoveState(this);
        attackStartState = new AttackStartState(this);
        attackEndState = new AttackEndState(this);
        deathState = new DeathState(this);

    }

    public override void ChangeToIdleState()
    {
        stateMachine.ChangeState(idleState);
        StartInvoke(move.GetNextMoveTime, ChangeToMoveState);
    }

    public override void ChangeToMoveState()
    {
        stateMachine.ChangeState(moveState);
    }

    public override void ChangeToChaseState()
    {
        stateMachine.ChangeState(chaseState);
    }

    public override void ChangeToAttackPrepareState()
    {
        stateMachine.ChangeState(attackStartState);
    }

    public override void ChangeToAttackEndState()
    {
        stateMachine.ChangeState(attackEndState);
    }

    public override void ChangeToStunState() { }

    protected override void OnDeath()
    {
        stateMachine.ChangeState(deathState);
    }

    public override void ChooseAttackPattern()
    {
    }

    public override void InterruptAction()
    {
    }
}
