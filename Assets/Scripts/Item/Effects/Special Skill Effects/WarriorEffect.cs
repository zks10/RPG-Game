using UnityEngine;

[CreateAssetMenu(fileName = "Warrior Effect", menuName = "Items Data/Item Effects/Warrior")]
public class WarriorEffect : ItemEffect
{
    [Header("Boost Values")]
    [SerializeField] private int offensiveBoostValue; 
    [SerializeField] private int defensiveBoostValue;
    [SerializeField] private float boostDuration = 10f;

    private bool isOffensiveActive;
    private bool isDefensiveActive;

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats stats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        bool shouldBeOffensive = stats.currentHP > stats.GetMaxHP() * 0.5f;

        if (shouldBeOffensive && !isOffensiveActive)
        {
            RemoveDefensiveBoost(stats);
            ApplyOffensiveBoost(stats);
        }
        else if (!shouldBeOffensive && !isDefensiveActive)
        {
            RemoveOffensiveBoost(stats);
            ApplyDefensiveBoost(stats);
        }
    }

    private void ApplyOffensiveBoost(PlayerStats stats)
    {
        isOffensiveActive = true;
        isDefensiveActive = false;

        stats.IncreaseStatBy(offensiveBoostValue, boostDuration, stats.strength);
        stats.IncreaseStatBy(offensiveBoostValue, boostDuration, stats.intelligence);

    }

    private void RemoveOffensiveBoost(PlayerStats stats)
    {
        if (!isOffensiveActive) return;
        isOffensiveActive = false;

        stats.strength.RemoveModifier(offensiveBoostValue);
        stats.intelligence.RemoveModifier(offensiveBoostValue);

    }

    private void ApplyDefensiveBoost(PlayerStats stats)
    {
        isDefensiveActive = true;
        isOffensiveActive = false;

        stats.IncreaseStatBy(defensiveBoostValue, boostDuration, stats.vitality);
        stats.IncreaseStatBy(defensiveBoostValue, boostDuration, stats.agility);

    }

    private void RemoveDefensiveBoost(PlayerStats stats)
    {
        if (!isDefensiveActive) return;
        isDefensiveActive = false;

        stats.vitality.RemoveModifier(defensiveBoostValue);
        stats.evasion.RemoveModifier(defensiveBoostValue);

    }
}
