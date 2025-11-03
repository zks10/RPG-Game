using UnityEngine;


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


[CreateAssetMenu(fileName = "Buff Effect", menuName = "Items Data/Item Effects/Buff Effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats playerStats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.IncreaseStatBy(buffAmount, buffDuration, StatToModifer());
    }

    private Stat StatToModifer()
    {
        switch (buffType)
        {
            case StatType.strength: return playerStats.strength;
            case StatType.agility: return playerStats.agility;
            case StatType.intelligence: return playerStats.intelligence;
            case StatType.vitality: return playerStats.vitality;
            case StatType.damage: return playerStats.damage;
            case StatType.critRate: return playerStats.critRate;
            case StatType.critDamage: return playerStats.critDamage;
            case StatType.HP: return playerStats.maxHP;
            case StatType.armor: return playerStats.armor;
            case StatType.evasion: return playerStats.evasion;
            case StatType.magicResistence: return playerStats.magicResistence;
            case StatType.fireDamage: return playerStats.fireDamage;
            case StatType.iceDamage: return playerStats.iceDamage;
            case StatType.lightningDamage: return playerStats.lightningDamage;
            default:
                return null;
        }
    }
}
