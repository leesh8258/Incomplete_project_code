using UnityEngine;

public class Enemy_9_000 : Enemy
{
    private IdleState idleState;
    private MoveState moveState;
    private AttackStartState attackStartState;
    private AttackEndState attackEndState;
    private DeathState deathState;

    private bool flag;
    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState(this);
        moveState = new MoveState(this);
        attackStartState = new AttackStartState(this);
        attackEndState = new AttackEndState(this);
        deathState = new DeathState(this);
    }

    public override void ChangeToAttackEndState()
    {
        stateMachine.ChangeState(attackEndState);
    }

    public override void ChangeToAttackPrepareState()
    {
        StopFindAttackTargetCoroutine();
        stateMachine.ChangeState(attackStartState);
    }

    public override void ChangeToChaseState()
    {
        StartFindAttackTargetCoroutine();
    }

    public override void ChangeToIdleState()
    {
        stateMachine.ChangeState(idleState);
    }

    public override void ChangeToMoveState()
    {
    }

    public override void ChangeToStunState()
    {
    }

    public override void ChooseAttackPattern()
    {
    }

    public override void InterruptAction()
    {
    }

    protected override void OnDeath()
    {
        stateMachine.ChangeState(deathState);
    }
}
