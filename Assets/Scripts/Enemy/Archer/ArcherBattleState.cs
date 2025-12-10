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

        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.position, enemy.transform.position) > 15f)
            {
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
        }

        if (player.position.x > enemy.transform.position.x && !VeryClose() && enemy.facingDir == -1)
            enemy.Flip();
        else if (player.position.x < enemy.transform.position.x && !VeryClose() && enemy.facingDir == 1)
            enemy.Flip();
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
        if (enemy.GroundBehindCheck() == false || enemy.WallBehindCheck() == true)
            return false;
        if (Time.time >= enemy.lastTimeJumped + enemy.jumpCooldown)
        {
            enemy.lastTimeJumped = Time.time;
            return true;
        }

        return false;
    }


}
