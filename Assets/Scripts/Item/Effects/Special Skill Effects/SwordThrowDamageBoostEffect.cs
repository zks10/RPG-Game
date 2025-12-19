using UnityEngine;

[CreateAssetMenu(fileName = "Sword Throw Damage Boost", menuName = "Items Data/Item Effects/Sword Throw Damage Boost")]
public class SwordThrowDamageBoostEffect : ItemEffect
{
    [Range(0f, 2f)]
    [SerializeField] private float bonusMultiplier = 0.6f; // 0.6 = +60%

    // This effect is meant for the equipped WEAPON, but executed from sword hit code.
    // We'll read it and use its value (not spawn anything).
    public float GetMultiplier() => 1f + bonusMultiplier;

    public override void ExecuteEffect(EffectContext ctx)
    {
        // Intentionally empty.
        // This effect is queried, not "executed" like VFX effects.
    }
}
