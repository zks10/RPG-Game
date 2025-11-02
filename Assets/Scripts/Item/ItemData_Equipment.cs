using UnityEngine;
using System.Collections.Generic;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Items Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType slotType;
    public ItemEffect[] itemEffects;

    [Header("Major stats")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Defensive stats")]
    public int HP;
    public int armor;
    public int evasion;
    public int magicResistence;

    [Header("Attack stats")]
    public int critRate;
    public int critDamage;
    public int damage;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft Requirements")]
    public List<InventoryItem> craftingMaterials;

    public void ItemEffect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        // Major stats
        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        // Defensive stats
        playerStats.maxHP.AddModifier(HP);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistence.AddModifier(magicResistence);

        // Attack stats
        playerStats.critRate.AddModifier(critRate);
        playerStats.critDamage.AddModifier(critDamage);
        playerStats.damage.AddModifier(damage);

        // Magic stats
        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        // Major stats
        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        // Defensive stats
        playerStats.maxHP.RemoveModifier(HP);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistence.RemoveModifier(magicResistence);

        // Attack stats
        playerStats.critRate.RemoveModifier(critRate);
        playerStats.critDamage.RemoveModifier(critDamage);
        playerStats.damage.RemoveModifier(damage);

        // Magic stats
        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }


}
