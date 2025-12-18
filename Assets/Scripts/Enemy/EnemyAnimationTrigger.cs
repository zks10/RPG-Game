using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    private Enemy enemy => GetComponentInParent<Enemy>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        AudioManager.instance.PlaySFX(37, enemy.transform);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();
                if (_target != null)
                    enemy.stats.DoDamageToPlayer(_target);

                ItemData_Equipment armor = Inventory.instance.GetEquipmentByType(EquipmentType.Armor);
                if (armor == null)
                    continue;
                armor.ItemEffect(new EffectContext
                {
                    trigger = ItemTrigger.OnTakeDamage,
                    user = hit.transform,       // player
                    target = enemy.transform    // enemy attacker
                });
            }
        }
    }

    private void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();
    private void DetectPlayerImage() => enemy.DetectPlayerImage();
    private void StartWalking() => enemy.StartWalking();
    private void Footstep() => enemy.Footstep();
    private void SpecialAttackTrigger() => enemy.AnimationSpecialAttackTrigger();
    

}
