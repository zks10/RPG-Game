using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFx fx;
    [Header("Major Stats")]
    public Stat strength; // ± 1 point => damage ± 1 && crit.damage ± 1%
    public Stat agility; // ± 1 point => evasion ± 1 && crit.rate ± 1%
    public Stat intelligence; // ± 1 point => magic damage ± 1 && magic resistence ± 3
    public Stat vitality; // ± 1 point => health ± 3~5

    [Header("Defensive Stats")]
    public Stat maxHP;
    public int currentHP;
    public System.Action onHealthChanged;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistence;

    [Header("Attack Stats")]
    public Stat critRate;
    public Stat critDamage;
    public Stat damage;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;
    [SerializeField] private int ailmentDuration = 3;
    public bool isIgnited; // Does damage over time
    public bool isFreezed; // reduce armor by 20% and move slower
    public bool isShocked; // reduce accuracy by 20%

    private float igniteTimer;
    private float freezeTimer;
    private float shockTimer;

    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;
    private int shockStrikeDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    public bool isDead { get; private set; }

    protected virtual void Start()
    {
        currentHP = GetMaxHP();
        critDamage.SetDefaultValue(150);
        fx = GetComponent<EntityFx>();

    }
    protected virtual void Update()
    {
        igniteTimer -= Time.deltaTime;
        freezeTimer -= Time.deltaTime;
        shockTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;


        if (igniteTimer < 0)
        {
            isIgnited = false;
        }
        if (freezeTimer < 0)
        {
            isFreezed = false;
        }
        if (shockTimer < 0)
        {
            isShocked = false;
        }

        if (isIgnited)
            ApplyIgniteDamage();
    }
    protected virtual void Die()
    {
        isDead = true;
    }

    protected virtual void DecreaseHPBy(int _damage)
    {
        currentHP -= _damage;
        onHealthChanged?.Invoke();
    }
    public virtual void TakeDamage(int _damage)
    {
        DecreaseHPBy(_damage);
        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHP <= 0 && !isDead)
            Die();
    }


    #region Magical Damage and Ailments
    public void SetIgniteDamage(int _damage) => igniteDamage = _damage;
    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            DecreaseHPBy(igniteDamage);
            if (currentHP <= 0 && !isDead)
                Die();
            igniteDamageTimer = igniteDamageCooldown;
        }
    }
    public void SetShockStrikeDamage(int _damage) => shockStrikeDamage = _damage;
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistence(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightingDamage);
    }
    private void AttemptToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyFreeze = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyFreeze && !canApplyShock)
        {
            if (Random.value < 1f / 3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyFreeze, canApplyShock);
                return;
            }
            if (Random.value < 2f / 3f && _iceDamage > 0)
            {
                canApplyFreeze = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyFreeze, canApplyShock);
                return;
            }
            if (Random.value >= 2f / 3f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyFreeze, canApplyShock);
                return;
            }

        }
        if (canApplyIgnite)
            _targetStats.SetIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        if (canApplyShock)
            _targetStats.SetShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .2f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyFreeze, canApplyShock);
    }
    public void ApplyAilments(bool _ignite, bool _freeze, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isFreezed && !isShocked;
        bool canApplyFreeze = !isIgnited && !isFreezed && !isShocked;
        bool canApplyShock = !isIgnited && !isFreezed;


        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            igniteTimer = ailmentDuration;
            fx.IgniteFxFor(ailmentDuration);
        }
        if (_freeze && canApplyFreeze)
        {
            isFreezed = _freeze;
            freezeTimer = ailmentDuration;

            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentDuration);
            fx.FreezeFxFor(ailmentDuration);
        }
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;
                HitNearestEnemyWithStrike();

            }


        }
    }
    private void HitNearestEnemyWithStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 20);
        float closeDist = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(hit.transform.position, transform.position) > 1)
            {
                float dist = Vector2.Distance(hit.transform.position, transform.position);
                if (dist < closeDist)
                {
                    dist = closeDist;
                    closestEnemy = hit.transform;
                }
            }
            if (closestEnemy == null)
                closestEnemy = transform;

        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ThunderStrikeController>().Setup(shockStrikeDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }
    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;
        isShocked = _shock;
        shockTimer = ailmentDuration;
        fx.ShockFxFor(ailmentDuration);
    }

    #endregion

    #region Stats and Calculation
    public int GetMaxHP() => maxHP.GetValue() + vitality.GetValue() * 5;
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Missed attack");
            return true;
        }
        return false;
    }
    private int CheckTargetsArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isFreezed)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private int CheckTargetResistence(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistence.GetValue() + 3 * _targetStats.intelligence.GetValue();
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }
    private bool CanCrit()
    {
        int totalCritRate = critRate.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) < totalCritRate)
        {
            return true;
        }
        return false;
    }
    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritDamage = (critDamage.GetValue() + strength.GetValue()) * .01f;

        float critPower = _damage * totalCritDamage;

        return Mathf.RoundToInt(critPower);
    }
    

    #endregion

    #region Physical Damage
    public virtual void DoPhysicalDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalPhysicalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
            totalPhysicalDamage = CalculateCriticalDamage(totalPhysicalDamage);

        totalPhysicalDamage = CheckTargetsArmor(_targetStats, totalPhysicalDamage);
        _targetStats.TakeDamage(totalPhysicalDamage);
    }

    #endregion


}
