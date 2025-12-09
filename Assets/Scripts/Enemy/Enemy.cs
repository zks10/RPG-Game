
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFx))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity
{

    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected LayerMask whatIsWall;

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
    protected bool isFrozen;    

    public float battleTime;
    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown;
    public float activateAttackDistance = 2;
    // public float minAttackCooldown;
    // public float maxAttackCooldown;
    public float viewDistance;
    [HideInInspector] public float lastTimeAttacked;
    [SerializeField] protected GameObject detectPlayerImage;
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
        isFrozen = _timeFrozen;
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
    public override void DamageImpact()
    {
        if (isFrozen)
            return;
        base.DamageImpact();
    }
    public virtual void FreezeTimeFor(float _seconds) => StartCoroutine(FreezeTimerCoroutine(_seconds));

    protected virtual IEnumerator FreezeTimerCoroutine(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    # region Counter Attack region
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        // counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        // counterImage.SetActive(false);
    }
    
    public virtual void DetectPlayerImage()
    {
        detectPlayerImage.SetActive(true);
    }

    public virtual void StartWalking() 
    {
        detectPlayerImage.SetActive(false);
    }

    public virtual void Footstep()
    {
        
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
    public virtual void AnimationSpecialAttackTrigger() { }
    public virtual RaycastHit2D IsPlayerDectected()
    {
        RaycastHit2D playerHit = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, viewDistance, whatIsPlayer);

        if (playerHit)
        {
            RaycastHit2D wallHit = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, playerHit.distance, whatIsWall);

            if (!wallHit)
                return playerHit;
        }

        return default;
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
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

    public override void Die()
    {
        base.Die();
    }
}
