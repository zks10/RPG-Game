using UnityEngine;

[CreateAssetMenu(menuName = "Items Data/Item Effects/Explosion Damage Boost")]
public class CrystalExplosionEffect : ItemEffect
{
    [SerializeField] private float damageMultiplier = 1.25f;
    [SerializeField] private int extraStacks = 2; 


    public override void ExecuteEffect(EffectContext ctx)
    {
        if (ctx.trigger != ItemTrigger.OnEquip)
            return;
        Player player = ctx.user?.GetComponent<Player>();
        if (player == null)
            return;
        player.skill.crystal.SetExplosionMultiplier(damageMultiplier - 1f); 
        
        if (player.skill.crystal.canUseMultiStacks)
            player.skill.crystal.SetExtraStacksFromItem(extraStacks);
        else
            player.skill.crystal.SetExtraStacksFromItem(0);
    }

}
