using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 네비게이션 기능 테스트를 위한 플레이어 대체 CS
/// </summary>
public class CubeMover : MonoBehaviour
{
    [Tooltip("테스트용 시작 위치 (X, Y, Z) - Y 값은 높이로 사용됩니다.")]
    public Vector3 startPosition;
    [Tooltip("테스트용 목표 위치 (X, Y, Z) - Y 값은 무시되고, X와 Z 좌표로 경로 계산합니다.")]
    public Vector3 targetPosition;
    public float moveSpeed = 2f;

    private List<Vector2Int> path;
    private int currentPathIndex;
    private AStarPathfinder pathfinder;

    void Start()
    {
        transform.position = startPosition;
        pathfinder = new AStarPathfinder();
        UpdatePath();
    }

    void Update()
    {
        FollowPath();
    }

    // 시작 위치에서 목표 위치까지 경로 계산 (MapManager의 더미 데이터 사용)
    void UpdatePath()
    {
        Vector2Int startPos = new Vector2Int(Mathf.RoundToInt(startPosition.x), Mathf.RoundToInt(startPosition.z));
        Vector2Int targetPos = new Vector2Int(Mathf.RoundToInt(targetPosition.x), Mathf.RoundToInt(targetPosition.z));
        path = pathfinder.FindPath(startPos, targetPos);
        currentPathIndex = 0;
        if (path.Count == 0)
        {
            Debug.LogWarning("CubeMover: No valid path found from " + startPosition + " to " + targetPosition);
        }
    }

    // 계산된 경로를 따라 Cube 이동
    void FollowPath()
    {
        if (path == null || path.Count == 0 || currentPathIndex >= path.Count)
            return;

        Vector2Int currentTarget = path[currentPathIndex];
        Vector3 targetWorldPos = new Vector3(currentTarget.x, transform.position.y, currentTarget.y);
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWorldPos) < 0.1f)
        {
            currentPathIndex++;
        }
    }

    // Scene 뷰에서 경로를 녹색 선과 와이어 큐브로 표시
    private void OnDrawGizmos()
    {
        // 계산된 경로가 있을 경우 그리기
        if (path != null && path.Count > 0)
        {
            Gizmos.color = Color.green;
            // 시작 위치에서 첫 노드까지 선을 그림
            Vector3 prevPos = new Vector3(Mathf.RoundToInt(startPosition.x), transform.position.y, Mathf.RoundToInt(startPosition.z));
            foreach (Vector2Int node in path)
            {
                Vector3 nodePos = new Vector3(node.x, transform.position.y, node.y);
                Gizmos.DrawLine(prevPos, nodePos);
                Gizmos.DrawWireCube(nodePos, Vector3.one);
                prevPos = nodePos;
            }
            // 마지막 노드에서 목표 위치까지 선을 그림
            Vector3 targetPos = new Vector3(Mathf.RoundToInt(targetPosition.x), transform.position.y, Mathf.RoundToInt(targetPosition.z));
            Gizmos.DrawLine(prevPos, targetPos);
        }
    }
}
