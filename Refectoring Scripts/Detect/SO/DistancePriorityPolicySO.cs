using UnityEngine;

[CreateAssetMenu(menuName = "TargetPriority/DistancePriorityPolicy")]
public class DistancePriorityPolicySO : ScriptableObject, ITargetPriorityPolicy
{
    public ITargetPriorityEvaluator CreateEvaluator()
    {
        return new DistancePriorityEvaluator();
    }
}