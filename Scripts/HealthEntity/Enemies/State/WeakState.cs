using UnityEngine;

public class WeakState : IState
{
    private Enemy enemy;

    public WeakState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void OnEnter()
    {
        enemy.GetSpriteManager.PlayAnimationToTrigger("Weak");
        enemy.GetSpriteManager.StartParticleSystems("Weak");
    }


    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        enemy.GetSpriteManager.StopParticleSystems("Weak");
    }
}
