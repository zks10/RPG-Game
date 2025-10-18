using System.Reflection;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;
    public EnemyStateMachine stateMachine { get; private set; }

    public override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }
}
