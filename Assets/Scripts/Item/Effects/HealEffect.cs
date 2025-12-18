using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Items Data/Item Effects/Heal Effect")]
public class HealEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercentage;

    public override void ExecuteEffect(EffectContext ctx)
    {
        Transform user = ctx.user != null ? ctx.user : PlayerManager.instance.player.transform;
        if (user == null) return;

        PlayerStats playerStats = user.GetComponent<PlayerStats>();
        if (playerStats == null) return;

        int amountToHeal = Mathf.RoundToInt(playerStats.GetMaxHP() * healPercentage);
        playerStats.IncreaseHPBy(amountToHeal);
    }
}
