using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
 
    } 

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
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
            stateMachine.ChangeState(player.idleState);
        }
    }
}
