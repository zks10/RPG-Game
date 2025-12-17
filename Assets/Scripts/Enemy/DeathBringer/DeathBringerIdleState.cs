using UnityEngine;

public class DeathBringerIdleState : DeathBringerGroundedState
{
    private Transform player;
    public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        player = PlayerManager.instance.player.transform;
        int randomIdle = Random.Range(3, 6);
        //AudioManager.instance.PlaySFX(randomIdle, enemy.transform);
    }
    public override void Exit()
    {
        base.Exit();

    }
    public override void Update()
    {
        base.Update();


        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }

        if (Vector2.Distance(player.position, enemy.transform.position) < 7)
        {
            enemy.bossFightBegun = true; 
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            stateMachine.ChangeState(enemy.teleportState);
        }

        if (stateTimer < 0 && enemy.bossFightBegun)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
