using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    protected PlayerStats playerStats;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            playerStats.DoMagicalDamage(collision.GetComponent<EnemyStats>());
        }
    }

}
