using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TargetPriority/Layer Priority Policy")]
public class LayerPriorityPolicySO : ScriptableObject, ITargetPriorityPolicy
{
    [System.Serializable]
    public struct LayerScore
    {
        public string layer;
        public float score;
    }

    public LayerScore[] scores;

    public Dictionary<int, float> ToMap()
    {
        var dict = new Dictionary<int, float>();
        foreach (var score in scores)
        {
            int layer = LayerMask.NameToLayer(score.layer);
            if (layer != -1) dict[layer] = score.score;
        }
        return dict;
    }

    // ✅ 2단계 핵심: evaluator를 생성하는 팩토리 메서드
    public ITargetPriorityEvaluator CreateEvaluator()
    {
        return new LayerPriorityEvaluator(ToMap());
    }
}
