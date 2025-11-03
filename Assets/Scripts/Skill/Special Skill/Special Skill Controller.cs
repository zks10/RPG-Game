using UnityEngine;

public class SpecialSkillController : MonoBehaviour
{
    protected PlayerStats playerStats;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            playerStats.DoMagicalDamage(collision.GetComponent<EnemyStats>());
        }
    }
}
