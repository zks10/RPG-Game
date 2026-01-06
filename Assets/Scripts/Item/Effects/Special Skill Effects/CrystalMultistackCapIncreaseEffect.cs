// using UnityEngine;

// [CreateAssetMenu(menuName = "Items Data/Item Effects/Multistack Cap Increase")]
// public class CrystalMultistackCapIncreaseEffect : ItemEffect
// {
//     [SerializeField] private int extraStacks = 2; // 3 → 5

//     public override void ExecuteEffect(EffectContext ctx)
//     {
//         if (ctx.trigger != ItemTrigger.OnQueryCrystalMaxStack)
//             return;

//         Player player = ctx.user?.GetComponent<Player>();
//         if (player == null)
//             return;

//         if (!player.skill.crystal.multiStackUnlock)
//             return;

//         ctx.intValue += extraStacks;
//     }
// }
