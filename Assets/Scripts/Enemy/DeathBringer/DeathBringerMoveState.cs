using UnityEngine;

public class DeathBringerMoveState : DeathBringerGroundedState
{


    public DeathBringerMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
       
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rb.linearVelocity.y);

        if (enemy.IsWallDectected() || !enemy.IsGroundDectected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
    }


}
