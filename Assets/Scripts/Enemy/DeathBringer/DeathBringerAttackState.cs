using UnityEngine;

public class DeathBringerAttackState : EnemyState
{
    private EnemyDeathBringer enemy;
    public DeathBringerAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
