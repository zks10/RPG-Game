using UnityEngine;

public class EnemySkeleton : Enemy
{
    # region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }

    #endregion

    public override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new SkeletonBattleState(this, stateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stun", this);
    }
    public override void Start()
    {
        base.Start();
        stateMachine.InitializeState(idleState); 
    }
    
    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.U)) 
            stateMachine.ChangeState(stunnedState);
        
    }

    public override bool CanBeStunned() 
    {
        if (base.CanBeStunned()) 
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }
}
