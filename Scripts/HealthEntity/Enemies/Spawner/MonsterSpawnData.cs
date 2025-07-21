using UnityEngine;

[System.Serializable]
public class MonsterSpawnData
{
    public int enemyID;
    public GameObject finalTargetObject;
    public LayerMask attackTargetLayer;
    public int spawnCount;

    // 행동 AI 타입
    public BehaviourType behaviourType;
}
