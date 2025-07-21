using System.Collections.Generic;

public class LayerPriorityEvaluator : ITargetPriorityEvaluator
{
    private readonly Dictionary<int, float> layerScores;

    public LayerPriorityEvaluator(Dictionary<int, float> layersScores)
    {
        this.layerScores = layersScores;
    }

    public float EvaluatePriority(TargetContext context, Enemy_NEW self)
    {
        if (!layerScores.TryGetValue(context.target.layer, out float baseScore)) return 0f;

        float distanceScore = -context.sqrDistance;

        return baseScore + distanceScore;
    }
}
