using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 네비게이션 기능 테스트를 위한 임시 맵 생성 CS
/// </summary>
public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    // 각 좌표에 설치된 GameObject (null이면 이동 가능)
    private Dictionary<Vector2Int, GameObject> mapData = new Dictionary<Vector2Int, GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeDummyMapData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDummyMapData()
    {
        int gridSize = 20;
        // 초기화
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector2Int coord = new Vector2Int(x, z);
                mapData[coord] = null;
            }
        }

        // 외곽 벽: 모든 가장자리 좌표에 장애물 설치
        for (int x = 0; x < gridSize; x++)
        {
            mapData[new Vector2Int(x, 0)] = new GameObject("Obstacle");
            mapData[new Vector2Int(x, gridSize - 1)] = new GameObject("Obstacle");
        }
        for (int z = 0; z < gridSize; z++)
        {
            mapData[new Vector2Int(0, z)] = new GameObject("Obstacle");
            mapData[new Vector2Int(gridSize - 1, z)] = new GameObject("Obstacle");
        }

        // 내부 미로 패턴: 짝수 행마다 장애물을 설치하되 하나의 갭을 남김
        // 예: z = 2, 4, 6, ... gridSize-2
        for (int z = 2; z < gridSize - 1; z += 2)
        {
            // 갭(gap) 위치는 행에 따라 번갈아가며 설정 (예: 짝수 행은 좌측에, 홀수 행은 우측에)
            int gap = (z % 4 == 0) ? 2 : gridSize - 3;
            for (int x = 1; x < gridSize - 1; x++)
            {
                if (x == gap)
                    continue;
                Vector2Int coord = new Vector2Int(x, z);
                mapData[coord] = new GameObject("Obstacle");
            }
        }

        Debug.Log("Maze dummy map data (20x20) initialized.");
    }


    // 지정 좌표가 이동 가능한지 확인 (데이터가 없으면 false)
    public bool IsWalkable(Vector2Int coord)
    {
        if (mapData.ContainsKey(coord))
        {
            return mapData[coord] == null;
        }
        else
        {
            Debug.LogWarning("Map data for coordinate " + coord + " is not loaded!");
            return false;
        }
    }

    // Scene 뷰에서 장애물을 빨간 큐브로 표시
    private void OnDrawGizmos()
    {
        if (mapData == null || mapData.Count == 0)
            return;

        Gizmos.color = Color.red;
        foreach (var kvp in mapData)
        {
            // 장애물이 존재하면 해당 좌표에 큐브를 그림 (높이는 0.5로 설정)
            if (kvp.Value != null)
            {
                Vector3 pos = new Vector3(kvp.Key.x, 0.5f, kvp.Key.y);
                Gizmos.DrawCube(pos, Vector3.one * 0.9f);
            }
        }
    }
}
