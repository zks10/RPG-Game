using UnityEngine;

public class ShadyDetectPlayerState : EnemyState
{
    private EnemyShady enemy;
    public ShadyDetectPlayerState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyShady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
