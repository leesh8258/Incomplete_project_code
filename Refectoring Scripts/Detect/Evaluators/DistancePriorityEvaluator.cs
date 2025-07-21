using UnityEngine;

public class DistancePriorityEvaluator : ITargetPriorityEvaluator
{
    public float EvaluatePriority(TargetContext context, Enemy_NEW self)
    {
        return 1f / (context.sqrDistance + 0.01f);
    }
}
