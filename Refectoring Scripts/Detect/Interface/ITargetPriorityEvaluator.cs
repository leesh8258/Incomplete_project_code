using UnityEngine;

public interface ITargetPriorityEvaluator
{
    float EvaluatePriority(TargetContext target, Enemy_NEW self);
}