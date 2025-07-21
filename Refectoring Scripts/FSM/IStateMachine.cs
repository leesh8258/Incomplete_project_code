public interface IStateMachine<T>
{
    IState<T> CurrentState { get; }
    void ChangeState(IState<T> newState);
    void Update();
}