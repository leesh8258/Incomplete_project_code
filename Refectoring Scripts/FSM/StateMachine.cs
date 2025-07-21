public class StateMachine<T> : IStateMachine<T>
{
    private IState<T> currentState;
    public IState<T> CurrentState => currentState;
    private readonly T owner;

    public StateMachine(T _owner) => owner = _owner;

    public void ChangeState(IState<T> newState)
    {
        if (newState == null) return;

        currentState?.OnExit(owner);
        currentState = newState;
        currentState?.OnEnter(owner);
    }

    public void Update()
    {
        currentState?.OnUpdate(owner);
    }
}