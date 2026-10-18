using UnityEngine;

public interface IStateMachine
{
    void ChangeState(State newState);
}

public class StateMachine<TState> : IStateMachine where TState : State
{
    public TState CurrentState { get; private set; }

    public void Init(TState startingState)
    {
        CurrentState = startingState;
        startingState.EnterState();
        Debug.Log($"init state {CurrentState}");
    }

    public void ChangeState(TState newState)
    {
        Debug.Log($"exit state {CurrentState}");
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
        Debug.Log($"entered state {CurrentState}");
    }
    void IStateMachine.ChangeState(State newState)
    {
        ChangeState((TState)newState);
    }
}
