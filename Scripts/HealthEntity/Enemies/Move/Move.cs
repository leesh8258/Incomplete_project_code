using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private enum NavigationType
    {
        findPath,
        breakObstacle,
        findPathAndBreakObstacle
    }

    private Enemy enemy;
    [Header("네비게이션 설정")]
    [SerializeField] private NavigationType navigationType;

    [Header("이동속도")]
    [SerializeField] private float moveSpeed;

    [Header("다음 이동까지 시간")]
    [SerializeField] private float minNextRandomMoveTime;
    [SerializeField] private float maxNextRandomMoveTime;

    [Header("이동 지속시간")]
    [SerializeField] private float minMoveDurationTime;
    [SerializeField] private float maxMoveDurationTime;

    private Vector3 moveDirection;
    private float nextMoveTime;
    private float moveDuration;

    [Header("네비게이션")]
    private List<Vector2Int> path;
    private int currentPathIndex;
    private AStarPathfinder pathfinder;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Coroutine repeatUpdatePathCoroutine;

    public Vector3 GetMoveDirection
    {
        get
        {
            enemy.GetSpriteManager.GetCardinalPoints(moveDirection);

            return moveDirection;
        }
    }

    public float GetNextMoveTime
    {
        get
        {
            nextMoveTime = Random.Range(minNextRandomMoveTime, maxNextRandomMoveTime);

            return nextMoveTime;
        }
    }

    public float GetMoveDuration
    {
        get
        {
            moveDuration = Random.Range(minMoveDurationTime, maxMoveDurationTime);

            return moveDuration;
        }
    }

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        pathfinder = new AStarPathfinder();
        moveDirection = Vector3.left;
    }

    public void SetStartAndTargetPosition()
    {
        startPosition = transform.position;

        if (enemy.GetStateMachine.GetCurrentState is MoveState)
        {
            targetPosition = SetMoveStateTargetPosition();
        }

        if(enemy.GetStateMachine.GetCurrentState is ChaseState)
        {
            if (enemy.GetDetectTarget.currentTarget == null && enemy.GetDetectTarget.finalTarget != null) targetPosition = enemy.GetDetectTarget.finalTarget.transform.position;
            else if (enemy.GetDetectTarget.currentTarget != null) targetPosition = enemy.GetDetectTarget.currentTarget.transform.position;
        }
    }
    
    public void UpdatePath()
    {
        SetStartAndTargetPosition();

        Vector2Int startPos = new Vector2Int(Mathf.RoundToInt(startPosition.x), Mathf.RoundToInt(startPosition.z));
        Vector2Int targetPos = new Vector2Int(Mathf.RoundToInt(targetPosition.x), Mathf.RoundToInt(targetPosition.z));
        currentPathIndex = 0;

        switch (navigationType)
        {
            case NavigationType.findPath:
                path = pathfinder.FindPath(startPos, targetPos);
                break;

            case NavigationType.breakObstacle:
                path = pathfinder.FindPathBreakingObstacles(startPos, targetPos);
                break;

            case NavigationType.findPathAndBreakObstacle:
                path = pathfinder.FindPath(startPos, targetPos);
                if (path.Count == 0)
                    path = pathfinder.FindPathBreakingObstacles(startPos, targetPos);
                break;
        }
    }

    public void FollowPath()
    {
        if (enemy.GetStateMachine.GetCurrentState is MoveState && currentPathIndex >= path.Count) enemy.ChangeToIdleState();
        
        if (path == null || path.Count == 0 || currentPathIndex >= path.Count) return;

        Vector2Int currentTarget = path[currentPathIndex];
        Vector3 targetWorldPos = new Vector3(currentTarget.x, transform.position.y, currentTarget.y);
        moveDirection = (targetWorldPos - transform.position).normalized;
        enemy.GetSpriteManager.GetCardinalPoints(moveDirection);
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWorldPos) < 0.08f)
        {
            currentPathIndex++;
        }
    }

    public Vector3 SetMoveStateTargetPosition()
    {
        //이게 진짜 가능한 좌표인지 확인
        int x = Random.Range(-10, 10);
        int z = Random.Range(-10, 10);
        Vector3 pos = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        return pos;
    }

    private IEnumerator RepeatUpdatePath()
    {
        while(true)
        {
            UpdatePath();
            yield return new WaitForSeconds(1f);
        }
    }

    public void StartRepeatUpdatePath()
    {
        if(repeatUpdatePathCoroutine == null) repeatUpdatePathCoroutine = StartCoroutine(RepeatUpdatePath());
    }

    public void StopRepeatUpdatePath()
    {
        if (repeatUpdatePathCoroutine != null)
        {
            StopCoroutine(repeatUpdatePathCoroutine);
            repeatUpdatePathCoroutine = null;
        }
    }

    public void SetAttackTargetDirection()
    {
        moveDirection = (enemy.GetDetectTarget.lastAttackTarget.transform.position - transform.position).normalized;
        enemy.GetSpriteManager.GetCardinalPoints(moveDirection);
    }

    public float ReturnEnemyToTargetDistance()
    {
        if(enemy.GetDetectTarget.currentTarget == null && enemy.GetDetectTarget.finalTarget != null)
        {
            return Vector3.Distance(enemy.GetDetectTarget.finalTarget.transform.position, this.transform.position);
        }

        else if(enemy.GetDetectTarget.currentTarget != null)
        {
            return Vector3.Distance(enemy.GetDetectTarget.currentTarget.transform.position, this.transform.position);
        }

        else return float.MaxValue;
    }

}
