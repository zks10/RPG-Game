
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;
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
}
