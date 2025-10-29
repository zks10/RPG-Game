using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, 0); 
        if (player.jumpForce != player.defaultJumpForce)
        {
            player.SetJump(player.defaultJumpForce);
        }
    }
    public override void Exit() 
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if (xInput != 0 && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }
        if (player.CheckDoubleTapKeyDown())
        {
            stateMachine.ChangeState(player.runningState);
            return;
        }
    }

}
