using System;
using UnityEngine;

public class ComparePriority : IComparable<ComparePriority>
{
    public GameObject target { get; set; }
    public float distance { get; set; }
    public int priority { get; set; }

    public ComparePriority(GameObject target, float distance, int priority)
    {
        this.target = target;
        this.distance = distance;
        this.priority = priority;
    }

    public int CompareTo(ComparePriority other)
    {
        // 1. 우선순위 비교 (높은 값 우선)
        if (this.priority != other.priority)
            return other.priority.CompareTo(this.priority);
        // 2. 거리 비교 (가까운 대상 우선)
        if (this.distance != other.distance)
            return this.distance.CompareTo(other.distance);
        // 3. 우선순위와 거리가 동일하면 0 반환 (즉, 먼저 계산된 후보가 선택됨)
        return 0;
    }
}
