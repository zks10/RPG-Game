using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();
    }

    protected override void Update()
    {
        base.Update();
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }
    protected override void Die()
    {
        base.Die();
        enemy.Die(); 
    }

}
