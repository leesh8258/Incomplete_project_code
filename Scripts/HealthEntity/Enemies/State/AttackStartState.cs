using UnityEngine;

public class AttackStartState : IState
{
    private Enemy enemy;

    public AttackStartState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void OnEnter()
    {
        switch(enemy.nextAttackindex)
        {
            case 1:
                enemy.GetSpriteManager.PlayAnimationToTrigger("AttackA");
                enemy.GetSpriteManager.StartParticleSystems("AttackA");
                break;
            case 2:
                enemy.GetSpriteManager.PlayAnimationToTrigger("AttackB");
                enemy.GetSpriteManager.StartParticleSystems("AttackB");
                break;
            case 3:
                enemy.GetSpriteManager.PlayAnimationToTrigger("AttackC");
                enemy.GetSpriteManager.StartParticleSystems("AttackC");
                break;
        }
       
        enemy.GetMove.SetAttackTargetDirection();
        enemy.StartCoroutine(enemy.StartAttackPatternCoroutine());
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
        switch (enemy.nextAttackindex)
        {
            case 1:
                enemy.GetSpriteManager.StopParticleSystems("AttackA");
                break;
            case 2:
                enemy.GetSpriteManager.StopParticleSystems("AttackB");
                break;
            case 3:
                enemy.GetSpriteManager.StopParticleSystems("AttackC");
                break;
        }
    }
}
