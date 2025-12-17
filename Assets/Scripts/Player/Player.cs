
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public bool canPickItem { get; private set; }
    public bool isSkillActive { get; private set; }
    public bool swordOut;

    [Header("Attack Info")]
    public Vector2[] attactMovement;
    public bool isBusy { get; private set; }

    [Header("Counter Attack Info")]
    public float counterAttackDuration = .2f;
    public event System.Action onCounterAttackSuccess;
    public bool lastCounterSuccessful { get; private set; }

    [Header("Move Info")]
    public float moveSpeed = 8.5f;
    public float runSpeed = 13;
    public float jumpForce;
    private float defaultMoveSpeed;
    private float defaultRunSpeed;
    [HideInInspector] public float defaultJumpForce;

    [Header("Double Jump")]
    public bool canDoubleJump;     
    public bool hasDoubleJumped;  

    public float swordReturnImpact;
    private float lastTapTime = 0f;
    private float doubleTapTimeWindow = 0.3f;
    private int lastTapDirection = 0;

    [Header("Dash Info")]
    public float dashSpeed;
    private float defaultDashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }
    public SkillManager skill { get; private set; }
    [HideInInspector] public GameObject sword;

    public bool isDead { get; private set; }

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
        canPickItem = true;
        stateMachine = new PlayerStateMachine();
        defaultJumpForce = jumpForce;
        defaultMoveSpeed = moveSpeed;
        defaultDashSpeed = dashSpeed;
        defaultRunSpeed = runSpeed;
        isDead = false;
        isSkillActive = false;

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
        if (Time.timeScale == 0) 
            return;
        stateMachine.currentState.Update();
        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked)
        {
            skill.crystal.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Inventory.instance.UseTrinket();
        }

        if (swordOut && sword == null)
        {
            swordOut = false;
        }

    }
    public void MarkCounterSuccess()
    {
        lastCounterSuccessful = true;
        onCounterAttackSuccess?.Invoke();
    }
    public void ResetCounterSuccess() => lastCounterSuccessful = false;
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
        swordOut = true;
    }

    public void SetSkillActive(bool _val) => isSkillActive = _val;

    public void CatchTheSword()
    {
        if (stateMachine.currentState == blackholeState)
        {
            Destroy(sword);
            sword = null;
            swordOut = false;
            return;
        }
        swordOut = false;
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
        sword = null;
        swordOut = false;

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

        if (!skill.dash.dashUnlocked)
            return;

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))  && SkillManager.instance.dash.CanUseSkill())
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


    public override void Die()
    {
        base.Die();
        isDead = true;
        stateMachine.ChangeState(deathState);
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        runSpeed = runSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
        runSpeed = defaultRunSpeed;
    }

    public void SetCanPickItm(bool _val) => canPickItem = _val;

    protected override void SetUpZeroKnockBackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }
}
