using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyObjectPool : MonoBehaviour
{
    public static EnemyObjectPool Instance { get; private set; }

    [System.Serializable]
    public class EnemyPrefabEntry
    {
        public int id;         // Enemy 타입을 구분하는 int형 ID
        public Enemy prefab;   // 해당 ID의 Enemy 프리팹
    }

    [SerializeField] private List<EnemyPrefabEntry> enemyPrefabEntries = new List<EnemyPrefabEntry>();

    // 각 Enemy 타입별로 ObjectPool을 관리할 Dictionary
    private Dictionary<int, ObjectPool<Enemy>> enemyPools = new Dictionary<int, ObjectPool<Enemy>>();

    private void Awake()
    {
        if(Instance == null) Instance = this;

        else
        {
            Destroy(gameObject);
            return;
        }

        // 각 프리팹별로 ObjectPool 생성
        foreach (var entry in enemyPrefabEntries)
        {
            ObjectPool<Enemy> pool = new ObjectPool<Enemy>(
                createFunc: () =>
                {
                    Enemy newEnemy = Instantiate(entry.prefab, transform);
                    newEnemy.gameObject.SetActive(false);
                    return newEnemy;
                },

                actionOnGet: enemy =>
                {
                    enemy.gameObject.SetActive(true);
                },

                actionOnRelease: enemy =>
                {
                    enemy.gameObject.SetActive(false);
                },

                actionOnDestroy: enemy =>
                {
                    Destroy(enemy.gameObject);
                },
                defaultCapacity: 100,
                maxSize: 200
            );

            enemyPools.Add(entry.id, pool);
        }
    }

    public Enemy GetEnemy(int id, Vector3 spawnTransform)
    {
        if (!enemyPools.ContainsKey(id))
        {
            Debug.LogError("Enemy pool not found for id: " + id);
            return null;
        }

        Enemy enemy = enemyPools[id].Get();
        // 전달된 spawnTransform의 위치와 회전을 적용
        enemy.transform.position = spawnTransform;
        enemy.transform.rotation = Quaternion.identity;
        return enemy;
    }

    public void ReleaseEnemy(int id, Enemy enemy)
    {
        if (!enemyPools.ContainsKey(id))
        {
            Debug.LogError("Enemy pool not found for id: " + id);
            return;
        }

        enemyPools[id].Release(enemy);
    }

}
