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
    public override void TakeDamage(int _damage, DamageType type = DamageType.Physical)
    {
        base.TakeDamage(_damage, type);
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
            player.fx.ScreenShake(player.fx.highDamageShake);
            player.SetUpKnockBackPower(new Vector2(10, 6));
        }
    }

    protected override void DecreaseHPBy(int _damage)
    {
        base.DecreaseHPBy(_damage);

        GetSiginificantDamage(_damage);

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
