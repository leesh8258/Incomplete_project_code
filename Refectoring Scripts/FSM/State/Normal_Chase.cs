using UnityEngine;

public class Normal_Chase : IState<Enemy_NEW>
{
    public void OnEnter(Enemy_NEW owner)
    {
        if(owner.GetCurrentTarget() == null)
        {
            owner.ToIdleState();
        }
    }

    public void OnExit(Enemy_NEW owner)
    {
    }

    public void OnUpdate(Enemy_NEW owner)
    {
        var target = owner.GetCurrentTarget();
        if (target == null)
        {
            owner.ToIdleState();
            return;
        }

    }
}
