
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(19, player.transform);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.jumpForce);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        
        if (rb.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
        if (xInput != 0)
        {
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.linearVelocity.y);
        }

    }
}
