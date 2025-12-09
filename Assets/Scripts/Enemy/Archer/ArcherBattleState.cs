using UnityEngine;

public class ArcherBattleState : EnemyState
{
    private EnemyArcher enemy;
    private Transform player;
    private const float veryCloseDistance = 0.2f;

    public ArcherBattleState(
        Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyArcher _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Update()
    {
        base.Update();

        RaycastHit2D detection = enemy.IsPlayerDectected();

        // Player detected ------------------------------------------------------
        if (detection)
        {
            stateTimer = enemy.battleTime;

            float distance = detection.distance;

            // 1. Too close → Jump away
            if (distance < enemy.safeDistance && CanJump())
            {
                stateMachine.ChangeState(enemy.jumpState);
                return;
            }

            // 2. In attack range → shoot
            if (distance < enemy.attackDistance && CanAttack())
            {
                stateMachine.ChangeState(enemy.attackState);
                return;
            }

            // 3. Otherwise → circle or reposition
            //HandleMovement();
        }
        else
        {
            // return to idle after battle timer ends or if player too far
            if (stateTimer < 0 || Vector2.Distance(player.position, enemy.transform.position) > 15f)
            {
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
        }
    }

    // FIXED — Now returns TRUE when you ARE allowed to attack
    private bool CanAttack()
    {
        // Enemy can attack if cooldown time has passed
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true; 
        }

        return false;
    }

    private bool VeryClose()
    {
        return Mathf.Abs(player.position.x - enemy.transform.position.x) < veryCloseDistance;
    }

    private bool CanJump()
    {
        if (Time.time >= enemy.lastTimeJumped + enemy.jumpCooldown)
        {
            enemy.lastTimeJumped = Time.time;
            return true;
        }

        return false;
    }


}
