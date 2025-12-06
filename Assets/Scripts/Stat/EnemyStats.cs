using UnityEngine;

public enum EnemyType
{
    Skeleton,
    Slime
}
public class EnemyStats : CharacterStats
{
    [SerializeField] private GameObject soulOrbPrefab;
    [SerializeField] private int minCoins = 3;
    [SerializeField] private int maxCoins = 6;

    [Header("Level Details")]
    [SerializeField] private int level = 1; 
    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .4f;

    [Header("Enemy Type")]
    [SerializeField] private EnemyType enemyType;

    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stat soulDropAmount;

    protected override void Start()
    {
        currentHP = GetMaxHP();
        critDamage.SetDefaultValue(150);
        soulDropAmount.SetDefaultValue(100);
        ApplyLevelModify();
        base.Start();
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
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
        Modify(soulDropAmount);
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
        Modify(lightningDamage);
    }


    protected override void Update()
    {
        base.Update();
    }
    public override void TakeDamage(int _damage, DamageType type = DamageType.Physical)
    {
        base.TakeDamage(_damage, type);

        if (enemyType == EnemyType.Skeleton)
        {
            int randomIdx = Random.Range(30, 33); 
            AudioManager.instance.PlaySFX(randomIdx, enemy.transform);
        }
    }
    public override void Die()
    {
        base.Die();
        enemy.Die();

        //PlayerManager.instance.currency += soulDropAmount.GetValue();
        SpawnSoulOrbs();
        myDropSystem.GenerateDrops();
        
        Destroy(gameObject, 1.5f);
    }
    public void DieOutSide()
    {
        enemy.Die();
        Destroy(gameObject, 1.5f);
    }

    private void SpawnSoulOrbs()
    {
        int totalSouls = soulDropAmount.GetValue();
        int coinCount = Random.Range(minCoins, maxCoins + 1);
        if (coinCount == 0)
            return;
        int soulsPerCoin = Mathf.Max(1, totalSouls / coinCount);

        for (int i = 0; i < coinCount; i++)
        {
            GameObject orbObj = Instantiate(soulOrbPrefab, transform.position, Quaternion.identity);
            AudioManager.instance.PlaySFX(10, transform);
            Rigidbody2D rb = orbObj.GetComponent<Rigidbody2D>();
            rb.gravityScale = 1;

            // pop-out force
            rb.linearVelocity = new Vector2(
                Random.Range(-3f, 3f),
                Random.Range(10f, 13f)
            );

            orbObj.GetComponent<SoulOrb>().Init(soulsPerCoin);
        }
    }


}
