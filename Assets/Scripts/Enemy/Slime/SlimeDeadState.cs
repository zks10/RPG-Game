
using UnityEngine;

public class SlimeDeadState : EnemyState
{
    protected EnemySlime enemy;
    public SlimeDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        // enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        // enemy.anim.speed = 0;
        // enemy.cd.enabled = false;

        stateTimer = .5f;

    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        // if (stateTimer > 0)
        //     rb.linearVelocity = new Vector2(0, 10);
        if (stateTimer < 0)
        {
            enemy.sr.color = new Color(1, 1, 1, enemy.sr.color.a - (Time.deltaTime * 1f)); 

            if (enemy.sr.color.a <= 0)
            {
                // enemy.anim.SetBool(enemy.lastAnimBoolName, true);
                // enemy.anim.speed = 0;
                // rb.linearVelocity = new Vector2(0, 10);
            }
        }
    }
} 
