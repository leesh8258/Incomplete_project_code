public interface IState<T>
{
    void OnEnter(T owner);
    void OnUpdate(T owner);
    void OnExit(T owner);
}