using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        if (!player.stats.diedInVoid)
            GameObject.Find("UI_Manager").GetComponent<UI>().SwitchOnEndScreen();
    }
    public override void Exit()
    {
        base.Exit();
        
    }
    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();


    }
}
