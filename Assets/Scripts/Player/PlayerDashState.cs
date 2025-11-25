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
        prevHasDoubleJumped = player.hasDoubleJumped;
        player.skill.dash.CloneOnDashStart();
        AudioManager.instance.PlaySFX(20, player.transform);
        stateTimer = player.dashDuration;
    }
    public override void Exit()
    {
        base.Exit();
        player.skill.dash.CloneOnDashEnd();
        player.SetVelocity(0, rb.linearVelocity.y);
        player.hasDoubleJumped = prevHasDoubleJumped;
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
            if (player.IsGroundDectected())
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.airState); 
        }

    }
}
