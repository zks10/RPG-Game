using UnityEngine;

[CreateAssetMenu(fileName = "Thorn Effect", menuName = "Items Data/Item Effects/Thorn")]
public class ThornEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float reflectPercentage = 0.5f;

    public override void ExecuteEffect(Transform _enemyTransform)
    {
        if (_enemyTransform == null)
            return;
        EnemyStats stats = _enemyTransform.GetComponent<EnemyStats>();

        if (stats == null)
            return;

        int damageToReflect = Mathf.RoundToInt(stats.GetCalculatedStatValue(StatType.damage) * reflectPercentage);

        stats.TakeDamage(damageToReflect);
            

    }
}
