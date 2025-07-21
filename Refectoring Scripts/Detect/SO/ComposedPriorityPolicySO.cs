using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TargetPriority/ComposedPolicy")]
public class ComposedPriorityPolicySO : ScriptableObject, ITargetPriorityPolicy
{
    [SerializeField]
    private List<WeightedEvaluatorEntry> entries = new();

    public ITargetPriorityEvaluator CreateEvaluator()
    {
        List<(ITargetPriorityEvaluator evaluator, float weight)> evaluators = new();

        foreach (var entry in entries)
        {
            if (entry.evaluatorPolicySO is ITargetPriorityPolicy policy)
            {
                var evaluator = policy.CreateEvaluator();
                evaluators.Add((evaluator, entry.weight));
            }
            else
            {
                Debug.LogWarning($"Invalid evaluator policy SO: {entry.evaluatorPolicySO.name}");
            }
        }

        return new ComposedPriorityEvaluator(evaluators);
    }
}
