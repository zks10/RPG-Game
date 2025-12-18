using UnityEngine;

[CreateAssetMenu(fileName = "Thorn Effect", menuName = "Items Data/Item Effects/Thorn")]
public class ThornEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float reflectPercentage = 0.5f;

    public override void ExecuteEffect(EffectContext ctx)
    {
        if (ctx.target == null) return;

        EnemyStats attackerStats = ctx.target.GetComponent<EnemyStats>();
        if (attackerStats == null || attackerStats.isDead) return;

        int attackerDamage = attackerStats.GetCalculatedStatValue(StatType.damage);
        int reflectDamage = Mathf.RoundToInt(attackerDamage * reflectPercentage);

        attackerStats.TakeDamage(reflectDamage);
    }
}
