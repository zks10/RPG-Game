using UnityEngine;

public class EnemySkeleton : Enemy
{
    # region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState{ get; private set; }

    #endregion

    public override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new SkeletonBattleState(this, stateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
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
