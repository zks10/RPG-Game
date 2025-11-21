using UnityEngine;

public class EnemyStats : CharacterStats
{
    [SerializeField] private GameObject soulOrbPrefab;
    [SerializeField] private int minCoins = 3;
    [SerializeField] private int maxCoins = 6;

    [Header("Level Details")]
    [SerializeField] private int level = 1; 
    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .4f;

    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stat soulDropAmount;

    protected override void Start()
    {
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
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }
    protected override void Die()
    {
        base.Die();
        enemy.Die();

        //PlayerManager.instance.currency += soulDropAmount.GetValue();
        SpawnSoulOrbs();
        myDropSystem.GenerateDrops();
    }

    private void SpawnSoulOrbs()
    {
        int totalSouls = soulDropAmount.GetValue();
        int coinCount = Random.Range(minCoins, maxCoins + 1);
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
