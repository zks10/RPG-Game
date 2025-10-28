using UnityEngine;

public class EnemyState
{
    protected Enemy enemybase;
    protected EnemyStateMachine stateMachine;
    protected bool triggerCalled;
    protected string animBoolName;
    protected float stateTimer;
    protected Rigidbody2D rb;

    public EnemyState(Enemy _enemybase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemybase = _enemybase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
 
    public virtual void Enter() 
    {
        triggerCalled = false;
        rb = enemybase.rb;
        enemybase.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        enemybase.anim.SetBool(animBoolName, false);
        enemybase.AssignLastAnimName(animBoolName);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

}
