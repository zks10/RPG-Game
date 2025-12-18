using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Enemy Effect", menuName = "Items Data/Item Effects/Freeze Enemy")]
public class FreezeEnemyEffect : ItemEffect
{
    [SerializeField] private float duration;
    [SerializeField] private float freezeRadius;

    public override void ExecuteEffect(EffectContext ctx)
    {
        if (ctx.user == null) return;

        PlayerStats stats = ctx.user.GetComponent<PlayerStats>();
        if (stats == null) return;

        if (stats.currentHP > stats.GetMaxHP() * 0.1f)
            return;

        if (!Inventory.instance.CanUseArmor())
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(ctx.user.position, freezeRadius);

        foreach (var hit in colliders)
        {
            Enemy e = hit.GetComponent<Enemy>();
            if (e != null)
                e.FreezeTimeFor(duration);
        }
    }
}
