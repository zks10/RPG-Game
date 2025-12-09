using UnityEngine;

public class ArcherJumpState : EnemyState
{
    private EnemyArcher enemy;
    public ArcherJumpState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyArcher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
        rb.linearVelocity = new Vector2(enemy.jumpVelocity.x * -enemy.facingDir, enemy.jumpVelocity.y);
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
        enemy.anim.SetFloat("yVelocity", rb.linearVelocity.y);
        if (rb.linearVelocity.y < 0 && enemy.IsGroundDectected())
            stateMachine.ChangeState(enemy.battleState);

    }
}
