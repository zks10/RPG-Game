
using UnityEngine;
using System.Collections;

public class Enemy : Entity
{   
    
    [SerializeField] protected LayerMask whatIsPlayer;
    private Transform player; 

    [Header("Stunned Info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move Info")]
    public float moveSpeed;
    public float battleMoveSpeed;
    public float idleTime;
    private float defaultMoveSpeed;
    private float defaultBattleMoveSpeed;
    public float battleTime;
    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown;
    public float viewDistance;
    [HideInInspector] public float lastTimeAttacked;
    //private bool timeFrozen = false;
    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName { get; private set; }

    public override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        CloseCounterAttackWindow();
        defaultMoveSpeed = moveSpeed;
        defaultBattleMoveSpeed = battleMoveSpeed;
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

    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            battleMoveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            battleMoveSpeed = defaultBattleMoveSpeed;
            anim.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeAllFor(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    # region Counter Attack region
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow() 
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }
    #endregion

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public virtual RaycastHit2D IsPlayerDectected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, viewDistance, whatIsPlayer);

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }

    public override void DamageImpact() {
        base.DamageImpact();
        StartCoroutine("HitKnockBack");

    }

    public IEnumerator HitKnockBack()
    {
        isKnocked = true;
        player = PlayerManager.instance.player.transform;
        float val = (player.position.x - this.transform.position.x);
        float dir = facingDir;

        if (val > 0)
            dir = -1;
        else
            dir = 1;

        rb.linearVelocity = new Vector2(knockbackDirection.x * dir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }
    
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        battleMoveSpeed = battleMoveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        if (Random.Range(0, 100) < 20)
        {
            moveSpeed = 0;
            battleMoveSpeed = 0;
            anim.speed = 0;
        }

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        battleMoveSpeed = defaultBattleMoveSpeed;
    }
}
