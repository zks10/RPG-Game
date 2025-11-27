
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    
    private EnemySkeleton enemy;
    private Transform player;
    private int moveDir;
    private const float veryCloseDistance = 0.2f;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
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

        enemy.SetVelocity(enemy.battleMoveSpeed * moveDir, rb.linearVelocity.y);

        var detection = enemy.IsPlayerDectected();

        if (detection)
        {
            stateTimer = enemy.battleTime;

            if (detection.distance < enemy.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.position, enemy.transform.position) > 15)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (player.position.x > enemy.transform.position.x && !VeryClose() && enemy.IsGroundDectected())
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x && !VeryClose() && enemy.IsGroundDectected())
            moveDir = -1;
    }

    private bool CanAttack()
    {
        // return Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown;
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            //enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown); 
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
 