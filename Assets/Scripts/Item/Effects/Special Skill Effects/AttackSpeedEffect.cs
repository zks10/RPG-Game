using UnityEngine;

[CreateAssetMenu(fileName = "Attack Speed Effect", menuName = "Items Data/Item Effects/Attack Speed")]
public class AttackSpeedEffect : ItemEffect
{
    [Range(0f, 2f)]
    public float attackSpeedMultiplier = 1.35f; // +35%

    // No ExecuteEffect needed
    public override void ExecuteEffect(EffectContext ctx) { }
}
