
using UnityEngine;

public class DeathState : IState
{
    private Enemy enemy;

    public DeathState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void OnEnter()
    {
        enemy.GetCharacterCollider.enabled = false;
        enemy.StopAllCharacterCoroutines();

        // Death 애니메이션 재생
        enemy.GetSpriteManager.PlayAnimationToTrigger("Death");
        enemy.GetSpriteManager.StartParticleSystems("Death");

    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
        enemy.GetCharacterCollider.enabled = true;
        enemy.GetSpriteManager.Appear();
    }
}
