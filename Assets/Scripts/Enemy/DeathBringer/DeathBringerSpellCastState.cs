using UnityEngine;

public class DeathBringerSpellCastState : EnemyState
{
    private EnemyDeathBringer enemy;
    private float spellTimer;
    private int amountOfSpells;
    public DeathBringerSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        amountOfSpells = enemy.amountOfSpells;
        
        spellTimer =  0.5f;
    }
    public override void Update()
    {
        base.Update();

        spellTimer -= Time.deltaTime;

        if (CanCast())
            enemy.CastSpell();
        
        
        if (amountOfSpells <= 0) 
            stateMachine.ChangeState(enemy.teleportState);


    }
    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeCast = Time.time;
    }
    private bool CanCast()
    {
        if (amountOfSpells > 0 && spellTimer < 0)
        {
            amountOfSpells -= 1;
            spellTimer = enemy.spellCooldown;
            return true;
        }
        return false;
    }
}
