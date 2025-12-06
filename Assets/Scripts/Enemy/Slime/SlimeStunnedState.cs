using UnityEngine;

public class SlimeStunnedState : EnemyState
{
    private EnemySlime enemy;
    public SlimeStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.fx.InvokeRepeating("RedColourBlink", 0, 0.1f);
        stateTimer = 1;
        rb.linearVelocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }
    public override void Exit()
    {
        base.Exit();
        enemy.stats.MakeInvencible(false);
    }
    public override void Update()
    {
        base.Update();
        if (rb.linearVelocity.y < .1f && enemy.IsGroundDectected())
        {
            enemy.fx.InvokeRepeating("CancelColorChange", 0, 0);
            enemy.stats.MakeInvencible(true);
            enemy.anim.SetTrigger("Stunned");
        } 

        if (stateTimer < 0) {
            stateMachine.ChangeState(enemy.battleState); 
        }
    }
}
