using UnityEngine;

public class PlayerAirState : PlayerState
{
    private bool isRunning;
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        isRunning = false;
        base.Enter();
        if (player.jumpForce != player.defaultJumpForce)
        {
            player.SetJump(player.defaultJumpForce);
            isRunning = true;
        }
    }
    public override void Exit()
    {
        base.Exit(); 
    }
    public override void Update()
    {
        base.Update();
        if (player.IsWallDectected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        if (player.IsGroundDectected())
        {
            stateMachine.ChangeState(player.idleState);
        }
        if (xInput != 0)
        {
            if (isRunning)
                player.SetVelocity(player.moveSpeed * 0.9f * xInput, rb.linearVelocity.y);
            else
                player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.linearVelocity.y);
            isRunning = false;  
        }
    }
}
