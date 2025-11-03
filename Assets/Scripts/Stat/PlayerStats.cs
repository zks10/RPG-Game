using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
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
        player.Die();
        GetComponent<PlayerItemDrop>().GenerateDrops();
    }
    
    protected override void DecreaseHPBy(int _damage)
    {
        base.DecreaseHPBy(_damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipementByType(EquipmentType.Armor);

        if (currentArmor != null)
        {
            currentArmor.ItemEffect(player.transform);
        }
    }
}
