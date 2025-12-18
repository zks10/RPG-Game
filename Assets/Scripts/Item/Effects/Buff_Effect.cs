using UnityEngine;

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Items Data/Item Effects/Buff Effect")]
public class Buff_Effect : ItemEffect
{
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;

    public override void ExecuteEffect(EffectContext ctx)
    {
        if (ctx.user == null) return;

        PlayerStats stats = ctx.user.GetComponent<PlayerStats>();
        if (stats == null) return;

        stats.IncreaseStatBy(buffAmount, buffDuration, stats.GetStat(buffType));
    }
}
