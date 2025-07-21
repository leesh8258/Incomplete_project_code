using UnityEngine;

public class IdleState : IState
{
    private Enemy enemy;

    public IdleState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void OnEnter()
    {
        enemy.GetSpriteManager.PlayAnimationToBoolean("Idle", true);
        enemy.GetSpriteManager.StartParticleSystems("Idle");
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
        enemy.GetSpriteManager.PlayAnimationToBoolean("Idle", false);
        enemy.GetSpriteManager.StopParticleSystems("Idle");
    }
}
