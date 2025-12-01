using UnityEngine;

public class PlayerBlackholeState : PlayerState 
{
    private float flyTime = .4f;
    private bool skillUsed;
    private float defaultGravity;
    private float blackHoleJumpHeight = 9f;
    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        skillUsed = false;
        stateTimer = flyTime;
        defaultGravity = rb.gravityScale;
        rb.gravityScale = 0;
        player.SetCanPickItm(false);
        AudioManager.instance.PlaySFX(13, player.transform);
    }
    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravity;
        player.fx.MakeTransparent(false);
        player.SetCanPickItm(true);
        player.isSkillActive = false;
    }
    public override void Update()
    {
        base.Update();
        
        if (stateTimer > 0)
            rb.linearVelocity = new Vector2(0, blackHoleJumpHeight);

        if (stateTimer < 0)
        {
            rb.linearVelocity = Vector2.zero;
            if (!skillUsed)
            {
                if (player.skill.blackhole.CanUseSkill())
                    skillUsed = true;
            }
        }
        if (player.skill.blackhole.SkillCompleted())
            stateMachine.ChangeState(player.airState);

    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
