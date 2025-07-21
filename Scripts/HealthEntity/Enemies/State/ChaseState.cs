using UnityEngine;

public class ChaseState : IState
{
    private Enemy enemy;
    private bool IsObjectExist;

    public ChaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void OnEnter()
    {
        IsObjectExist = true;
        enemy.StopInvoke();
        enemy.GetSpriteManager.PlayAnimationToBoolean("Chase", true);
        enemy.GetSpriteManager.StartParticleSystems("Chase");
        enemy.StartFindAttackTargetCoroutine();

        enemy.GetMove.StartRepeatUpdatePath();
    }

    public void OnUpdate()
    {
        if (!enemy.GetDetectTarget.IsTargetExist(enemy.GetDetectTarget.currentTarget) &&
            !enemy.GetDetectTarget.IsTargetExist(enemy.GetDetectTarget.finalTarget) &&
            IsObjectExist)
        {
            IsObjectExist = false;
            enemy.ChangeToIdleState();
        }

        else
        {
            enemy.GetMove.FollowPath();
        }
    }

    public void OnExit()
    {
        enemy.StopFindAttackTargetCoroutine();
        enemy.GetMove.StopRepeatUpdatePath();
        enemy.GetSpriteManager.PlayAnimationToBoolean("Chase", false);
        enemy.GetSpriteManager.StopParticleSystems("Chase");
    }

}
