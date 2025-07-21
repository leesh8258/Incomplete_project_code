using UnityEngine;

public class MoveState : IState
{
    private Enemy enemy;

    private float timer;
    private float moveDuration;

    private bool isChangingState;

    public MoveState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void OnEnter()
    {
        timer = 0f;
        moveDuration = enemy.GetMove.GetMoveDuration;
        isChangingState = false;

        enemy.GetSpriteManager.PlayAnimationToBoolean("Move", true);  // 이동 애니메이션 실행
        enemy.GetSpriteManager.StartParticleSystems("Move");

        enemy.GetMove.UpdatePath();
    }

    public void OnUpdate()
    {
        if (isChangingState) return;

        timer += Time.deltaTime;

        enemy.GetMove.FollowPath();

        if (timer >= moveDuration)
        {
            isChangingState = true;
            enemy.ChangeToIdleState();
        }
    }

    public void OnExit()
    {
        enemy.GetSpriteManager.PlayAnimationToBoolean("Move", false);
        enemy.GetSpriteManager.StopParticleSystems("Move");
    }

}
