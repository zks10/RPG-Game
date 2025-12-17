
using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
    
    private EnemyDeathBringer enemy;
    private Transform player;
    private int moveDir;
    private const float veryCloseDistance = 0.2f;

    public DeathBringerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeathBringer _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.ZeroVelocity();
        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        var detection = enemy.IsPlayerDectected();

        if (detection)
        {
            stateTimer = enemy.battleTime;

            if (detection.distance < enemy.attackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
                // else 
                //     stateMachine.ChangeState(enemy.idleState);

            }
        }


        if (player.position.x > enemy.transform.position.x && !VeryClose() && enemy.IsGroundDectected())
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x && !VeryClose() && enemy.IsGroundDectected())
            moveDir = -1;
        
        if (enemy.IsPlayerDectected() && enemy.IsPlayerDectected().distance < enemy.attackDistance - 1f)
            return;
        enemy.SetVelocity(enemy.battleMoveSpeed * moveDir, rb.linearVelocity.y);
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return false;
        }
        return true;
    }

    private bool VeryClose()
    {
        return Mathf.Abs(player.position.x - enemy.transform.position.x) < veryCloseDistance ;
    }
}
 