using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
 
    } 

    public override void Enter()
    {
        base.Enter();
        //AudioManager.instance.PlaySFX(26, player.transform);
    }
    public override void Exit()
    {
        base.Exit();
        //AudioManager.instance.StopSFX(26);
    }
    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }
        if (yInput < 0)
        {
            player.SetVelocity(0, rb.linearVelocity.y);
            // AudioManager.instance.StopSFX(26);
            // float slideSpeed = Mathf.Abs(rb.velocity.y);
            // float pitch = Mathf.Clamp(0.8f + slideSpeed / 10f, 0.8f, 2f);
            // AudioManager.instance.PlaySFX(26, player.transform, pitch);

        }
        else
        {
            player.SetVelocity(0, rb.linearVelocity.y * 0.7f);
        }

        if (xInput != 0 && player.facingDir != xInput || player.IsGroundDectected())
        {
            stateMachine.ChangeState(player.idleState);
        }

        if ( !player.IsWallDectected())
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
