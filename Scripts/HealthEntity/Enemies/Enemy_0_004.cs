using UnityEngine;

public class Enemy_0_004 : Enemy
{
    private IdleState idleState;
    private AttackStartState attackStartState;
    private AttackEndState attackEndState;
    private DeathState deathState;

    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState(this);
        attackStartState = new AttackStartState(this);
        attackEndState = new AttackEndState(this);
        deathState = new DeathState(this);

    }

    public override void ChangeToIdleState()
    {
        stateMachine.ChangeState(idleState);
    }

    public override void ChangeToMoveState() { }

    public override void ChangeToChaseState()
    {
        StartFindAttackTargetCoroutine();
    }

    public override void ChangeToAttackPrepareState()
    {
        StopFindAttackTargetCoroutine();
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

    public override void ChooseAttackPattern() { }

    public override void InterruptAction() { }
}
