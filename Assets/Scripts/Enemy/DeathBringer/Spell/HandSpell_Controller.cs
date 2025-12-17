using UnityEngine;

public class HandSpell_Controller : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;
    private CharacterStats myStats;
    
    public void SetUpSpell(CharacterStats _stats) => myStats = _stats;
    private void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, whatIsPlayer);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                if (hit.GetComponent<Player>().isDead) continue;
                hit.GetComponent<Entity>().SetUpKnockBackDir(transform);
                myStats.DoMagicalDamage(hit.GetComponent<CharacterStats>());
                
            }
        }
        
    }

    private void OnDrawGizmos() => Gizmos.DrawWireCube(check.position, boxSize);

    private void SelfDestroy() => Destroy(gameObject);
    
}
