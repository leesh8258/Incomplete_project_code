using UnityEngine;

public class AttackEndState : IState
{
    private Enemy enemy;
    private float duration;
    private float lastTime;
    private bool isFlag;

    public AttackEndState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void OnEnter()
    {
        enemy.GetSpriteManager.PlayAnimationToBoolean("Idle", true);
        enemy.GetSpriteManager.StartParticleSystems("Idle");
        duration = enemy.attackEndDuration;
        lastTime = Time.time;
        isFlag = false;
    }

    public void OnUpdate()
    {
        if (Time.time - lastTime < duration) return;

        if (!isFlag)
        {
            isFlag = true;
            if(!enemy.StartFindAttackTargetOnce()) enemy.ChangeToChaseState();
        }
    }

    public void OnExit()
    {
        enemy.GetSpriteManager.PlayAnimationToBoolean("Idle", false);
        enemy.GetSpriteManager.StopParticleSystems("Idle");
    }
}
