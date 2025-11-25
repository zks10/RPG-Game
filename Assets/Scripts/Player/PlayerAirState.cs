using UnityEngine;

public class PlayerAirState : PlayerState
{
    private bool isRunning;
    private bool bufferedJump;
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        isRunning = false;
        bufferedJump = false;
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
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            bufferedJump = true;

        if (bufferedJump)
        {
            if (player.canDoubleJump && !player.hasDoubleJumped)
            {
                player.hasDoubleJumped = true;
                bufferedJump = false;
                stateMachine.ChangeState(player.jumpState);
                return;
            }
        }

        if (player.IsWallDectected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        if (player.IsGroundDectected())
        {
            AudioManager.instance.PlaySFX(18, player.transform);
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
