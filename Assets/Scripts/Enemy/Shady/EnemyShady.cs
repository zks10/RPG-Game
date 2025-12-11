using UnityEngine;

public class EnemyShady : Enemy
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float growSpeed = 8;
    [SerializeField] private float maxSize = 6;
    public int attackCounter { get; private set;}
    private float explosionRadius;
    public Transform explosionCheck;
    public float explosionCheckRadius;

    # region States
    public ShadyIdleState idleState { get; private set; }
    public ShadyMoveState moveState { get; private set; }
    public ShadyBattleState battleState { get; private set; }
    public ShadyAttackState attackState { get; private set; }
    public ShadyStunnedState stunnedState { get; private set; }
    public ShadyDeadState deadState { get; private set; }
    public ShadyDetectPlayerState detectPlayerState { get; private set; }
    public ShadyExplodeState explodeState { get; private set; }

    #endregion

    public override void Awake()
    {
        base.Awake();
        idleState = new ShadyIdleState(this, stateMachine, "Idle", this);
        moveState = new ShadyMoveState(this, stateMachine, "Move", this);
        battleState = new ShadyBattleState(this, stateMachine, "MoveFast", this);
        attackState = new ShadyAttackState(this, stateMachine, "Attack", this);
        stunnedState = new ShadyStunnedState(this, stateMachine, "Stunned", this);
        deadState = new ShadyDeadState(this, stateMachine, "Die", this);
        detectPlayerState = new ShadyDetectPlayerState(this, stateMachine, "Detect", this);
        explodeState = new ShadyExplodeState(this, stateMachine, "Explode", this);
        attackCounter = 0;
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
        // int[] footstepIndexes = { 33, 34, 35, 36 };
        // int sfxIndex = footstepIndexes[Random.Range(0, footstepIndexes.Length)];

        // AudioManager.instance.PlaySFX(sfxIndex, transform);
    }

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newExplosion = Instantiate(explosionPrefab, explosionCheck.position, Quaternion.identity);
        newExplosion.GetComponent<Explosive_Controller>().SetUpExplosive(stats, growSpeed, maxSize, explosionCheckRadius);
        cd.enabled = false;
        rb.gravityScale = 0;
    }
    
    public void SelfDestroy() => Destroy(gameObject);

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(explosionCheck.position, explosionCheckRadius);
    }

    public void HasAttacked() => attackCounter++;
}
