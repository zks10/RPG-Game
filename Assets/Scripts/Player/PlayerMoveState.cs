using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX(6);
    }
    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(6);
    }
    public override void Update()
    {
        base.Update();
        player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocity.y);

        if (xInput == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
        
    }

}
