using UnityEngine;

public class ArcherAttackState : EnemyState
{
    private EnemyArcher enemy;
    public ArcherAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyArcher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        float randomAttack = Random.value < 0.5f ? 0f : 1f;
        if (randomAttack == 0)
            enemy.attackCheck.localPosition = enemy.lowerAttackOffset;
        else
            enemy.attackCheck.localPosition = enemy.normalAttackOffset;
        enemy.anim.SetFloat("attackIndex", randomAttack);

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
