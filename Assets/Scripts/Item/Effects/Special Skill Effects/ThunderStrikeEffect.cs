using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Items Data/Item Effects/Thunder Strike")]
public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(EffectContext ctx)
    {
        if (ctx.target == null) return;

        GameObject obj = Instantiate(thunderStrikePrefab, ctx.target.position, Quaternion.identity);
        Destroy(obj, 0.5f);
    }
}
