using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 네비게이션 기능을 위한 A*알고리즘
/// </summary>
public class AStarPathfinder
{
    // 미리 정의한 오프셋 배열
    private readonly Vector2Int[] offsets = {
        new Vector2Int(-1,-1), new Vector2Int(0,-1),  new Vector2Int(1,-1),
        new Vector2Int(-1, 0),                        new Vector2Int(1, 0),
        new Vector2Int(-1, 1),  new Vector2Int(0, 1), new Vector2Int(1, 1)
    };

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        PriorityQueue<Node> openQueue = new PriorityQueue<Node>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

        Node startNode = new Node(start, IsWalkable(start));
        Node targetNode = new Node(target, IsWalkable(target));

        if (!startNode.walkable)
        {
            Debug.LogError("Start node is not walkable!");
            return new List<Vector2Int>();
        }

        if (!targetNode.walkable)
        {
            Vector2Int newTarget = GetNearestWalkableCell(start, target);
            if (newTarget != target)
            {
                Debug.Log("Target node is not walkable, using nearest walkable cell: " + newTarget);
                target = newTarget;
                targetNode = new Node(target, true); // walkable한 셀로 설정
            }

            else
            {
                Debug.LogError("Target node is not walkable and no adjacent walkable cell found!");
                return new List<Vector2Int>();
            }
        }

        openQueue.Push(startNode);

        while (openQueue.Count() > 0)
        {
            Node currentNode = openQueue.Pop();

            if (closedSet.Contains(currentNode.position))
                continue;
            closedSet.Add(currentNode.position);

            if (currentNode.position == targetNode.position)
            {
                List<Vector2Int> path = RetracePath(startNode, currentNode);
                Debug.Log("Optimized Path found: " + path.Count + " nodes.");
                return path;
            }

            foreach (Vector2Int offset in offsets)
            {
                Vector2Int neighborPos = currentNode.position + offset;

                if (!IsWalkable(neighborPos))
                    continue;

                if (closedSet.Contains(neighborPos))
                    continue;

                float tentativeGCost = currentNode.gCost + GetDistance(currentNode.position, neighborPos);

                // 임시로 이웃 노드를 생성
                Node neighborNode = new Node(neighborPos, true)
                {
                    hCost = GetDistance(neighborPos, targetNode.position)
                };

                // openQueue에 이미 존재하는지 확인
                if (openQueue.Contains(neighborNode))
                {
                    // 기존에 있던 노드를 가져옴
                    Node existingNode = openQueue.Get(neighborNode);
                    if (tentativeGCost < existingNode.gCost)
                    {
                        existingNode.gCost = tentativeGCost;
                        existingNode.parent = currentNode;
                        openQueue.DecreaseKey(existingNode);
                    }
                }

                else
                {
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.parent = currentNode;
                    openQueue.Push(neighborNode);
                }
            }
        }

        Debug.LogWarning("Optimized: No path found from " + start + " to " + target);
        return new List<Vector2Int>();
    }

    /// <summary>
    /// 장애물을 피해 갈 수 없을 경우, ground만 존재하면 장애물을 무시하고
    /// 장애물을 뚫고 갈 수 있도록 경로를 찾는 함수
    /// </summary>
    public List<Vector2Int> FindPathBreakingObstacles(Vector2Int start, Vector2Int target)
    {
        PriorityQueue<Node> openQueue = new PriorityQueue<Node>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

        // IsFloorAvailable()를 사용하여, 바닥이 있는지만 체크
        Node startNode = new Node(start, IsFloorAvailable(start));
        Node targetNode = new Node(target, IsFloorAvailable(target));

        if (!startNode.walkable)
        {
            Debug.LogError("Start node has no floor!");
            return new List<Vector2Int>();
        }
        if (!targetNode.walkable)
        {
            Debug.LogError("Target node has no floor!");
            return new List<Vector2Int>();
        }

        openQueue.Push(startNode);

        while (openQueue.Count() > 0)
        {
            Node currentNode = openQueue.Pop();

            if (closedSet.Contains(currentNode.position))
                continue;
            closedSet.Add(currentNode.position);

            if (currentNode.position == targetNode.position)
            {
                List<Vector2Int> path = RetracePath(startNode, currentNode);
                Debug.Log("Path found through obstacles: " + path.Count + " nodes.");
                return path;
            }

            foreach (Vector2Int offset in offsets)
            {
                Vector2Int neighborPos = currentNode.position + offset;

                if (!IsFloorAvailable(neighborPos))
                    continue;
                if (closedSet.Contains(neighborPos))
                    continue;

                float tentativeGCost = currentNode.gCost + GetDistance(currentNode.position, neighborPos);

                Node neighborNode = new Node(neighborPos, true)
                {
                    hCost = GetDistance(neighborPos, targetNode.position)
                };

                if (openQueue.Contains(neighborNode))
                {
                    Node existingNode = openQueue.Get(neighborNode);
                    if (tentativeGCost < existingNode.gCost)
                    {
                        existingNode.gCost = tentativeGCost;
                        existingNode.parent = currentNode;
                        openQueue.DecreaseKey(existingNode);
                    }
                }
                else
                {
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.parent = currentNode;
                    openQueue.Push(neighborNode);
                }
            }
        }

        Debug.LogWarning("No path found even when breaking obstacles from " + start + " to " + target);
        return new List<Vector2Int>();
    }

    /// <summary>
    /// 목표 좌표(pos)에 대해 바닥(ground)가 존재하는지만 검사
    /// WorldManager의 blockDataDictionary에 y=0 높이의 데이터가 있으면 true
    /// </summary>
    private bool IsFloorAvailable(Vector2Int pos)
    {
        Vector3Int groundPos = new Vector3Int(pos.x, 0, pos.y);
        return WorldManager.Instance.CheckBlockData(groundPos);
    }

    private Vector2Int GetNearestWalkableCell(Vector2Int start, Vector2Int target)
    {
        // start와 target 간의 방향 (정규화된 벡터)
        Vector2 direction = ((Vector2)start - (Vector2)target).normalized;

        Debug.Log(start);
        Debug.Log(target);

        // offsets 배열을 List로 복사
        List<Vector2Int> sortedOffsets = new List<Vector2Int>(offsets);

        // 각 offset을 (target에서 상대적인) 방향과의 내적을 기준으로 내림차순 정렬 
        // (즉, 내적 값이 클수록, target에서 바라보는 방향과 일치하는 offset)
        sortedOffsets.Sort((a, b) =>
        {
            // a, b를 Vector2로 변환 후 정규화
            float dpA = Vector2.Dot(new Vector2(a.x, a.y).normalized, direction);
            float dpB = Vector2.Dot(new Vector2(b.x, b.y).normalized, direction);
            return dpB.CompareTo(dpA);  // 내적이 큰 값이 앞에 오도록 정렬
        });

        // 정렬된 순서대로 target의 인접 셀을 검사
        foreach (Vector2Int offset in sortedOffsets)
        {
            Vector2Int neighbor = target + offset;
            if (IsWalkable(neighbor))
            {
                Debug.Log(neighbor);
                return neighbor;
            }
        }

        // 만약 우선 순위대로 검사해도 walkable한 셀이 없다면 target 그대로 반환
        return target;
    }


    private bool IsWalkable(Vector2Int pos)
    {
        Vector3Int groundPos = new Vector3Int(pos.x, 0, pos.y);
        Vector3Int wallPos = new Vector3Int(pos.x, 1, pos.y);

        if (WorldManager.Instance.CheckBlockData(groundPos) &&
            !WorldManager.Instance.CheckBlockData(wallPos))
            return true;

        return false;
    }

    private List<Vector2Int> RetracePath(Node startNode, Node endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    // 두 좌표 간의 이동 비용 계산 (대각선: 1.41, 수평/수직: 1)
    private float GetDistance(Vector2Int a, Vector2Int b)
    {
        float randomness = Random.Range(0f, 0.3f); // 적당한 랜덤 가중치
        int dstX = Mathf.Abs(a.x - b.x);
        int dstY = Mathf.Abs(a.y - b.y);
        if (dstX > dstY)
            return 1.41f * dstY + 1f * (dstX - dstY) + randomness;
        return 1.41f * dstX + 1f * (dstY - dstX) + randomness;
    }
}
