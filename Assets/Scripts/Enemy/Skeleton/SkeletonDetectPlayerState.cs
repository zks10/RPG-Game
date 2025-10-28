using UnityEngine;

public class SkeletonDetectPlayerState : EnemyState
{
    private EnemySkeleton enemy;
    public SkeletonDetectPlayerState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
