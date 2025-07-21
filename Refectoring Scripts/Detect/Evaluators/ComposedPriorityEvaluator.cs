using System.Collections.Generic;

public class ComposedPriorityEvaluator : ITargetPriorityEvaluator
{
    private readonly List<(ITargetPriorityEvaluator evaluator, float weight)> evaluators;

    public ComposedPriorityEvaluator(List<(ITargetPriorityEvaluator, float)> evaluators)
    {
        this.evaluators = evaluators;
    }

    public float EvaluatePriority(TargetContext context, Enemy_NEW self)
    {
        float total = 0f;

        foreach (var (evaluator, weight) in evaluators)
        {
            total += evaluator.EvaluatePriority(context, self) * weight;
        }

        return total;
    }
}
