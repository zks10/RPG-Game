using UnityEngine;


[CreateAssetMenu(fileName = "Heal Effect", menuName = "Items Data/Item Effects/Heal Effect")]
public class HealEffect : ItemEffect
{
    [Range(0f,1f)]
    [SerializeField] private float healPercentage;
    public override void ExecuteEffect(Transform _respawnPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        int amountToHeal = Mathf.RoundToInt(playerStats.GetMaxHP() * healPercentage);

        playerStats.IncreaseHPBy(amountToHeal);
    }
}
