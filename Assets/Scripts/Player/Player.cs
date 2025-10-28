
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack Info")]
    public Vector2[] attactMovement;
    public bool isBusy { get; private set; }

    [Header("Counter Attack Info")]
    public float counterAttackDuration = .2f;
    [SerializeField] public float counterAttackCooldown;
    [HideInInspector] public float counterAttackUsageTimer;

    [Header("Move Info")]
    public float moveSpeed = 8.5f;
    public float runSpeed = 13f;
    public float jumpForce;
    [HideInInspector] public float defaultJumpForce;
    public float swordReturnImpact;
    private float lastTapTime = 0f;
    private float doubleTapTimeWindow = 0.3f;
    private int lastTapDirection = 0;
    
    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }
    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerRunningState runningState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackholeState blackholeState { get; private set; }
    public PlayerDeathState deathState { get; private set; }

    #endregion

    public override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        defaultJumpForce = jumpForce;
        
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        runningState = new PlayerRunningState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump");
        deathState = new PlayerDeathState(this, stateMachine, "Die");

    }

    public override void Start()
    {
        base.Start();
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);
    }

    public override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();
        counterAttackUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F))
        {
            skill.crystal.CanUseSkill();
        }
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }
    private void CheckForDashInput()
    {
        if (IsWallDectected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
            {
                dashDir = facingDir;
            }
            stateMachine.ChangeState(dashState);
        }
    }

    public bool CheckDoubleTapKeyDown()
    {
        int direction = 0;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            direction = -1;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            direction = 1;

        if (direction == 0) return false;

        float now = Time.time;

        if (lastTapDirection == direction && now - lastTapTime <= doubleTapTimeWindow)
        {
            lastTapTime = 0f;
            lastTapDirection = 0;
            return true;
        }

        lastTapDirection = direction;
        lastTapTime = now;
        return false;
    }

    public void SetJump(float _jumpForce)
    {
        jumpForce = _jumpForce;
    }
    
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public IEnumerator HitKnockBack() 
    {
        isKnocked = true;

        rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }

    public override void Damage()
    {
        base.Damage();
        StartCoroutine("HitKnockBack");
    }
    
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);
    }

}
