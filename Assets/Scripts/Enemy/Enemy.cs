using System.Reflection;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine { get; private set; }
    // public EnemyIdleState idleState { get; private set; }
    // public EnemyMoveState moveState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        // idleState = new EnemyIdleState(this, stateMachine, "Idle");
        // moveState = new EnemyMoveState(this, stateMachine, "Move");
    }
    protected override void Start()
    {
        base.Start();
        // stateMachine.InitializeState(idleState);
    }

    protected override void Update()
    {
        base.Update();
        // stateMachine.currentState.Update();
    }
}
