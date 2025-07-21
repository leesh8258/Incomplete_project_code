using UnityEngine;

[System.Serializable]
public class WeightedEvaluatorEntry
{
    public ScriptableObject evaluatorPolicySO; // ex. LayerPriorityPolicySO
    public float weight = 1f;
}