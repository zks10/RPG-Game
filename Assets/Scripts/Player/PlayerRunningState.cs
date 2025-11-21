using UnityEngine;

public class PlayerRunningState : PlayerGroundedState
{
    public PlayerRunningState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        player.SetJump(player.jumpForce + 2);
        base.Enter();
        AudioManager.instance.PlaySFX(7);
    }
    public override void Exit()
    {
        base.Exit();
        if (player.jumpForce > player.defaultJumpForce + 2)
            player.SetJump(player.defaultJumpForce + 2);
        AudioManager.instance.StopSFX(7);
    }
    public override void Update()
    {
        base.Update();
        player.SetVelocity(xInput * player.runSpeed, rb.linearVelocity.y);

        if (xInput == 0)
            stateMachine.ChangeState(player.idleState);
        // else if (xInput != player.facingDir)
        //     stateMachine.ChangeState(player.moveState);

    }
}
