using UnityEngine;

public struct TargetContext
{
    public GameObject target { get; }
    public Vector3 position { get; }
    public float sqrDistance { get; }
    public float score { get; private set; }

    public TargetContext(GameObject target, Vector3 self)
    {
        this.target = target;
        position = target.transform.position;
        sqrDistance = (position - self).sqrMagnitude;
        score = 0f;
    }

    public void SetScore(float score)
    {
        this.score = score;
    }
}
