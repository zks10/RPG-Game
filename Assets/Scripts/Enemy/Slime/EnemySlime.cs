using UnityEngine;

public class EnemySlime : Enemy
{
    # region States
    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeadState deadState { get; private set; }
    public SlimeDetectPlayerState detectPlayerState { get; private set; }

    #endregion

    public override void Awake()
    {
        base.Awake();
        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SlimeStunnedState(this, stateMachine, "Stun", this);
        deadState = new SlimeDeadState(this, stateMachine, "Die", this);
        detectPlayerState = new SlimeDetectPlayerState(this, stateMachine, "Detect", this);
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

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }
    
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public override void Footstep()
    {
        int[] footstepIndexes = { 33, 34, 35, 36 };
        int sfxIndex = footstepIndexes[Random.Range(0, footstepIndexes.Length)];

        AudioManager.instance.PlaySFX(sfxIndex, transform);
    }

}
