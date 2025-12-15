using UnityEngine;
using System.Collections;

public class EnemyDeathBringer : Enemy
{
    # region States
    public DeathBringerIdleState idleState { get; private set; }
    public DeathBringerMoveState moveState { get; private set; }
    public DeathBringerBattleState battleState { get; private set; }
    public DeathBringerAttackState attackState { get; private set; }
    public DeathBringerDeadState deadState { get; private set; }
    public DeathBringerDetectPlayerState detectPlayerState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }
    public DeathBringerSpellCastState spellCastState { get; private set; }
    public DeathBringerStunnedState stunnedState { get; private set; }

    [Header("Teleport Details")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;

    #endregion

    public override void Awake()
    {
        base.Awake();
        idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);
        moveState = new DeathBringerMoveState(this, stateMachine, "Move", this);
        battleState = new DeathBringerBattleState(this, stateMachine, "Move", this);
        attackState = new DeathBringerAttackState(this, stateMachine, "Attack", this);
        deadState = new DeathBringerDeadState(this, stateMachine, "Die", this);
        detectPlayerState = new DeathBringerDetectPlayerState(this, stateMachine, "Detect", this);
        teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport", this);
        spellCastState = new DeathBringerSpellCastState(this, stateMachine, "Cast", this);
        stunnedState = new DeathBringerStunnedState(this, stateMachine, "Stun", this);
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

    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));
        
        if (!GroundBelow() || SomethingIsAround())
        {
            Debug.Log("Looking for new pos");
            FindPosition();
        }
    }
    public override void Footstep()
    {
        int[] footstepIndexes = { 33, 34, 35, 36 };
        int sfxIndex = footstepIndexes[Random.Range(0, footstepIndexes.Length)];

        //AudioManager.instance.PlaySFX(sfxIndex, transform);
    }

    private RaycastHit2D GroundBelow () => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    private bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));  
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }
}
