using UnityEngine;

public class PlayerStats : CharacterStats, ISaveManager
{
    private Player player;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
        if (currentHP <= 0)
            currentHP = GetMaxHP();
        player.stats.diedInVoid = false;

    }

    protected override void Update()
    {
        base.Update();
    }

    public void LoadData(GameData _data)
    {
        currentHP = _data.currentHP;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currentHP = this.currentHP;
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        //int randomIdx = Random.Range(27, 30);
        if (!isInvencible)
            AudioManager.instance.PlaySFX(27, player.transform);
    }
    public override void Die()
    {
        base.Die();
        player.Die();
        GetComponent<PlayerItemDrop>().GenerateDrops();
        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;

    }

    private void GetSiginificantDamage(int _damage)
    {
        if (_damage > GetMaxHP() * 0.3f)
        {
            if (lastDamageSource != null)
            {
                // 1 = knock to the right, -1 = knock to the left
                int _knockDir = transform.position.x > lastDamageSource.position.x ? 1 : -1;
                Debug.Log(_knockDir);
                // Start with horizontal knockback via knockDir only
                Vector2 dir = new Vector2(_knockDir, 0f);

                // Add vertical arc
                dir.y = 0.6f;

                // Normalize so magnitude stays consistent
                // dir = dir.normalized;

                float force = 10f;
                player.SetUpKnockBackPower(dir * force);
            }
        }
    }

    protected override void DecreaseHPBy(int _damage)
    {
        base.DecreaseHPBy(_damage);

        // GetSiginificantDamage(_damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipmentByType(EquipmentType.Armor);

        if (currentArmor != null)
        {
            currentArmor.ItemEffect(player.transform);
        }
    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();
    }
    
    public void CloneDoDamage(CharacterStats _targetStats, float _multiplier)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;
        int totalPhysicalDamage = damage.GetValue() + strength.GetValue();

        if (_multiplier > 0)
            totalPhysicalDamage = Mathf.RoundToInt(totalPhysicalDamage * _multiplier);
            
        if (CanCrit())
            totalPhysicalDamage = CalculateCriticalDamage(totalPhysicalDamage);

        totalPhysicalDamage = CheckTargetsArmor(_targetStats, totalPhysicalDamage);
        _targetStats.TakeDamage(totalPhysicalDamage);
    }
}
