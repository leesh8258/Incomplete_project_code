using UnityEngine;
using System;

public class Node : IComparable<Node>
{
    public Vector2Int position;
    public bool walkable;
    public float gCost;
    public float hCost;
    public float fCost { get { return gCost + hCost; } }
    public Node parent;

    public Node(Vector2Int pos, bool walkable)
    {
        this.position = pos;
        this.walkable = walkable;
    }

    // fCost가 낮을수록 높은 우선순위를 갖도록 비교
    public int CompareTo(Node other)
    {
        // 기본적으로 fCost 비교 (더 낮은 값이 우선순위가 높음)
        int result = other.fCost.CompareTo(this.fCost);  // fCost가 낮으면 this가 더 큰 값을 반환
        if (result == 0)
        {
            result = other.hCost.CompareTo(this.hCost);
        }
        return result;
    }
}
