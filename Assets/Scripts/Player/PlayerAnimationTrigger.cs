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
                if (hit.GetComponent<EnemyStats>().isDead) continue;
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                if (_target != null)
                    player.stats.DoPhysicalDamage(_target);

                ItemData_Equipment weapon = Inventory.instance.GetEquipmentByType(EquipmentType.Weapon);
                if (weapon == null)
                    continue; 
                weapon.ItemEffect(_target.transform);

            }

            
                
        }
    }

    private void ThrowSword()
    {
        AudioManager.instance.PlaySFX(22, player.transform);
        SkillManager.instance.sword.CreateSword();
    }
    
}
