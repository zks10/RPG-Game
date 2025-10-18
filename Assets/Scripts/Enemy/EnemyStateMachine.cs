using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState { get; private set; }
    public void InitializeState(EnemyState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }
    
    public void ChangeState(EnemyState _currentState)
    {
        currentState.Exit();
        currentState = _currentState;
        currentState.Enter();
    }
}
