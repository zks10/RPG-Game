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
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        spellTimer -= Time.deltaTime;

        if (CanCast())
        {
            amountOfSpells -= 1;
            enemy.CastSpell();
        }
        
        if (amountOfSpells <= 0) 
            stateMachine.ChangeState(enemy.teleportState);


    }

    private bool CanCast()
    {
        if (amountOfSpells > 0 && spellTimer < 0)
        {
            spellTimer = enemy.spellCooldown;
            return true;
        }
        return false;
    }
}
