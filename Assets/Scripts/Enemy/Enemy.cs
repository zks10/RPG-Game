
using UnityEngine;
using System.Collections;

public class Enemy : Entity
{   
    
    [SerializeField] protected LayerMask whatIsPlayer;
    private Transform player;
    [Header("Stunned Info")]
    public float stunDuration;
    public Vector2 stunDirection;
    
    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown;
    public float viewDistance;
    [HideInInspector] public float lastTimeAttacked;



    public EnemyStateMachine stateMachine { get; private set; }

    public override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
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

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public virtual RaycastHit2D IsPlayerDectected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, viewDistance, whatIsPlayer);

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }

    public override void Damage() {
        base.Damage();
        StartCoroutine("HitKnockBack");

    }

    public IEnumerator HitKnockBack() 
    {
        isKnocked = true;
        player = GameObject.Find("Player").transform;
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
}
