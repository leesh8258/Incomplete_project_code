using UnityEngine;

public class DebugStatDisplay : MonoBehaviour
{
    private StatProcessor statProcessor;

    private void Awake()
    {
        var receiver = GetComponent<BuffReceiver>();
        if (receiver != null)
        {
            statProcessor = receiver.GetProcessor();  // BuffReceiver 내부에 getter 만들기
        }
    }

    private void Update()
    {
        if (statProcessor == null)
            return;

        var result = statProcessor.Final;
        Debug.Log($"[{name}] Melee: {result.meleeAttackPower} / Ranged: {result.rangeAttackPower} / Defense: {result.defense}");

    }
}
