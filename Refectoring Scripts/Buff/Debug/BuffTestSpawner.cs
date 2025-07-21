using UnityEngine;

public class BuffTestSpawner : MonoBehaviour
{
    [Header("테스트용 공격자/타겟")]
    public GameObject attackerPrefab;
    public GameObject targetPrefab;
    public AttackDataSO attackData;

    private void Start()
    {
        GameObject attacker = Instantiate(attackerPrefab, new Vector3(-2, 0, 0), Quaternion.identity);
        GameObject target = Instantiate(targetPrefab, new Vector3(2, 0, 0), Quaternion.identity);

        Debug.Log("<color=cyan>▶ 공격 시작</color>");
        AttackEffectTargetUtility.Apply(attacker, target, attackData);
    }
}
