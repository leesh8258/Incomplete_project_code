using System.Collections;
using UnityEngine;

public class HitState : IState
{
    private Enemy enemy;
    private Coroutine hitCoroutine;
    private float lastHitTime;
    private float hitStateDuration = 1.0f;
    private bool isRecovering = false;

    public HitState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void OnEnter()
    {
        lastHitTime = Time.time;

        enemy.GetSpriteManager.PlayAnimationToTrigger("Hit");

        isRecovering = false;
        hitCoroutine = enemy.StartCoroutine(RecoverAfterDelay());
    }

    public void OnUpdate()
    {
        if(Time.time - lastHitTime <= hitStateDuration)
        {
            return;
        }

        if (!isRecovering)
        {
            isRecovering = true;
            enemy.ChangeToIdleState();
        }
    }

    public void OnExit()
    {
    }

    public void RegisterHit()
    {
        lastHitTime = Time.time;

        if(hitCoroutine != null)
        {
            enemy.StopCoroutine(hitCoroutine);
        }

        hitCoroutine = enemy.StartCoroutine(RecoverAfterDelay());
    }

    private IEnumerator RecoverAfterDelay()
    {
        yield return new WaitForSeconds(hitStateDuration);

        if (!isRecovering)
        {
            isRecovering = true;
            enemy.ChangeToIdleState();
        }
    }
}
