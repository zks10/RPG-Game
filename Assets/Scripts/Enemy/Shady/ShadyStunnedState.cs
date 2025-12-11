using UnityEngine;

public class ShadyStunnedState : EnemyState
{
    private EnemyShady enemy;
    public ShadyStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyShady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
        enemy.fx.InvokeRepeating("CancelColorChange", 0, 0);
    }
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0) {
            stateMachine.ChangeState(enemy.battleState); 
        }
    }
}
