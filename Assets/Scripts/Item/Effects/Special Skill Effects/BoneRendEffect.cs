using UnityEngine;

[CreateAssetMenu(
    fileName = "Bone Rend Effect",
    menuName = "Items Data/Item Effects/Bone Rend")]
public class BoneRendEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float procChance = 0.2f;

    [SerializeField] private int bonusDamage = 8;

    public override void ExecuteEffect(EffectContext ctx)
    {
        if (ctx.target == null) return;

        if (Random.value > procChance)
            return;

        EnemyStats enemyStats = ctx.target.GetComponent<EnemyStats>();
        if (enemyStats == null || enemyStats.isDead)
            return;

        enemyStats.TakeDamage(bonusDamage);
    }
}
