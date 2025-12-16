using UnityEngine;

public class DeathBringerTeleportState : EnemyState
{
    private EnemyDeathBringer enemy;
    public DeathBringerTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        //enemy.SetHPBarStatus(false);
        enemy.stats.MakeInvencible(true);

    }
    public override void Exit()
    {
        base.Exit();
        enemy.stats.MakeInvencible(false);

    }
    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            // enemy.FindPosition();
            // enemy.SetHPBarStatus(true);
            stateMachine.ChangeState(enemy.battleState);
        }

    }
}
