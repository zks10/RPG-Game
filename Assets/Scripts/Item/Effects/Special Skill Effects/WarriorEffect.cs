using UnityEngine;

[CreateAssetMenu(fileName = "Warrior Effect", menuName = "Items Data/Item Effects/Warrior")]
public class WarriorEffect : ItemEffect
{
    [Header("Boost Values")]
    [SerializeField] private int offensiveBoostValue;
    [SerializeField] private int defensiveBoostValue;
    [SerializeField] private float boostDuration = 10f;

    public override void ExecuteEffect(EffectContext ctx)
    {
        if (ctx.user == null) return;

        PlayerStats stats = ctx.user.GetComponent<PlayerStats>();
        if (stats == null) return;

        bool offensive = stats.currentHP > stats.GetMaxHP() * 0.5f;

        if (offensive)
        {
            stats.IncreaseStatBy(offensiveBoostValue, boostDuration, stats.strength);
            stats.IncreaseStatBy(offensiveBoostValue, boostDuration, stats.intelligence);
        }
        else
        {
            stats.IncreaseStatBy(defensiveBoostValue, boostDuration, stats.vitality);
            stats.IncreaseStatBy(defensiveBoostValue, boostDuration, stats.agility);
        }
    }
}
