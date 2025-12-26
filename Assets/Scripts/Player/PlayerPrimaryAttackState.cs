using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; }
    private float lastTimeAttacked;
    private float comboWindow = 2;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        AudioManager.instance.PlaySFX(comboCounter == 0 ? 1 : 2);

        xInput = 0;
        player.anim.SetInteger("ComboCounter", comboCounter);

        float attackDir = player.facingDir;

        // ✅ Attack speed from weapon
        ItemData_Equipment weapon = Inventory.instance.GetEquipmentByType(EquipmentType.Weapon);
        float speedMult = weapon != null ? weapon.GetAttackSpeedMultiplier() : 1f;
        player.anim.speed = speedMult;

        player.SetVelocity(player.attactMovement[comboCounter].x * attackDir,
                        player.attactMovement[comboCounter].y);

        stateTimer = .15f;
    }
    public override void Exit()
    {
        base.Exit();
        player.anim.speed = 1f;
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
