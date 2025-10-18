using UnityEngine;

public class EnemySkeleton : Enemy
{
    # region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }

    #endregion

    public override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this); 
    }
    public override void Start()
    {
        base.Start();
        stateMachine.InitializeState(idleState);
    }
    
    public override void Update()
    {
        base.Update();
    }
}
