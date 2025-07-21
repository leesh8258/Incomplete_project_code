using System;
using System.Collections;
using UnityEngine;

public class Enemy_0_006 : Enemy
{
    private IdleState idleState;
    private ChaseState chaseState;
    private AttackStartState attackStartState;
    private AttackEndState attackEndState;
    private DeathState deathState;

    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState(this);
        chaseState = new ChaseState(this);
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
        if (detectTarget.currentTarget == null) return;

        if (detectTarget.IsExistBlockBetweenTarget())
        {
            nextAttackindex = 3;
        }

        else
        {
            nextAttackindex = 1;
        }

    }

    public override void InterruptAction() { }
}
