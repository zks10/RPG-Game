using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                if (_target != null)
                    player.stats.DoPhysicalDamage(_target);

                var weapon = Inventory.instance.GetEquipementByType(EquipmentType.Weapon);
                if (weapon == null)
                    return; 
                weapon.ItemEffect(_target.transform);

            }

            
                
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
    
}
