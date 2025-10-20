using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = 2;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0;

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);

        float attackDir = player.facingDir;
        if (xInput != 0)
            attackDir = xInput;
        
        // player.anim.speed = 1.1f; // could decrease the speed if holding heavy sword or potion
        player.SetVelocity(player.attactMovement[comboCounter].x * attackDir, player.attactMovement[comboCounter].y);

        stateTimer = .15f;
    }
    public override void Exit()
    {
        base.Exit();
        // player.anim.speed = 1;
        // player.StartCoroutine("BusyFor", .15f); freeze the character while attacking
        comboCounter++;
        lastTimeAttacked = Time.time;
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetVelocity(0, 0);

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }

    }
}
