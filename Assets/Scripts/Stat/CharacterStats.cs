using UnityEngine;
using System.Collections;
using TMPro;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critRate,
    critDamage,
    HP,
    armor,
    evasion,
    magicResistence,
    fireDamage,
    iceDamage,
    lightningDamage
}
public enum DamageType
{
    Physical,
    Magic,
    Critical
}
public class CharacterStats : MonoBehaviour
{
    protected EntityFx fx;
    [Header("Major Stats")]
    public Stat strength; // ± 1 point => damage ± 1 && crit.damage ± 1%
    public Stat agility; // ± 1 point => evasion ± 1 && crit.rate ± 1%
    public Stat intelligence; // ± 1 point => magic damage ± 1 && magic resistence ± 3
    public Stat vitality; // ± 1 point => health ± 3~5

    [Header("Defensive Stats")]
    public Stat maxHP;
    public int currentHP;
    public System.Action onHealthChanged;
    public System.Action onDeath;
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
    public Stat lightningDamage;
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
    public bool isVolunerable { get; private set; }
    public bool isInvencible { get; private set; }
    public Transform lastDamageSource { get; private set; }
    public bool diedInVoid { get; set; } = false;



    protected virtual void Awake()
    {
        currentHP = GetMaxHP();
        critDamage.SetDefaultValue(150);
        fx = GetComponent<EntityFx>();

    }
    protected virtual void Start()
    {
        
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

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }
    public int GetAilmentDuration() => ailmentDuration;

    private IEnumerator VolunerableForCoroutine(float _duration)
    {
        isVolunerable = true;
        yield return new WaitForSeconds(_duration);
        isVolunerable = false;
    }
    
    public void MakeVolunerableFor(float _duration) =>  StartCoroutine(VolunerableForCoroutine(_duration));
    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }
    public virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        onDeath?.Invoke();
    }


    protected virtual void DecreaseHPBy(int _damage)
    {
        if (isDead) 
            return; 
            
        if (isVolunerable)
            _damage = Mathf.RoundToInt(_damage * 1.1f);
        currentHP -= _damage;


        onHealthChanged?.Invoke();
    }
    public virtual void IncreaseHPBy(int _heal)
    {
        currentHP += _heal;
        if (currentHP > GetMaxHP())
            currentHP = GetMaxHP();
        onHealthChanged?.Invoke();
    }
    public virtual void TakeDamage(int _damage, Transform _damageSource, DamageType type = DamageType.Physical)
    {
        lastDamageSource = _damageSource;
        TakeDamage(_damage, type);
    }

    public virtual void TakeDamage(int _damage, DamageType type = DamageType.Physical)
    {
        if (isInvencible)
            return;

        DecreaseHPBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        Color popupColor = Color.white;

        switch (type)
        {
            case DamageType.Physical:
                fx.CreatePopUpText(
                    _damage.ToString(),
                    Color.white,
                    1f,
                    FontStyles.Normal,
                    usePunch: false,
                    useSquash: false,
                    useCurve: false
                );
                break;

            case DamageType.Magic:
                fx.CreatePopUpText(
                    _damage.ToString(),
                    new Color(0.85f, 0.45f, 1f),
                    1f,
                    FontStyles.Normal,
                    usePunch: false,
                    useSquash: false,
                    useCurve: true
                );
                break;

            case DamageType.Critical:
                fx.CreatePopUpText(
                    _damage.ToString(),
                    Color.yellow,
                    1.5f,
                    FontStyles.Bold,
                    usePunch: true,
                    useSquash: false,
                    useCurve: true
                );
                break;
        }


        if (currentHP <= 0 && !isDead)
            Die();
    }

    public void MakeInvencible(bool _invencible) => isInvencible = _invencible;

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
    public virtual void DoMagicalDamage(CharacterStats _targetStats, float multiplier = 1f)
    {
        if (_targetStats.isInvencible)
            return;
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistence(_targetStats, totalMagicalDamage);
        totalMagicalDamage = Mathf.RoundToInt(totalMagicalDamage * multiplier);
        _targetStats.TakeDamage(totalMagicalDamage, transform, DamageType.Magic);
    

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
            return;

        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }
    private void AttemptToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyFreeze = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

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
            if (Random.value >= 2f / 3f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyFreeze, canApplyShock);
                return;
            }

        }
        if (canApplyIgnite)
            _targetStats.SetIgniteDamage(Mathf.RoundToInt(_fireDamage * .1f));

        if (canApplyShock)
            _targetStats.SetShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * .2f));

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
                if (hit.GetComponent<EnemyStats>().isDead) continue;
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
            newShockStrike.GetComponent<ShockStrikeController>().Setup(shockStrikeDamage, closestEnemy.GetComponent<CharacterStats>());
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

    public virtual void OnEvasion()
    {
        
    }
    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }
        return false;
    }
    protected int CheckTargetsArmor(CharacterStats _targetStats, int totalDamage)
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
    protected bool CanCrit()
    {
        int totalCritRate = critRate.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) < totalCritRate)
        {
            return true;
        }
        return false;
    }
    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritDamage = (critDamage.GetValue() + strength.GetValue()) * .01f;

        float critPower = _damage * totalCritDamage;

        return Mathf.RoundToInt(critPower);
    }
    

    #endregion

    #region Physical Damage
    public virtual void DoPhysicalDamage(CharacterStats _targetStats, float damageMultiplier = 1f)
    {
        bool critStrike = false;

        if (_targetStats.isInvencible)
            return;

        if (TargetCanAvoidAttack(_targetStats))
            return;
            
        _targetStats.GetComponent<Entity>().SetUpKnockBackDir(transform);
        
        int totalPhysicalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalPhysicalDamage = CalculateCriticalDamage(totalPhysicalDamage);
            critStrike = true;
        }
        fx.CreateHitFX(_targetStats.transform, critStrike);
        totalPhysicalDamage = CheckTargetsArmor(_targetStats, totalPhysicalDamage);
        totalPhysicalDamage = Mathf.RoundToInt(totalPhysicalDamage * damageMultiplier);
        if (critStrike)
            _targetStats.TakeDamage(totalPhysicalDamage, transform, DamageType.Critical);
        else 
            _targetStats.TakeDamage(totalPhysicalDamage, transform, DamageType.Physical);
    }

    #endregion

    public Stat GetStat(StatType _statType)
    {
        switch (_statType)
        {
            case StatType.strength: return strength;
            case StatType.agility: return agility;
            case StatType.intelligence: return intelligence;
            case StatType.vitality: return vitality;
            case StatType.damage: return damage;
            case StatType.critRate: return critRate;
            case StatType.critDamage: return critDamage;
            case StatType.HP: return maxHP;
            case StatType.armor: return armor;
            case StatType.evasion: return evasion;
            case StatType.magicResistence: return magicResistence;
            case StatType.fireDamage: return fireDamage;
            case StatType.iceDamage: return iceDamage;
            case StatType.lightningDamage: return lightningDamage;
            default:
                return null;
        }
    }
    
    public int GetCalculatedStatValue(StatType statType)
    {
        switch (statType)
        {
            case StatType.HP:
                return GetMaxHP();

            case StatType.damage:
                return damage.GetValue() + strength.GetValue();

            case StatType.critDamage:
                return critDamage.GetValue() + strength.GetValue();

            case StatType.critRate:
                return critRate.GetValue() + agility.GetValue();

            case StatType.evasion:
                return evasion.GetValue() + agility.GetValue();

            case StatType.magicResistence:
                return magicResistence.GetValue() + (intelligence.GetValue() * 3);

            default:
                return GetStat(statType)?.GetValue() ?? 0;
        }
    }

    public void ReflectDamage(CharacterStats attacker, int originalDamage)
    {
        if (attacker == null)
            return;

        float reflectPercent = 0.5f; // 50%
        int reflectedDamage = Mathf.RoundToInt(originalDamage * reflectPercent);

        attacker.TakeDamage(reflectedDamage);

    }
    public virtual void DoDamageToPlayer(PlayerStats _target) { }

}
