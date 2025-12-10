using UnityEngine;

public class EnemyArcher : Enemy
{
    [Header("Archer specific info")]
    public Vector2 jumpVelocity;
    [SerializeField] private GameObject arrowPrefab;
    public float jumpCooldown;
    [HideInInspector] public float lastTimeJumped;
    public float safeDistance;
    [SerializeField] private float arrowSpeed;
    private CharacterStats archerStats;

    public Vector2 normalAttackOffset = new Vector2(0.4f, 0.43f);
    public Vector2 lowerAttackOffset = new Vector2(0.4f, 0.09f);
    [Header("Additional collision check")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;



      # region States
    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherAttackState attackState { get; private set; }
    //public SkeletonStunnedState stunnedState { get; private set; }
    public ArcherDeadState deadState { get; private set; }
    public ArcherDetectPlayerState detectPlayerState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }

    #endregion

    public override void Awake()
    {
        base.Awake();
        archerStats = GetComponent<CharacterStats>();
        idleState = new ArcherIdleState(this, stateMachine, "Idle", this);
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        battleState = new ArcherBattleState(this, stateMachine, "Idle", this); 
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        deadState = new ArcherDeadState(this, stateMachine, "Die", this);
        detectPlayerState = new ArcherDetectPlayerState(this, stateMachine, "Detect", this);
        jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);
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
            //stateMachine.ChangeState(stunnedState);
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
        // int[] footstepIndexes = { 33, 34, 35, 36 };
        // int sfxIndex = footstepIndexes[Random.Range(0, footstepIndexes.Length)];

        // AudioManager.instance.PlaySFX(sfxIndex, transform);
    }

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newArrow = Instantiate(arrowPrefab, attackCheck.transform.position, Quaternion.identity);
        newArrow.GetComponent<Arrow_Controller>().SetUpArrow(arrowSpeed, archerStats);
        if (facingDir == -1)
            newArrow.GetComponent<Arrow_Controller>().FlipArrow("Player");
    }

    public bool GroundBehindCheck() => Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero,0, whatIsGround);
    public bool WallBehindCheck() => Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDir, wallCheckDistance + 2, whatIsGround);

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireCube(groundBehindCheck.position, groundBehindCheckSize);
    }
}
