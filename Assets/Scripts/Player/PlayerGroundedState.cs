using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.hasDoubleJumped = false;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttackState);

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!player.swordOut && player.skill.sword.swordUnlock)
                stateMachine.ChangeState(player.aimSwordState);
            else if (player.swordOut)
                player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        }

        if (Input.GetKeyDown(KeyCode.Q) && player.counterAttackUsageTimer < 0 && player.skill.counterAttack.counterAttackUnlocked)
        {
            player.counterAttackUsageTimer = player.counterAttackCooldown;
            stateMachine.ChangeState(player.counterAttackState);
        }

        if (Input.GetKeyDown(KeyCode.E) && player.skill.blackhole.blackholeUnlock)
        {
            if (player.skill.blackhole.cooldownTimer > 0)
                return;
            
            stateMachine.ChangeState(player.blackholeState);
        }
            
        if (!player.IsGroundDectected())
        {
            stateMachine.ChangeState(player.airState);
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && player.IsGroundDectected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }

    private bool HasNoSword() => !player.swordOut;
    

}
