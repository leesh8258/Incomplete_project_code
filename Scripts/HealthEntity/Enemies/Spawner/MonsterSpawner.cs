using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StructureEntity))]
public class MonsterSpawner : MonoBehaviour
{
    public enum SpawnType
    {
        Timer,
        HP,
        Detect,
        Turret,
        Position
    }

    public enum SpawnMode
    {
        All,    // 리스트에 있는 모든 몬스터 데이터를 한꺼번에 소환
        Random  // 리스트에서 하나를 랜덤 선택해서 소환
    }

    public SpawnType spawnType;
    
    private Coroutine DetectCoroutine;
    private Coroutine SpawnCoroutine;
    private StructureEntity block;
    private HashSet<Vector3Int> occupiedPositions;
    private int spawnCounter = 0;
    private Collider[] colliders;
    private Transform playerPosition;

    
    [SerializeField] private float detectRange;
    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private List<MonsterSpawnData> monsterSpawnList;
    [SerializeField] private SpawnMode spawnMode;

    [SerializeField] private int spawnResetThreshold = 5;

    [SerializeField] private int minSpawnRange;
    [SerializeField] private int maxSpawnRange;
    
    [SerializeField] private GameObject spawnPrefab;

    [SerializeField] private float spawnTime;
    [SerializeField] private float damageToSpawn;
    [SerializeField, ReadOnly] private float lastSpawnHp;
    [SerializeField] private float MaxDetectRange;
    [SerializeField] private Transform[] positions;

    private void Awake()
    {
        block = GetComponent<StructureEntity>();
        colliders = new Collider[10];
        occupiedPositions = new HashSet<Vector3Int>();
    }

    private void OnEnable()
    {
        StartDetectCoroutine();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void StartDetectCoroutine()
    {
        DetectCoroutine = StartCoroutine(DetectPlayerCoroutine());
    }

    private void StartSpawnCoroutine()
    {
        switch (spawnType)
        {
            case SpawnType.Timer:
                SpawnCoroutine = StartCoroutine(TimerSpawnCoroutine());
                break;

            case SpawnType.HP:
                lastSpawnHp = block.GetCurrentHP;
                SpawnCoroutine = StartCoroutine(HPSpawnCoroutine());
                break;

            case SpawnType.Detect:
                SpawnCoroutine = StartCoroutine(DetecSpawnCoroutine());
                break;

            case SpawnType.Turret:
                SpawnCoroutine = StartCoroutine(TurretSpawnCoroutine());
                break;

            case SpawnType.Position:
                SpawnCoroutine = StartCoroutine(PositionSpawnCoroutine());
                break;

            default:
                break;
        }
    }

    private IEnumerator DetectPlayerCoroutine()
    {
        yield return null;

        while (true)
        {
            int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, detectRange, colliders, targetLayer);

            if (colliderCount > 0)
            {
                playerPosition = colliders[0].transform;
                StartSpawnCoroutine();
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator TimerSpawnCoroutine()
    {
        yield return null;

        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            SpawnEnemyPrefab();
        }
    }

    private IEnumerator HPSpawnCoroutine()
    {
        yield return null;

        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            float currentHp = block.GetCurrentHP;

            if (lastSpawnHp - currentHp >= damageToSpawn)
            {
                lastSpawnHp = currentHp;
                SpawnEnemyPrefab();
            }
        }
    }

    private IEnumerator DetecSpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (playerPosition == null) continue;

            Vector3 direction = (playerPosition.position - transform.position).normalized;
            Ray ray = new Ray(transform.position, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, MaxDetectRange))
            {
                if (((1 << hit.collider.gameObject.layer) & targetLayer) != 0)
                {
                    SpawnEnemyPrefab();
                    break;
                }
            }
        }
    }

    private IEnumerator TurretSpawnCoroutine()
    {
        yield return null;

        Vector3 spawnPosition = new Vector3(transform.position.x, 1f, transform.position.z);
        foreach(var spawnData in monsterSpawnList)
        {
            Enemy enemy = EnemyObjectPool.Instance.GetEnemy(spawnData.enemyID, spawnPosition);
            enemy.InitializeSpawnData(spawnData);
            spawnPrefab = enemy.gameObject;

            if(spawnPrefab != null)
            {
                enemy.OnDeathAction += block.OnDeathAction;
            }
        }
    }

    private IEnumerator PositionSpawnCoroutine()
    {
        yield return null;

        foreach(var position in positions)
        {
            foreach(var spawnData in monsterSpawnList)
            {
                Enemy enemy = EnemyObjectPool.Instance.GetEnemy(spawnData.enemyID, position.position);
                enemy.InitializeSpawnData(spawnData);
            }

            yield return new WaitForSeconds(2f);
        }

        Destroy(gameObject);
    }

    private void SpawnEnemyPrefab()
    {
        int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, detectRange, colliders, LayerMask.NameToLayer("Enemy"));
        if (colliderCount > 20) return;

        List<Vector3Int> availableSpawnPositions = GetAvailableSpawnPosition();
        if (availableSpawnPositions.Count == 0) return;

        if (spawnMode == SpawnMode.All)
        {
            // monsterSpawnList에 있는 모든 그룹을 순회하며 각각 소환
            foreach (var spawnData in monsterSpawnList)
            {
                for (int i = 0; i < spawnData.spawnCount; i++)
                {
                    if (availableSpawnPositions.Count == 0) break;

                    int randomIndex = Random.Range(0, availableSpawnPositions.Count);
                    Vector3Int spawnGridPosition = availableSpawnPositions[randomIndex];
                    availableSpawnPositions.RemoveAt(randomIndex);

                    Vector3 finalPosition = new Vector3(spawnGridPosition.x, spawnGridPosition.y, spawnGridPosition.z);
                    Enemy enemy = EnemyObjectPool.Instance.GetEnemy(spawnData.enemyID, finalPosition);
                    enemy.InitializeSpawnData(spawnData);
                    occupiedPositions.Add(spawnGridPosition);
                }
            }
        }
        else if (spawnMode == SpawnMode.Random)
        {
            // monsterSpawnList 내에서 하나의 그룹을 랜덤 선택하여 소환
            if (monsterSpawnList.Count > 0)
            {
                int randomIndex = Random.Range(0, monsterSpawnList.Count);
                MonsterSpawnData spawnData = monsterSpawnList[randomIndex];
                for (int i = 0; i < spawnData.spawnCount; i++)
                {
                    if (availableSpawnPositions.Count == 0) break;

                    int posIndex = Random.Range(0, availableSpawnPositions.Count);
                    Vector3Int spawnGridPosition = availableSpawnPositions[posIndex];
                    availableSpawnPositions.RemoveAt(posIndex);

                    Vector3 finalPosition = new Vector3(spawnGridPosition.x, spawnGridPosition.y, spawnGridPosition.z);
                    Enemy enemy = EnemyObjectPool.Instance.GetEnemy(spawnData.enemyID, finalPosition);
                    enemy.InitializeSpawnData(spawnData);
                    occupiedPositions.Add(spawnGridPosition);
                }
            }
        }

        spawnCounter++;

        if (spawnCounter >= spawnResetThreshold)
        {
            occupiedPositions.Clear();
            spawnCounter = 0;
            Debug.Log("스폰 위치 초기화");
        }
    }

    private List<Vector3Int> GetAvailableSpawnPosition()
    {
        List<Vector3Int> availablePositions = new List<Vector3Int>();
        float dynamicMaxSpawnRange = maxSpawnRange;
        int maxAttempts = 50;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float radius = Random.Range(minSpawnRange, dynamicMaxSpawnRange);
            Vector3 spawnPosition = new Vector3(
                transform.position.x + radius * Mathf.Cos(angle),
                1f, // Y 값 고정
                transform.position.z + radius * Mathf.Sin(angle)
            );

            Vector3Int gridPos = new Vector3Int(
                Mathf.RoundToInt(spawnPosition.x),
                Mathf.RoundToInt(spawnPosition.y),
                Mathf.RoundToInt(spawnPosition.z)
            );

            if (!occupiedPositions.Contains(gridPos))
            {
                availablePositions.Add(gridPos);
            }

            attempts++;

            if (attempts >= maxAttempts && availablePositions.Count == 0)
            {
                dynamicMaxSpawnRange += 2;
                attempts = 0;
            }
        }

        return availablePositions;
    }

    public void SetFinalTarget(GameObject target)
    {
        foreach(var monster in monsterSpawnList)
        {
            monster.finalTargetObject = target;
        }
    }
}
