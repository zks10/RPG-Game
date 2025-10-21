
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;

        catchSword();

        rb.linearVelocity = new Vector2(player.swordReturnImpact * -player.facingDir, rb.linearVelocity.y);

    }
    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.1f);
    }
    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);

    }
    public void catchSword()
    {
        if (player.transform.position.x > sword.position.x && player.facingRight)
            player.Flip();
        else if (player.transform.position.x < sword.position.x && !player.facingRight)
            player.Flip();
    }
}
