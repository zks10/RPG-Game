using UnityEngine;

public class EnemyStats : CharacterStats
{
    [Header("Level Details")]
    [SerializeField] private int level = 1; 
    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .4f;

    private Enemy enemy;
    protected override void Start()
    {
        ApplyLevelModify();
        base.Start();
        enemy = GetComponent<Enemy>();
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;
            _stat.AddModifier(Mathf.RoundToInt(modifier));

        }
    }

    private void ApplyLevelModify()
    {
        // Major stats
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        // Defensive stats
        Modify(armor);
        Modify(evasion);
        Modify(magicResistence);
        Modify(maxHP); 

        // Attack stats
        Modify(critRate);
        Modify(critDamage);
        Modify(damage);

        // Magic stats
        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);
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
