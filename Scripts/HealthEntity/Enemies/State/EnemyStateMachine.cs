using System.Collections;
using UnityEngine;

public class EnemyStateMachine
{
    private IState currentState;
    public IState GetCurrentState => currentState;
    private Enemy enemy;

    public EnemyStateMachine(Enemy enemy)
    {
        this.enemy = enemy;
    }

    private IEnumerator ChangeStateCoroutine(IState newState)
    {
        yield return null;
        
        if (currentState is HitState hitState && newState is HitState)
        {
            hitState.RegisterHit();
        }

        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }


    public void ChangeState(IState newState)
    {
        enemy.StartCoroutine(ChangeStateCoroutine(newState));
    }

    public void UpdateState()
    {
        currentState?.OnUpdate();
    }
}
