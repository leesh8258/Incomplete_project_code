using System.Collections.Generic;
using UnityEngine;

public interface IEnemyTargetDetector
{
    GameObject DetectTarget();                                  // 우선순위 가장 높은 타겟
    List<TargetContext> DetectTargetsSorted();                  // ✅ 우선순위 정렬 리스트 반환
    GameObject[] DetectAllTargets();                            // 단순 감지용
    bool HasTarget();                                           // 타겟 존재 여부
    void Initialize(ITargetPriorityEvaluator evaluator);        // evaluator 주입
}
