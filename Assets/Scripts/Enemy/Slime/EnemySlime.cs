using UnityEngine;

public enum SlimeType
{
    big, 
    medium,
    small
}

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

    [Header("Slime Specific")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int slimeCreate;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minCreationVelocity;
    [SerializeField] private Vector2 maxCreationVelocity; 
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

        if (slimeType == SlimeType.small)
            return;

        CreateSlime(slimeCreate, slimePrefab);
    }

    public override void Footstep()
    {
        int[] footstepIndexes = { 33, 34, 35, 36 };
        int sfxIndex = footstepIndexes[Random.Range(0, footstepIndexes.Length)];

        AudioManager.instance.PlaySFX(sfxIndex, transform);
    }

    private void CreateSlime(int _amountOfSlimes, GameObject _slimePrefab)
    {
        for (int i = 0; i < _amountOfSlimes; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, _slimePrefab.transform.rotation);
            newSlime.GetComponent<EnemySlime>().SetUpSlime();
        }
    }

    public void SetUpSlime()
    {
        float xVelocity = Random.Range(minCreationVelocity.x, maxCreationVelocity.x);
        float yVelocity = Random.Range(minCreationVelocity.y, maxCreationVelocity.y);

        isKnocked = true;


        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(xVelocity, yVelocity);

        Invoke(nameof(CancelKnockBack), 1.5f);
    }



    private void CancelKnockBack() => isKnocked = false;

    
}
