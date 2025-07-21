using System.Collections;
using UnityEngine;

public class Enemy_0_002 : Enemy
{
    private IdleState idleState;
    private MoveState moveState;
    private ChaseState chaseState;
    private AttackStartState attackStartState;
    private AttackEndState attackEndState;
    private StunState stunState;
    private DeathState deathState;

    [Header("Stun Duration")]
    [SerializeField] private float stunDuration = 4.0f;

    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState(this);
        moveState = new MoveState(this);
        chaseState = new ChaseState(this);
        attackStartState = new AttackStartState(this);
        attackEndState = new AttackEndState(this);
        stunState = new StunState(this);
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

    public override void ChangeToStunState()
    {
        StartInvoke(stunDuration, ChangeToAttackEndState);
        stateMachine.ChangeState(stunState);
    }

    protected override void OnDeath()
    {
        stateMachine.ChangeState(deathState);
    }

    public override void ChooseAttackPattern() { }

    public override void InterruptAction()
    {
        ChangeToStunState();
    }
}
