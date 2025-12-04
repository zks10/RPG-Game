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
        player.SetSkillActive(false);
    }
    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();
        
        Collider2D [] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders) {
            if (hit.GetComponent<Enemy>() != null) 
            {
                if (hit.GetComponent<EnemyStats>().isDead) continue;
                if (hit.GetComponent<Enemy>().CanBeStunned()) 
                {
                    stateTimer = 10;
                    player.anim.SetBool("SucessfulCounterAttack", true);
                    player.MarkCounterSuccess();
                    player.fx.ScreenShake(player.fx.counterAttackShake);
                    AudioManager.instance.PlaySFX(0);

                    player.skill.counterAttack.UseSkill();
                    // If only one counter clone
                    // if (canCreateClone)
                    // {
                    //     canCreateClone = false;
                    //     player.skill.coounterAttack.MakeMirageOnCounterAttack(hit.transform);
                    // }
                    if (player.skill.counterAttack.mirageCounterAttackUnlocked)
                        player.skill.clone.CreateCloneWithDelay(hit.transform);
                } 
                else 
                    player.ResetCounterSuccess();
            }
        }
        if (stateTimer < 0 || triggerCalled) {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
