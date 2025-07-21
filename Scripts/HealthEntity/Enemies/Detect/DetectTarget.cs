using System.Collections;
using UnityEngine;

public class DetectTarget : MonoBehaviour
{
    private Coroutine DetectTargetCoroutine;
    private Coroutine DetectAttackTargetCoroutine;

    private PriorityQueue<ComparePriority> detectCurrentTargetQueue = new();
    private PriorityQueue<ComparePriority> detectAttackTargetQueue = new();
    private Collider[] colliders = new Collider[10];

    private Enemy enemy;
    private int priority = -1;
    
    public GameObject finalTarget;
    public GameObject currentTarget;
    public GameObject lastAttackTarget;

    public LayerMask targetLayer;
    public LayerMask IsExistBlockBetweenTargetLayer;

    [SerializeField] private float detectRange;
    [SerializeField] private float detectEndRange;

    public BehaviourType behaviourType;
    public LayerMask blockedLayerMask;
    public float GetDetectRange { get { return detectRange; } }
    public float GetDetectEndRange { get {return detectEndRange; } }

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public bool IsTargetExist(GameObject target)
    {
        if (target != null && target.activeSelf == false || target == null) return false;
        else return true;
    }

    private bool DetectColliderToOverlapSphere(PriorityQueue<ComparePriority> queue, float range, float sectorAngle, LayerMask targetLayer)
    {
        queue.Clear();

        Vector3 origin = transform.position;
        Vector3 direction = enemy.GetMove.GetMoveDirection;
        float halfAngle = sectorAngle / 2f;
        LayerMask layerMask = targetLayer;

        int colliderCount = Physics.OverlapSphereNonAlloc(origin, range, colliders, layerMask);
        int validTargetCount = 0;
        
        if (colliderCount > 0)
        {
            for (int i = 0; i < colliderCount; i++)
            {
                Collider targetCollider = colliders[i];

                if (targetCollider.gameObject == this.gameObject) continue;

                Vector3 toCollider = (targetCollider.transform.position - origin).normalized;
                float angle = Vector3.Angle(direction, toCollider);
                if(angle <= halfAngle)
                {
                    float distance = Vector3.Distance(targetCollider.transform.position, this.transform.position);
                    // enemy와 candidate 사이로 ray를 쏴서, blockedLayerMask에 해당하는 오브젝트가 있다면 스킵
                    if (Physics.Raycast(origin, toCollider, out RaycastHit blockHit, distance, blockedLayerMask))
                    {
                        // candidate와 enemy 사이에 부서지지 않는 벽이 있다면 candidate 무시
                        if (blockHit.collider.gameObject != targetCollider.gameObject)
                        {
                            continue;
                        }
                    }

                    int candidatePriority = GetPriority(targetCollider.gameObject);

                    queue.Push(new ComparePriority(targetCollider.gameObject, distance, candidatePriority));
                    validTargetCount++;
                }
            }

            return validTargetCount > 0;
        }

        return false;
    }

    private int GetPriority(GameObject candidate)
    {
        LayerMask[] masks = behaviourType.GetLayerMasks();
        int count = masks.Length;
        for (int i = 0; i < count; i++)
        {
            if ((masks[i].value & (1 << candidate.layer)) != 0)
            {
                return count - i;
            }
        }
        return 0;
    }

    public bool IsExistBlockBetweenTarget()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = currentTarget.transform.position;
        Vector3 direction = (endPos - startPos).normalized;
        float distance = Vector3.Distance(startPos, endPos);

        RaycastHit[] hits = Physics.RaycastAll(startPos, direction, distance, IsExistBlockBetweenTargetLayer);
        foreach(var hit in hits)
        {
            if(hit.collider.gameObject != this.gameObject && hit.collider.gameObject != currentTarget)
            {
                return true;
            }
        }

        return false;
    }

    public bool DetectAttackTarget(AttackBase pattern)
    {
        if (DetectColliderToOverlapSphere(detectAttackTargetQueue, pattern.GetAttackDetectRange, 360f, pattern.GetTargetLayerMask))
        {
            SetAttackTarget();
            enemy.ChangeToAttackPrepareState();
            return true;
        }

        return false;
    }

    private IEnumerator DetectCurrentTarget()
    {
        yield return null;

        while (true)
        {
            if (DetectColliderToOverlapSphere(detectCurrentTargetQueue, GetDetectRange, 360f, targetLayer))
            {
                SetChaseTarget();
            }
            if ((currentTarget != null || finalTarget != null) &&
                (enemy.GetStateMachine.GetCurrentState is IdleState || enemy.GetStateMachine.GetCurrentState is MoveState)) enemy.ChangeToChaseState();
            
            yield return new WaitForSeconds(1f);
        }
    }

    private void SetAttackTarget()
    {
        if (detectAttackTargetQueue.Count() == 0) return;
        ComparePriority candidate = detectAttackTargetQueue.Pop();
        lastAttackTarget = candidate.target;
    }

    private void SetChaseTarget()
    {
        if (detectCurrentTargetQueue.Count() == 0) return;
        ComparePriority candidate = detectCurrentTargetQueue.Pop();
        if (currentTarget != null && candidate.priority <= priority) return;
        currentTarget = candidate.target;
        priority = candidate.priority;
    }
    
    public void SetTargetIsDead()
    {
        if (!IsTargetExist(currentTarget) || 
            enemy.GetMove.ReturnEnemyToTargetDistance() > GetDetectEndRange) currentTarget = null;
        if (!IsTargetExist(finalTarget)) finalTarget = null;
    }

    public void StartDetectTargetCoroutine()
    {
        if (DetectTargetCoroutine != null) return;
        DetectTargetCoroutine = StartCoroutine(DetectCurrentTarget());
    }

    public void StopDetectTargetCoroutine()
    {
        if (DetectTargetCoroutine == null) return;
        StopCoroutine(DetectTargetCoroutine);
        DetectTargetCoroutine = null;
    }

}
