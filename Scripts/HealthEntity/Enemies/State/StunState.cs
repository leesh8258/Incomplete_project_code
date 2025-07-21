using UnityEngine;

public class StunState : IState
{
    private Enemy enemy;
    
    public StunState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void OnEnter()
    {
        enemy.interruptAction = false;
        enemy.GetSpriteManager.PlayAnimationToTrigger("Stun");
        enemy.GetSpriteManager.StartParticleSystems("Stun");
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
        enemy.GetSpriteManager.StopParticleSystems("Stun");
    }

}
