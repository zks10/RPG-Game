using UnityEngine;


[CreateAssetMenu(fileName = "Buff Effect", menuName = "Items Data/Item Effects/Buff Effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats playerStats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.IncreaseStatBy(buffAmount, buffDuration, playerStats.GetStat(buffType));
    }
}
