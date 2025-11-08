using UnityEngine;

public class SkeletonAnimationTrigger : MonoBehaviour
{
    private EnemySkeleton skeleton => GetComponentInParent<EnemySkeleton>();
    private void AnimationTrigger()
    {
        skeleton.AnimationFinishTrigger();
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(skeleton.attackCheck.position, skeleton.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();
                if (_target != null)
                    skeleton.stats.DoPhysicalDamage(_target);

                var armor = Inventory.instance.GetEquipementByType(EquipmentType.Armor);
                if (armor == null)
                    return; 
                armor.ItemEffect(skeleton.transform);
            }
        }
    }

    private void OpenCounterAttackWindow() => skeleton.OpenCounterAttackWindow();
    private void CloseCounterAttackWindow() => skeleton.CloseCounterAttackWindow();
}
