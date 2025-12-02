using UnityEngine;

public class PlayerDashState : PlayerState
{
    private bool prevHasDoubleJumped;

    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.stats.MakeInvencible(true);
        prevHasDoubleJumped = player.hasDoubleJumped;
        player.skill.dash.CloneOnDashStart();
        AudioManager.instance.PlaySFX(20, player.transform);
        stateTimer = player.dashDuration;
    }
    public override void Exit()
    {
        base.Exit();
        player.stats.MakeInvencible(false);
        player.skill.dash.CloneOnDashEnd();
        player.SetVelocity(0, rb.linearVelocity.y);
        player.hasDoubleJumped = prevHasDoubleJumped;
        player.SetSkillActive(false);
    }
    public override void Update()
    {
        base.Update();
        if (!player.IsGroundDectected() && player.IsWallDectected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (stateTimer < 0)
        {
            if (player.isDead)
                stateMachine.ChangeState(player.deathState);
            else if (player.IsGroundDectected())
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.airState); 
        }
        player.fx.CreateAfterImage();
    }
}
