
using UnityEngine;

public class ArcherGroundedState : EnemyState
{
    protected EnemyArcher enemy;
    private Transform player;
    public ArcherGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyArcher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
