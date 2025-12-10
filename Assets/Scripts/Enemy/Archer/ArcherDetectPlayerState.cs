using UnityEngine;

public class ArcherDetectPlayerState : EnemyState
{
    private EnemyArcher enemy;
    public ArcherDetectPlayerState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyArcher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.3f;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        enemy.ZeroVelocity();

        if (triggerCalled || stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
