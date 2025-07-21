using UnityEngine;

public class Normal_Idle : IState<Enemy_NEW>
{
    private float detectInterval = 0.5f;
    private float timer;

    public void OnEnter(Enemy_NEW owner)
    {
        timer = 0f;
        //적을 감지
        //Idle 애니메이션
    }

    public void OnExit(Enemy_NEW owner)
    {
        //적 감지 해제
        //Idle 애니메이션 해제
    }

    public void OnUpdate(Enemy_NEW owner)
    {
        timer += Time.deltaTime;
        if (timer >= detectInterval)
        {
            timer = 0f;
            GameObject target = owner.TargetDetector.DetectTarget();
            if (target != null)
            {
                owner.SetCurrentTarget(target);
                Debug.Log("CurrentTarget : " + owner.GetCurrentTarget().name);
                //owner.ToChaseState();
            }
        }
        //일정 시간 경과 후 Move
    }
}
