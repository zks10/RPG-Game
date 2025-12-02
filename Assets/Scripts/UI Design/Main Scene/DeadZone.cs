using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            StartCoroutine(PlayerFallSequence(playerStats));
            return;
        }

        EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            enemyStats.DieOutSide();  
        }

        Destroy(collision.gameObject);
    }

    private IEnumerator PlayerFallSequence(PlayerStats playerStats)
    {
        UI ui = GameObject.Find("UI_Manager").GetComponent<UI>();
        ui.SwitchOnEndScreen();

        yield return new WaitForSecondsRealtime(1.3f);
        playerStats.diedInVoid = true; 
        playerStats.currentHP = playerStats.GetMaxHP();
        playerStats.Die();
    }
}
