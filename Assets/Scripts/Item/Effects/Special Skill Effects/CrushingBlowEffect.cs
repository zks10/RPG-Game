using UnityEngine;

[CreateAssetMenu(fileName = "Crushing Blow Effect", menuName = "Items Data/Item Effects/Crushing Blow")]
public class CrushingBlowEffect : ItemEffect
{
    [SerializeField] private int bonusDamage = 15;

    public override void ExecuteEffect(EffectContext context)
    {
        CharacterStats targetStats = context.target.GetComponent<CharacterStats>();

        if (targetStats == null)
            return;

        targetStats.TakeDamage(bonusDamage);

        Debug.Log("Crushing Blow activated: bonus damage");
    }
}