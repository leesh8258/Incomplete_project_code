using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DropItem))]
[RequireComponent(typeof(DetectTarget))]
[RequireComponent(typeof(Move))]
public abstract class Enemy : Character 
{
    //각 몬스터 스탯 및 행동 설정
    protected EnemyStateMachine stateMachine;
    protected DetectTarget detectTarget;
    protected DropItem dropItem;
    protected Move move;
    
    public int nextAttackindex;
    public EnemyStateMachine GetStateMachine => stateMachine;
    public DetectTarget GetDetectTarget => detectTarget;
    public DropItem GetDropItem => dropItem;
    public Move GetMove => move;

    private Coroutine InvokeCoroutine;
    private Coroutine DisableTimerCoroutine;
    private Coroutine CheckChunkCoroutine;

    [Header("적 스탯")]
    [SerializeField] private EnemyStatBase enemyStat;
    private EnemyStatBase currentEnemyStat;
    public override CharacterStatBase CharacterStat { get { return currentEnemyStat; } }

    [Header("적 공격 패턴")]
    [SerializeField] protected AttackBase[] attackPatternA;
    [SerializeField] protected AttackBase[] attackPatternB;
    [SerializeField] protected AttackBase[] attackPatternC;

    [Header("적 공격이후 딜레이 시간")]
    public float attackEndDuration;

    [Header("공격 실행 코루틴")]
    protected Coroutine findAttackCoroutine;

    [Header("공격 딕셔너리 / 리스트")]
    protected Dictionary<int, AttackBase[]> attackPatternDictionary = new Dictionary<int, AttackBase[]>();
    protected List<Coroutine> attackList = new List<Coroutine>();

    [Header("Prefab Default TargetLayer and FinalTarget")]
    protected LayerMask defaultTargetLayer;
    protected GameObject defaultFinalTarget;

    [Header("청크 밖 소멸 시간")]
    [SerializeField] protected float disableDelay = 5f;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine(this);

        dropItem = GetComponent<DropItem>();
        detectTarget = GetComponent<DetectTarget>();
        move = GetComponent<Move>();
        //Enemy_UI = GetComponentInChildren<Enemy_UI>();

        nextAttackindex = 1;

        if (attackPatternA != null && attackPatternA.Length > 0)
            attackPatternDictionary.Add(1, attackPatternA);

        if (attackPatternB != null && attackPatternB.Length > 0)
            attackPatternDictionary.Add(2, attackPatternB);

        if (attackPatternC != null && attackPatternC.Length > 0)
            attackPatternDictionary.Add(3, attackPatternC);

        defaultFinalTarget = detectTarget.finalTarget;
        defaultTargetLayer = detectTarget.targetLayer;
    }

    private void OnEnable()
    {
        
        interruptAction = false;
        LoadStatsFromDatabase();
        ChangeToIdleState();
        detectTarget.StartDetectTargetCoroutine();
        StartCheckChunkCoroutine();
        //Enemy_UI.SetHealth(HpValue());
    }

    private void OnDisable()
    {
        interruptAction = false;
        StopAllCharacterCoroutines();
        StopCheckChunkCoroutine();
        detectTarget.StopDetectTargetCoroutine();
        ResetSpawnData();
    }

    private void Start()
    {
        ApplyPermanentBuff();
    }

    protected virtual void Update()
    {
        stateMachine.UpdateState();
        detectTarget.SetTargetIsDead();
    }

    protected void LoadStatsFromDatabase()
    {
        currentEnemyStat = enemyStat;
        currentHP = enemyStat.HP;
    }

    public void InitializeSpawnData(MonsterSpawnData spawnData)
    {
        if (spawnData == null || detectTarget == null) return;

        // 행동 타입 설정
        detectTarget.behaviourType = spawnData.behaviourType;

        if(spawnData.behaviourType == BehaviourType.None)
        {
            detectTarget.targetLayer = defaultTargetLayer;
            detectTarget.finalTarget = defaultFinalTarget;
        }

        else
        {
            // 타겟 오브젝트 설정
            detectTarget.finalTarget = spawnData.finalTargetObject;

            // 행동 타입에 따른 타겟 레이어 설정
            LayerMask[] masks = spawnData.behaviourType.GetLayerMasks();
            int combined = 0;
            foreach (LayerMask mask in masks)
            {
                combined |= mask.value;
            }
            detectTarget.targetLayer = combined;
        }

    }

    public void ResetSpawnData()
    {
        // 풀에 반환할 때 기본값으로 복원
        GetDetectTarget.targetLayer = defaultTargetLayer;
        GetDetectTarget.finalTarget = defaultFinalTarget;
        GetDetectTarget.behaviourType = BehaviourType.None;
    }

    protected void ApplyPermanentBuff()
    {
        foreach (var buffType in enemyStat.permanentBuffs)
        {
            Buff buff = BuffFactory.CreateBuff(buffType);
            buffSystem.AddBuff(buff);
        }
    }

    public override void ApplyStatBoost(Action<Character> action)
    {
        action(this);
    }

    public void InterruptAttackPattern()
    {
        foreach (var cor in attackList)
        {
            if (cor != null)
            {
                StopCoroutine(cor);
            }
        }
        attackList.Clear();
    }

    public IEnumerator StartAttackPatternCoroutine()
    {
        attackList.Clear();
        AttackBase[] currentPatterns = attackPatternDictionary[nextAttackindex];

        for (int i = 0; i < currentPatterns.Length; i++)
        {
            int index = i;
            Coroutine c = StartCoroutine(RunAttackSequenceWrapper(currentPatterns[i], () => attackList[index] = null));
            attackList.Add(c);
        }

        yield return new WaitUntil(() => attackList.TrueForAll(x => x == null) || interruptAction);

        if (interruptAction)
        {
            InterruptAttackPattern();
            InterruptAction();
        }
        
        else ChangeToAttackEndState();
    }

    private IEnumerator RunAttackSequenceWrapper(AttackBase attack, Action onComplete)
    {
        yield return StartCoroutine(attack.AttackSequence());
        onComplete?.Invoke();
    }

    private bool FindAttackTargetOnce()
    {
        ChooseAttackPattern(); //index를 설정하는 함수
        AttackBase pattern = attackPatternDictionary[nextAttackindex][0];
        bool res = detectTarget.DetectAttackTarget(pattern);
        return res;
    }

    private IEnumerator FindAttackTargetRepeat()
    {
        while(true)
        {
            if(FindAttackTargetOnce()) yield break;
            
            yield return new WaitForSeconds(1f);
        }

    }

    public bool StartFindAttackTargetOnce()
    {
        return FindAttackTargetOnce();
    }

    public void StartFindAttackTargetCoroutine()
    {
        if (findAttackCoroutine != null) return;
        findAttackCoroutine = StartCoroutine(FindAttackTargetRepeat());
    }

    public void StopFindAttackTargetCoroutine()
    {
        if (findAttackCoroutine == null) return;
        StopCoroutine(findAttackCoroutine);
        findAttackCoroutine = null;
    }

    protected IEnumerator InvokeAfterDuration(float duration, Action callback)
    {
        yield return new WaitForSeconds(duration);
        callback?.Invoke();
        InvokeCoroutine = null;
    }
    
    public void StartInvoke(float duration, Action callback)
    {
        if (InvokeCoroutine != null) return;
        InvokeCoroutine = StartCoroutine(InvokeAfterDuration(duration, callback));
        
    }

    public void StopInvoke()
    {
        if(InvokeCoroutine == null) return;
        StopCoroutine(InvokeCoroutine);
        InvokeCoroutine = null;
    }

    public IEnumerator StartDissolve()
    {
        yield return StartCoroutine(spriteManager.Dissolve());
        EnemyObjectPool.Instance.ReleaseEnemy(enemyStat.ID, this);
    }

    private IEnumerator CheckChunck()
    {
        while(true)
        {
            Vector3Int currentPosition = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), Mathf.FloorToInt(transform.position.z));
            Vector3Int chunkPosition = ChunkData.WorldToChunkCoordinates(currentPosition);

            if (!WorldManager.Instance.CheckChunk(chunkPosition) 
                && detectTarget.finalTarget == null && detectTarget.currentTarget == null
                 && !WorldManager.Instance.CheckConstructCenterArea(currentPosition))
            {
                Debug.Log(currentPosition);
                if (DisableTimerCoroutine == null) DisableTimerCoroutine = StartCoroutine(DisableAfterDelay(disableDelay));
            }

            else
            {
                if (DisableTimerCoroutine != null)
                {
                    StopCoroutine(DisableTimerCoroutine);
                    DisableTimerCoroutine = null;
                }
            }

            yield return new WaitForSeconds(1f);
        }

    }

    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EnemyObjectPool.Instance.ReleaseEnemy(enemyStat.ID, this);
        DisableTimerCoroutine = null;
    }


    private void StartCheckChunkCoroutine()
    {
        if (CheckChunkCoroutine != null) return;
        CheckChunkCoroutine = StartCoroutine(CheckChunck());
    }

    private void StopCheckChunkCoroutine()
    {
        if (CheckChunkCoroutine == null) return;
        StopCoroutine(CheckChunck());
        CheckChunkCoroutine = null;
    }

    public abstract void ChangeToIdleState();

    public abstract void ChangeToMoveState();

    public abstract void ChangeToChaseState();

    public abstract void ChangeToAttackPrepareState();

    public abstract void ChangeToAttackEndState();

    public abstract void ChangeToStunState();

    public abstract void ChooseAttackPattern();

    public abstract void InterruptAction();
}
