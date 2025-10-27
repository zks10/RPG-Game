using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    // private bool canCreateClone; only want one counter clone
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        // canCreateClone = true; only want one counter clone
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SucessfulCounterAttack", false);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();
        
        Collider2D [] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders) {
            if (hit.GetComponent<Enemy>() != null) {
                if (hit.GetComponent<Enemy>().CanBeStunned()) {
                    stateTimer = 10;
                    player.counterAttackUsageTimer = 0;
                    player.anim.SetBool("SucessfulCounterAttack", true);
                    // If only one counter clone
                    // if (canCreateClone)
                    // {
                    //     canCreateClone = false;
                    //     player.skill.clone.CreateCloneOnCounterAttack(hit.transform);
                    // }
                    player.skill.clone.CreateCloneOnCounterAttack(hit.transform);
                }
            }
        }
        if (stateTimer < 0 || triggerCalled) {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
