using UnityEngine;

public class Enemy_NEW : Health, IStatProvider
{
    private IStateMachine<Enemy_NEW> stateMachine;
    public IStateMachine<Enemy_NEW> StateMachine => stateMachine;

    private IEnemyTargetDetector targetDetector;
    public IEnemyTargetDetector TargetDetector => targetDetector;

    //private IMover mover;
    //public IMover Mover => mover;

    private EnemyAttackController attackController;
    public EnemyAttackController AttackController => attackController;

    [SerializeField] private ComposedPriorityPolicySO priorityPolicySO;
    [SerializeField] private EnemyStatBase stat;
    
    private GameObject currentTarget;
    private BuffReceiver buffReceiver;
    private IEnemyBehaviorSet behaviorSet;
    private StatModifierContainer modifierContainer;
    private StatProcessor statProcessor;

    private readonly IEnemyBehaviorSet debugBehaviorSet = new NormalBehaviorSet(); //임시 테스트용
    public float GetStat(StatAttributeType type) => statProcessor.Final.GetStat(type);
    public void SetCurrentTarget(GameObject target) => currentTarget = target;
    public GameObject GetCurrentTarget() => currentTarget;

    protected override void Awake()
    {
        base.Awake();
        OnDied += HandleDeath;
        attackController = GetComponent<EnemyAttackController>();
        targetDetector = GetComponent<TargetDetector>();
        buffReceiver = GetComponent<BuffReceiver>();

        modifierContainer = new StatModifierContainer();
        statProcessor = new StatProcessor(stat, modifierContainer);

        var evaluator = priorityPolicySO.CreateEvaluator();
        targetDetector.Initialize(evaluator);
        buffReceiver.Initialize(statProcessor);

        InitializeBehaviorSet(debugBehaviorSet);
    }


    //spawner에서 소환할 때 설정
    public void InitializeBehaviorSet(IEnemyBehaviorSet set)
    {
        behaviorSet = set;
        stateMachine = new StateMachine<Enemy_NEW>(this);
        stateMachine.ChangeState(behaviorSet.IdleState);
    }

    private void Update()
    {
        stateMachine?.Update();
    }

    private void HandleDeath() => ToDeathState();
    public void ToHitState() => stateMachine?.ChangeState(behaviorSet.HitState);
    public void ToIdleState() => stateMachine?.ChangeState(behaviorSet.IdleState);
    public void ToChaseState() => stateMachine?.ChangeState(behaviorSet.ChaseState);
    public void ToAttackState() => stateMachine?.ChangeState(behaviorSet.AttackState);
    public void ToDeathState() => stateMachine?.ChangeState(behaviorSet.DeathState);
}
