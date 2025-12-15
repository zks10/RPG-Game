
using UnityEngine;

public class DeathBringerGroundedState : EnemyState
{
    protected EnemyDeathBringer enemy;
    private Transform player;
    public DeathBringerGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDectected())
            stateMachine.ChangeState(enemy.detectPlayerState);
        else if(Vector2.Distance(player.transform.position, enemy.transform.position) < enemy.activateAttackDistance)
            stateMachine.ChangeState(enemy.battleState);
    }
} 
