using UnityEngine;

public class ShadyAttackState : EnemyState
{
    private EnemyShady enemy;
    public ShadyAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyShady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();

    }
    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }
    public override void Update()
    {
        base.Update();
        enemy.ZeroVelocity();



        if (triggerCalled)
        {
            enemy.ZeroVelocity();
            enemy.HasAttacked(); 
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
