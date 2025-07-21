using UnityEngine;

public interface ITargetPriorityPolicy
{
    ITargetPriorityEvaluator CreateEvaluator();
}
