using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour, IEnemyTargetDetector
{
    [SerializeField] private Vector3 boxHalfSize = new Vector3(3f, 2f, 3f);
    [SerializeField] private LayerMask targetMask;

    private Collider[] buffer = new Collider[10];
    private ITargetPriorityEvaluator evaluator;
    private Enemy_NEW self;

    private void Awake()
    {
        self = GetComponent<Enemy_NEW>();
    }

    public void Initialize(ITargetPriorityEvaluator evaluator)
    {
        this.evaluator = evaluator;
    }

    public GameObject DetectTarget()
    {
        var sortedTargets = DetectTargetsSorted();
        return sortedTargets.Count > 0 ? sortedTargets[0].target : null;
    }

    public GameObject[] DetectAllTargets()
    {
        int count = Physics.OverlapBoxNonAlloc(
            transform.position,
            boxHalfSize,
            buffer,
            Quaternion.identity,
            targetMask
        );

        GameObject[] result = new GameObject[count];
        for (int i = 0; i < count; i++)
            result[i] = buffer[i]?.gameObject;

        return result;
    }

    public List<TargetContext> DetectTargetsSorted()
    {
        int count = Physics.OverlapBoxNonAlloc(
            transform.position,
            boxHalfSize,
            buffer,
            Quaternion.identity,
            targetMask
        );

        List<TargetContext> results = new(count);
        for(int i = 0; i < count; i++)
        {
            if (buffer[i] == null) continue;

            TargetContext context = new TargetContext(buffer[i].gameObject, self.transform.position);
            float score = evaluator.EvaluatePriority(context, self);

            context.SetScore(score);
            results.Add(context);
        }

        results.Sort((a, b) => b.score.CompareTo(a.score));
        Debug.Log("정리중");
        return results;
    }

    public bool HasTarget() => DetectTarget() != null;
}
