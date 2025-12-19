using UnityEngine;

public enum HandSpellTarget
{
    Player,
    Enemy
}

public class HandSpell_Controller : MonoBehaviour
{
    [Header("Hitbox")]
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;

    [Header("Targeting")]
    [SerializeField] private HandSpellTarget targetType = HandSpellTarget.Player;
    [SerializeField] private LayerMask whatIsTarget;

    private CharacterStats myStats;

    public void SetUpSpell(CharacterStats stats) => myStats = stats;

    // Call from animation event
    private void AnimationTrigger()
    {
        if (myStats == null) return;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, 0f, whatIsTarget);

        foreach (var hit in colliders)
        {
            if (targetType == HandSpellTarget.Player)
            {
                Player p = hit.GetComponent<Player>();
                if (p == null || p.isDead) continue;

                p.GetComponent<Entity>().SetUpKnockBackDir(transform);
                myStats.DoMagicalDamage(p.GetComponent<CharacterStats>());
            }
            else // Enemy
            {
                EnemyStats e = hit.GetComponent<EnemyStats>();
                if (e == null || e.isDead) continue;

                myStats.DoMagicalDamage(e, 1.5f);

            }
        }
    }

    private void SelfDestroy() => Destroy(gameObject);

    private void OnDrawGizmosSelected()
    {
        if (check == null) return;
        Gizmos.DrawWireCube(check.position, boxSize);
    }
}
