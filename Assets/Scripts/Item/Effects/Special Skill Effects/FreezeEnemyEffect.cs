using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Enemy Effect", menuName = "Items Data/Item Effects/Freeze Enemy")]
public class FreezeEnemyEffect : ItemEffect
{
    [SerializeField] private float duration;
    [SerializeField] private float freezeRadius;

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats stats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (stats.currentHP > stats.GetMaxHP() * .1f)
            return;
        if (!Inventory.instance.CanUseArmor())
            return;
            
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, freezeRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().FreezeTimeFor(duration);
            }
        } 
    }

}
