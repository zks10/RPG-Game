using UnityEngine;

public class DeathBringerIdleState : DeathBringerGroundedState
{
    public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;

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

        if (Input.GetKeyDown(KeyCode.V))
        {
            stateMachine.ChangeState(enemy.teleportState);
        }
    }
}
